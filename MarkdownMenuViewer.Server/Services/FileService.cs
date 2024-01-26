using MarkdownMenuViewer.Server.Models;
using System.IO;
using Markdig;

namespace MarkdownMenuViewer.Server.Services
{
    public class FileService : IFileService
    {
        private string _findType(FileSystemInfo info)
        {
            string type = info is DirectoryInfo ? "directory" : info.Extension.ToLowerInvariant();
            return type;
        }

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
                    Type = _findType(info)
                };

                if(directoryInfo.Parent != null && directoryInfo.Parent.Exists == true)   //checking whether the parent exists and if parent is null it means it is root
                {
                    directoryItem.ParentDir = directoryInfo.Parent.FullName;
                }
                return directoryItem;
            });
            return await Task.FromResult(directoryItems);
        }

        public async Task<MarkdownFile> GetMarkdownFileContentAsync(string path)
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
            var HTMLContent = Markdown.ToHtml(content);

            var markdownFile = new MarkdownFile
            {
                Content = HTMLContent,
                FileType = _findType(new FileInfo(path))
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
                string fileType = _findType(info);

                var fileSystemObject = new FileSystemObject
                {
                    File = !isDirectory ? new MarkdownFile
                    {
                        FileType = fileType,
                        Content = (fileType == ".txt" || fileType == ".md") ? Markdown.ToHtml(await File.ReadAllTextAsync(info.FullName)) : "not supported file type:" + fileType
                    } : null,
                    Directory = isDirectory ? new DirectoryItem
                    {
                        Name = info.Name,
                        Path = info.FullName,
                        ParentDir = directoryInfo.Parent?.FullName,
                        Type = fileType
                     } : null
                };
                fileSystemObjects.Add(fileSystemObject);
            }
            return await Task.FromResult(fileSystemObjects);
        }

        public async Task<IEnumerable<GeneralItemsAllRecursive>> GetAllRecursivesAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            }

            var items = new List<GeneralItemsAllRecursive>();
            await ProcessDirectoryAsync(path, items);
            return items;
        }

        private async Task ProcessDirectoryAsync(string path, List<GeneralItemsAllRecursive> items)
        {
            var info = new FileInfo(path);
            if (info.Attributes.HasFlag(FileAttributes.Directory))
            {
                var dir = new DirectoryInfo(path);
                var directoryItem = new GeneralItemsAllRecursive
                {
                    Type = "folder",
                    Name = dir.Name,
                    Content = null,
                    Data = new List<GeneralItemsAllRecursive>()
                };

                foreach (var child in dir.EnumerateFileSystemInfos())
                {
                    await ProcessDirectoryAsync(child.FullName, directoryItem.Data);
                }

                items.Add(directoryItem);
            }
            else
            {
                var fileItem = new GeneralItemsAllRecursive
                {
                    Type = "file",
                    Name = info.Name,
                    //Content = (info.Extension.ToLowerInvariant() == ".md" || /*info.Extension.ToLowerInvariant() == ".txt" UNCOMMENT THIS TO ENABLE TXT READING FEATURE*/ false) ? Markdown.ToHtml(await File.ReadAllTextAsync(path)) : "not supported file type: " + info.Extension
                    Content = GetContent(info, path)
                };

                items.Add(fileItem);
            }
        }

        private string GetContent(FileSystemInfo info, string path)
        {
            if(info.Extension.ToLowerInvariant() == ".md")
            {
                return Markdown.ToHtml(File.ReadAllText(path));
            }
            else if(info.Extension.ToLowerInvariant() == ".txt")
            {
                return System.Net.WebUtility.HtmlEncode(File.ReadAllText(path));
            }
            return "not supported file type: " + info.Extension;
        }
    }

}

