using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smocks;
using System.IO;
using System.Security.Cryptography;

namespace DFD.Tests
{
    [TestFixture]
    class FileHasherTests
    {
        private FileHasher _fileHasher;
        private Mock<SHA512> _sha512Mock;
        private Mock<Stream> _streamMock;
        
        [Test]
        public void ComputeHashForFile_WhenFilePathValid_ReturnsHash()
        {
            Smock.Run(context =>
            {
                _sha512Mock = new Mock<SHA512>();
                _streamMock = new Mock<Stream>();
                _fileHasher = new FileHasher(_sha512Mock.Object, _streamMock.Object);

                var bytes = new byte[] { 1, 2, 3 };

                context.Setup(() => It.IsAny<SHA512>().ComputeHash(It.IsAny<Stream>())).Returns(bytes);

                var result = _fileHasher.ComputeHashForFile();

                result.Should().BeEquivalentTo(bytes);
            });
        }
    }
}