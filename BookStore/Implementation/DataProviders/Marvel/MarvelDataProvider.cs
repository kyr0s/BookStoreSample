using System;
using System.Linq;
using NLog;

namespace BookStore.Implementation.DataProviders.Marvel
{
    public class MarvelDataProvider : ISpecificBookDataProvider
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IXmlFileService xmlFileService;
        private readonly IMarvelParser marvelParser;

        public MarvelDataProvider(
            IXmlFileService xmlFileService,
            IMarvelParser marvelParser)
        {
            this.xmlFileService = xmlFileService;
            this.marvelParser = marvelParser;
        }

        public string Name => "Marvel";

        public Book[] SelectAll()
        {
            var providerXmlDocuments = xmlFileService.LoadAll();
            var books = providerXmlDocuments
                .Where(d => marvelParser.CanParse(d))
                .SelectMany(SafeParse)
                .ToArray();
            return books;
        }

        private Book[] SafeParse(ProviderXmlData providerXmlData)
        {
            try
            {
                return marvelParser.Parse(providerXmlData);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Can't parse Marvel xml data file \"{providerXmlData.FilePath}\"");
                return new Book[0];
            }
        }
    }
}