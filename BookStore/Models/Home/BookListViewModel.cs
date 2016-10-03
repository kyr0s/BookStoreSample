namespace BookStore.Models.Home
{
    public class BookListViewModel
    {
        public string Query { get; set; }
        public BookViewModel[] Books { get; set; }
    }
}