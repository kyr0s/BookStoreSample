namespace BookStore.Implementation.DataProviders
{
    public interface IXmlFileService
    {
        ProviderXmlData[] LoadAll();
    }
}