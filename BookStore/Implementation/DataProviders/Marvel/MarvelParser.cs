﻿using System;
using System.Linq;
using System.Xml;

namespace BookStore.Implementation.DataProviders.Marvel
{
    public class MarvelParser : IMarvelParser
    {
        private const string publisherName = "marvel";

        private readonly IBookFactory bookFactory;

        public MarvelParser(IBookFactory bookFactory)
        {
            this.bookFactory = bookFactory;
        }

        public bool CanParse(ProviderXmlData providerXmlData)
        {
            var publisherNameNode = providerXmlData.Data.SelectSingleNode("/data/publisher/name/text()");
            if (publisherNameNode == null)
            {
                return false;
            }

            var name = publisherNameNode.Value;
            return string.Equals(name, publisherName, StringComparison.OrdinalIgnoreCase);
        }

        public Book[] Parse(ProviderXmlData providerXmlData)
        {
            var booksList = providerXmlData.Data.SelectNodes("/data/booklist/book");
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
            var isbn = ReadAttribute(source, "isbn");
            var author = ReadAttribute(source, "author");
            var title = ReadAttribute(source, "title");

            return bookFactory.Create(isbn, author, title);
        }

        private string ReadAttribute(XmlNode source, string name)
        {
            if (source.Attributes == null)
            {
                throw new Exception($"Can't parse book from\r\n{source.OuterXml}");
            }

            var attribute = source.Attributes[name];
            if (attribute == null)
            {
                throw new Exception($"Can't get book attribute from\r\n{source.OuterXml}");
            }

            var value = attribute.Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"Attribute value is empty in node\r\n{source.OuterXml}");
            }

            return value;
        }
    }
}