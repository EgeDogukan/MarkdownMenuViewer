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
                throw new DirectoryNotFoundException("The directory at " + path + " does not exist.");
            }

            //check whether the path is empty
            if(path == null)
            {
                throw new ArgumentNullException(path, "path is null");
            }

            var directoryInfo = new DirectoryInfo(path); //directoryinfo class has the abilites to get parent, enumerate content etc.
            var fileSystemInfos = directoryInfo.EnumerateFileSystemInfos();
            //var DirectoryItems = new List<DirectoryItem>();
            var directoryItems = fileSystemInfos.Select(info =>
            {
                var directoryItem = new DirectoryItem
                {
                    Name = info.Name,
                    Path = info.FullName,
                    IsDirectory = (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory,
                    ParentDir = null,
                    Children = null //initialize to null by default
                };

                if (info is DirectoryInfo)      //checking if the info is either a directory or a file
                {
                    directoryItem.Children = new List<DirectoryItem>();
                }

                if(directoryInfo.Parent != null && directoryInfo.Parent.Exists == true)   //checking whether the parent exists and if parent is null it means it is root
                {
                    directoryItem.ParentDir = directoryInfo.Parent.FullName;
                }

                return directoryItem;
            });
            return await Task.FromResult(directoryItems);
        }

        public async Task<MarkdownFile> GetMarkdownFileAsync(string path)
        {

            if(File.Exists(path) == false)
            {
                throw new FileNotFoundException("The file at " + path + " does not exist.");
            }

            if(path == null)
            {
                throw new ArgumentNullException(path, "path is null");
            } 

            var content = await File.ReadAllTextAsync(path);

            var markdownFile = new MarkdownFile
            {
                Name = Path.GetFileName(path),
                Path = path,
                Content = content
            };
            
            //var markdownFile = new MarkdownFile();

            return await Task.FromResult(markdownFile);
        }
    }
}
