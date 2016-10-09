using System;
using BookStore.Implementation;
using BookStore.Models.Home;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Models.Home
{
    public class BookListViewModelBuilderTest : UnitTestBase
    {
        private const int maxSearchResultsCount = 20;

        private IBookIndex bookIndex;
        private BookListViewModelBuilder bookListViewModelBuilder;

        protected override void SetUp()
        {
            base.SetUp();

            bookIndex = NewMock<IBookIndex>();
            bookListViewModelBuilder = new BookListViewModelBuilder(bookIndex);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void BuildTestForEmptyQuery(string query)
        {
            var book1 = new BookWrapper(new Book("isbn1", "author1", "title1"), new [] {"provider1"});
            var book2 = new BookWrapper(new Book("isbn2", "author2", "title2"), new [] {"provider1", "provider2"});
            var book3 = new BookWrapper(new Book("isbn3", "author3", "title3"), new [] {"provider2"});

            using (MocksRecord())
            {
                bookIndex.Expect(i => i.Search(query, maxSearchResultsCount)).Return(new[] {book1, book2, book3});
            }

            var expected = new BookListViewModel
                           {
                               Query = query,
                               Books = new[]
                                       {
                                           new BookViewModel {ISBN = book1.Book.ISBN, Author = book1.Book.Author, Title = book1.Book.Title, Providers = book1.Providers},
                                           new BookViewModel {ISBN = book2.Book.ISBN, Author = book2.Book.Author, Title = book2.Book.Title, Providers = book2.Providers},
                                           new BookViewModel {ISBN = book3.Book.ISBN, Author = book3.Book.Author, Title = book3.Book.Title, Providers = book3.Providers},
                                       }
                           };
            var actual = bookListViewModelBuilder.Build(query);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void BuildTestForNormalQuery()
        {
            var query = Guid.NewGuid().ToString();
            var book1 = new BookWrapper(new Book("isbn1", "author1", "title1"), new[] { "provider1" });
            var book2 = new BookWrapper(new Book("isbn2", "author2", "title2"), new[] { "provider1", "provider2" });
            var book3 = new BookWrapper(new Book("isbn3", "author3", "title3"), new[] { "provider2" });

            using (MocksRecord())
            {
                bookIndex.Expect(i => i.Search(query, maxSearchResultsCount)).Return(new[] { book1, book2, book3 });
            }

            var expected = new BookListViewModel
                           {
                               Query = query,
                               Books = new[]
                                       {
                                           new BookViewModel {ISBN = book1.Book.ISBN, Author = book1.Book.Author, Title = book1.Book.Title, Providers = book1.Providers},
                                           new BookViewModel {ISBN = book2.Book.ISBN, Author = book2.Book.Author, Title = book2.Book.Title, Providers = book2.Providers},
                                           new BookViewModel {ISBN = book3.Book.ISBN, Author = book3.Book.Author, Title = book3.Book.Title, Providers = book3.Providers},
                                       }
                           };
            var actual = bookListViewModelBuilder.Build(query);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
