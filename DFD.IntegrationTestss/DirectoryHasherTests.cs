using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DFD.IntegrationTests
{
    [TestFixture]
    class DirectoryHasherTests
    {
        [Test]
        public void ComputeHashes_WhenFolderWithSubfoldersUniqueAndDifferentFilesSupplied_ReturnsHashes()
        {
            var numberOfUniqueHashes = 4;

            var expectedHashes = new byte[numberOfUniqueHashes][];
            expectedHashes[0] = new byte[] {
                55, 199, 131, 184, 11, 29, 69, 139, 137, 231, 18, 194,
                223, 226, 119, 112, 80, 239, 240, 174, 252, 159, 109, 139,
                238, 222, 231, 120, 7, 217, 174, 178, 226, 125, 20, 129, 92,
                244, 240, 34, 155, 29, 54, 193, 134, 187, 95, 43, 94, 245,
                94, 99, 43, 16, 140, 196, 30, 159, 185, 100, 195, 155, 66, 165
            };
            expectedHashes[1] = new byte[] {
                7, 42, 9, 144, 82, 4, 171, 86, 36, 94, 207, 68, 255, 70,
                126, 144, 70, 174, 117, 191, 62, 76, 122, 214, 206, 17,
                34, 79, 144, 224, 220, 208, 163, 63, 180, 253, 109, 130,
                152, 177, 0, 232, 183, 78, 37, 114, 45, 111, 130, 80, 93,
                214, 116, 110, 163, 85, 162, 13, 169, 248, 195, 183, 29, 137
            };
            expectedHashes[2] = new byte[] {
                165, 94, 26, 158, 233, 129, 229, 197, 236, 69, 18, 55, 65,
                216, 194, 143, 128, 219, 209, 66, 246, 105, 207, 105, 44,
                207, 93, 21, 160, 129, 88, 68, 147, 68, 10, 35, 189, 10, 48,
                49, 69, 196, 110, 43, 176, 84, 142, 250, 95, 28, 112, 18,
                151, 109, 5, 135, 134, 239, 60, 96, 218, 238, 147, 172
            };
            expectedHashes[3] = new byte[] {
                106, 135, 132, 92, 131, 205, 209, 189, 177, 171, 90, 164,
                201, 124, 63, 69, 155, 34, 61, 237, 0, 42, 41, 210, 9, 88,
                25, 240, 100, 255, 148, 223, 93, 46, 34, 78, 103, 101, 185,
                186, 58, 119, 143, 0, 222, 219, 77, 88, 252, 94, 218, 140,
                49, 29, 117, 235, 252, 57, 93, 104, 186, 123, 197, 65
            };

            var path = AppDomain.CurrentDomain.BaseDirectory + "Test Documents";
            var hasher = new DirectoryHasher(new DirectoryWrapper(), path);

            var result = hasher.ComputeHashes();

            var actualHashes = new byte[numberOfUniqueHashes][];
            result.HashesAndFiles.Keys.CopyTo(actualHashes, 0);

            actualHashes.Length.Should().Be(numberOfUniqueHashes);
            
            for (var i = 0; i < actualHashes.Length; i++)
            {
                actualHashes[i].Should().BeEquivalentTo(expectedHashes[i]);
            }

            var paths = new List<string>[numberOfUniqueHashes];
            result.HashesAndFiles.Values.CopyTo(paths, 0);

            paths[0][0].Should().EndWith("\\Test Documents\\empty-file1.txt");
            paths[0][1].Should().EndWith("\\Test Documents\\empty-file2.txt");

            paths[1][0].Should().EndWith("\\Test Documents\\test-file1.txt");
            paths[1][1].Should().EndWith("\\Test Documents\\test-file2.txt");

            paths[2][0].Should().EndWith("\\Test Documents\\Inner Folder 1\\image1.jpg");
            paths[2][1].Should().EndWith("\\Test Documents\\Inner Folder 1\\Inner Inner Folder 1_1\\image1.jpg");
            paths[2][2].Should().EndWith("\\Test Documents\\Inner Folder 2\\image1.jpg");

            paths[3][0].Should().EndWith("\\Test Documents\\Inner Folder 2\\image2.jpg");
        }
    }
}