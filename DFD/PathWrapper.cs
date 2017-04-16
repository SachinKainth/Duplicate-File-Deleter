using System.IO;

namespace DFD
{
    internal class PathWrapper : IPathWrapper
    {
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}