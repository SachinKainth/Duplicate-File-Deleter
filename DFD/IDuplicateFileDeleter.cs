namespace DFD
{
    public interface IDuplicateFileDeleter
    {
        void Delete(Hashes hashes);
        void Delete(Hashes hashes1, Hashes hashes2);
    }
}