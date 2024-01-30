using MarkdownMenuViewer.Server.Models;

namespace MarkdownMenuViewer.Server.Services
{
    public interface IAzureFileService
    {
        Task<IEnumerable<GeneralItemsAllRecursive>> GetAllRecursivesAsync(string path);
    }
}
