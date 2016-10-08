using System.IO;

namespace BookStore.Implementation.Utils
{
    public interface IDirectoryWrapper
    {
        bool Exists(string path);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    }
}
