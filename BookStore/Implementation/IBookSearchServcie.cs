namespace BookStore.Implementation
{
    public interface IBookSearchServcie
    {
        BookWrapper[] Search(string query, int count);
        void InitializeIndex();
    }
}