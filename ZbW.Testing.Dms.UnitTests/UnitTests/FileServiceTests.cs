using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.UnitTests
{
    [TestFixture]
    class FileServiceTests
    {
        private const string destPath = @"C:\temp\DMS";
        private const string sourcePath = @"C:\test\test.txt";
        const string newFilename = "newFilename";
        private const string directory = @"C:\temp";
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        [Test]
        public void CopyFile_Copy_IsSuccessful()
        {
            //arrange
            string savePath = destPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourcePath, new MockFileData("dummy text")}
            });
            var fileService = new FileService(savePath, destPath, fileSystem);
            //act
            fileService.CopyFile(sourcePath,newFilename,false);
            //assert
            Assert.That(fileSystem.File.Exists(sourcePath));
            Assert.That(fileSystem.File.Exists(savePath + @"\" + newFilename));
        }

        [Test]
        public void CopyFile_Move_IsSuccessful()
        {
            //arrange
            string savePath = destPath + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourcePath, new MockFileData("dummy text")}
            });
            var fileService = new FileService(savePath, destPath, fileSystem);
            //act
            fileService.CopyFile(sourcePath,newFilename,true);
            //assert
            Assert.That(fileSystem.File.Exists(sourcePath), Is.False);
            Assert.That(fileSystem.File.Exists(savePath + @"\" + newFilename));
        }

        [Test]
        public void FileExtension_IsSuccessful()
        {
            //arrange
            string savePath = destPath + @"\" + _currentYear;
            string expectedResult = ".xml";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {savePath + @"\test1.xml", new MockFileData("dummy text")}
            });
            var fileService = new FileService(savePath, destPath, fileSystem);
            //act
            var result = fileService.FileExtension(savePath + @"\test1.xml");
            //assert
            Assert.That(result.Equals(expectedResult));
        }

        [Test]
        public void CreateNewFolder_IsSuccessful()
        {
            //arrange
            string savePath = destPath + @"\" + _currentYear;
            const string newFilename = "test.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {sourcePath, new MockFileData("dummy text")}
            });
            var fileService = new FileService(savePath, destPath, fileSystem);
            //act
            fileService.CopyFile(sourcePath, newFilename, false);
            //assert
            Assert.That(fileSystem.Directory.Exists(savePath));
        }

        [Test]
        public void GetPaths_IsSuccessful()
        {
            //arrange
            string testPath = directory + @"\" + _currentYear;
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {testPath +@"\test.xml", new MockFileData("dummy text")}
            });
            var fileService = new FileService("",directory,fileSystem);
            string[] expectedResult = {testPath + @"\test.xml"};
            //act
            var result = fileService.getPaths();
            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }
    }
}
