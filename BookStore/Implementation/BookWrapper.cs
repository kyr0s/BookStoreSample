namespace BookStore.Implementation
{
    public class BookWrapper
    {
        public BookWrapper(Book book, string[] providers)
        {
            Book = book;
            Providers = providers;
        }

        public Book Book { get; }
        public string[] Providers { get; set; }
    }
}