using System;
using System.Linq;
using System.Xml;
using BookStore.Implementation;
using BookStore.Implementation.DataProviders;
using BookStore.Implementation.DataProviders.Marvel;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Implementation.DataProviders.Marvel
{
    public class MarvelDataProviderTest : UnitTestBase
    {
        private IXmlFileService xmlFileService;
        private IMarvelParser marvelParser;
        private MarvelDataProvider marvelDataProvider;

        protected override void SetUp()
        {
            base.SetUp();

            xmlFileService = NewMock<IXmlFileService>();
            marvelParser = NewMock<IMarvelParser>();
            marvelDataProvider = new MarvelDataProvider(xmlFileService, marvelParser);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("Marvel", marvelDataProvider.Name);
        }

        [Test]
        public void SelectAllTestWhenFilesNotFound()
        {
            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(new ProviderXmlData[0]);
            }

            var actual = marvelDataProvider.SelectAll();
            actual.Should().BeEmpty();
        }

        [Test]
        public void SelectAllTestWhenCantParseAnything()
        {
            var data1 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data2 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data3 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var xmlFiles = new[] {data1, data2, data3};

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
                marvelParser.Expect(p => p.CanParse(data1)).Return(false);
                marvelParser.Expect(p => p.CanParse(data2)).Return(false);
                marvelParser.Expect(p => p.CanParse(data3)).Return(false);
            }

            var actual = marvelDataProvider.SelectAll();
            actual.Should().BeEmpty();
        }

        [Test]
        public void SelectAllTestWhenSomeDataBroken()
        {
            var data1 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data2 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data3 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var xmlFiles = new[] { data1, data2, data3 };
            var books1 = new[]
                        {
                            new Book("isbn11", "author1", "title1"),
                            new Book("isbn12", "author2", "title2"),
                            new Book("isbn13", "author3", "title3")
                        };
            var books3 = new[]
                        {
                            new Book("isbn31", "author1", "title1"),
                            new Book("isbn32", "author2", "title2"),
                            new Book("isbn33", "author3", "title3")
                        };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
                marvelParser.Expect(p => p.CanParse(data1)).Return(true);
                marvelParser.Expect(p => p.Parse(data1)).Return(books1);
                marvelParser.Expect(p => p.CanParse(data2)).Return(true);
                marvelParser.Expect(p => p.Parse(data2)).Throw(new Exception("FATAL ERROR!!!!111"));
                marvelParser.Expect(p => p.CanParse(data3)).Return(true);
                marvelParser.Expect(p => p.Parse(data3)).Return(books3);
            }

            var expected = books1.Concat(books3).ToArray();
            var actual = marvelDataProvider.SelectAll();
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SelectAllTestNormalBehavior()
        {
            var data1 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data2 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var data3 = new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument());
            var xmlFiles = new[] { data1, data2, data3 };
            var books1 = new[]
                        {
                            new Book("isbn11", "author1", "title1"),
                            new Book("isbn12", "author2", "title2"),
                        };
            var books2 = new[]
                         {
                             new Book("isbn21", "author1", "title1"),
                             new Book("isbn22", "author3", "title3")
                         };
            var books3 = new[]
                        {
                            new Book("isbn31", "author1", "title1"),
                            new Book("isbn32", "author2", "title2"),
                        };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
                marvelParser.Expect(p => p.CanParse(data1)).Return(true);
                marvelParser.Expect(p => p.Parse(data1)).Return(books1);
                marvelParser.Expect(p => p.CanParse(data2)).Return(true);
                marvelParser.Expect(p => p.Parse(data2)).Return(books2);
                marvelParser.Expect(p => p.CanParse(data3)).Return(true);
                marvelParser.Expect(p => p.Parse(data3)).Return(books3);
            }

            var expected = books1.Concat(books2).Concat(books3).ToArray();
            var actual = marvelDataProvider.SelectAll();
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
