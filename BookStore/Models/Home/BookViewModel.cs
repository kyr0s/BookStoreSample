namespace BookStore.Models.Home
{
    public class BookViewModel
    {
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string[] Providers { get; set; }
    }
}