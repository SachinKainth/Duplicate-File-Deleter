using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DFD
{
    public class EmptyFolderDeleter : IEmptyFolderDeleter
    {
        private IDirectoryWrapper _directoryWrapper;
        private IPathWrapper _pathWrapper;
        private IConsoleWrapper _consoleWrapper;

        public EmptyFolderDeleter(
            IDirectoryWrapper directoryWrapper, IPathWrapper pathWrapper, IConsoleWrapper consoleWrapper)
        {
            _directoryWrapper = directoryWrapper;
            _pathWrapper = pathWrapper;
            _consoleWrapper = consoleWrapper;
        }

        public void Delete(string folder)
        {
            var folders = GetAllSubFolders(folder);
            var sortedMappings = GetFolderPathDepthsInOrder(folders);
            DeleteEmptyFolders(sortedMappings);
        }

        private void DeleteEmptyFolders(IOrderedEnumerable<KeyValuePair<string, int>> sortedMappings)
        {
            foreach(var folder in sortedMappings)
            {
                var files = _directoryWrapper.EnumerateFiles
                    (folder.Key, "*.*", SearchOption.TopDirectoryOnly).ToList();

                if(!files.Any())
                {
                    _consoleWrapper.WriteLine($"Folder deleted: {folder.Key}");
                    _directoryWrapper.Delete(folder.Key);
                } 
            }
        }

        private IOrderedEnumerable<KeyValuePair<string, int>> GetFolderPathDepthsInOrder
            (IEnumerable<string> folders)
        {
            var mappings = new Dictionary<string, int>();
            foreach (var f in folders)
            {
                mappings.Add(f, _pathWrapper.GetFullPath(f).Split(Path.DirectorySeparatorChar).Length);
            }
            var sortedMappings = from entry in mappings orderby entry.Value descending select entry;

            return sortedMappings;
        }

        private IEnumerable<string> GetAllSubFolders(string folder)
        {
            return _directoryWrapper.EnumerateDirectories(folder, "*.*", SearchOption.AllDirectories);
        }
    }
}