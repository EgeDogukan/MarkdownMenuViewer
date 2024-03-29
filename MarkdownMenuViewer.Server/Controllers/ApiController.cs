﻿using MarkdownMenuViewer.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarkdownMenuViewer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSystemController : ControllerBase
    {

        private readonly IFileService _fileService;

        public FileSystemController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpGet("directory")]
        public async Task<IActionResult> GetDirectoryContents(string path)
        {
            try
            {
                var contents = await _fileService.GetDirectoryContentsAsync(path);
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
                var file = await _fileService.GetMarkdownFileContentAsync(path);
                return Ok(file);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetFileSystemObject(string path)
        {
            try
            {
                var fileSystemObjects = await _fileService.GetFileSystemObjectAsync(path);
                return Ok(fileSystemObjects);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllPath")]
        public async Task<IActionResult> GetItemsAllRecursive(string path)
        {
            try
            {
                var allItems = await _fileService.GetAllRecursivesAsync(path);
                return Ok(allItems);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAllString")]
        public async Task<IActionResult> GetItemsAllStringRecursive(string name)
        {
            //TODO: add a function which takes a name then gives the saved path in the server.
            try
            {
                var allItems = await _fileService.GetAllRecursivesAsync(name);
                return Ok(allItems);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
