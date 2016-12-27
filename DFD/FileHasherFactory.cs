using System.IO;
using System.Security.Cryptography;

namespace DFD
{
    public class FileHasherFactory
    {
        public static FileHasher Create(string path)
        {
            return new FileHasher(new SHA512Managed(), new FileStream(path, FileMode.Open));
        }
    }
}