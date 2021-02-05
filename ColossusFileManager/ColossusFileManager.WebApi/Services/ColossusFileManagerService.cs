using ColossusFileManager.Shared.Interfaces;
using ColossusFileManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColossusFileManager.WebApi.Services
{
    public class ColossusFileManagerService : IFileManagerService
    {

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


        /// <summary>
        /// Lists ALL files and folders recursively as a list of CbFolder objects.
        /// (Optional) A folder path can be provided to select a folder to start from. If the folder doesn't exist, the result will be an empty list.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public async Task<List<CbFolder>> ListStructure(string folderPath = null)
        {
            throw new NotImplementedException();
        }
    }
}
