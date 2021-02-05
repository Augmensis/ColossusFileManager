using ColossusFileManager.Shared.Interfaces;
using ColossusFileManager.Shared.Models;
using ColossusFileManager.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ColossusFileManager.Tests.UnitTests.Controllers
{
    public class FileManagerController_UnitTests
    {

        private FileManagerController _FileManagerController { get; set; }

        private Mock<IFileManagerService> _mockFileManagerService { get; set; }
        private Mock<ILogger<FileManagerController>> _mockLogger { get; set; }




        [SetUp]
        public void Setup()
        {
            _mockFileManagerService = new Mock<IFileManagerService>();
            _mockLogger = new Mock<ILogger<FileManagerController>>();

            _mockFileManagerService.Setup(x => x.CreateNewFile("folderexists", "fileDoesntExist")).Returns(Task.FromResult(new Dictionary<string, string>()));
            _mockFileManagerService.Setup(x => x.CreateNewFile("folderDoesntExist", "fileDoesntExist")).Returns(Task.FromResult(new Dictionary<string, string>()));
            _mockFileManagerService.Setup(x => x.CreateNewFile("folderexists", "fileExists")).Returns(Task.FromResult(new Dictionary<string, string>()));
            _mockFileManagerService.Setup(x => x.CreateNewFile("error", "error")).Returns(Task.FromResult(new Dictionary<string, string> { {"Error", "Error" } }));
            _mockFileManagerService.Setup(x => x.CreateNewFile("exception", "exception")).Returns(Task.FromResult(new Dictionary<string, string> { { "Exception", "A handled error" } }));

            _mockFileManagerService.Setup(x => x.CreateNewFolder("notexists")).Returns(Task.FromResult(new Dictionary<string, string>()));
            _mockFileManagerService.Setup(x => x.CreateNewFolder("exists")).Returns(Task.FromResult(new Dictionary<string, string> { { "Error", "Folder already exists"} }));
            _mockFileManagerService.Setup(x => x.CreateNewFolder("exception")).Returns(Task.FromResult(new Dictionary<string, string> { { "Exception", "A handled error"} }));
            
            
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsOnce", null)).Returns(Task.FromResult(new List<CbFile> { new CbFile { Id = 1, DateCreated = new DateTime(2020, 2, 5), FileName = "File 1.txt", FileExtension = "txt" } }));
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsOnce", "pathExists")).Returns(Task.FromResult(new List<CbFile> { new CbFile { Id = 1, DateCreated = new DateTime(2020, 2, 5), FileName = "File 1.txt", FileExtension = "txt" } }));
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsOnce", "pathDoesntExist")).Returns(Task.FromResult(new List<CbFile>()));
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsMultiple", null)).Returns(Task.FromResult(new List<CbFile> { new CbFile { Id = 1, DateCreated = new DateTime(2020, 2, 5), FileName = "File 1.txt", FileExtension = "txt" }, new CbFile { Id = 2, DateCreated = new DateTime(2020, 2, 4), FileName = "File 2.png", FileExtension = "png" }, new CbFile { Id = 3, DateCreated = new DateTime(2020, 2, 1), FileName = "File 3.txt", FileExtension = "txt" } }));
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsMultiple", "pathExists")).Returns(Task.FromResult(new List<CbFile> { new CbFile { Id = 1, DateCreated = new DateTime(2020, 2, 5), FileName = "File 1.txt", FileExtension = "txt" }, new CbFile { Id = 2, DateCreated = new DateTime(2020, 2, 4), FileName = "File 2.png", FileExtension = "png" }, new CbFile { Id = 3, DateCreated = new DateTime(2020, 2, 1), FileName = "File 3.txt", FileExtension = "txt" } }));
            _mockFileManagerService.Setup(x => x.FindFile("fileExistsMultiple", "pathDoesntExist")).Returns(Task.FromResult(new List<CbFile>()));
           
            _mockFileManagerService.Setup(x => x.ListStructure(null)).Returns(Task.FromResult(new List<CbFolder> { 
                                                                                                new CbFolder { Id = 1,
                                                                                                                FolderName = "",
                                                                                                                FolderPath = "/",
                                                                                                                ChildFolders = new List<CbFolder> {
                                                                                                                    new CbFolder { Id = 2,
                                                                                                                        FolderName = "Foo",
                                                                                                                        FolderPath = "/Foo",
                                                                                                                        ParentFolderId = 1,
                                                                                                                        Files = new List<CbFile> {
                                                                                                                            new CbFile { FileName = "Mock File 4" },
                                                                                                                            new CbFile { FileName = "Mock File 5" },
                                                                                                                            new CbFile { FileName = "Mock File 6" },
                                                                                                                        }
                                                                                                                    },
                                                                                                                    new CbFolder { Id = 3,
                                                                                                                        FolderName = "Bar",
                                                                                                                        FolderPath = "/Bar",
                                                                                                                        ParentFolderId = 1
                                                                                                                    },

                                                                                                                },
                                                                                                                Files = new List<CbFile> {
                                                                                                                    new CbFile { FileName = "Mock File 1" },
                                                                                                                    new CbFile { FileName = "Mock File 2" },
                                                                                                                    new CbFile { FileName = "Mock File 3" },
                                                                                                                }
                                                                                                }}));

            _mockFileManagerService.Setup(x => x.ListStructure("/Foo")).Returns(Task.FromResult(new List<CbFolder> {                                                                                                
                                                                                                        new CbFolder { Id = 2,
                                                                                                            FolderName = "Foo",
                                                                                                            FolderPath = "/Foo",
                                                                                                            ParentFolderId = 1,
                                                                                                            Files = new List<CbFile> {
                                                                                                                new CbFile { FileName = "Mock File 4" },
                                                                                                                new CbFile { FileName = "Mock File 5" },
                                                                                                                new CbFile { FileName = "Mock File 6" },
                                                                                                            }
                                                                                                        }                                                                                                                    
                                                                                                }));

            _mockFileManagerService.Setup(x => x.ListStructure("/Bar")).Returns(Task.FromResult(new List<CbFolder> {
                                                                                                        new CbFolder { Id = 3,
                                                                                                                        FolderName = "Bar",
                                                                                                                        FolderPath = "/Bar",
                                                                                                                        ParentFolderId = 1
                                                                                                                    }
                                                                                                }));


            _mockFileManagerService.Setup(x => x.ListStructure("/DoesntExist")).Returns(Task.FromResult(new List<CbFolder>()));


            _FileManagerController = new FileManagerController(_mockLogger.Object, _mockFileManagerService.Object);
        }

        #region CreateFolder

        [Test]
        public async void CreateFolder_When_FolderNotExists_Returns_NotNull()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.NotNull(response);
        }

        [Test]
        public async void CreateFolder_When_FolderNotExists_Returns_OK()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async void CreateFolder_When_FolderNotExists_Returns_NoErrors()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.IsEmpty(response.Errors);
        }


        [Test]
        public async void CreateFolder_When_FolderExists_Returns_Errors()
        {
            var response = await _FileManagerController.CreateFolder("exists");

            Assert.IsNotEmpty(response.Errors);
        }


        [Test]
        public async void CreateFolder_When_FolderNotExists_Returns_Conflict()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Conflict);
        }


        #endregion CreateFolder


        #region CreateFile

        [Test]
        public async void CreateFile_When_FileNotExists_Returns_NotNull()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.NotNull(response);
        }

        [Test]
        public async void CreateFile_When_FileNotExists_Returns_OK()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async void CreateFile_When_FileNotExists_Returns_NoErrors()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.IsEmpty(response.Errors);
        }


        [Test]
        public async void CreateFile_When_FileExists_Returns_NotNull()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.NotNull(response);
        }

        [Test]
        public async void CreateFile_When_FileExists_Returns_OK()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async void CreateFile_When_FileExists_Returns_NoErrors()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.IsEmpty(response.Errors);
        }


        [Test]
        public async void CreateFile_When_HasErrors_Returns_Errors()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.IsNotEmpty(response.Errors);
        }


        [Test]
        public async void CreateFile_When_HasException_Returns_Errors()
        {
            var response = await _FileManagerController.CreateFolder("notexists");

            Assert.IsNotEmpty(response.Errors);
        }

        #endregion CreateFile


        #region ListStructure

        [Test]
        public async void ListStructure_When_Folder_Is_NULL_Returns_AllFolders()
        {
            var response = await _FileManagerController.ListStructure(null);

            Assert.IsNotEmpty(response.Data);
        }

        [Test]
        public async void ListStructure_When_Folder_Is_NULL_Returns_OK()
        {
            var response = await _FileManagerController.ListStructure(null);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async void ListStructure_When_Folder_Is_RootPath_Returns_AllFolders()
        {
            var response = await _FileManagerController.ListStructure("/");

            Assert.IsNotEmpty(response.Data);
        }


        [Test]
        public async void ListStructure_When_Folder_Is_RootPath_Returns_OK()
        {
            var response = await _FileManagerController.ListStructure(null);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }


        [TestCase("/Foo", 3)]
        [TestCase("/Bar", 0)]
        public async void ListStructure_When_Folder_Exists_Returns_FilteredFolders(string folder, int fileCount)
        {
            var response = await _FileManagerController.ListStructure(folder);

            Assert.AreEqual(response.Data.Sum(x => x.Files.Count), fileCount);
        }


        [TestCase("/Foo")]
        [TestCase("/Bar")]
        public async void ListStructure_When_Folder_Exists_RootPath_Returns_OK(string folder)
        {
            var response = await _FileManagerController.ListStructure(folder);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }


        [Test]
        public async void ListStructure_When_Folder_DoesntExist_Returns_EmptyList()
        {
            var response = await _FileManagerController.ListStructure("/DoesntExist");

            Assert.IsEmpty(response.Data);
        }


        [Test]
        public async void ListStructure_When_Folder_DoesntExist_RootPath_Returns_NotFound()
        {
            var response = await _FileManagerController.ListStructure("/DoesntExist");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
        }

        #endregion ListStructure


        #region FindFile

        [Test]
        public async void FindFile_When_SearchTerm_Is_NULL_And_FolderPath_Is_Null_Returns_BadRequest()
        {
            var response = await _FileManagerController.FindFile(null, null);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async void FindFile_When_SearchTerm_Is_NULL_And_FolderPath_Is_Null_Returns_Errors()
        {
            var response = await _FileManagerController.FindFile(null, null);

            Assert.IsNotEmpty(response.Errors);
        }

        [Test]
        public async void FindFile_When_SearchTerm_Is_NULL_And_FolderPath_Is_Null_Returns_NoData()
        {
            var response = await _FileManagerController.FindFile(null, null);

            Assert.IsEmpty(response.Data);
        }


        [Test]
        public async void FindFile_When_SearchTerm_Exists_Once_And_FolderPath_Is_Null_Returns_One()
        {
            var response = await _FileManagerController.FindFile("fileExistsOnce", null);

            Assert.AreEqual(response.Data.Count(), 1);
        }


        [Test]
        public async void FindFile_When_SearchTerm_Exists_Multiple_And_FolderPath_Is_Null_Returns_Many()
        {
            var response = await _FileManagerController.FindFile("fileExistsMultiple", null);

            Assert.Greater(response.Data.Count(), 1);
        }


        [Test]
        public async void FindFile_When_SearchTerm_Exists_Once_And_FolderPath_Exists_Returns_One()
        {
            var response = await _FileManagerController.FindFile("fileExistsOnce", "pathExists");

            Assert.AreEqual(response.Data.Count(), 1);
        }

        [Test]
        public async void FindFile_When_SearchTerm_Exists_Multiple_And_FolderPath_Exists_Returns_Many()
        {
            var response = await _FileManagerController.FindFile("fileExistsMultiple", "pathExists");

            Assert.Greater(response.Data.Count(), 1);
        }


        [Test]
        public async void FindFile_When_SearchTerm_Exists_Once_And_FolderPath_DoesNotExist_Returns_None()
        {
            var response = await _FileManagerController.FindFile("fileExistsOnce", "pathDoesntExist");

            Assert.AreEqual(response.Data.Count(), 0);
        }

        [Test]
        public async void FindFile_When_SearchTerm_Exists_Multiple_And_FolderPath_DoesNotExist_Returns_None()
        {
            var response = await _FileManagerController.FindFile("fileExistsMultiple", "pathDoesntExist");

            Assert.AreEqual(response.Data.Count(), 0);
        }

        #endregion


    }
}
