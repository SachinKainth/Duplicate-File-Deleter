namespace DFD
{
    public interface IDuplicateFileDisplayer
    {
        void Display(Hashes hashes);
        void Display(Hashes hashes1, Hashes hashes2);
    }
}