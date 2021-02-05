using ColossusFileManager.Shared.Interfaces;
using ColossusFileManager.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColossusFileManager.WebApi.Services
{
    public class ColossusFileManagerService : IFileManagerService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ColossusFileManagerService> _logger;

        public ColossusFileManagerService(ILogger<ColossusFileManagerService> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new file in the given folderPath. If the folder doesn't exist, it will be created.
        /// If the file already exists, it will be overwritten.
        /// Any errors will be returned as a Key Value Pair.
        /// Operation is successful if the returned dictionary is empty or null.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="newfileName"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> CreateNewFile(string folderPath, string newfileName)
        {
            var errors = new Dictionary<string, string>();

            try
            {
                if (!folderPath.StartsWith("/"))
                    folderPath = $"/{folderPath}";

                // Check Folder Exists
                var folderExists = _dbContext.Folders.Any(x => x.FolderPath == folderPath);

                // Create folders if they don't exist
                if (!folderExists)
                {
                    errors = await CreateNewFolder(folderPath);
                }

                // Check for errors so far
                if (!errors.Any())
                {
                    // Check if file exists. Save if new, overwrite DateUpdated if exists
                    var fileExists = _dbContext.Folders.Any(x => x.FolderPath == folderPath && x.Files.Any(y => y.FileName == newfileName));

                    if (fileExists)
                    {
                        // Get the existing file and update its DateUpdated property
                        var existingFile = _dbContext.Folders.Single(x => x.FolderPath == folderPath).Files.Single(x => x.FileName == newfileName);

                        existingFile.Dateupdated = DateTime.UtcNow;

                        _dbContext.Files.Update(existingFile);

                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Save a new file to the existing folder that matches the given path
                        var existingFolder = _dbContext.Folders.Single(x => x.FolderPath == folderPath);

                        var newFile = new CbFile(newfileName);

                        existingFolder.Files.Add(newFile);

                        _dbContext.Folders.Update(existingFolder);

                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "CreateNewFile");
                errors.Add("Exception", ex.Message);
            }

            return errors;
        }


        /// <summary>
        /// Creates a new folder with the given folder path.
        /// If it already exists the operation will return an error.
        /// Any errors will be returned as a Key Value Pair.
        /// Operation is successful if the returned dictionary is empty or Null
        /// </summary>
        /// <param name="newFolderPath"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> CreateNewFolder(string newFolderPath)
        {
            var errors = new Dictionary<string, string>();

            try
            {
                // Check Folder Exists
                var folderExists = _dbContext.Folders.Any(x => x.FolderPath == newFolderPath || x.FolderName == newFolderPath);

                // Create folders if they don't exist
                if (folderExists)
                {
                    errors.Add("Exists", "Folder already exists");
                }

                // Check for errors so far
                if (!errors.Any())
                {
                    // Since a given filepath can have multiple levels, split and rebuild the folder structure, level by level
                    var folders = newFolderPath.Split("/");

                    var currentPath = "";
                    CbFolder parentFolder = null;

                    foreach (var currentFolderName in folders)
                    {
                        // Update the current path
                        currentPath += $"/{currentFolderName}";

                        // Check exists
                        folderExists = _dbContext.Folders.Any(x => x.FolderPath == currentPath);

                        CbFolder currentFolder = null;

                        // Build if it doesn't exist
                        if (!folderExists)
                        {
                            currentFolder = new CbFolder(currentFolderName, currentPath);

                            // If there is a parent folder, attach it to the child
                            if (parentFolder != null)
                                currentFolder.ParentFolder = parentFolder;

                            _dbContext.Folders.Add(currentFolder);

                            // Update the database
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            // Otherwise grab the existing matching folder to use as the parent
                            currentFolder = _dbContext.Folders.First(x => x.FolderPath == currentPath);
                        }

                        // Update the parent to the current folder
                        parentFolder = currentFolder;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "CreateNewFile");
                errors.Add("Exception", ex.Message);
            }

            return errors;
        }


        /// <summary>
        /// Searches for existing file names that start with the search term.
        /// (Optional) A folder path can be provided to narrow the search. If the folder doesn't exist, the result will be an empty list.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public async Task<List<CbFile>> FindFile(string searchTerm, string folderPath = null)
        {
            if(!string.IsNullOrEmpty(folderPath))
            {
                // Filter results by folderPath
                return _dbContext.Folders.First(x => x.FolderPath == folderPath).Files.Where(x => x.FileName.StartsWith(searchTerm)).ToList();
            }
                       
            // Otherwise just send back all files matching the search term
            return await _dbContext.Files.Where(x => x.FileName.StartsWith(searchTerm)).ToListAsync();
            
        }


        /// <summary>
        /// Lists ALL files and folders recursively as a list of CbFolder objects.
        /// (Optional) A folder path can be provided to select a folder to start from. If the folder doesn't exist, the result will be an empty list.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public async Task<List<CbFolder>> ListStructure(string folderPath = null)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                // Send back the whole list of folders from the top level (anything wihout a parent)
                // All parents should have complete child data appear as part of a cascade
                return await _dbContext.Folders.AsNoTracking().Where(x => x.ParentFolder == null)
                        .Include(x => x.Files)  
                        .Include(x => x.ChildFolders)
                            .ThenInclude(childFolder => childFolder.ChildFolders)
                            .ThenInclude(childFolder => childFolder.Files)
                        .ToListAsync();

                // BUG: Can't figure out EF recursive includes for cascading relationships past one level.
                // In real life I would raise the issue with the team to figure it out and then post on StackOverflow where appropriate
            }

            // Filter the folder list by the 
            return _dbContext.Folders.AsNoTracking().Where(x => x.FolderPath == folderPath).Include(x => x.ChildFolders).Include(x => x.Files).ToList();
        }


    }
}
