using System.IO;

namespace BookStore.Implementation.Utils
{
    public class FileWrapper : IFileWrapper
    {
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
    }
}