using System.IO;
using System.Linq;
using System.Xml;

namespace BookStore.Implementation.DataProviders
{
    public class XmlFileService : IXmlFileService
    {
        private const string dataDirectory = "Upload";

        public ProviderXmlData[] LoadAll()
        {
            var dataFiles = Directory.GetFiles(dataDirectory, "*.xml", SearchOption.AllDirectories);
            var documents = dataFiles.Select(Create).ToArray();

            return documents;
        }

        private static ProviderXmlData Create(string filePath)
        {
            var xmlDocument = LoadDocument(filePath);
            var providerXmlData = new ProviderXmlData(filePath, xmlDocument);

            return providerXmlData;
        }

        private static XmlDocument LoadDocument(string filePath)
        {
            var document = new XmlDocument();
            document.Load(filePath);

            return document;
        }
    }
}