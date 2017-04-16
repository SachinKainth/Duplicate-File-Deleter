using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace DFD.UnitTests
{
    [TestFixture]
    class EmptyFileDeleterTests
    {
        private IEmptyFolderDeleter _emptyFolderDeleter;
        private Mock<IDirectoryWrapper> _directoryWrapperMock;
        private Mock<IPathWrapper> _pathWrapperMock;
        private Mock<IConsoleWrapper> _consoleWrapperMock;

        [SetUp]
        public void Setup()
        {
            _directoryWrapperMock = new Mock<IDirectoryWrapper>();
            _pathWrapperMock = new Mock<IPathWrapper>();
            _consoleWrapperMock = new Mock<IConsoleWrapper>();

            _emptyFolderDeleter = new EmptyFolderDeleter
                (_directoryWrapperMock.Object, _pathWrapperMock.Object, _consoleWrapperMock.Object);
        }

        [Test]
        public void Delete_WhenNoSubFolders_NothingGetsDeleted()
        {
            _directoryWrapperMock
                .Setup(_ => _.EnumerateDirectories("some path", "*.*", SearchOption.AllDirectories))
                .Returns(new List<string>());

            _emptyFolderDeleter.Delete("some path");

            _pathWrapperMock.Verify(_ => _.GetFullPath(It.IsAny<string>()), Times.Never);
            _directoryWrapperMock.Verify(_ => _.EnumerateFiles(
                It.IsAny<string>(), "*.*", SearchOption.TopDirectoryOnly), Times.Never);
            _directoryWrapperMock.Verify(_ => _.EnumerateDirectories(
                It.IsAny<string>(), "*.*", SearchOption.TopDirectoryOnly), Times.Never);
            _consoleWrapperMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never);
            _directoryWrapperMock.Verify(_ => _.Delete(It.IsAny<string>()), Times.Never);
        }

        [TestCase(null, "some file")]
        [TestCase("some folder", null)]
        [TestCase("some folder", "some file")]
        public void Delete_WhenSubFolderExistsButIsNotEmpty_NothingGetsDeleted(string folder, string file)
        {
            _directoryWrapperMock
                .Setup(_ => _.EnumerateDirectories("some path", "*.*", SearchOption.AllDirectories))
                .Returns(new[] { "some sub path" });
            _pathWrapperMock
                .Setup(_ => _.GetFullPath("some sub path"))
                .Returns("some sub path");
            _directoryWrapperMock
                .Setup(_ => _.EnumerateFiles("some sub path", "*.*", SearchOption.TopDirectoryOnly))
                .Returns(file == null? new string[] {} : new string[] { "some file" });
            _directoryWrapperMock
                .Setup(_ => _.EnumerateDirectories("some sub path", "*.*", SearchOption.TopDirectoryOnly))
                .Returns(folder == null? new string[] {} : new string[] { "some folder" });

            _emptyFolderDeleter.Delete("some path");
            
            _consoleWrapperMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never);
            _directoryWrapperMock.Verify(_ => _.Delete(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Delete_WhenSubFolderExistsAndIsEmpty_TheSubFolderGetsDeleted()
        {
            _directoryWrapperMock
                .Setup(_ => _.EnumerateDirectories("some path", "*.*", SearchOption.AllDirectories))
                .Returns(new[] { "some sub path" });
            _pathWrapperMock
                .Setup(_ => _.GetFullPath("some sub path"))
                .Returns("some sub path");
            _directoryWrapperMock
                .Setup(_ => _.EnumerateFiles("some sub path", "*.*", SearchOption.TopDirectoryOnly))
                .Returns(new string[] {});
            _directoryWrapperMock
               .Setup(_ => _.EnumerateDirectories("some sub path", "*.*", SearchOption.TopDirectoryOnly))
               .Returns(new string[] {});

            _emptyFolderDeleter.Delete("some path");

            _consoleWrapperMock.Verify(_ => _.WriteLine("Folder deleted: some sub path"), Times.Once);
            _directoryWrapperMock.Verify(_ => _.Delete("some sub path"), Times.Once);
        }
    }
}