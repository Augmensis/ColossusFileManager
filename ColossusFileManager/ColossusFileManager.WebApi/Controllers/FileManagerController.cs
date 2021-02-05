using ColossusFileManager.Shared.Interfaces;
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
        public async Task<ApiResponse> CreateFolder(string newFolderPath)
        {
            var response = new ApiResponse();

            try
            {

                var errors = await _fileManagerService.CreateNewFolder(newFolderPath);

                if (errors.Any())
                {
                    response.Errors = errors;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }

            } 
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "CreateFolder");
                response.Errors.Add("Exception", ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }


        [HttpGet("/CreateFile")]
        public async Task<ApiResponse> CreateFile(string folderPath, string fileName)
        {
            var response = new ApiResponse();

            try
            {
                var errors = await _fileManagerService.CreateNewFile(folderPath, fileName);

                if (errors.Any())
                {
                    response.Errors = errors;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "CreateFile");
                response.Errors.Add("Exception", ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }


        [HttpGet("/ListStructure")]
        public async Task<ApiResponse<List<CbFolder>>> ListStructure(string folderPath = null)
        {
            var response = new ApiResponse<List<CbFolder>>();

            try
            {
                var result = await _fileManagerService.ListStructure(folderPath);

                if (result != null)
                    response.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "ListStructure");
                response.Errors.Add("Exception", ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }


        [HttpGet("/FindFile")]
        public async Task<ApiResponse<List<CbFile>>> FindFile(string searchTerm, string folderPath = null)
        {
            var response = new ApiResponse<List<CbFile>>();

            try
            {
                var result = await _fileManagerService.FindFile(searchTerm, folderPath);

                if (result != null)
                    response.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "FindFile");
                response.Errors.Add("Exception", ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
