using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Docker
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Docker/DevOps Chapter 51 ────────────────────────────
        new() { Id=51, Category="docker", Order=2, Level="intermediate", Icon="📦", Title="Docker 進階與 Compose", Slug="docker-compose", IsPublished=true, Content=@"
# Docker 進階與 Compose

## Multi-stage Build（多階段建置）

> 💡 **比喻：搬家**
> 你在舊家（Build 階段）有很多工具、材料，
> 但搬到新家（Runtime 階段）只帶需要的家具就好。
> Multi-stage Build 就是只把「成品」帶到最終的 Image，
> 不帶開發工具和原始碼，讓 Image 更小更安全。

### .NET Multi-stage Dockerfile

```dockerfile
# === 階段 1：Build（建置）===
# 使用完整的 SDK Image（包含編譯工具）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# 設定工作目錄
WORKDIR /src
# 先複製 csproj 檔案（利用 Docker 快取層）
COPY *.csproj ./
# 還原 NuGet 套件（這層很少變動，可以快取）
RUN dotnet restore
# 複製所有原始碼
COPY . .
# 發佈應用程式（Release 模式）
RUN dotnet publish -c Release -o /app/publish

# === 階段 2：Runtime（執行）===
# 使用精簡的 ASP.NET Runtime Image（沒有 SDK）
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# 設定工作目錄
WORKDIR /app
# 只從 build 階段複製發佈結果
COPY --from=build /app/publish .
# 設定容器對外的 port
EXPOSE 8080
# 設定啟動命令
ENTRYPOINT [""dotnet"", ""MyApp.dll""]
```

### Image 大小比較

```
方式                       Image 大小
────────────────────────────────────────
直接用 SDK Image           ≈ 800 MB（包含編譯工具）
Multi-stage（aspnet）      ≈ 220 MB（只有 Runtime）
Multi-stage（alpine）      ≈ 110 MB（精簡 Linux）

alpine 版本更小，但可能缺少某些 Linux 套件
```

---

## Docker Compose

> 💡 **比喻：樂團指揮**
> 你的應用程式像一個樂團：
> - Web App = 主唱
> - Database = 鼓手
> - Redis Cache = 吉他手
> Docker Compose 就是指揮，一個手勢讓所有人同時開始演奏。

### docker-compose.yml 基本結構

```yaml
# Docker Compose 設定檔版本
version: '3.8'

# 定義所有服務
services:
  # 服務 1：Web 應用程式
  web:
    # 從當前目錄的 Dockerfile 建置
    build: .
    # port 對應：主機 5000 → 容器 8080
    ports:
      - ""5000:8080""
    # 環境變數
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Default=Server=db;Database=myapp;User=sa;Password=YourPassword123!
    # 相依性：web 會等 db 和 redis 啟動後才啟動
    depends_on:
      db:
        condition: service_healthy
      redis:
        condition: service_started
    # 掛載磁碟區（開發時方便修改）
    volumes:
      - ./logs:/app/logs
    # 連接到自訂網路
    networks:
      - app-network
    # 健康檢查
    healthcheck:
      test: [""CMD"", ""curl"", ""-f"", ""http://localhost:8080/health""]
      interval: 30s
      timeout: 10s
      retries: 3

  # 服務 2：SQL Server 資料庫
  db:
    # 使用官方 SQL Server Image
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - ""1433:1433""
    # 持久化資料（容器刪除後資料還在）
    volumes:
      - sql_data:/var/opt/mssql
    networks:
      - app-network
    # 資料庫健康檢查
    healthcheck:
      test: [""CMD"", ""/opt/mssql-tools18/bin/sqlcmd"", ""-S"", ""localhost"", ""-U"", ""sa"", ""-P"", ""YourPassword123!"", ""-Q"", ""SELECT 1"", ""-C""]
      interval: 10s
      timeout: 5s
      retries: 5

  # 服務 3：Redis 快取
  redis:
    # 使用官方 Redis Alpine Image（精簡版）
    image: redis:alpine
    ports:
      - ""6379:6379""
    networks:
      - app-network

# 定義持久化磁碟區
volumes:
  sql_data:

# 定義網路
networks:
  app-network:
    driver: bridge
```

### 常用 Docker Compose 命令

```bash
# 建置並啟動所有服務（背景執行）
docker compose up -d --build

# 查看所有服務狀態
docker compose ps

# 查看特定服務的 Log
docker compose logs web

# 即時追蹤 Log（像 tail -f）
docker compose logs -f web

# 停止所有服務
docker compose stop

# 停止並刪除所有容器、網路
docker compose down

# 停止並刪除所有容器、網路、磁碟區（資料也會刪除！）
docker compose down -v

# 只重建並重啟 web 服務
docker compose up -d --build web

# 進入容器內部執行命令
docker compose exec web bash
```

---

## 環境變數管理

### .env 檔案

```bash
# .env 檔案（不要推上 Git！記得加到 .gitignore）
# 資料庫密碼
SA_PASSWORD=YourPassword123!
# JWT 金鑰
JWT_KEY=your-super-secret-jwt-key
# 應用程式環境
ASPNETCORE_ENVIRONMENT=Development
```

```yaml
# docker-compose.yml 中使用 .env 變數
services:
  db:
    environment:
      # 用 ${} 引用 .env 中的變數
      - SA_PASSWORD=${SA_PASSWORD}
  web:
    environment:
      - JWT_KEY=${JWT_KEY}
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
```

---

## Health Checks（健康檢查）

```csharp
// ASP.NET Core 設定健康檢查端點
// Program.cs
// 加入健康檢查服務
builder.Services.AddHealthChecks()
    // 檢查資料庫連線
    .AddSqlServer(
        builder.Configuration.GetConnectionString(""Default"")!,
        name: ""database"")
    // 檢查 Redis 連線
    .AddRedis(""localhost:6379"", name: ""redis"");

// 設定健康檢查路由
app.MapHealthChecks(""/health"");
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Image 太大（沒用 Multi-stage Build）

```dockerfile
# ❌ 錯誤：用 SDK Image 來執行（超大！800MB）
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o /out
# SDK 包含編譯器、NuGet 快取等，不需要出現在正式環境
ENTRYPOINT [""dotnet"", ""/out/MyApp.dll""]

# ✅ 正確：用 Multi-stage Build，最終 Image 只有 Runtime
# 參考上面的 Multi-stage Dockerfile 範例
# 最終 Image 只有 ~110-220 MB
```

### ❌ 錯誤 2：容器用 root 執行

```dockerfile
# ❌ 錯誤：預設以 root 身份執行（安全風險）
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [""dotnet"", ""MyApp.dll""]

# ✅ 正確：建立非 root 使用者來執行
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# 建立一個叫 appuser 的非 root 使用者
RUN adduser --disabled-password --gecos """" appuser
# 切換到 appuser 身份執行
USER appuser
ENTRYPOINT [""dotnet"", ""MyApp.dll""]
```

### ❌ 錯誤 3：把密碼寫在 docker-compose.yml 中

```yaml
# ❌ 錯誤：密碼直接寫在設定檔中（推上 Git 就洩漏了）
services:
  db:
    environment:
      - SA_PASSWORD=MyRealPassword123!

# ✅ 正確：用 .env 檔案或 Docker Secrets
# 1. 使用 .env（記得加入 .gitignore）
# 2. 使用 Docker Secrets（適合 Swarm 模式）
# 3. 使用 CI/CD 平台的環境變數功能
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Multi-stage Build | 分階段建置，最終 Image 只包含 Runtime |
| Docker Compose | 一次管理多個容器的工具 |
| depends_on | 定義服務啟動順序 |
| volumes | 持久化資料，容器刪除後資料還在 |
| networks | 容器間的虛擬網路 |
| Health Check | 定期檢查服務是否正常運作 |
| .env | 環境變數檔案，不要推上 Git |
" },

        // ── Docker/DevOps Chapter 52 ────────────────────────────
        new() { Id=52, Category="docker", Order=3, Level="intermediate", Icon="🔄", Title="CI/CD 持續整合部署", Slug="docker-cicd", IsPublished=true, Content=@"
# CI/CD 持續整合部署

## 什麼是 CI/CD？

> 💡 **比喻：工廠生產線**
> 傳統做法：工人手工組裝每一件產品，品質不穩定，速度慢。
> CI/CD：建立自動化生產線，每個步驟都自動檢查，有問題立刻停下來。
>
> - **CI（Continuous Integration）持續整合** = 自動檢查零件品質
> - **CD（Continuous Deployment）持續部署** = 自動包裝出貨

### CI/CD 流程

```
開發者推 Code
    ↓
CI 自動觸發
    ├── 1. 還原套件（dotnet restore）
    ├── 2. 編譯（dotnet build）
    ├── 3. 執行測試（dotnet test）
    ├── 4. 程式碼品質檢查
    └── 5. 建置 Docker Image
    ↓
CD 自動部署
    ├── 6. 推送 Image 到 Registry
    ├── 7. 部署到測試環境
    ├── 8. 部署到正式環境
    └── 9. 通知團隊
```

---

## GitHub Actions 基礎

### Workflow 設定檔位置

```
你的專案/
├── .github/
│   └── workflows/
│       ├── ci.yml        ← CI 工作流程
│       └── deploy.yml    ← 部署工作流程
├── src/
├── tests/
└── Dockerfile
```

### 基本 CI Workflow

```yaml
# .github/workflows/ci.yml
# 工作流程名稱
name: CI Pipeline

# 觸發條件
on:
  # 推送到 main 或 develop 分支時觸發
  push:
    branches: [ main, develop ]
  # Pull Request 到 main 時觸發
  pull_request:
    branches: [ main ]

# 定義工作
jobs:
  # 工作名稱：build-and-test
  build-and-test:
    # 執行環境：最新版 Ubuntu
    runs-on: ubuntu-latest

    # 步驟
    steps:
      # 步驟 1：拉取程式碼
      - name: Checkout code
        uses: actions/checkout@v4

      # 步驟 2：安裝 .NET SDK
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      # 步驟 3：還原 NuGet 套件
      - name: Restore dependencies
        run: dotnet restore

      # 步驟 4：編譯專案
      - name: Build
        run: dotnet build --configuration Release --no-restore

      # 步驟 5：執行單元測試
      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity normal

      # 步驟 6：發佈應用程式
      - name: Publish
        run: dotnet publish --configuration Release --no-build -o ./publish
```

### 加入 Docker Build

```yaml
# .github/workflows/docker.yml
# Docker 建置和推送工作流程
name: Docker Build & Push

on:
  push:
    branches: [ main ]

jobs:
  docker:
    runs-on: ubuntu-latest

    steps:
      # 拉取程式碼
      - name: Checkout
        uses: actions/checkout@v4

      # 登入 Docker Hub
      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          # 從 GitHub Secrets 讀取帳號密碼
          username: ${{ secrets.DOCKER_USERNAME }}
          password: ${{ secrets.DOCKER_PASSWORD }}

      # 建置並推送 Docker Image
      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          # 標記 Image 名稱和版本
          tags: |
            myapp:latest
            myapp:${{ github.sha }}
```

---

## Build → Test → Deploy Pipeline

### 完整的 CI/CD Pipeline

```yaml
# .github/workflows/pipeline.yml
# 完整的 CI/CD Pipeline
name: Full Pipeline

on:
  push:
    branches: [ main ]

jobs:
  # === 階段 1：Build & Test ===
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      # 還原、編譯、測試一氣呵成
      - name: Build and Test
        run: |
          dotnet restore
          dotnet build -c Release --no-restore
          dotnet test -c Release --no-build

      # 上傳建置產物（給下一個 Job 使用）
      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: publish
          path: ./publish/

  # === 階段 2：Deploy to Staging ===
  deploy-staging:
    # 依賴 build 工作完成後才執行
    needs: build
    runs-on: ubuntu-latest
    # 只有 main 分支才部署
    if: github.ref == 'refs/heads/main'
    # 指定環境（GitHub 環境保護規則）
    environment: staging

    steps:
      # 下載上一階段的建置產物
      - name: Download artifact
        uses: actions/download-artifact@v4
        with:
          name: publish

      # 部署到 Staging 環境
      - name: Deploy to Staging
        run: echo ""部署到測試環境...""

  # === 階段 3：Deploy to Production ===
  deploy-production:
    # 依賴 staging 部署完成
    needs: deploy-staging
    runs-on: ubuntu-latest
    # 需要手動審核的環境
    environment: production

    steps:
      - name: Download artifact
        uses: actions/download-artifact@v4
        with:
          name: publish

      # 部署到正式環境
      - name: Deploy to Production
        run: echo ""部署到正式環境...""
```

---

## Branch 策略

```
推薦的分支策略：

main（正式環境）
  ↑ merge（需要 PR + Code Review）
develop（開發環境）
  ↑ merge（需要 PR）
feature/add-login（功能分支）

流程：
1. 從 develop 切出 feature 分支
2. 在 feature 分支開發、commit
3. 推上 GitHub，建立 PR 到 develop
4. CI 自動跑測試 ← 測試失敗就不能 merge！
5. Code Review 通過 → merge 到 develop
6. develop 累積足夠功能 → merge 到 main
7. main 自動部署到正式環境
```

```
分支命名規範：

feature/add-login        → 新功能
feature/user-profile     → 新功能
bugfix/fix-login-error   → 修 Bug
hotfix/security-patch    → 緊急修復（直接從 main 切）
release/v1.2.0           → 發版準備
```

### GitHub Branch Protection

```
建議的 Branch Protection 設定：

main 分支：
├── ✅ Require pull request（必須 PR，不能直接 push）
├── ✅ Require approvals（至少 1 人 Code Review）
├── ✅ Require status checks（CI 必須通過）
├── ✅ Require branches to be up to date（必須是最新的）
└── ❌ Allow force push（禁止 force push）

develop 分支：
├── ✅ Require pull request
├── ✅ Require status checks
└── ⬜ Require approvals（可選）
```

---

## GitHub Secrets 管理

```
GitHub Secrets 用於存儲機密資料：

設定路徑：
Repository → Settings → Secrets and variables → Actions

常見的 Secrets：
├── DOCKER_USERNAME    → Docker Hub 帳號
├── DOCKER_PASSWORD    → Docker Hub 密碼
├── DEPLOY_KEY         → 部署用的 SSH Key
├── DATABASE_URL       → 資料庫連線字串
└── JWT_SECRET         → JWT 簽章金鑰
```

```yaml
# 在 Workflow 中使用 Secrets
env:
  # 用 ${{ secrets.名稱 }} 讀取 Secrets
  DATABASE_URL: ${{ secrets.DATABASE_URL }}

steps:
  - name: Deploy
    # Secrets 不會出現在 Log 中（自動遮罩）
    run: echo ""部署中..."" && deploy --db $DATABASE_URL
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：CI 沒有跑測試

```yaml
# ❌ 錯誤：CI 只有 build，沒有 test
steps:
  - run: dotnet restore
  - run: dotnet build
  # 沒有 dotnet test！
  # 這樣就算程式碼有 Bug，CI 還是會通過

# ✅ 正確：CI 一定要包含測試
steps:
  - run: dotnet restore
  - run: dotnet build -c Release --no-restore
  # 一定要跑測試！測試失敗就停止 Pipeline
  - run: dotnet test -c Release --no-build
```

### ❌ 錯誤 2：從本機直接部署

```
❌ 錯誤流程：
開發者在自己電腦 → dotnet publish → 手動複製到伺服器

問題：
├── 「在我電腦上明明可以跑啊！」（環境不一致）
├── 忘記跑測試就部署了
├── 不知道是誰部署的、部署了什麼版本
└── 無法回滾到上一版

✅ 正確流程：
開發者 push code → GitHub Actions 自動 build + test → 自動部署

好處：
├── 環境一致（都在 CI 的 Ubuntu 上 build）
├── 自動跑測試（測試失敗不會部署）
├── 有完整的部署紀錄
└── 可以隨時回滾
```

### ❌ 錯誤 3：把 Secrets 寫在 Workflow 檔案中

```yaml
# ❌ 錯誤：密碼直接寫在 YAML 中（推上 Git 就洩漏了）
env:
  DOCKER_PASSWORD: ""my-real-password-123""

# ✅ 正確：使用 GitHub Secrets
env:
  # ${{ secrets.DOCKER_PASSWORD }} 會從 GitHub Secrets 讀取
  # 而且不會出現在 CI 的 Log 中
  DOCKER_PASSWORD: ${{ secrets.DOCKER_PASSWORD }}
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| CI（持續整合） | 自動 build + test，確保程式碼品質 |
| CD（持續部署） | 自動部署到測試/正式環境 |
| GitHub Actions | GitHub 內建的 CI/CD 工具 |
| Workflow | YAML 格式的自動化流程定義 |
| Branch Protection | 保護重要分支，強制 PR 和 Code Review |
| GitHub Secrets | 安全儲存機密資料（密碼、Key） |
" },

        // ── Docker/DevOps Chapter 53 ────────────────────────────
        new() { Id=53, Category="docker", Order=4, Level="advanced", Icon="☁️", Title="雲端部署實戰", Slug="docker-cloud-deploy", IsPublished=true, Content=@"
# 雲端部署實戰

## 部署選項比較

```
平台              難度    價格        適合場景
──────────────────────────────────────────────────
Railway           低      免費方案    個人專案、學習用（我們實際在用！）
Azure App Service 中      免費方案    .NET 專案、企業級
AWS EC2           高      按用量      完全自訂、大規模
Heroku            低      免費方案    快速原型
DigitalOcean      中      固定月費    中小型專案
```

---

## Railway 部署（我們實際用的！）

> 💡 Railway 是一個簡單的雲端平台，非常適合學習和小型專案。
> 支援從 GitHub 自動部署，幾乎不用設定就能上線。

### 部署步驟

```
1. 到 railway.app 註冊帳號（用 GitHub 登入）
2. 點「New Project」
3. 選「Deploy from GitHub repo」
4. 選擇你的 Repository
5. Railway 自動偵測 Dockerfile 或 .NET 專案
6. 設定環境變數
7. 部署完成！自動產生 URL
```

### Railway 環境變數設定

```
在 Railway Dashboard 設定：

變數名稱                              值
─────────────────────────────────────────────────────
ASPNETCORE_ENVIRONMENT               Production
ConnectionStrings__Default           Server=...;Database=...
JWT_KEY                              your-production-jwt-key
PORT                                 8080（Railway 自動設定）
```

### Railway 用的 Dockerfile

```dockerfile
# 階段 1：建置
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# 複製專案檔案並還原套件
COPY *.csproj ./
RUN dotnet restore
# 複製所有原始碼並發佈
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 階段 2：執行
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# 從建置階段複製成品
COPY --from=build /app/publish .
# Railway 使用 PORT 環境變數
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
# 啟動應用程式
ENTRYPOINT [""dotnet"", ""MyApp.dll""]
```

---

## Azure App Service 部署

### 使用 Azure CLI 部署

```bash
# 安裝 Azure CLI 後登入
az login

# 建立 Resource Group（資源群組）
az group create --name myapp-rg --location eastasia

# 建立 App Service Plan（選擇免費方案）
az appservice plan create \
  --name myapp-plan \
  --resource-group myapp-rg \
  --sku F1 \
  --is-linux

# 建立 Web App
az webapp create \
  --name myapp-unique-name \
  --resource-group myapp-rg \
  --plan myapp-plan \
  --runtime ""DOTNETCORE:8.0""

# 設定環境變數
az webapp config appsettings set \
  --name myapp-unique-name \
  --resource-group myapp-rg \
  --settings ASPNETCORE_ENVIRONMENT=Production

# 部署程式碼
az webapp up \
  --name myapp-unique-name \
  --resource-group myapp-rg
```

### GitHub Actions 部署到 Azure

```yaml
# .github/workflows/azure-deploy.yml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      # 拉取程式碼
      - uses: actions/checkout@v4

      # 安裝 .NET
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      # 建置和發佈
      - name: Build and Publish
        run: dotnet publish -c Release -o ./publish

      # 部署到 Azure App Service
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          # App Service 的名稱
          app-name: myapp-unique-name
          # 從 GitHub Secrets 讀取發佈設定
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
          # 指定發佈目錄
          package: ./publish
```

---

## 反向代理（Reverse Proxy）

> 💡 **比喻：公司總機**
> 客戶打電話到公司總機（反向代理），
> 總機根據需求轉接到不同部門（後端伺服器）。
> 客戶不需要知道每個部門的分機號碼。

### Nginx 基本設定

```nginx
# /etc/nginx/sites-available/myapp
# Nginx 反向代理設定
server {
    # 監聽 80 port（HTTP）
    listen 80;
    # 網域名稱
    server_name myapp.example.com;

    # 轉址到 HTTPS
    return 301 https://$server_name$request_uri;
}

server {
    # 監聽 443 port（HTTPS）
    listen 443 ssl;
    server_name myapp.example.com;

    # SSL 憑證路徑
    ssl_certificate /etc/letsencrypt/live/myapp.example.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/myapp.example.com/privkey.pem;

    # 把所有請求轉發到 .NET 應用程式
    location / {
        # 轉發到本機的 5000 port（Kestrel）
        proxy_pass http://localhost:5000;
        # 傳遞原始的 Host 標頭
        proxy_set_header Host $host;
        # 傳遞使用者的真實 IP
        proxy_set_header X-Real-IP $remote_addr;
        # 傳遞轉發鏈的 IP
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        # 告訴後端使用了 HTTPS
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### Docker Compose 中使用 Nginx

```yaml
# docker-compose.yml 中加入 Nginx
services:
  # Nginx 反向代理
  nginx:
    image: nginx:alpine
    ports:
      # 對外只開 80 和 443
      - ""80:80""
      - ""443:443""
    volumes:
      # 掛載 Nginx 設定檔
      - ./nginx.conf:/etc/nginx/conf.d/default.conf
      # 掛載 SSL 憑證
      - ./certs:/etc/nginx/certs
    depends_on:
      - web

  # .NET 應用程式（不直接對外）
  web:
    build: .
    # 不需要對外開 port，Nginx 會轉發
    expose:
      - ""8080""
```

---

## SSL/TLS 設定

### Let's Encrypt 免費 SSL 憑證

```bash
# 安裝 Certbot（Let's Encrypt 的工具）
sudo apt install certbot python3-certbot-nginx

# 自動取得和設定 SSL 憑證
sudo certbot --nginx -d myapp.example.com

# 憑證自動更新（Let's Encrypt 憑證 90 天過期）
sudo certbot renew --dry-run

# 設定自動更新排程（每天檢查一次）
# sudo crontab -e
# 0 0 * * * certbot renew --quiet
```

---

## 監控與 Logging

### Serilog 結構化日誌

```csharp
// 安裝套件：
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.Console
// dotnet add package Serilog.Sinks.File

// Program.cs 設定 Serilog
using Serilog;

// 建立 Serilog Logger
Log.Logger = new LoggerConfiguration()
    // 最低日誌等級
    .MinimumLevel.Information()
    // 輸出到 Console（結構化格式）
    .WriteTo.Console()
    // 輸出到檔案（每天一個新檔案）
    .WriteTo.File(
        ""logs/myapp-.log"",
        rollingInterval: RollingInterval.Day,  // 每天產生新檔案
        retainedFileCountLimit: 30              // 保留 30 天
    )
    .CreateLogger();

// 在 ASP.NET Core 中使用 Serilog
builder.Host.UseSerilog();

// 在程式碼中記錄日誌
app.MapGet(""/api/users/{id}"", (int id, ILogger<Program> logger) =>
{
    // 結構化日誌：用 {} 包裹參數名稱
    logger.LogInformation(""查詢使用者 {UserId}"", id);

    try
    {
        // 處理邏輯...
        return Results.Ok();
    }
    catch (Exception ex)
    {
        // 記錄錯誤日誌（包含例外堆疊）
        logger.LogError(ex, ""查詢使用者 {UserId} 時發生錯誤"", id);
        return Results.StatusCode(500);
    }
});
```

### Health Endpoint（健康檢查端點）

```csharp
// Program.cs 設定完整的健康檢查
builder.Services.AddHealthChecks()
    // 檢查資料庫
    .AddSqlServer(
        builder.Configuration.GetConnectionString(""Default"")!,
        name: ""database"",
        timeout: TimeSpan.FromSeconds(5))
    // 自訂健康檢查
    .AddCheck(""disk_space"", () =>
    {
        // 檢查磁碟空間
        var drive = new DriveInfo(""C"");
        var freeSpacePercent = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;
        // 磁碟空間低於 10% 就回報不健康
        return freeSpacePercent > 10
            ? HealthCheckResult.Healthy($""磁碟空間：{freeSpacePercent:F1}%"")
            : HealthCheckResult.Unhealthy($""磁碟空間不足：{freeSpacePercent:F1}%"");
    });

// 設定健康檢查路由
app.MapHealthChecks(""/health"", new HealthCheckOptions
{
    // 自訂回應格式（JSON）
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = ""application/json"";
        // 組合所有檢查結果
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        // 序列化成 JSON 回傳
        await context.Response.WriteAsJsonAsync(result);
    }
});
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：把連線字串寫死在程式碼中

```csharp
// ❌ 錯誤：連線字串寫死在程式碼中
var connectionString = ""Server=myserver.database.windows.net;Database=mydb;User=admin;Password=P@ssw0rd123"";
// 推上 Git 就洩漏了！

// ✅ 正確：從環境變數或設定檔讀取
// 開發環境：User Secrets
// dotnet user-secrets set ""ConnectionStrings:Default"" ""Server=...""

// 正式環境：環境變數
// Railway/Azure 的 Dashboard 設定環境變數
var connectionString2 = builder.Configuration.GetConnectionString(""Default"");
```

### ❌ 錯誤 2：沒有健康檢查端點

```
❌ 問題：應用程式掛了但不知道

沒有健康檢查的後果：
├── 使用者反應「網站掛了」才知道有問題
├── 不知道是應用程式掛了還是資料庫掛了
├── Docker/K8s 無法自動重啟不健康的容器
└── 負載平衡器不知道要把流量導到哪裡

✅ 正確：設定 /health 端點
├── 檢查應用程式是否活著
├── 檢查資料庫連線是否正常
├── 檢查外部服務是否可用
├── Docker healthcheck 會定期呼叫
└── 監控系統可以監測並告警
```

### ❌ 錯誤 3：沒有設定 Logging

```csharp
// ❌ 錯誤：用 Console.WriteLine 當日誌
Console.WriteLine(""使用者登入了"");  // 沒有時間戳、等級、結構化
Console.WriteLine($""錯誤：{ex.Message}"");  // 沒有堆疊追蹤

// ✅ 正確：使用結構化日誌（Serilog）
// 有時間戳、日誌等級、結構化參數
logger.LogInformation(""使用者 {UserId} 於 {LoginTime} 登入"", userId, DateTime.UtcNow);
// 錯誤日誌包含完整的例外堆疊
logger.LogError(ex, ""使用者 {UserId} 登入失敗"", userId);

// Serilog 的好處：
// ├── 結構化日誌（可以搜尋、過濾）
// ├── 自動包含時間戳和日誌等級
// ├── 可以輸出到多個目標（Console、檔案、Seq、Elasticsearch）
// └── 效能優秀（非同步寫入）
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Railway | 簡單的雲端平台，適合學習和小型專案 |
| Azure App Service | 微軟的雲端應用程式代管服務 |
| Reverse Proxy | 反向代理（Nginx），轉發請求到後端 |
| SSL/TLS | Let's Encrypt 提供免費 SSL 憑證 |
| Serilog | .NET 的結構化日誌框架 |
| Health Check | 健康檢查端點，讓監控系統知道服務是否正常 |
| 環境變數 | 不同環境用不同的設定，不寫死在程式碼中 |
" },
    };
}
