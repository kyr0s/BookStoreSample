using System.IO;
using System.Web;

namespace BookStore.Implementation.Utils
{
    public class PathUtility : IPathUtility
    {
        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(GetRoot(), relativePath);
        }

        private string GetRoot()
        {
            return HttpContext.Current.Server.MapPath("~");
        }
    }
}