using System;
using System.IO;
using System.Linq;
using NLog;

namespace BookStore.Implementation.DataProviders.DC
{
    public class DCComicsDataProvider : ISpecificBookDataProvider
    {
        private const string dcDataFileName = "dc.xml";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IXmlFileService xmlFileService;
        private readonly IDCComicsParser dcComicsParser;

        public DCComicsDataProvider(
            IXmlFileService xmlFileService,
            IDCComicsParser dcComicsParser)
        {
            this.xmlFileService = xmlFileService;
            this.dcComicsParser = dcComicsParser;
        }

        public string ProviderName => "DC Comics";

        public Book[] SelectAll()
        {
            var dcData = xmlFileService.LoadAll().SingleOrDefault(d => IsDCDataFile(d.FilePath));
            if (dcData == null)
            {
                return new Book[0];
            }

            try
            {
                var books = dcComicsParser.Parse(dcData);
                return books;
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Can't parse DC Comics file \"{dcData.FilePath}\"");
                return new Book[0];
            }
        }

        private bool IsDCDataFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            return string.Equals(dcDataFileName, fileName, StringComparison.OrdinalIgnoreCase);
        }
    }
}