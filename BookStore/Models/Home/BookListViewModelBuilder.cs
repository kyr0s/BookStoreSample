using System.Linq;
using BookStore.Implementation;

namespace BookStore.Models.Home
{
    public class BookListViewModelBuilder : IBookListViewModelBuilder
    {
        private const int maxResults = 20;
        private readonly IBookSearchService bookSearchService;

        public BookListViewModelBuilder(IBookSearchService bookSearchService)
        {
            this.bookSearchService = bookSearchService;
        }

        public BookListViewModel Build(string query)
        {
            var searchResult = bookSearchService.Search(query, maxResults);
            var books = searchResult
                .Select(b => Create(b.Book.ISBN, b.Book.Author, b.Book.Title, b.Providers))
                .ToArray();

            return new BookListViewModel
            {
                Query = query,
                Books = books
            };
        }

        private static BookViewModel Create(string isbn, string author, string title, string[] providers)
        {
            return new BookViewModel
            {
                ISBN = isbn,
                Author = author,
                Title = title,
                Providers = providers
            };
        }
    }
}