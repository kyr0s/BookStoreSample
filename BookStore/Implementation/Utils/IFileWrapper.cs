using System.IO;

namespace BookStore.Implementation.Utils
{
    public interface IFileWrapper
    {
        Stream OpenRead(string path);
    }
}