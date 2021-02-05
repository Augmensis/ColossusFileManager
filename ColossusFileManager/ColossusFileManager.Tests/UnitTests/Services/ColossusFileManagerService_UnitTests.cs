using ColossusFileManager.Shared.Interfaces;
using ColossusFileManager.WebApi.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColossusFileManager.Tests.UnitTests.Services
{
    public class ColossusFileManagerService_UnitTests
    {

        private IFileManagerService _fileManagerService { get; set; }


        [SetUp]
        public void Setup()
        {
            _fileManagerService = new ColossusFileManagerService();
        }


        [Test]
        public void ColossusFileManagerService_CreateNewFile_When_Not_Exists_Result_Is_Empty()
        {
            Assert.Pass();
        }
    }
}
