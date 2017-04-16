using System.Collections.Generic;
using System.IO;

namespace DFD
{
    public interface IDirectoryWrapper
    {
        bool Exists(string path);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

        void Delete(string path);

        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
    }
}