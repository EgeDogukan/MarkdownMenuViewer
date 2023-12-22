namespace MarkdownMenuViewer.Server.Models
{
    public class DirectoryItem
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? ParentDir { get; set; }
        public string? Type { get; set; }
    }
}
