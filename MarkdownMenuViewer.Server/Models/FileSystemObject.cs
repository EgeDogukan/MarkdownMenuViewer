namespace MarkdownMenuViewer.Server.Models
{
    public class FileSystemObject
    {
        public string? Type { get; set; }
        public DirectoryItem? Directory { get; set; }
        public MarkdownFile? File { get; set; }
    }
}
