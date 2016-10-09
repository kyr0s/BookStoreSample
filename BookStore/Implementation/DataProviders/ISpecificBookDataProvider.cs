namespace BookStore.Implementation.DataProviders
{
    public interface ISpecificBookDataProvider
    {
        string Name { get; }
        Book[] SelectAll();
    }
}
