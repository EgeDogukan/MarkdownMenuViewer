using MarkdownMenuViewer.Server.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkdownMenuViewer.Server.Services
{
    public class AzureFileService
    {

        private BlobServiceClient _blobServiceClient;
        private string _containerName;

        public AzureFileService(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        public async Task<IEnumerable<GeneralItemsAllRecursive>> GetAllRecursivesAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            }

            var items = new List<GeneralItemsAllRecursive>();
            await ProcessBlobDirectoryAsync(path, items);
            var sorted_items = Sort(items);
            return sorted_items;
        }

        private async Task ProcessBlobDirectoryAsync(string path, List<GeneralItemsAllRecursive> items)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var resultSegment = containerClient.GetBlobsByHierarchyAsync(prefix: path, delimiter: "/");

            await foreach (var blobItem in resultSegment)
            {
                if (blobItem.IsPrefix)
                {
                    // Process virtual directory
                    var directoryItem = new GeneralItemsAllRecursive
                    {
                        Type = "folder",
                        Name = blobItem.Prefix,
                        Content = null,
                        Data = new List<GeneralItemsAllRecursive>()
                    };
                    await ProcessBlobDirectoryAsync(blobItem.Prefix, directoryItem.Data);
                    items.Add(directoryItem);
                }
                else
                {
                    // Process blob
                    var blobClient = containerClient.GetBlobClient(blobItem.Blob.Name);
                    var fileItem = new GeneralItemsAllRecursive
                    {
                        Type = "file",
                        Name = blobItem.Blob.Name,
                        Content = await GetBlobContentAsync(blobClient)
                    };
                    items.Add(fileItem);
                }
            }
        }

        private async Task<string> GetBlobContentAsync(BlobClient blobClient)
        {
            var response = await blobClient.DownloadAsync();
            using var reader = new System.IO.StreamReader(response.Value.Content);
            return await reader.ReadToEndAsync();
        }

        private List<GeneralItemsAllRecursive> Sort(List<GeneralItemsAllRecursive> items)
        {
            items.Sort((x, y) =>
            {
                if (x.Type == y.Type) // If both are files or both are folders, sort alphabetically
                {
                    return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
                }
                else if (x.Type == "folder" && y.Type == "file") // If x is a folder and y is a file, x comes first
                {
                    return -1; // x comes first
                }
                else // If x is a file and y is a folder, y comes first
                {
                    return 1; // y comes first
                }
            });

            // Sort child Data recursively
            foreach (var item in items.Where(item => item.Type == "folder" && item.Data != null))
            {
                item.Data = Sort(item.Data);
            }
            return items;
        }
    }
}

