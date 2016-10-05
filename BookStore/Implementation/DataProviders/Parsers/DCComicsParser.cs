using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace BookStore.Implementation.DataProviders.Parsers
{
    public class DCComicsParser : IProviderXmlParser
    {
        public string ProviderName => "DC Comics";

        public bool CanParse(ProviderXmlData providerXmlData)
        {
            var fileName = Path.GetFileNameWithoutExtension(providerXmlData.FilePath);
            return !string.IsNullOrWhiteSpace(fileName) &&
                   fileName.StartsWith("dc_", StringComparison.OrdinalIgnoreCase);
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

            return new Book(isbn, author, title);
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