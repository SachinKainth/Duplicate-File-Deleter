using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DFD.UnitTests
{
    [TestFixture]
    class DuplicateFileDeleterTests
    {
        private IDuplicateFileDeleter _duplicateFileDeleter;
        private Mock<IFileWrapper> _fileWrapperMock;


        [SetUp]
        public void Setup()
        {
            _fileWrapperMock = new Mock<IFileWrapper>();
            _duplicateFileDeleter = new DuplicateFileDeleter(_fileWrapperMock.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Delete1Arg_WhenNullInput_DoesNotDeleteAnyFiles(bool nullObject)
        {
            _duplicateFileDeleter.Delete(nullObject ? null : new Hashes { });

            _fileWrapperMock.Verify(_ => _.Delete(It.IsAny<string>()), Times.Never());
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Delete2Args_WhenNullInput_DoesNotDeleteAnyFiles(bool nullObject1, bool nullObject2)
        {
            _duplicateFileDeleter.Delete(
                nullObject1 ? null : new Hashes { }, nullObject2 ? null : new Hashes { });

            _fileWrapperMock.Verify(_ => _.Delete(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void Delete1Arg_WhenHashesSupplied_DeletesFiles()
        {
            var hashes = new Dictionary<byte[], List<string>>();
            hashes.Add(new byte[] { 1 }, new List<string>() {
                "C:\\path\file1.txt", "C:\\path\file2.txt" });
            hashes.Add(new byte[] { 2 }, new List<string>() {
                "C:\\path\file3.jpg", "C:\\path\file4.jpg", "C:\\path\file5.jpg" });
            hashes.Add(new byte[] { 3 }, new List<string>() { "C:\\path\file6.jpg" });

            _duplicateFileDeleter.Delete(new Hashes { HashesAndFiles = hashes });

            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file2.txt"), Times.Once());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file4.jpg"), Times.Once());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file5.jpg"), Times.Once());

            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file1.txt"), Times.Never());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file3.jpg"), Times.Never());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path\file6.jpg"), Times.Never()); 
        }

        [Test]
        public void Delete2Args_WhenHashesSupplied_DeletesFiles()
        {
            var hashes1 = new Dictionary<byte[], List<string>>();
            hashes1.Add(new byte[] { 1 }, new List<string>() { "C:\\path 1\file1.txt" });
            hashes1.Add(new byte[] { 2 }, new List<string>() { "C:\\path 1\file2.jpg", });
            hashes1.Add(new byte[] { 3 }, new List<string>() { "C:\\path 1\file3.dll", });

            var hashes2 = new Dictionary<byte[], List<string>>();
            hashes2.Add(new byte[] { 1 }, new List<string>() { "C:\\path 2\file4.txt" });
            hashes2.Add(new byte[] { 3 }, new List<string>() { "C:\\path 2\file5.dll", });
            hashes2.Add(new byte[] { 4 }, new List<string>() { "C:\\path 2\file6.exe", });

            _duplicateFileDeleter.Delete(
                new Hashes { HashesAndFiles = hashes1 },
                new Hashes { HashesAndFiles = hashes2 });

            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 2\file4.txt"), Times.Once());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 2\file5.dll"), Times.Once());

            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 1\file1.txt"), Times.Never());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 1\file2.jpg"), Times.Never());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 1\file3.dll"), Times.Never());
            _fileWrapperMock.Verify(_ => _.Delete("C:\\path 2\file6.exe"), Times.Never());
        }
    }
}