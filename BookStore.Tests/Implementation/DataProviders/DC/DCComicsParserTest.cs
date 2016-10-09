using System;
using System.Xml;
using BookStore.Implementation;
using BookStore.Implementation.DataProviders;
using BookStore.Implementation.DataProviders.DC;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Implementation.DataProviders.DC
{
    public class DCComicsParserTest : UnitTestBase
    {
        private IBookFactory bookFactory;
        private DCComicsParser dcComicsParser;

        protected override void SetUp()
        {
            base.SetUp();

            bookFactory = NewMock<IBookFactory>();
            dcComicsParser = new DCComicsParser(bookFactory);
        }

        [Test]
        public void ParseTestWhenBooksListIsEmpty()
        {
            const string xmlText = @"<?xml version=""1.0""?><Data><publisher></publisher></Data>";
            var providerXmlData = CreateProviderData(xmlText);

            var actual = dcComicsParser.Parse(providerXmlData);
            actual.Should().BeEmpty();
        }

        [Test]
        [TestCase("<book><isbn>ISBN</isbn><author>AUTHOR</author><title></title></book>")]
        [TestCase("<book><isbn>ISBN</isbn><author>AUTHOR</author></book>")]
        [TestCase("<book><isbn>ISBN</isbn><author></author><title>TITLE</title></book>")]
        [TestCase("<book><isbn>ISBN</isbn><title>TITLE</title></book>")]
        [TestCase("<book><isbn></isbn><author>AUTHOR</author><title>TITLE</title></book>")]
        [TestCase("<book><author>AUTHOR</author><title>TITLE</title></book>")]
        public void ParseTestThrowsExceptionWhenCantParseBookCorrectly(string bookContent)
        {
            var xmlText =
                $@"<?xml version=""1.0""?>
                  <Data>
                    {bookContent}
                  </Data>";
            var providerXmlData = CreateProviderData(xmlText);

            Assert.Throws<Exception>(() => dcComicsParser.Parse(providerXmlData));
        }

        [Test]
        public void ParseTestNormalBehavior()
        {
            const string isbn1 = "ISBN1", author1 = "AUTHOR1", title1 = "TITLE1";
            var book1 = new Book(isbn1, author1, title1);

            const string isbn2 = "ISBN2", author2 = "AUTHOR2", title2 = "TITLE2";
            var book2 = new Book(isbn2, author2, title2);

            const string isbn3 = "ISBN3", author3 = "AUTHOR3", title3 = "TITLE3";
            var book3 = new Book(isbn3, author3, title3);

            var xmlText =
               $@"<?xml version=""1.0""?>
                  <Data>
                    <book><isbn>{isbn1}</isbn><author>{author1}</author><title>{title1}</title></book>
                    <book><isbn>{isbn2}</isbn><author>{author2}</author><title>{title2}</title></book>
                    <book><isbn>{isbn3}</isbn><author>{author3}</author><title>{title3}</title></book>
                  </Data>";
            var providerXmlData = CreateProviderData(xmlText);

            using (MocksRecord())
            {
                bookFactory.Expect(f => f.Create(isbn1, author1, title1)).Return(book1);
                bookFactory.Expect(f => f.Create(isbn2, author2, title2)).Return(book2);
                bookFactory.Expect(f => f.Create(isbn3, author3, title3)).Return(book3);
            }

            var actual = dcComicsParser.Parse(providerXmlData);
            var expected = new[] {book1, book2, book3};

            actual.ShouldBeEquivalentTo(expected);
        }

        private static ProviderXmlData CreateProviderData(string xmlText)
        {
            var filePath = $@"C:\{Guid.NewGuid()}\Upload\{Guid.NewGuid()}.xml";
            var xmlDocument = CreateXmlDocument(xmlText);
            var providerXmlData = new ProviderXmlData(filePath, xmlDocument);
            return providerXmlData;
        }

        private static XmlDocument CreateXmlDocument(string xmlText)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);
            return xmlDocument;
        }
    }
}
