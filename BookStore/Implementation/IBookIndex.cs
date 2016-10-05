namespace BookStore.Implementation
{
    public interface IBookIndex
    {
        BookWrapper[] Search(string query, int count);
        void Rebuild(BookWrapper[] books);
    }
}