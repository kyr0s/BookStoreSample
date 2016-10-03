namespace BookStore.Implementation.DataProviders
{
    public interface IProviderXmlParser
    {
        string ProviderName { get; }
        bool CanParse(ProviderXmlData providerXmlData);
        Book[] Parse(ProviderXmlData providerXmlData);
    }
}