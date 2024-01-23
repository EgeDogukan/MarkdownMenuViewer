using MarkdownMenuViewer.Server.Models;

namespace MarkdownMenuViewer.Server.Services
{
    public interface IFileService
    {
        Task<IEnumerable<DirectoryItem>> GetDirectoryContentsAsync(string path);
        Task<MarkdownFile> GetMarkdownFileContentAsync(string path);
        Task<IEnumerable<FileSystemObject>> GetFileSystemObjectAsync(string path);
        Task<IEnumerable<GeneralItemsAllRecursive>> GetAllRecursivesAsync(string path);
    }
}
