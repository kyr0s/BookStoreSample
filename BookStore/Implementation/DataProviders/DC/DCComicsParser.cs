using System;
using System.Linq;
using System.Xml;

namespace BookStore.Implementation.DataProviders.DC
{
    public class DCComicsParser : IDCComicsParser
    {
        private readonly IBookFactory bookFactory;

        public DCComicsParser(IBookFactory bookFactory)
        {
            this.bookFactory = bookFactory;
        }

        public Book[] Parse(ProviderXmlData providerXmlData)
        {
            var booksList = providerXmlData.Data.SelectNodes("/Data/book");
            if (booksList == null || booksList.Count == 0)
            {
                return new Book[0];
            }

            var books = booksList
                .Cast<XmlNode>()
                .Select(ParseBook)
                .ToArray();

            return books;
        }

        private Book ParseBook(XmlNode source)
        {
            var isbn = ReadNodeText(source, "isbn");
            var author = ReadNodeText(source, "author");
            var title = ReadNodeText(source, "title");

            return bookFactory.Create(isbn, author, title);
        }

        private string ReadNodeText(XmlNode source, string nodeName)
        {
            var node = source.SelectSingleNode(nodeName);
            if (string.IsNullOrWhiteSpace(node?.Value))
            {
                throw new Exception($"Can't parse node \"{nodeName}\" from\r\n{source.OuterXml}");
            }

            return node.Value;
        }
    }
}