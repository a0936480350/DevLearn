using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ErrorLogController : ControllerBase
{
    private readonly AppDbContext _db;
    public ErrorLogController(AppDbContext db) => _db = db;

    // 前端/後端回報錯誤
    [HttpPost("report")]
    public async Task<IActionResult> Report([FromBody] ErrorReportReq req)
    {
        var log = new ErrorLog
        {
            PageUrl = req.PageUrl ?? "",
            ErrorMessage = req.ErrorMessage ?? "",
            StackTrace = req.StackTrace ?? "",
            Source = req.Source ?? "frontend",
            UserId = HttpContext.Session.GetString("SessionId") ?? "",
            UserAgent = Request.Headers.UserAgent.ToString(),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? ""
        };
        _db.ErrorLogs.Add(log);
        await _db.SaveChangesAsync();
        return Ok(new { success = true, id = log.Id });
    }

    // AI 讀取未解決的錯誤（供 Claude Code 排程用）
    [HttpGet("unresolved")]
    public async Task<IActionResult> GetUnresolved()
    {
        var errors = await _db.ErrorLogs
            .Where(e => !e.IsResolved)
            .OrderByDescending(e => e.CreatedAt)
            .Take(20)
            .Select(e => new { e.Id, e.PageUrl, e.ErrorMessage, e.StackTrace, e.Source, e.CreatedAt })
            .ToListAsync();
        return Ok(errors);
    }

    // AI 標記錯誤已解決
    [HttpPost("resolve")]
    public async Task<IActionResult> Resolve([FromBody] ResolveReq req)
    {
        var log = await _db.ErrorLogs.FindAsync(req.Id);
        if (log == null) return NotFound();
        log.IsResolved = true;
        log.ResolvedBy = req.ResolvedBy ?? "AI";
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // AI 記錄工作紀錄
    [HttpPost("ai-work")]
    public async Task<IActionResult> LogAIWork([FromBody] AIWorkReq req)
    {
        var work = new AIWorkLog
        {
            TaskType = req.TaskType ?? "BugFix",
            Description = req.Description ?? "",
            FilesModified = req.FilesModified ?? "",
            ErrorLogId = req.ErrorLogId ?? "",
            Status = req.Status ?? "completed",
            Result = req.Result ?? "",
            DurationSeconds = req.DurationSeconds,
            StartedAt = DateTime.Now.AddSeconds(-req.DurationSeconds),
            CompletedAt = DateTime.Now
        };
        _db.AIWorkLogs.Add(work);
        await _db.SaveChangesAsync();
        return Ok(new { success = true, id = work.Id });
    }

    // 取得 AI 工作紀錄
    [HttpGet("ai-work")]
    public async Task<IActionResult> GetAIWork()
    {
        var logs = await _db.AIWorkLogs
            .OrderByDescending(l => l.StartedAt)
            .Take(50)
            .ToListAsync();
        return Ok(logs);
    }
}

public record ErrorReportReq(string? PageUrl, string? ErrorMessage, string? StackTrace, string? Source);
public record ResolveReq(int Id, string? ResolvedBy);
public record AIWorkReq(string? TaskType, string? Description, string? FilesModified, string? ErrorLogId, string? Status, string? Result, int DurationSeconds);
