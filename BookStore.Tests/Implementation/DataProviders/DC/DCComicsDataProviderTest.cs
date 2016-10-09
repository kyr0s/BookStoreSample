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
    public class DCComicsDataProviderTest : UnitTestBase
    {
        private IXmlFileService xmlFileService;
        private IDCComicsParser dcComicsParser;
        private DCComicsDataProvider dcComicsDataProvider;

        protected override void SetUp()
        {
            base.SetUp();

            xmlFileService = NewMock<IXmlFileService>();
            dcComicsParser = NewMock<IDCComicsParser>();
            dcComicsDataProvider = new DCComicsDataProvider(xmlFileService, dcComicsParser);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("DC Comics", dcComicsDataProvider.Name);
        }

        [Test]
        public void SelectAllTestWhenDataFileNotFound()
        {
            var xmlFiles = new []
                           {
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                           };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
            }

            var actual = dcComicsDataProvider.SelectAll();
            actual.Should().BeEmpty();
        }

        [Test]
        public void SelectAllTestThrowsExceptionWhenSeveralDataFilesFound()
        {
            var xmlFiles = new[]
                           {
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}\dc.xml", new XmlDocument()),
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}\DC.xml", new XmlDocument()),
                           };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
            }

            Assert.Throws<InvalidOperationException>(() => dcComicsDataProvider.SelectAll());
        }

        [Test]
        public void SelectAllTestWhenParserThrowsException()
        {
            var dcData = new ProviderXmlData($@"c:\{Guid.NewGuid()}\dc.xml", new XmlDocument());
            var xmlFiles = new[]
                           {
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                               dcData,
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                           };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
                dcComicsParser.Expect(p => p.Parse(dcData)).Throw(new Exception());
            }

            var actual = dcComicsDataProvider.SelectAll();
            actual.Should().BeEmpty();
        }

        [Test]
        public void SelectAllTestNormalBehavior()
        {
            var dcData = new ProviderXmlData($@"c:\{Guid.NewGuid()}\dc.xml", new XmlDocument());
            var xmlFiles = new[]
                           {
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                               dcData,
                               new ProviderXmlData($@"c:\{Guid.NewGuid()}.xml", new XmlDocument()),
                           };
            var books = new []
                        {
                            new Book("isbn1", "author1", "title1"),
                            new Book("isbn2", "author2", "title2"),
                            new Book("isbn3", "author3", "title3")
                        };

            using (MocksRecord())
            {
                xmlFileService.Expect(s => s.LoadAll()).Return(xmlFiles);
                dcComicsParser.Expect(p => p.Parse(dcData)).Return(books);
            }

            var actual = dcComicsDataProvider.SelectAll();
            actual.ShouldBeEquivalentTo(books);
        }
    }
}
