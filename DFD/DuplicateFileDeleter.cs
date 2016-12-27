namespace DFD
{
    public class DuplicateFileDeleter : IDuplicateFileDeleter
    {
        private IFileWrapper _fileWrapper;

        public DuplicateFileDeleter(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public void Delete(Hashes hashes)
        {
            if (hashes == null || hashes.HashesAndFiles == null)
            {
                return;
            }

            foreach (var h in hashes.HashesAndFiles)
            {
                if (h.Value.Count > 1)
                {
                    for (var i = 1; i < h.Value.Count; i++)
                    {
                        _fileWrapper.Delete(h.Value[i]);
                    }
                }
            }
        }

        public void Delete(Hashes hashes1, Hashes hashes2)
        {
            if (hashes1 == null || hashes1.HashesAndFiles == null ||
                hashes2 == null || hashes2.HashesAndFiles == null)
            {
                return;
            }

            var comparer = new ByteArrayComparer();

            foreach (var h1 in hashes1.HashesAndFiles)
            {
                foreach (var h2 in hashes2.HashesAndFiles)
                {
                    if (comparer.Equals(h1.Key, h2.Key))
                    {
                        _fileWrapper.Delete(h2.Value[0]);
                    }
                }
            }
        }
    }
}