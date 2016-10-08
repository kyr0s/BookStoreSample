using System;
using System.IO;
using System.Linq;
using System.Xml;
using BookStore.Implementation.Utils;
using NLog;

namespace BookStore.Implementation.DataProviders
{
    public class XmlFileService : IXmlFileService
    {
        private const string dataDirectory = "Upload";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IPathUtility pathUtility;
        private readonly IFileWrapper file;
        private readonly IDirectoryWrapper directory;

        public XmlFileService(
            IPathUtility pathUtility,
            IFileWrapper file,
            IDirectoryWrapper directory)
        {
            this.pathUtility = pathUtility;
            this.file = file;
            this.directory = directory;
        }

        public ProviderXmlData[] LoadAll()
        {
            var absolutePath = pathUtility.GetAbsolutePath(dataDirectory);

            if (!directory.Exists(absolutePath))
            {
                return new ProviderXmlData[0];
            }

            var dataFiles = directory.GetFiles(absolutePath, "*.xml", SearchOption.AllDirectories);
            var documents = dataFiles
                .Select(Create)
                .Where(f => f != null)
                .ToArray();

            return documents;
        }

        private ProviderXmlData Create(string filePath)
        {
            XmlDocument xmlDocument;
            var providerXmlData = TryLoadDocument(filePath, out xmlDocument)
                ? new ProviderXmlData(filePath, xmlDocument)
                : null;

            return providerXmlData;
        }

        private bool TryLoadDocument(string filePath, out XmlDocument document)
        {
            try
            {
                using (var stream = file.OpenRead(filePath))
                using (var reader = XmlReader.Create(stream))
                {
                    document = new XmlDocument();
                    document.Load(reader);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Can't load xml document from {filePath}");

                document = null;
                return false;
            }
        }
    }
}