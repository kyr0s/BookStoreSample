using System;

namespace BookStore.Implementation
{
    public class BookFactory : IBookFactory
    {
        public Book Create(string isbn, string author, string title)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                throw new ArgumentNullException(nameof(isbn));
            }
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentNullException(nameof(author));
            }
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            return new Book(isbn, author, title);
        }

        public BookWrapper Create(Book book, string[] providers)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }
            if (providers.Length == 0)
            {
                throw new ArgumentException($"Providers list must not be empty", nameof(providers));
            }

            return new BookWrapper(book, providers);
        }
    }
}