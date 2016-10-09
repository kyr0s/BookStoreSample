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
        private readonly IBookFactory bookFactory;
        private readonly ISpecificBookDataProvider[] specificBookDataProviders;

        public BookIndexBuilder(
            IBookIndex bookIndex,
            IBookFactory bookFactory,
            ISpecificBookDataProvider[] specificBookDataProviders)
        {
            this.bookIndex = bookIndex;
            this.bookFactory = bookFactory;
            this.specificBookDataProviders = specificBookDataProviders;
        }

        public void Build()
        {
            var books = specificBookDataProviders
                .SelectMany(p => SafeSelect(p).Select(b => new {Book = b, ProviderName = p.Name}))
                .GroupBy(p => p.Book)
                .Select(g => bookFactory.Create(g.Key, g.Select(p => p.ProviderName).ToArray()))
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
                log.Error(ex, $"Can't retrieve books from \"{provider.Name}\" ({provider.GetType()})");
                return new Book[0];
            }
        }
    }
}