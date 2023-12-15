using MarkdownMenuViewer.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarkdownMenuViewer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSystemController : ControllerBase
    {

        private readonly IFileService fileService;

        public FileSystemController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet("directory")]
        public async Task<IActionResult> GetDirectoryContents(string path)
        {
            try
            {
                var contents = await fileService.GetDirectoryContentsAsync(path);
                return Ok(contents);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("file")]
        public async Task<IActionResult> GetMarkdownFile(string path)
        {
            try
            {
                var file = await fileService.GetMarkdownFileAsync(path);
                return Ok(file);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
