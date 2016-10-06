using System;
using System.Linq;
using BookStore.Implementation.DataProviders;
using NLog;

namespace BookStore.Implementation
{
    public class BookIndexBuilder : IBookIndexBuilder
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

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
                .SelectMany(p => SafeSelect(p).Select(b => new {Book = b, p.ProviderName}))
                .GroupBy(p => p.Book)
                .Select(g => new BookWrapper(g.Key, g.Select(p => p.ProviderName).ToArray()))
                .ToArray();

            bookIndex.Rebuild(books);
        }

        private Book[] SafeSelect(ISpecificBookDataProvider provider)
        {
            try
            {
                return provider.SelectAll();
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Can't retrieve books from \"{provider.ProviderName}\" ({provider.GetType()})");
                return new Book[0];
            }
        }
    }
}