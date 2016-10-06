using System.Linq;
using BookStore.Implementation.DataProviders;

namespace BookStore.Implementation
{
    public class BookIndexBuilder : IBookIndexBuilder
    {
        private readonly IBookIndex bookIndex;
        private readonly ISpecificBookDataProvider[] specificBookDataProviders;

        public BookIndexBuilder(
            IBookIndex bookIndex,
            ISpecificBookDataProvider[] specificBookDataProviders)
        {
            this.bookIndex = bookIndex;
            this.specificBookDataProviders = specificBookDataProviders;
        }

        public void Build()
        {
            var books = specificBookDataProviders
                .SelectMany(p => p.SelectAll().Select(b => new { Book = b, p.ProviderName }))
                .GroupBy(p => p.Book)
                .Select(g => new BookWrapper(g.Key, g.Select(p => p.ProviderName).ToArray()))
                .ToArray();

            bookIndex.Rebuild(books);
        }
    }
}