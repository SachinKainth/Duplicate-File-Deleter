using System.IO;
using System.Security.Cryptography;

namespace DFD
{
    public class FileHasher
    {
        private SHA512 _sha512;
        private Stream _stream;

        public FileHasher(SHA512 sha512, Stream stream)
        {
            _sha512 = sha512;
            _stream = stream;
        }

        public byte[] ComputeHashForFile()
        {
            byte[] bytes;
            
            using (_stream)
            {
                bytes = _sha512.ComputeHash(_stream);
            }
            
            return bytes;
        }
    }
}