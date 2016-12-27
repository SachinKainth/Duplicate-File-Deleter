using System.IO;

namespace DFD
{
    public interface IDirectoryWrapper
    {
        bool Exists(string path);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    }
}