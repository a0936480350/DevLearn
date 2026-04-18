# DevLearn · System Architecture

> **Last updated**: 2026-04-19
> **Maintainer**: Mike (邱瀚賢 · [@a0936480350](https://github.com/a0936480350))
> **Live**: https://devlearn-dotnet.azurewebsites.net

This document explains how DevLearn is put together — the high-level components,
why they were chosen, and how they fit together in production.  For specific
design decisions (trade-offs), see the ADRs under [`/docs/adr`](./adr).

---

## 1 · What DevLearn is

A full-stack **.NET learning platform** that bundles:

- **232 bite-sized chapters** across C#, ASP.NET, SQL, JavaScript, Vue, AI-assisted dev, system design
- **8 interactive mini-games** (code monopoly, bug detective, speed arena, battle mode, code puzzles, etc.)
- **Teacher marketplace** (multi-domain 1-on-1 lessons with calendar + booking + reviews)
- **Community layer** (Q&A board, knowledge-sharing wall, file sharing, SignalR live chat, private messaging)
- **Gamification** (XP, badges, achievements, streaks, leaderboard)
- **Payment** (ECPay subscription — teacher Premium monthly, sandbox-ready)
- **LifeQuest integration** (separate .NET MAUI app syncs its Quest board from DevLearn chapters via `/api/integration/*`)

It's monolithic by design today — **one ASP.NET Core 8 app talks to one PostgreSQL** — but carved so a
hot module (AI Code Tutor) can be extracted as a service when load demands it.  See [ADR-001](./adr/ADR-001-mvc-vs-webapi.md)
for the MVC vs pure-API framing.

---

## 2 · System context (C4 Level 1)

```
                    ┌────────────────────────┐
                    │  Browser (desktop/mo.) │
                    │                        │
                    │  • Student             │
                    │  • Teacher             │
                    │  • Admin (Mike)        │
                    └───────────┬────────────┘
                                │ HTTPS
                                ▼
             ┌──────────────────────────────────────────┐
             │                                          │
             │          DevLearn Web App                │
             │     (ASP.NET Core 8 · MVC + SignalR)     │
             │                                          │
             │  • Cookie auth / session store           │
             │  • Google OAuth (optional)               │
             │  • 232 chapters + quizzes                │
             │  • 8 mini-games                          │
             │  • Teacher / booking / reviews           │
             │  • SignalR hubs (chat, battle)           │
             │  • ECPay payment callback                │
             │  • /api/integration/* for satellites     │
             │                                          │
             └──┬──────────────┬────────────┬───────────┘
                │              │            │
                ▼              ▼            ▼
     ┌──────────────────┐  ┌───────────┐  ┌──────────────────┐
     │  PostgreSQL      │  │ %HOME%\   │  │ LifeQuest MAUI   │
     │ (Azure Flexible) │  │ data\*    │  │ (external satellite)
     │  ~40 tables      │  │ uploads   │  │  • Syncs chapters
     │                  │  │           │  │    as Quests
     └──────────────────┘  └───────────┘  │  • Gold/XP rewards
                                          └──────────────────┘
                │
                ▼
     ┌──────────────────┐
     │ External services│
     │  • Google OAuth  │
     │  • SMTP (Gmail)  │
     │  • ECPay         │
     └──────────────────┘
```

---

## 3 · Container diagram (C4 Level 2)

Inside the DevLearn web app process:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         ASP.NET Core 8 · Kestrel                        │
│                                                                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────────────┐   │
│  │ MVC Pipeline │  │ SignalR Hubs │  │ API Controllers              │   │
│  │              │  │              │  │ (Attribute-routed)           │   │
│  │ • Home       │  │ • ChatHub    │  │                              │   │
│  │ • Chapter    │  │ • BattleHub  │  │ • IntegrationController      │   │
│  │ • Teacher    │  │              │  │   /api/integration/me        │   │
│  │ • Admin      │  │              │  │   /api/integration/chapters  │   │
│  │ • 8 games    │  │              │  │   /api/integration/chapters/ │   │
│  │ • Auth       │  │              │  │        progress              │   │
│  │ • Promotion  │  │              │  │ • PaymentController          │   │
│  └──────┬───────┘  └──────┬───────┘  │   (ECPay callback)           │   │
│         │                 │          └──────────────┬───────────────┘   │
│         │                 │                         │                   │
│         └─────────────────┴─────────────────────────┘                   │
│                                │                                        │
│                                ▼                                        │
│                ┌──────────────────────────────┐                         │
│                │  Service layer               │                         │
│                │  • EmailService (SMTP)       │                         │
│                │  • EcpayService (sign/verify)│                         │
│                │  • ErrorScannerService       │                         │
│                │    (hosted, scans logs)      │                         │
│                └──────────────┬───────────────┘                         │
│                               │                                         │
│                               ▼                                         │
│                ┌──────────────────────────────┐                         │
│                │  Data layer                  │                         │
│                │  AppDbContext (EF Core)      │                         │
│                │   ~40 DbSets                 │                         │
│                └──────────────┬───────────────┘                         │
│                               │ Npgsql                                  │
└───────────────────────────────┼─────────────────────────────────────────┘
                                ▼
                    PostgreSQL (Azure Flexible Server)
```

**Middleware ordering** (in `Program.cs`, top to bottom):

1. `UseStaticFiles()` — `/wwwroot` + `/uploads`
2. `UseRouting()`
3. `UseCors("IntegrationPolicy")` — only `/api/integration/*` allows cross-origin
4. `UseSession()` — cookies + DB-backed session fallback
5. `UseAuthentication()` — cookie primary, Google OAuth secondary
6. Anonymous-ID middleware (custom) — issues `DotNetLearner` cookie if absent
7. `UseAuthorization()`
8. `MapControllerRoute()` + `MapControllers()` + SignalR hubs

---

## 4 · Data model highlights

Full schema lives in `Data/AppDbContext.cs`.  The interesting groups:

| Domain            | Key tables                                                                  |
| ----------------- | --------------------------------------------------------------------------- |
| Identity          | `SiteUsers` (AnonymousId primary, supports anonymous → registered upgrade)  |
| Content           | `Chapters`, `Questions`, `CodeSnippets`, `Flashcards`                       |
| Progress          | `Progresses`, `QuizAttempts`, `CheckIns`, `StudyLogs`, `Achievements`       |
| Community         | `Ideas`, `Replies`, `QnAs`, `QnAAnswers`, `ChatMessages`, `PrivateMessages` |
| Games             | `BugChallenges`, `CodePuzzles`, `ArenaChallenges`, `BattleRecords`, etc.    |
| Teacher system    | `Teachers`, `TeacherSlots`, `Bookings`, `Reviews`, `TeacherPosts`, `FavoriteTeachers` |
| Admin / ops       | `SupportTickets`, `AuditLog`, `ErrorLogs`, `ClaudeTasks`, `Announcements`   |
| Payment           | `Payments`, `TeacherSubscriptions` (ECPay sandbox → prod via env vars)       |
| Shared files      | `SharedFiles` (stored in `%HOME%\data\shared\` since wwwroot is read-only)   |

**Non-trivial design choices**:

- `SiteUser.AnonymousId` is the **de-facto foreign key** for most child tables —
  not `SiteUser.Id`.  This is deliberate so that a guest can accumulate data
  (chapters read, quiz scores) before registering, and on register the data
  follows the cookie.  Other modules (Booking, FavoriteTeacher, PrivateMessage)
  also store `AnonymousId` as `StudentId` / `SenderId` rather than numeric FK.
- PostgreSQL casing is enforced (`"SiteUsers"` not `siteusers`) because EF Core's
  default naming convention uses PascalCase identifiers; the raw SQL bootstrap
  in `Program.cs` matches this.
- The bootstrap block in `Program.cs` issues `CREATE TABLE IF NOT EXISTS` for
  every table to survive a fresh deploy without running `EnsureCreated()`
  (which can drift from the EF model on an existing DB).

---

## 5 · Deployment topology

```
  GitHub (main branch)
        │
        │   push
        ▼
  GitHub Actions · deploy.yml
        │
        │   dotnet publish → zip
        ▼
  azure/webapps-deploy@v3
        │
        │   AZURE_PUBLISH_PROFILE secret
        ▼
  ┌──────────────────────────────────────────────┐
  │  Azure App Service  `devlearn-dotnet`        │
  │  • Linux container                           │
  │  • WEBSITE_RUN_FROM_PACKAGE=1 (zip mount)    │
  │  • DATABASE_URL env var (PG connection)      │
  │  • GOOGLE_CLIENT_ID/SECRET, SMTP creds…      │
  │                                              │
  │  Requires explicit Kudu restart after deploy │
  │  for new package to remount.                 │
  └──────────┬───────────────────────────────────┘
             │
             ▼
  Azure PostgreSQL Flexible Server
  `devlearn-pg` · devadmin user
  Single region (East Asia)

  Keep-alive cron (GitHub Actions, every 10 min)
  pings /health to fight cold starts.
```

**Why run-from-package**:

- Single-file, immutable, atomic deploys (no half-deployed state)
- `wwwroot` becomes read-only — file uploads instead go to `%HOME%\data\*`
  (persistent across deploys, NOT in the zip), see `SharedFileController.GetStorageRoot()`
- Remounting the new zip requires a restart; CI triggers it via `azure/webapps-deploy@v3`,
  but in practice an explicit `POST /api/app/restart` to Kudu is occasionally
  needed when the app reports 404 on newly added routes

---

## 6 · Cross-project integrations

### 6.1 LifeQuest satellite (MAUI app)

- Separate repo: <https://github.com/a0936480350/LifeQuest>
- Calls DevLearn's `/api/integration/*` (CORS wide-open, Phase 1)
- Flow: `anonId` → `JWT` signed by LifeQuest → sync 232 chapters as Quests
- Users see a DevLearn chapter show up as a daily Quest with XP reward in LifeQuest
- Planned Phase 2.5: LifeQuest issues discount coupons that DevLearn `/api/integration/redeem-coupon` validates at checkout

### 6.2 ECPay payment

- `EcpayService` (singleton) handles `CheckMacValue` (SHA-256 + URL-encode with custom char replacement per ECPay spec)
- Sandbox uses ECPay public test MerchantID; production flips via env vars (`ECPAY_MERCHANT_ID` etc.)
- Two callbacks:
  - `POST /Payment/EcpayReturn` — server-to-server (authoritative, idempotent, verifies signature)
  - `POST /Payment/EcpayOrderResult` — browser lands here after payment

---

## 7 · Scalability & known limits

| Concern                         | Status today                                      | When it'd bite                                  | Plan                                                         |
| ------------------------------- | ------------------------------------------------- | ----------------------------------------------- | ------------------------------------------------------------ |
| **Chapter read traffic**        | Every request hits Postgres                       | > 1000 concurrent readers                       | Redis cache (Month 1 Week 3) · 5-min TTL on Chapter list     |
| **Session state**               | EF-backed in `SiteUsers.LastActiveAt`              | Too many writes throttle LastActive path        | Already throttled (5-min debounce per user)                  |
| **AI Code Tutor CPU**           | Shares the app process                            | Heavy AI load blocks web requests               | Extract as microservice (Month 1 Week 3)                     |
| **SignalR scale-out**           | Single-instance                                    | Multiple App Service instances                  | Redis backplane when needed (not today, single node scales)   |
| **Search**                      | `WHERE LIKE '%...'` on titles/content             | > 10k chapters or full-text queries             | PostgreSQL GIN index or move to Elasticsearch                 |
| **Observability**               | `Console.WriteLine` + Azure log stream            | First real user complaint                       | Serilog → Application Insights (Month 1 Week 4 candidate)     |

---

## 8 · Security posture

- **AuthN**: cookie + session + Google OAuth; admin uses a shared cookie value (`AdminAuth=pxmart-admin-verified-2026`) — **good enough for MVP, not production**
- **AuthZ**: role field on `SiteUser` (`guest/member/teacher/admin`), checked in-controller
- **Passwords**: BCrypt (cost 10)
- **CORS**: wide-open on `/api/integration/*` (Phase 1 only; should narrow to LifeQuest origin later)
- **Secrets**: env vars on Azure App Service Configuration; `.env.local` gitignored
- **CSRF**: antiforgery tokens on all form POSTs except ECPay callback (`[IgnoreAntiforgeryToken]` — unavoidable, signature-verified)
- **Upload validation**: extension allowlist in `SharedFileController`, 50 MB cap

Known gap: admin cookie mechanism is a hardcoded string; should be time-bounded JWT once there's a second admin.

---

## 9 · Project layout

```
DotNetLearning/
├── Controllers/        Controllers (each under its own concern)
├── Views/              Razor views (Home, Chapter, Admin, Teacher, Payment, SharedFile…)
├── Models/             POCO entities
├── Data/               AppDbContext + Seed*.cs files
├── Services/           Cross-cutting (EmailService, EcpayService, ErrorScannerService)
├── Hubs/               SignalR (ChatHub, BattleHub)
├── Middleware/         (mostly inline in Program.cs; grows here as needed)
├── wwwroot/            Static assets: CSS, game JS, seed images
│   ├── css/
│   ├── js/games/       Phaser/vanilla game code (monopoly, arena, bugs…)
│   └── images/
├── docs/               → you are here
│   ├── ARCHITECTURE.md
│   └── adr/
└── .github/workflows/  deploy.yml + keep-alive.yml
```

---

## 10 · Related documents

- [ADR-001 · MVC vs Web API](./adr/ADR-001-mvc-vs-webapi.md)
- [ADR-002 · PostgreSQL vs Azure SQL Server](./adr/ADR-002-postgres-vs-sqlserver.md)
- [ADR-003 · SignalR vs raw WebSocket vs SSE](./adr/ADR-003-signalr-vs-websocket.md)
- [README](../README.md) — quick start + screenshots
- [LifeQuest (satellite)](https://github.com/a0936480350/LifeQuest) — MAUI app that integrates via `/api/integration/*`
