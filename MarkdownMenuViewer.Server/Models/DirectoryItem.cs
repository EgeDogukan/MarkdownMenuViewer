namespace MarkdownMenuViewer.Server.Models
{
    public class DirectoryItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }
        public List<DirectoryItem> Children { get; set; }
        public string ParentDir { get; set; }
    }
}
