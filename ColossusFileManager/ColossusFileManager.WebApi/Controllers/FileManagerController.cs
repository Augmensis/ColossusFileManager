﻿using ColossusFileManager.Shared.Interfaces;
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
        private readonly IFileManagerService _fileManagerService;


        public FileManagerController(ILogger<FileManagerController> logger, IFileManagerService fileManagerService)
        {
            _logger = logger;

            _fileManagerService = fileManagerService;
        }


        [HttpPost("/CreateFolder")]
        public ApiResponse CreateFolder(string newFolderPath)
        {            
            throw new NotImplementedException();
        }


        [HttpGet("/CreateFile")]
        public ApiResponse CreateFile(string folderPath, string fileName)
        {
            throw new NotImplementedException();
        }


        [HttpGet("/ListStructure")]
        public ApiResponse<List<CbFolder>> ListStructure(string folderPath = null)
        {
            throw new NotImplementedException();
        }


        [HttpGet("/FindFile")]
        public ApiResponse<List<CbFile>> FindFile(string searchTerm, string folderPath = null)
        {
            throw new NotImplementedException();
        }
    }
}
