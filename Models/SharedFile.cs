namespace DotNetLearning.Models;

public class SharedFile
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string FileName { get; set; } = "";        // 原始檔名
    public string StoragePath { get; set; } = "";     // wwwroot 下的相對路徑
    public long FileSize { get; set; }                 // bytes
    public string MimeType { get; set; } = "";
    public string Category { get; set; } = "general"; // general / tutorial / notes / slides
    public string UploadedBy { get; set; } = "admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int DownloadCount { get; set; } = 0;
    public bool IsPublic { get; set; } = true;
    public string? Tags { get; set; }                  // 逗號分隔的標籤
}
