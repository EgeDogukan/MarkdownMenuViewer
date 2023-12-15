namespace MarkdownMenuViewer.Server.Models
{
    public class FileSystemObject
    {
        public bool IsFile { get; set; }
        public DirectoryItem Directory { get; set; }
        public MarkdownFile File { get; set; }
    }
}
