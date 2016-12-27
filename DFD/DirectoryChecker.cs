using System;
using System.IO;

namespace DFD
{
    public class DirectoryChecker : IDirectoryChecker
    {
        private IDirectoryWrapper _directoryWrapper;

        public DirectoryChecker(IDirectoryWrapper directoryWrapper)
        {
            _directoryWrapper = directoryWrapper;
        }

        public void Check(string[] args)
        {
            CheckForCorrectNumberOfArguments(args);

            var path1 = args[0];
            var path2 = args[1];
            CheckThatPathsAreRealFolders(path1, path2);
            CheckThatPathsAreNotTheSameOrOneIsNotASubfolderOfAnother(path1, path2);
        }
        
        private void CheckForCorrectNumberOfArguments(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Usage: DFD.exe <folder 1> <folder 2>");
            }
        }

        private void CheckThatPathsAreRealFolders(string path1, string path2)
        {
            var path1Null = string.IsNullOrWhiteSpace(path1);
            var path1DoesntExist = !_directoryWrapper.Exists(path1);
            var path2Null = string.IsNullOrWhiteSpace(path2);
            var path2DoesntExist = !_directoryWrapper.Exists(path2);

            if (path1Null || path1DoesntExist || path2Null || path2DoesntExist)
            {
                throw new ArgumentException("Please enter valid folder paths.");
            }
        }

        private void CheckThatPathsAreNotTheSameOrOneIsNotASubfolderOfAnother(string path1, string path2)
        {
            if(path1.IsSubPathOf(path2) || path2.IsSubPathOf(path1))
            { 
                throw new ArgumentException("Neither folder can be the same as or a subfolder of the other.");
            }
        }
    }
}