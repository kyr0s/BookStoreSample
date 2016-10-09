namespace BookStore.Implementation
{
    public interface IBookFactory
    {
        Book Create(string isbn, string author, string title);
        BookWrapper Create(Book book, string[] providers);
    }
}