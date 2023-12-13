using MarkdownMenuViewer.Server.Models;

namespace MarkdownMenuViewer.Server.Services
{
    public class FileService : IFileService
    {
        public async Task<IEnumerable<DirectoryItem>> GetDirectoryContentsAsync(string path)
        {
            //check whether the directory in path variable exists
            if(Directory.Exists(path) == false)
            {
                throw new DirectoryNotFoundException("The directory at " + path + "does not exist.");
            }

            //check whether the path is empty
            if(path == null)
            {
                throw new ArgumentNullException(path);
            }


            var DirectoryItems = new List<DirectoryItem>();

            return await Task.FromResult(DirectoryItems);
        }

        public async Task<MarkdownFile> GetMarkdownFileAsync(string path)
        {


            var MarkdownFile = new MarkdownFile();

            return await Task.FromResult(MarkdownFile);
        }

    }
}
