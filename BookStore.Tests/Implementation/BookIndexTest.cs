using BookStore.Implementation;
using FluentAssertions;
using NUnit.Framework;

namespace BookStore.Tests.Implementation
{
    public class BookIndexTest : UnitTestBase
    {
        private BookIndex bookIndex;

        protected override void SetUp()
        {
            base.SetUp();
            bookIndex = new BookIndex();
            InitializeIndex();
        }

        private void InitializeIndex()
        {
            var books = new[]
                        {
                            new BookWrapper(new Book("978-953-51-2650-8", "Faris Yilmaz", "High Performance Concrete Technology and Applications"), new[] {"provider1"}),
                            new BookWrapper(new Book("978-953-51-2636-2", "Mohamed Amine Fakhfakh", "Modeling and Simulation for Electric Vehicle Applications"), new[] {"provider2"}),
                            new BookWrapper(new Book("978-953-51-2642-3", "Sonia A.C. Carabineiro", "Advances in Carbon Nanostructures"), new[] {"provider1", "provider3"}),
                            new BookWrapper(new Book("978-953-51-2660-7", "Theodore Hromadka ", "Topics in Climate Modeling"), new[] {"provider2"}),
                            new BookWrapper(new Book("978-953-51-2690-4", "Faris Yilmaz", "Conducting Polymers"), new[] {"provider3"}),
                        };
            bookIndex.Rebuild(books);
        }

        [Test]
        public void SearchTestByIsbn()
        {
            var foundBooks = bookIndex.Search("978-953-51-2660-7", 50);
            foundBooks.Should().HaveCount(1);

            var expected = new BookWrapper(new Book("978-953-51-2660-7", "Theodore Hromadka ", "Topics in Climate Modeling"), new[] {"provider2"});
            var actual = foundBooks[0];
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SearchTestByIsbnPrefix()
        {
            var foundBooks = bookIndex.Search("978-953-51-266", 50);
            foundBooks.Should().HaveCount(1);

            var expected = new BookWrapper(new Book("978-953-51-2660-7", "Theodore Hromadka ", "Topics in Climate Modeling"), new[] { "provider2" });
            var actual = foundBooks[0];
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SearchTestByUniqueAuthor()
        {
            var foundBooks = bookIndex.Search("Sonia Carabineiro", 50);
            foundBooks.Should().HaveCount(1);

            var expected = new BookWrapper(new Book("978-953-51-2642-3", "Sonia A.C. Carabineiro", "Advances in Carbon Nanostructures"), new[] {"provider1", "provider3"});
            var actual = foundBooks[0];
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SearchTestByNonUniqueAuthor()
        {
            var foundBooks = bookIndex.Search("Faris Yilmaz", 50);
            foundBooks.Should().HaveCount(2);

            var expected1 = new BookWrapper(new Book("978-953-51-2650-8", "Faris Yilmaz", "High Performance Concrete Technology and Applications"), new[] {"provider1"});
            var expected2 = new BookWrapper(new Book("978-953-51-2690-4", "Faris Yilmaz", "Conducting Polymers"), new[] {"provider3"});
            var actual = foundBooks;
            actual.ShouldBeEquivalentTo(new[] {expected1, expected2});
        }

        [Test]
        public void SearchTestByUniqueTitle()
        {
            var foundBooks = bookIndex.Search("Concrete Technology", 50);
            foundBooks.Should().HaveCount(1);

            var expected = new BookWrapper(new Book("978-953-51-2650-8", "Faris Yilmaz", "High Performance Concrete Technology and Applications"), new[] {"provider1"});
            var actual = foundBooks[0];
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SearchTestByNonUniqueTitle()
        {
            var foundBooks = bookIndex.Search("Applications", 50);
            foundBooks.Should().HaveCount(2);

            var expected1 = new BookWrapper(new Book("978-953-51-2650-8", "Faris Yilmaz", "High Performance Concrete Technology and Applications"), new[] {"provider1"});
            var expected2 = new BookWrapper(new Book("978-953-51-2636-2", "Mohamed Amine Fakhfakh", "Modeling and Simulation for Electric Vehicle Applications"), new[] {"provider2"});
            var actual = foundBooks;
            actual.ShouldBeEquivalentTo(new[] {expected1, expected2});
        }
    }
}
