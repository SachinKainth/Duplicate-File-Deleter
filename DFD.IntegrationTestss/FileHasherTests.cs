using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Cryptography;

namespace DFD.IntegrationTests
{
    [TestFixture]
    class FileHasherTests
    {
        [Test]
        public void ComputeHashForFile_WhenFileIsNotEmpty_ReturnsHash()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\test-file1.txt";

            var fileHasher = new FileHasher(new SHA512Managed(), new FileStream(path, FileMode.Open));
            
            var result = fileHasher.ComputeHashForFile();

            result.Should().BeEquivalentTo(new byte[] {
                7, 42, 9, 144, 82, 4, 171, 86, 36, 94, 207, 68, 255, 70,
                126, 144, 70, 174, 117, 191, 62, 76, 122, 214, 206, 17,
                34, 79, 144, 224, 220, 208, 163, 63, 180, 253, 109, 130,
                152, 177, 0, 232, 183, 78, 37, 114, 45, 111, 130, 80, 93,
                214, 116, 110, 163, 85, 162, 13, 169, 248, 195, 183, 29, 137
            });
        }

        [Test]
        public void ComputeHashForFile_WhenFileIsEmpty_ReturnsHash()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\empty-file1.txt";

            var fileHasher = new FileHasher(new SHA512Managed(), new FileStream(path, FileMode.Open));
            
            var result = fileHasher.ComputeHashForFile();

            result.Should().BeEquivalentTo(new byte[] {
                55, 199, 131, 184, 11, 29, 69, 139, 137, 231, 18, 194,
                223, 226, 119, 112, 80, 239, 240, 174, 252, 159, 109, 139,
                238, 222, 231, 120, 7, 217, 174, 178, 226, 125, 20, 129, 92,
                244, 240, 34, 155, 29, 54, 193, 134, 187, 95, 43, 94, 245,
                94, 99, 43, 16, 140, 196, 30, 159, 185, 100, 195, 155, 66, 165
            });
        }

        [Test]
        public void ComputeHashForFile_TwoFilesWithSameContent_ReturnsSameHash()
        {
            var path1 = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\test-file1.txt";
            var path2 = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\test-file2.txt";

            var fileHasher1 = new FileHasher(new SHA512Managed(), new FileStream(path1, FileMode.Open));
            var fileHasher2 = new FileHasher(new SHA512Managed(), new FileStream(path2, FileMode.Open));
            
            var result1 = fileHasher1.ComputeHashForFile();
            var result2 = fileHasher2.ComputeHashForFile();

            result1.Should().BeEquivalentTo(result2);
        }

        [Test]
        public void ComputeHashForFile_TwoFilesWithEmptyContent_ReturnsSameHash()
        {
            var path1 = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\empty-file1.txt";
            var path2 = AppDomain.CurrentDomain.BaseDirectory + "Test Documents\\empty-file2.txt";

            var fileHasher1 = new FileHasher(new SHA512Managed(), new FileStream(path1, FileMode.Open));
            var fileHasher2 = new FileHasher(new SHA512Managed(), new FileStream(path2, FileMode.Open));
            
            var result1 = fileHasher1.ComputeHashForFile();
            var result2 = fileHasher2.ComputeHashForFile();

            result1.Should().BeEquivalentTo(result2);
        }
    }
}