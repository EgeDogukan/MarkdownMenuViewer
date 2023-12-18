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

        public async Task<IEnumerable<FileSystemObject>> GetFileSystemObjectAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "path cannot be null or empty.");
            }

            var fileSystemObjects = new List<FileSystemObject>();

            if (File.Exists(path))
            {
                var content = await File.ReadAllTextAsync(path);
                fileSystemObjects.Add(new FileSystemObject
                {
                    IsFile = true,
                    File = new MarkdownFile
                    {
                        Name = Path.GetFileName(path),
                        Path = path,
                        Content = content
                    },
                    Directory = null
                });
            }
            else if (Directory.Exists(path))
            {
                var directoryInfo = new DirectoryInfo(path);
                var fileSystemInfos = directoryInfo.EnumerateFileSystemInfos();

                foreach (var info in fileSystemInfos)
                {
                    bool isDirectory = info is DirectoryInfo;

                    if (!isDirectory)
                    {
                        //a file read its content
                        var content = await File.ReadAllTextAsync(info.FullName);
                        fileSystemObjects.Add(new FileSystemObject
                        {
                            IsFile = true,
                            File = new MarkdownFile
                            {
                                Name = info.Name,
                                Path = info.FullName,
                                Content = content
                            },
                            Directory = null
                        });
                    }
                    else
                    {
                        //a directory add a FileSystemObject
                        fileSystemObjects.Add(new FileSystemObject
                        {
                            IsFile = false,
                            Directory = new DirectoryItem
                            {
                                Name = info.Name,
                                Path = info.FullName,
                                IsDirectory = isDirectory,
                                ParentDir = directoryInfo.Parent?.FullName,
                                Children = new List<DirectoryItem>() // Initialize, but don't populate children here
                            },
                            File = null
                        });
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("The specified path does not exist.");
            }
            return fileSystemObjects;
        }
    }
}
