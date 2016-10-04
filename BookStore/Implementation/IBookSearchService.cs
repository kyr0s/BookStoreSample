namespace BookStore.Implementation
{
    public interface IBookSearchService
    {
        BookWrapper[] Search(string query, int count);
        void InitializeIndex();
    }
}