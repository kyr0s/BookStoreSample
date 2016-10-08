using System;

namespace BookStore.Implementation
{
    public class Book
    {
        public Book(string isbn, string author, string title)
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

            ISBN = isbn;
            Author = author;
            Title = title;
        }

        public string ISBN { get; }
        public string Author { get; }
        public string Title { get; }

        private bool Equals(Book other)
        {
            return string.Equals(ISBN, other.ISBN, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Book) obj);
        }

        public override int GetHashCode()
        {
            return ISBN?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    }
}