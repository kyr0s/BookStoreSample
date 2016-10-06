namespace BookStore.Implementation.DataProviders
{
    public interface IPathUtility
    {
        string GetAbsolutePath(string relativePath);
    }
}