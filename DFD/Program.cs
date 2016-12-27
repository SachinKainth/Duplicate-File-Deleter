using System;

namespace DFD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var duplicateFileDisplayer = new DuplicateFileDisplayer(args, new ConsoleWrapper());
                var duplicateFileDeleter = new DuplicateFileDeleter(new FileWrapper());

                var hashes1 = new DirectoryHasher(new DirectoryWrapper(), args[0]).ComputeHashes();
                var hashes2 = new DirectoryHasher(new DirectoryWrapper(), args[1]).ComputeHashes();

                duplicateFileDisplayer.Display(hashes1);
                duplicateFileDisplayer.Display(hashes2);

                duplicateFileDeleter.Delete(hashes1);
                duplicateFileDeleter.Delete(hashes2);

                duplicateFileDisplayer.Display(hashes1, hashes2);

                duplicateFileDeleter.Delete(hashes1, hashes2);

                Console.Read();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}