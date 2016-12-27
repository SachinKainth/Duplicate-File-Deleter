using FluentAssertions;
using NUnit.Framework;
using System;

namespace DFD.IntegrationTests
{
    [TestFixture]
    class DirectoryCheckerTests
    {
        private IDirectoryChecker _directoryChecker;

        [SetUp]
        public void Setup()
        {
            _directoryChecker = new DirectoryChecker(new DirectoryWrapper());
        }

        [TestCase(@"Test Documents", @"Test Documents")]
        [TestCase(@"Test Documents\", @"Test Documents")]
        [TestCase(@"Test Documents", @"Test Documents\")]
        [TestCase(@"Test Documents\", @"Test Documents\")]
        [TestCase(@"Test Documents\Inner Folder 1", @"Test Documents")]
        [TestCase(@"Test Documents\Inner Folder 1\", @"Test Documents")]
        [TestCase(@"Test Documents\Inner Folder 1", @"Test Documents\")]
        [TestCase(@"Test Documents\Inner Folder 1\", @"Test Documents\")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1", @"Test Documents")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1\", @"Test Documents")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1", @"Test Documents\")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1\", @"Test Documents\")]
        [TestCase(@"Test Documents", @"Test Documents\Inner Folder 1")]
        [TestCase(@"Test Documents", @"Test Documents\Inner Folder 1\")]
        [TestCase(@"Test Documents\", @"Test Documents\Inner Folder 1")]
        [TestCase(@"Test Documents\", @"Test Documents\Inner Folder 1\")]
        [TestCase(@"Test Documents", @"Test Documents\Inner Folder 1\Inner Inner Folder 1")]
        [TestCase(@"Test Documents", @"Test Documents\Inner Folder 1\Inner Inner Folder 1\")]
        [TestCase(@"Test Documents\", @"Test Documents\Inner Folder 1\Inner Inner Folder 1")]
        [TestCase(@"Test Documents\", @"Test Documents\Inner Folder 1\Inner Inner Folder 1\")]

        [TestCase(@"TEST DOCUMENTS", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1\", @"Test Documents")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1\", @"Test Documents\")]
        [TestCase(@"TEST DOCUMENTS", @"Test Documents\Inner Folder 1")]
        [TestCase(@"TEST DOCUMENTS", @"Test Documents\Inner Folder 1\")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents\Inner Folder 1")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents\Inner Folder 1\")]
        [TestCase(@"TEST DOCUMENTS", @"Test Documents\Inner Folder 1\Inner Inner Folder 1")]
        [TestCase(@"TEST DOCUMENTS", @"Test Documents\Inner Folder 1\Inner Inner Folder 1\")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents\Inner Folder 1\Inner Inner Folder 1")]
        [TestCase(@"TEST DOCUMENTS\", @"Test Documents\Inner Folder 1\Inner Inner Folder 1\")]

        [TestCase(@"Test Documents", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents\Inner Folder 1", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents\Inner Folder 1\", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents\Inner Folder 1", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents\Inner Folder 1\", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1\", @"TEST DOCUMENTS")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents\Inner Folder 1\Inner Inner Folder 1\", @"TEST DOCUMENTS\")]
        [TestCase(@"Test Documents", @"TEST DOCUMENTS\INNER FOLDER 1")]
        [TestCase(@"Test Documents", @"TEST DOCUMENTS\INNER FOLDER 1\")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS\INNER FOLDER 1")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS\INNER FOLDER 1\")]
        [TestCase(@"Test Documents", @"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1")]
        [TestCase(@"Test Documents", @"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1\")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1")]
        [TestCase(@"Test Documents\", @"TEST DOCUMENTS\INNER FOLDER 1\INNER INNER FOLDER 1\")]
        public void Check_WhenOneFolderSameAsOrSubfolderOfAnother_ThrowException(string path1, string path2)
        {
            var args = new string[] {
                AppDomain.CurrentDomain.BaseDirectory + path1,
                AppDomain.CurrentDomain.BaseDirectory + path2
            };

            Action action = () => _directoryChecker.Check(args);
            action
                .ShouldThrow<ArgumentException>()
                .WithMessage("Neither folder can be the same as or a subfolder of the other.");
        }
    }
}