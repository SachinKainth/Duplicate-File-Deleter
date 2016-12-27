using System.IO;

namespace DFD
{
    public class FileWrapper : IFileWrapper
    {
        public void Delete(string file)
        {
            File.Delete(file);
        } 
    }
}