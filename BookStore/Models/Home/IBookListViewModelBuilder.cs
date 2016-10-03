namespace BookStore.Models.Home
{
    public interface IBookListViewModelBuilder
    {
        BookListViewModel Build(string query);
    }
}