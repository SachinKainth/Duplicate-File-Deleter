using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace DFD.UnitTests
{
    [TestFixture]
    class DirectoryHasherTests
    {
        private Mock<IDirectoryWrapper> _directoryWrapperMock;

        [SetUp]
        public void Setup()
        {
            _directoryWrapperMock = new Mock<IDirectoryWrapper>();
        }
        
        [Test]
        public void Ctor_WhenInputValid_DoesntThrowException()
        {
            string path = "some path";

            _directoryWrapperMock.Setup(_ => _.Exists(path)).Returns(true);

            Action action = () => new DirectoryHasher(_directoryWrapperMock.Object, path);
            action
                .ShouldNotThrow<ArgumentException>();
        }

        [Test]
        public void ComputeHashes_WhenNoFilesFound_NoHashesAreGenerated()
        {
            string path = "some path";
            var files = new string[] { };

            _directoryWrapperMock.Setup(_ => _.Exists(path)).Returns(true);

            _directoryWrapperMock.Setup(_ => 
                _.GetFiles(path, "*.*", SearchOption.AllDirectories))
                .Returns(files);

            var directoryHasher = new DirectoryHasher(_directoryWrapperMock.Object, path);

            directoryHasher.ComputeHashes().HashesAndFiles.ShouldBeEquivalentTo(new Dictionary<byte[], List<string>>(new ByteArrayComparer()));
        }
        
        [Test]
        public void ComputeHashes_WhenFilesFound_HashesAreGenerated()
        {
            Smock.Run(context =>
            {
                var directoryHasher = Arrange(context);

                var hashes = directoryHasher.ComputeHashes();

                Assert(hashes);
            });
        }

        private DirectoryHasher Arrange(ISmocksContext context)
        {
            _directoryWrapperMock = new Mock<IDirectoryWrapper>();

            _directoryWrapperMock.Setup(_ => _.Exists(It.IsAny<string>())).Returns(true);

            var files = new string[] { "a.txt", "b.txt", "c.jpg" };

            _directoryWrapperMock.Setup(_ =>
                _.GetFiles(It.IsAny<string>(), "*.*", SearchOption.AllDirectories))
                .Returns(files);

            var textFileFileHasher = new FileHasher(new SHA512Managed(), new MemoryStream());
            var jpgFileHasher = new FileHasher(new SHA512Managed(), new MemoryStream());

            context
                .Setup(() => FileHasherFactory.Create("a.txt"))
                .Returns(textFileFileHasher);

            context
                .Setup(() => FileHasherFactory.Create("b.txt"))
                .Returns(textFileFileHasher);

            context
                .Setup(() => FileHasherFactory.Create("c.jpg"))
                .Returns(jpgFileHasher);

            context.Setup(() => textFileFileHasher.ComputeHashForFile()).Returns(new byte[] { 1 });
            context.Setup(() => jpgFileHasher.ComputeHashForFile()).Returns(new byte[] { 2 });

            string path = "some path";

            var directoryHasher = new DirectoryHasher(_directoryWrapperMock.Object, path);

            return directoryHasher;
        }

        private void Assert(Hashes hashes)
        {
            var numberOfUniqueHashes = 2;

            var actualHashes = new byte[numberOfUniqueHashes][];
            hashes.HashesAndFiles.Keys.CopyTo(actualHashes, 0);

            actualHashes.Length.Should().Be(numberOfUniqueHashes);

            var expectedHashes = new byte[numberOfUniqueHashes][];
            expectedHashes[0] = new byte[] { 1 };
            expectedHashes[1] = new byte[] { 2 };

            for (var i = 0; i < actualHashes.Length; i++)
            {
                actualHashes[i].Should().BeEquivalentTo(expectedHashes[i]);
            }

            int index = 0;
            foreach (var i in hashes.HashesAndFiles.Values)
            {
                if (index == 0)
                {
                    i.Count.Should().Be(2);
                    i[0].Should().Be("a.txt");
                    i[1].Should().Be("b.txt");
                }
                else if (index == 1)
                {
                    i.Count.Should().Be(1);
                    i[0].Should().Be("c.jpg");
                }
                index++;
            }
        }
    }
}