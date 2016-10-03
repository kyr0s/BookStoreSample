namespace BookStore.Implementation.DataProviders
{
    public interface IBookDataProvider
    {
        BookWrapper[] SelectAll();
    }
}