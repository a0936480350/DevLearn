using Microsoft.AspNetCore.Mvc;
using DotNetLearning.Data;
using DotNetLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Controllers;

public class ClaudeTaskController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    public ClaudeTaskController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // GET /claude-task  → phone UI
    [HttpGet("/claude-task")]
    public IActionResult Index()
    {
        var pwd = HttpContext.Session.GetString("ClaudeAuth");
        ViewBag.Authed = pwd == "ok";
        return View();
    }

    // POST /claude-task/login
    [HttpPost("/claude-task/login")]
    public IActionResult Login(string password)
    {
        var correct = _config["ClaudeTaskPassword"] ?? "devlearn2026";
        if (password == correct)
            HttpContext.Session.SetString("ClaudeAuth", "ok");
        return Redirect("/claude-task");
    }

    // POST /claude-task/submit
    [HttpPost("/claude-task/submit")]
    public async Task<IActionResult> Submit(string prompt)
    {
        if (HttpContext.Session.GetString("ClaudeAuth") != "ok")
            return Unauthorized();
        if (string.IsNullOrWhiteSpace(prompt))
            return BadRequest("Prompt required");

        var task = new ClaudeTask { Prompt = prompt.Trim() };
        _db.ClaudeTasks.Add(task);
        await _db.SaveChangesAsync();
        return Ok(new { id = task.Id, message = "任務已排入佇列！" });
    }

    // GET /claude-task/status → JSON list of recent tasks
    [HttpGet("/claude-task/status")]
    public async Task<IActionResult> Status()
    {
        if (HttpContext.Session.GetString("ClaudeAuth") != "ok")
            return Unauthorized();
        var tasks = await _db.ClaudeTasks
            .OrderByDescending(t => t.CreatedAt)
            .Take(10)
            .Select(t => new { t.Id, t.Prompt, t.Status, t.Result, t.CreatedAt, t.CompletedAt })
            .ToListAsync();
        return Ok(tasks);
    }

    // GET /api/claude-task/next  → Claude Code cron polls this
    [HttpGet("/api/claude-task/next")]
    public async Task<IActionResult> Next([FromQuery] string token)
    {
        var correct = _config["ClaudeTaskToken"] ?? "claude-cron-token-2026";
        if (token != correct) return Unauthorized();

        var task = await _db.ClaudeTasks
            .Where(t => t.Status == "pending")
            .OrderBy(t => t.CreatedAt)
            .FirstOrDefaultAsync();

        if (task == null) return Ok(new { found = false });

        task.Status = "running";
        task.StartedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new { found = true, id = task.Id, prompt = task.Prompt });
    }

    // POST /api/claude-task/complete  → Claude Code reports result
    [HttpPost("/api/claude-task/complete")]
    public async Task<IActionResult> Complete([FromQuery] string token, [FromBody] TaskCompleteReq req)
    {
        var correct = _config["ClaudeTaskToken"] ?? "claude-cron-token-2026";
        if (token != correct) return Unauthorized();

        var task = await _db.ClaudeTasks.FindAsync(req.Id);
        if (task == null) return NotFound();

        task.Status = req.Success ? "done" : "failed";
        task.Result = req.Result ?? "";
        task.CompletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok();
    }
}

public record TaskCompleteReq(int Id, bool Success, string? Result);
