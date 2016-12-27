using System;

namespace DFD
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}