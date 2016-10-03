namespace BookStore.Implementation
{
    public class Book
    {
        public Book(string isbn, string author, string title)
        {
            ISBN = isbn;
            Author = author;
            Title = title;
        }

        public string ISBN { get; }
        public string Author { get; }
        public string Title { get; }

        protected bool Equals(Book other)
        {
            return string.Equals(ISBN, other.ISBN);
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
            return ISBN?.GetHashCode() ?? 0;
        }
    }
}