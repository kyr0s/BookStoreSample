namespace BookStore.Implementation.DataProviders
{
    public interface ISpecificBookDataProvider
    {
        string ProviderName { get; }
        Book[] SelectAll();
    }
}
