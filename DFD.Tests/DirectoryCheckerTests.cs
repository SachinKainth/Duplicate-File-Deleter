using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smocks;
using System;

namespace DFD.UnitTests
{
    [TestFixture]
    class DirectoryCheckerTests
    {
        private Mock<IDirectoryWrapper> _directoryWrapperMock;
        private IDirectoryChecker _directoryChecker;

        [SetUp]
        public void Setup()
        {
            _directoryWrapperMock = new Mock<IDirectoryWrapper>();
            _directoryChecker = new DirectoryChecker(_directoryWrapperMock.Object);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Check_ArgsLengthNot2_ThrowsException(int argsLength)
        {
            var args = new string[argsLength];
            
            Action action = () => _directoryChecker.Check(args);
            action
                .ShouldThrow<ArgumentException>()
                .WithMessage("Usage: DFD.exe <folder 1> <folder 2>");
        }

        [TestCase(null, null, false, false)]
        [TestCase(null, "", false, false)]
        [TestCase("", null, false, false)]
        [TestCase(null, " ", false, false)]
        [TestCase(" ", null, false, false)]
        [TestCase(null, "path2", false, false)]
        [TestCase("path1", null, false, false)]
        [TestCase("", "", false, false)]
        [TestCase(" ", "", false, false)]
        [TestCase("", " ", false, false)]
        [TestCase("", "path2", false, false)]
        [TestCase("path1", "", false, false)]
        [TestCase(" ", " ", false, false)]
        [TestCase("path1", " ", false, false)]
        [TestCase(" ", "path2", false, false)]
        [TestCase("path1", "path2", false, false)]

        [TestCase(null, null, false, true)]
        [TestCase(null, "", false, true)]
        [TestCase("", null, false, true)]
        [TestCase(null, " ", false, true)]
        [TestCase(" ", null, false, true)]
        [TestCase(null, "path2", false, true)]
        [TestCase("path1", null, false, true)]
        [TestCase("", "", false, true)]
        [TestCase(" ", "", false, true)]
        [TestCase("", " ", false, true)]
        [TestCase("", "path2", false, true)]
        [TestCase("path1", "", false, true)]
        [TestCase(" ", " ", false, true)]
        [TestCase("path1", " ", false, true)]
        [TestCase(" ", "path2", false, true)]
        [TestCase("path1", "path2", false, true)]

        [TestCase(null, null, true, false)]
        [TestCase(null, "", true, false)]
        [TestCase("", null, true, false)]
        [TestCase(null, " ", true, false)]
        [TestCase(" ", null, true, false)]
        [TestCase(null, "path2", true, false)]
        [TestCase("path1", null, true, false)]
        [TestCase("", "", false, true)]
        [TestCase(" ", "", false, true)]
        [TestCase("", " ", false, true)]
        [TestCase("", "path2", false, true)]
        [TestCase("path1", "", false, true)]
        [TestCase(" ", " ", false, true)]
        [TestCase("path1", " ", false, true)]
        [TestCase(" ", "path2", false, true)]
        [TestCase("path1", "path2", false, true)]

        [TestCase(null, null, true, true)]
        [TestCase(null, "", true, true)]
        [TestCase("", null, true, true)]
        [TestCase(null, " ", true, true)]
        [TestCase(" ", null, true, true)]
        [TestCase(null, "path2", true, true)]
        [TestCase("path1", null, true, true)]
        [TestCase("", "", true, true)]
        [TestCase(" ", "", true, true)]
        [TestCase("", " ", true, true)]
        [TestCase("", "path2", true, true)]
        [TestCase("path1", "", true, true)]
        [TestCase(" ", " ", true, true)]
        [TestCase("path1", " ", true, true)]
        [TestCase(" ", "path2", true, true)]
        public void Check_WhenInputInvalid_ThrowException(string path1, string path2, bool exists1, bool exists2)
        {
            _directoryWrapperMock.Setup(_ => _.Exists(path1)).Returns(exists1);
            _directoryWrapperMock.Setup(_ => _.Exists(path2)).Returns(exists2);

            var args = new string[] { path1, path2 };

            Action action = () => _directoryChecker.Check(args);
            action
                .ShouldThrow<ArgumentException>()
                .WithMessage("Please enter valid folder paths.");
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Check_WhenIsSubPathOf_ThrowException(bool isSubPath1, bool isSubPath2)
        {
            Smock.Run(context =>
            {
                var path1 = "some fake path 1";
                var path2 = "some fake path 2";

                var directoryWrapperMock = new Mock<IDirectoryWrapper>();
                var directoryChecker = new DirectoryChecker(directoryWrapperMock.Object);

                directoryWrapperMock.Setup(_ => _.Exists(path1)).Returns(true);
                directoryWrapperMock.Setup(_ => _.Exists(path2)).Returns(true);
                
                context.Setup(() => path1.IsSubPathOf(path2)).Returns(isSubPath1);
                context.Setup(() => path2.IsSubPathOf(path1)).Returns(isSubPath2);

                var args = new string[] { path1, path2 };

                Action action = () => directoryChecker.Check(args);
                action
                    .ShouldThrow<ArgumentException>()
                    .WithMessage("Neither folder can be the same as or a subfolder of the other.");
            });
        }

        [Test]
        public void Check_WhenNotIsSubPathOf_DontThrowException()
        {
            Smock.Run(context =>
            {
                var path1 = "some fake path 1";
                var path2 = "some fake path 2";

                var directoryWrapperMock = new Mock<IDirectoryWrapper>();
                var directoryChecker = new DirectoryChecker(directoryWrapperMock.Object);

                directoryWrapperMock.Setup(_ => _.Exists(path1)).Returns(true);
                directoryWrapperMock.Setup(_ => _.Exists(path2)).Returns(true);

                context.Setup(() => path1.IsSubPathOf(path2)).Returns(false);
                context.Setup(() => path2.IsSubPathOf(path1)).Returns(false);

                var args = new string[] { path1, path2 };

                Action action = () => directoryChecker.Check(args);
                action.ShouldNotThrow<ArgumentException>();
            });
        }
    }
}