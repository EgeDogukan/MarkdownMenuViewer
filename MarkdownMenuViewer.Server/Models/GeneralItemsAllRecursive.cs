using System;
namespace MarkdownMenuViewer.Server.Models
{
	public class GeneralItemsAllRecursive
	{
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public List<GeneralItemsAllRecursive>? Data { get; set; }
    }
}

