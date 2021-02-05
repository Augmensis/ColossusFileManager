using ColossusFileManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ColossusFileManager.Shared.Interfaces
{
    public interface IFileManagerService
    {
        Task<Dictionary<string, string>> CreateNewFolder(string newFolderPath);

        Task<Dictionary<string, string>> CreateNewFile(string folderPath, string newfileName);

        Task<List<CbFolder>> ListStructure(string folderPath = null);

        Task<List<CbFile>> FindFile(string searchTerm, string folderPath = null);

    }
}
