using DotNetLearning.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNetLearning.Services;

public class ErrorScannerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public ErrorScannerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[ErrorScanner] Service started. Scanning every 6 hours.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ScanErrors();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ErrorScanner] Error: {ex.Message}");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ScanErrors()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var unresolved = await db.ErrorLogs
            .Where(e => !e.IsResolved)
            .OrderByDescending(e => e.CreatedAt)
            .Take(100)
            .ToListAsync();

        if (!unresolved.Any())
        {
            Console.WriteLine($"[ErrorScanner] {DateTime.Now:yyyy-MM-dd HH:mm} — No unresolved errors.");
            return;
        }

        // Categorize errors
        var categories = new Dictionary<string, int>();
        foreach (var err in unresolved)
        {
            var msg = err.ErrorMessage ?? "";
            string category;
            if (msg.Contains("404") || msg.Contains("Not Found")) category = "404 Not Found";
            else if (msg.Contains("500") || msg.Contains("Internal Server")) category = "500 Server Error";
            else if (msg.Contains("timeout") || msg.Contains("Timeout")) category = "Timeout";
            else if (msg.Contains("null") || msg.Contains("NullReference")) category = "Null Reference";
            else if (msg.Contains("connection") || msg.Contains("Connection")) category = "Connection Error";
            else if (msg.Contains("CORS") || msg.Contains("cors")) category = "CORS Error";
            else if (msg.Contains("Script error") || msg.Contains("ResizeObserver")) category = "Browser/Client Error";
            else category = "Other";

            categories[category] = categories.GetValueOrDefault(category, 0) + 1;
        }

        // Log summary
        Console.WriteLine($"[ErrorScanner] {DateTime.Now:yyyy-MM-dd HH:mm} — Found {unresolved.Count} unresolved errors:");
        foreach (var kv in categories.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"  [{kv.Key}]: {kv.Value} errors");
        }

        // Auto-resolve known non-issues (browser errors, ResizeObserver, etc.)
        var autoResolvable = unresolved.Where(e =>
        {
            var msg = (e.ErrorMessage ?? "") + (e.StackTrace ?? "");
            return msg.Contains("ResizeObserver") ||
                   msg.Contains("Script error.") ||
                   msg.Contains("net::ERR_") ||
                   msg.Contains("Failed to fetch") ||
                   msg.Contains("Load failed") ||
                   msg.Contains("ChunkLoadError") ||
                   msg.Contains("cache") ||
                   (e.Source == "frontend" && msg.Contains("undefined"));
        }).ToList();

        if (autoResolvable.Any())
        {
            foreach (var err in autoResolvable)
            {
                err.IsResolved = true;
                err.ResolvedBy = "AutoScanner";
            }
            await db.SaveChangesAsync();
            Console.WriteLine($"[ErrorScanner] Auto-resolved {autoResolvable.Count} browser/network errors.");
        }

        // Write scan result to AIWorkLogs
        db.AIWorkLogs.Add(new DotNetLearning.Models.AIWorkLog
        {
            TaskType = "ErrorScan",
            Description = $"Scanned {unresolved.Count} unresolved errors. Auto-resolved {autoResolvable.Count} non-issues. Categories: {string.Join(", ", categories.Select(kv => $"{kv.Key}({kv.Value})"))}",
            FilesModified = "",
            Status = "done",
            Result = $"Total: {unresolved.Count}, Auto-resolved: {autoResolvable.Count}, Remaining: {unresolved.Count - autoResolvable.Count}",
            DurationSeconds = 0,
            StartedAt = DateTime.Now,
            CompletedAt = DateTime.Now
        });
        await db.SaveChangesAsync();
    }
}
