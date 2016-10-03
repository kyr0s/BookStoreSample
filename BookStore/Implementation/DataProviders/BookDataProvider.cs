using System.Collections.Generic;
using System.Linq;

namespace BookStore.Implementation.DataProviders
{
    public class BookDataProvider : IBookDataProvider
    {
        private readonly IXmlFileService xmlFileService;
        private readonly IProviderXmlParser[] providerXmlParsers;

        public BookDataProvider(
            IXmlFileService xmlFileService,
            IProviderXmlParser[] providerXmlParsers)
        {
            this.xmlFileService = xmlFileService;
            this.providerXmlParsers = providerXmlParsers;
        }

        public BookWrapper[] SelectAll()
        {
            var dataFiles = xmlFileService.LoadAll();

            var booksByProvider = new Dictionary<string, List<Book>>();
            foreach (var providerXmlData in dataFiles)
            {
                var parser = providerXmlParsers.First(p => p.CanParse(providerXmlData));
                var parsedBooks = parser.Parse(providerXmlData);

                List<Book> providerBooks;
                if (!booksByProvider.TryGetValue(parser.ProviderName, out providerBooks))
                {
                    providerBooks = new List<Book>(parsedBooks.Length);
                    booksByProvider.Add(parser.ProviderName, providerBooks);
                }

                providerBooks.AddRange(parsedBooks);
            }

            var books = booksByProvider
                .SelectMany(pair => pair.Value.Select(b => new {Book = b, ProviderName = pair.Key}))
                .GroupBy(p => p.Book)
                .Select(g => new BookWrapper(g.Key, g.Select(p => p.ProviderName).ToArray()))
                .ToArray();

            return books;
        }
    }
}