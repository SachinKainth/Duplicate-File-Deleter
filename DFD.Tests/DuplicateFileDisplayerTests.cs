using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DFD.UnitTests
{
    [TestFixture]
    class DuplicateFileDisplayerTests
    {
        Mock<IConsoleWrapper> _consoleWrapperMock;

        [SetUp]
        public void Setup()
        {
            _consoleWrapperMock = new Mock<IConsoleWrapper>();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Ctor_ArgsLengthNot2_ThrowsException(int argsLength)
        {
            var args = new string[argsLength];

            Action action = () => new DuplicateFileDisplayer(args, new ConsoleWrapper());
            action
                .ShouldThrow<ArgumentException>()
                .WithMessage("Usage: DFD.exe <folder 1> <folder 2>");
        }

        [Test]
        public void Ctor_ArgsLength2_DoesNotThrowException()
        {
            var args = new string[2];

            Action action = () => new DuplicateFileDisplayer(args, new ConsoleWrapper());
            action.ShouldNotThrow<ArgumentException>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Display1Arg_WhenNullInput_DoesNotPrintToConsole(bool nullObject)
        {
            var args = new string[2];

            var duplicateFileDisplayer = new DuplicateFileDisplayer(args, _consoleWrapperMock.Object);

            duplicateFileDisplayer.Display(nullObject? null : new Hashes { });

            _consoleWrapperMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never());
            _consoleWrapperMock.Verify(_ => _.WriteLine(), Times.Never());
        }

        [Test]
        public void Display1Arg_WhenHashesSupplied_PrintsToConsole()
        {
            var args = new string[2];

            var duplicateFileDisplayer = new DuplicateFileDisplayer(args, _consoleWrapperMock.Object);

            var hashes = new Dictionary<byte[], List<string>>();
            hashes.Add(new byte[] { 1 }, new List<string>() {
                "C:\\path\file1.txt", "C:\\path\file2.txt" });
            hashes.Add(new byte[] { 2 }, new List<string>() {
                "C:\\path\file3.jpg", "C:\\path\file4.jpg", "C:\\path\file5.jpg" });
            hashes.Add(new byte[] { 3 }, new List<string>() { "C:\\path\file6.jpg" });

            duplicateFileDisplayer.Display(new Hashes
            {
                Path = "C:\\path",
                HashesAndFiles = hashes
            });

            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path: C:\\path"), Times.Exactly(1));

            _consoleWrapperMock.Verify(_ => _.WriteLine($"Number of Copies: 2"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Number of Copies: 3"),
                Times.Exactly(1));

            _consoleWrapperMock.Verify(_ => _.WriteLine($"Copy 1: C:\\path\file1.txt"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Copy 2: C:\\path\file2.txt"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Copy 1: C:\\path\file3.jpg"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Copy 2: C:\\path\file4.jpg"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Copy 3: C:\\path\file5.jpg"),
                Times.Exactly(1));

            _consoleWrapperMock.Verify(_ => _.WriteLine(), Times.Exactly(4));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Display2Args_WhenNullInput_DoesNotPrintToConsole(bool nullObject1, bool nullObject2)
        {
            var args = new string[2];

            var duplicateFileDisplayer = new DuplicateFileDisplayer(args, _consoleWrapperMock.Object);

            duplicateFileDisplayer.Display(
                nullObject1 ? null : new Hashes { }, nullObject2 ? null : new Hashes { });

            _consoleWrapperMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never());
            _consoleWrapperMock.Verify(_ => _.WriteLine(), Times.Never());
        }

        [Test]
        public void Display2Args_WhenHashesSupplied_PrintsToConsole()
        {
            var args = new string[2];

            var duplicateFileDisplayer = new DuplicateFileDisplayer(args, _consoleWrapperMock.Object);

            var hashes1 = new Dictionary<byte[], List<string>>();
            hashes1.Add(new byte[] { 1 }, new List<string>() { "C:\\path 1\file1.txt" });
            hashes1.Add(new byte[] { 2 }, new List<string>() { "C:\\path 1\file2.jpg", });
            hashes1.Add(new byte[] { 3 }, new List<string>() { "C:\\path 1\file3.dll", });

            var hashes2 = new Dictionary<byte[], List<string>>();
            hashes2.Add(new byte[] { 1 }, new List<string>() { "C:\\path 2\file4.txt" });
            hashes2.Add(new byte[] { 3 }, new List<string>() { "C:\\path 2\file5.dll", });
            hashes2.Add(new byte[] { 4 }, new List<string>() { "C:\\path 2\file6.exe", });

            duplicateFileDisplayer.Display(
                new Hashes { Path = "C:\\path 1", HashesAndFiles = hashes1 },
                new Hashes { Path = "C:\\path 2", HashesAndFiles = hashes2 });

            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 1: C:\\path 1"), Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 2: C:\\path 2"), Times.Exactly(1));
            
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 1 duplicate file: C:\\path 1\file1.txt"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 2 duplicate file: C:\\path 2\file4.txt"),
                Times.Exactly(1));

            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 1 duplicate file: C:\\path 1\file3.dll"),
                Times.Exactly(1));
            _consoleWrapperMock.Verify(_ => _.WriteLine($"Path 2 duplicate file: C:\\path 2\file5.dll"),
                Times.Exactly(1));

            _consoleWrapperMock.Verify(_ => _.WriteLine(), Times.Exactly(2));
        }
    }
}