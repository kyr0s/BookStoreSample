using System;

namespace BookStore.Implementation
{
    public class BookWrapper
    {
        public BookWrapper(Book book, string[] providers)
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

            Book = book;
            Providers = providers;
        }

        public Book Book { get; }
        public string[] Providers { get; }
    }
}