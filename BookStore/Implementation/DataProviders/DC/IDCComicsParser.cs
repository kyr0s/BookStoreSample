namespace BookStore.Implementation.DataProviders.DC
{
    public interface IDCComicsParser
    {
        Book[] Parse(ProviderXmlData providerXmlData);
    }
}