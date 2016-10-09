using System;
using System.Xml;
using BookStore.Implementation;
using BookStore.Implementation.DataProviders;
using BookStore.Implementation.DataProviders.Marvel;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Implementation.DataProviders.Marvel
{
    public class MarvelParserTest : UnitTestBase
    {
        private IBookFactory bookFactory;
        private MarvelParser marvelParser;

        protected override void SetUp()
        {
            base.SetUp();

            bookFactory = NewMock<IBookFactory>();
            marvelParser = new MarvelParser(bookFactory);
        }

        [Test]
        public void CanParseTestIsFalseWhenPublisherNameNotFound()
        {
            const string xmlText = @"<?xml version=""1.0""?><Data><someContent></someContent></Data>";
            var providerXmlData = CreateProviderData(xmlText);

            var actual = marvelParser.CanParse(providerXmlData);
            Assert.IsFalse(actual);
        }

        [Test]
        [TestCase("marvel", true)]
        [TestCase("MaRvEl", true)]
        [TestCase("MARVEL", true)]
        [TestCase("someOneElse", false)]
        [TestCase("marvelSuffix", false)]
        [TestCase("prefixMarvel", false)]
        [TestCase("", false)]
        public void CanParseTestWhenPublisherNameDefined(string name, bool expected)
        {
            var xmlText = $@"<?xml version=""1.0""?><data><publisher><name>{name}</name></publisher></data>";
            var providerXmlData = CreateProviderData(xmlText);

            var actual = marvelParser.CanParse(providerXmlData);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseTestReturnsEmptyWhenBookConentNotFound()
        {
            const string xmlText = @"<?xml version=""1.0""?><data><booklist></booklist></data>";
            var providerXmlData = CreateProviderData(xmlText);

            var actual = marvelParser.Parse(providerXmlData);
            actual.Should().BeEmpty();
        }

        [Test]
        [TestCase(@"<book isbn="""" author=""AUTHOR"" title=""TITLE""/>")]
        [TestCase(@"<book author=""AUTHOR"" title=""TITLE""/>")]
        [TestCase(@"<book isbn=""ISBN"" author="""" title=""TITLE""/>")]
        [TestCase(@"<book isbn=""ISBN"" title=""TITLE""/>")]
        [TestCase(@"<book isbn=""ISBN"" author=""AUTHOR"" title=""""/>")]
        [TestCase(@"<book isbn=""ISBN"" author=""AUTHOR"" />")]
        public void ParseTestThrowsExceptionCantParseBookCorrectly(string book)
        {
            var xmlText = $@"<?xml version=""1.0""?><data><booklist>{book}</booklist></data>";
            var providerXmlData = CreateProviderData(xmlText);

            Assert.Throws<Exception>(() => marvelParser.Parse(providerXmlData));
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
                  <data>
                    <booklist>
                        <book isbn=""{isbn1}"" author=""{author1}"" title=""{title1}"" />
                        <book isbn=""{isbn2}"" author=""{author2}"" title=""{title2}"" />
                        <book isbn=""{isbn3}"" author=""{author3}"" title=""{title3}"" />
                    </booklist>
                  </data>";
            var providerXmlData = CreateProviderData(xmlText);

            using (MocksRecord())
            {
                bookFactory.Expect(f => f.Create(isbn1, author1, title1)).Return(book1);
                bookFactory.Expect(f => f.Create(isbn2, author2, title2)).Return(book2);
                bookFactory.Expect(f => f.Create(isbn3, author3, title3)).Return(book3);
            }

            var actual = marvelParser.Parse(providerXmlData);
            var expected = new[] {book1, book2, book3};

            actual.ShouldAllBeEquivalentTo(expected);
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
