using System;

namespace DFD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var folder1 = args[0];
                var folder2 = args[1];

                new DirectoryChecker(new DirectoryWrapper()).Check(args);

                var duplicateFileDisplayer = new DuplicateFileDisplayer(args, new ConsoleWrapper());
                var duplicateFileDeleter = new DuplicateFileDeleter(new FileWrapper());
                var emptyFolderDeleter = new EmptyFolderDeleter
                    (new DirectoryWrapper(), new PathWrapper(), new ConsoleWrapper());

                var hashes1 = new DirectoryHasher(new DirectoryWrapper(), folder1).ComputeHashes();
                var hashes2 = new DirectoryHasher(new DirectoryWrapper(), folder2).ComputeHashes();

                duplicateFileDisplayer.Display(hashes1);
                duplicateFileDisplayer.Display(hashes2);

                duplicateFileDeleter.Delete(hashes1);
                duplicateFileDeleter.Delete(hashes2);

                duplicateFileDisplayer.Display(hashes1, hashes2);

                duplicateFileDeleter.Delete(hashes1, hashes2);

                emptyFolderDeleter.Delete(folder1);
                emptyFolderDeleter.Delete(folder2);

                Console.Read();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}