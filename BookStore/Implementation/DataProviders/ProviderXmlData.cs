using System.Xml;

namespace BookStore.Implementation.DataProviders
{
    public class ProviderXmlData
    {
        public ProviderXmlData(string filePath, XmlDocument data)
        {
            FilePath = filePath;
            Data = data;
        }

        public string FilePath { get; }
        public XmlDocument Data { get; }
    }
}