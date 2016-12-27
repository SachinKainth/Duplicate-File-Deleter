using System.IO;

namespace DFD
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }
    }
}