namespace BookStore.Implementation.DataProviders.Marvel
{
    public interface IMarvelParser
    {
        bool CanParse(ProviderXmlData providerXmlData);
        Book[] Parse(ProviderXmlData providerXmlData);
    }
}