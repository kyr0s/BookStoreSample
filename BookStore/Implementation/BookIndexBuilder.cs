using BookStore.Implementation.DataProviders;

namespace BookStore.Implementation
{
    public class BookIndexBuilder : IBookIndexBuilder
    {
        private readonly IBookIndex bookIndex;
        private readonly IBookDataProvider bookDataProvider;

        public BookIndexBuilder(
            IBookIndex bookIndex,
            IBookDataProvider bookDataProvider)
        {
            this.bookIndex = bookIndex;
            this.bookDataProvider = bookDataProvider;
        }

        public void Build()
        {
            var books = bookDataProvider.SelectAll();
            bookIndex.Rebuild(books);
        }
    }
}