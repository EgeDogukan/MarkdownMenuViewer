using MarkdownMenuViewer.Server.Models;
using System.IO;

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
            var directoryItems = fileSystemInfos.Select(info =>
            {
                var directoryItem = new DirectoryItem
                {
                    Name = info.Name,
                    Path = info.FullName,
                    ParentDir = null,
                };

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
            
            return await Task.FromResult(markdownFile);
        }

        public async Task<IEnumerable<FileSystemObject>> GetFileSystemObjectAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "path cannot be null or empty.");
            }

            var fileSystemObjects = new List<FileSystemObject>();
            var directoryInfo = new DirectoryInfo(path);
            var fileSystemInfos = directoryInfo.EnumerateFileSystemInfos();

            foreach (var info in fileSystemInfos)
            {
                bool isDirectory = info is DirectoryInfo;
                string fileType = isDirectory ? "directory" : info.Extension.ToLowerInvariant();

                var fileSystemObject = new FileSystemObject
                {
                    Type = fileType, 
                    File = !isDirectory ? new MarkdownFile
                    {
                        Name = info.Name,
                        Path = info.FullName,
                        Content = (fileType == ".txt" || fileType == ".md") ? await File.ReadAllTextAsync(info.FullName) : "not supported file type:" + fileType
                    } : null,
                    Directory = isDirectory ? new DirectoryItem
                    {
                        Name = info.Name,
                        Path = info.FullName,
                        ParentDir = directoryInfo.Parent?.FullName,
                     } : null
                };
                fileSystemObjects.Add(fileSystemObject);
            }
            return fileSystemObjects;
        }
    }
}
