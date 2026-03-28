using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using ClosedXML.Excel;

namespace DotNetLearning.Controllers;

public class ScoresController : Controller
{
    private readonly AppDbContext _db;
    public ScoresController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        var attempts = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId)
            .OrderByDescending(a => a.TakenAt)
            .ToListAsync();

        var chapterList = await _db.Chapters.Where(c => c.IsPublished).ToListAsync();
        var chapterTitles = chapterList.ToDictionary(c => c.Id, c => c.Title);
        var chapterSlugs = chapterList.ToDictionary(c => c.Id, c => c.Slug);

        var completed = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .CountAsync();

        ViewBag.ChapterTitles = chapterTitles;
        ViewBag.ChapterSlugs = chapterSlugs;
        ViewBag.TotalChapters = await _db.Chapters.CountAsync(c => c.IsPublished);
        ViewBag.CompletedChapters = completed;
        ViewBag.TotalAttempts = attempts.Count;
        ViewBag.AvgScore = attempts.Count > 0
            ? (int)attempts.Average(a => a.Total > 0 ? a.Score * 100.0 / a.Total : 0)
            : 0;

        return View(attempts);
    }

    public async Task<IActionResult> ExportExcel()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        var attempts = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId)
            .OrderByDescending(a => a.TakenAt)
            .ToListAsync();

        var chaptersAll = await _db.Chapters.ToListAsync();
        var chapters = chaptersAll.ToDictionary(c => c.Id, c => c.Title);
        var progresses = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .ToListAsync();

        using var workbook = new XLWorkbook();

        // ── 測驗成績工作表 ──────────────────────────────
        var quizSheet = workbook.Worksheets.Add("測驗成績");
        quizSheet.Cell(1, 1).Value = "章節名稱";
        quizSheet.Cell(1, 2).Value = "答對題數";
        quizSheet.Cell(1, 3).Value = "總題數";
        quizSheet.Cell(1, 4).Value = "得分 %";
        quizSheet.Cell(1, 5).Value = "測驗時間";

        // 標題列樣式
        var headerRange = quizSheet.Range(1, 1, 1, 5);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1A1A2E");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int i = 0; i < attempts.Count; i++)
        {
            var a = attempts[i];
            var row = i + 2;
            var pct = a.Total > 0 ? (int)(a.Score * 100.0 / a.Total) : 0;

            quizSheet.Cell(row, 1).Value = chapters.GetValueOrDefault(a.ChapterId, $"章節 {a.ChapterId}");
            quizSheet.Cell(row, 2).Value = a.Score;
            quizSheet.Cell(row, 3).Value = a.Total;
            quizSheet.Cell(row, 4).Value = pct;
            quizSheet.Cell(row, 5).Value = a.TakenAt.ToString("yyyy/MM/dd HH:mm");

            // 依分數上色
            var scoreCell = quizSheet.Cell(row, 4);
            scoreCell.Style.Fill.BackgroundColor = pct >= 80 ? XLColor.LightGreen
                : pct >= 60 ? XLColor.LightYellow
                : XLColor.FromHtml("#FFB3B3");
            scoreCell.Style.Font.Bold = true;
        }

        quizSheet.Columns().AdjustToContents();

        // ── 學習進度工作表 ──────────────────────────────
        var progressSheet = workbook.Worksheets.Add("學習進度");
        progressSheet.Cell(1, 1).Value = "章節名稱";
        progressSheet.Cell(1, 2).Value = "是否完成";
        progressSheet.Cell(1, 3).Value = "完成時間";

        var pHeaderRange = progressSheet.Range(1, 1, 1, 3);
        pHeaderRange.Style.Font.Bold = true;
        pHeaderRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#0D3349");
        pHeaderRange.Style.Font.FontColor = XLColor.White;

        var allChapters = await _db.Chapters
            .Where(c => c.IsPublished)
            .OrderBy(c => c.Order)
            .ToListAsync();

        for (int i = 0; i < allChapters.Count; i++)
        {
            var ch = allChapters[i];
            var progress = progresses.FirstOrDefault(p => p.ChapterId == ch.Id);
            var row = i + 2;

            progressSheet.Cell(row, 1).Value = ch.Title;
            progressSheet.Cell(row, 2).Value = progress?.IsCompleted == true ? "✅ 已完成" : "○ 未完成";
            progressSheet.Cell(row, 3).Value = progress?.IsCompleted == true
                ? progress.CompletedAt.ToString("yyyy/MM/dd HH:mm")
                : "-";

            if (progress?.IsCompleted == true)
                progressSheet.Cell(row, 2).Style.Font.FontColor = XLColor.Green;
        }

        progressSheet.Columns().AdjustToContents();

        // ── 匯出 ──────────────────────────────────────
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"DotNet學習紀錄_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
