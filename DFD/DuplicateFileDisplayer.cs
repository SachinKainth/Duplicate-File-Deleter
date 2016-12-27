using System;

namespace DFD
{
    public class DuplicateFileDisplayer : IDuplicateFileDisplayer
    {
        private IConsoleWrapper _consoleWrapper;

        public DuplicateFileDisplayer(string[] args, IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void Display(Hashes hashes)
        {
            if (hashes == null || hashes.HashesAndFiles == null)
            {
                return;
            }

            _consoleWrapper.WriteLine($"Path: {hashes.Path}");
            _consoleWrapper.WriteLine();
            _consoleWrapper.WriteLine();

            foreach (var h in hashes.HashesAndFiles)
            {
                if (h.Value.Count > 1)
                {
                    _consoleWrapper.WriteLine($"Number of Copies: {h.Value.Count}");
                    for (var i = 0; i < h.Value.Count; i++)
                    {
                        var f = h.Value[i];
                        _consoleWrapper.WriteLine($"Copy {i+1}: {f}");
                    }
                    _consoleWrapper.WriteLine();
                }
            }
        }

        public void Display(Hashes hashes1, Hashes hashes2)
        {
            if (hashes1 == null || hashes1.HashesAndFiles == null ||
                hashes2 == null || hashes2.HashesAndFiles == null)
            {
                return;
            }

            _consoleWrapper.WriteLine($"Path 1: {hashes1.Path}");
            _consoleWrapper.WriteLine($"Path 2: {hashes2.Path}");
            _consoleWrapper.WriteLine();
            _consoleWrapper.WriteLine();

            var comparer = new ByteArrayComparer();

            foreach (var h1 in hashes1.HashesAndFiles)
            {
                foreach (var h2 in hashes2.HashesAndFiles)
                {
                    if (comparer.Equals(h1.Key, h2.Key))
                    {
                        _consoleWrapper.WriteLine($"Path 1 duplicate file: {h1.Value[0]}");
                        _consoleWrapper.WriteLine($"Path 2 duplicate file: {h2.Value[0]}");
                    }
                }
            }
        }
    }
}