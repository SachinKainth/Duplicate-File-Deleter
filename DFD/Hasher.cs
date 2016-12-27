using System.Collections.Generic;

namespace DFD
{
    public class Hashes
    {
        public string Path { get; set; }
        public IDictionary<byte[], List<string>> HashesAndFiles { get; set; }
    }
}