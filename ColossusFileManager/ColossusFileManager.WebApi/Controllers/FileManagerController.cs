using ColossusFileManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColossusFileManager.WebApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class FileManagerController : ControllerBase
    {

        private readonly ILogger<FileManagerController> _logger;


        public FileManagerController(ILogger<FileManagerController> logger)
        {
            _logger = logger;
        }


        [HttpPost("/CreateFolder")]
        public IActionResult CreateFolder(string newFolderPath)
        {
            throw new NotImplementedException();
        }


        [HttpGet("/CreateFile")]
        public IActionResult CreateFile(string folderPath, string fileName)
        {
            throw new NotImplementedException();
        }


        [HttpGet("/ListStructure")]
        public IEnumerable<CbFolder> ListStructure(string folderPath = null)
        {
            throw new NotImplementedException();
        }


        [HttpGet("/FindFile")]
        public IEnumerable<CbFile> FindFile(string searchTerm, string folderPath = null)
        {
            throw new NotImplementedException();
        }
    }
}
