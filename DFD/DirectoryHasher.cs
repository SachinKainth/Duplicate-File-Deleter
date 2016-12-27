using System;
using System.Collections.Generic;
using System.IO;

namespace DFD
{
    public class DirectoryHasher
    {
        private string _path;
        private IDirectoryWrapper _directoryWrapper;

        public DirectoryHasher(IDirectoryWrapper directoryWrapper, string path)
        {
            _directoryWrapper = directoryWrapper;
            _path = path;

            if (string.IsNullOrWhiteSpace(path) || !_directoryWrapper.Exists(path))
            {
                throw new ArgumentException("Please enter a valid folder path.");
            }
        }
        
        public Hashes ComputeHashes()
        {
            var hashes = new Dictionary<byte[], List<string>>(new ByteArrayComparer());

            string[] files = _directoryWrapper.GetFiles(_path, "*.*", SearchOption.AllDirectories);

            foreach(var file in files)
            {
                var fileHasher = FileHasherFactory.Create(file);
                var hash = fileHasher.ComputeHashForFile();
                if (hashes.ContainsKey(hash))
                {
                    hashes[hash].Add(file);
                }
                else
                {
                    hashes.Add(hash, new List<string> { file });
                }
            }

            return new Hashes { Path = _path, HashesAndFiles = hashes };
        }
    }
}