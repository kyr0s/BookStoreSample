using System;
using BookStore.Implementation;
using FluentAssertions;
using NUnit.Framework;

namespace BookStore.Tests.Implementation
{
    public class BookFactoryTest : UnitTestBase
    {
        private BookFactory bookFactory;

        protected override void SetUp()
        {
            base.SetUp();
            bookFactory = new BookFactory();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CreateBookTestThrowsExceptionWhenIsbnIsEmpty(string isbn)
        {
            Assert.Throws<ArgumentNullException>(() => bookFactory.Create(isbn, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CreateBookTestThrowsExceptionWhenAuthorIsEmpty(string author)
        {
            Assert.Throws<ArgumentNullException>(() => bookFactory.Create(Guid.NewGuid().ToString(), author, Guid.NewGuid().ToString()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CreateBookTestThrowsExceptionWhenTitleIsEmpty(string title)
        {
            Assert.Throws<ArgumentNullException>(() => bookFactory.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), title));
        }

        [Test]
        public void CreateBookTestNormalBehavior()
        {
            var isbn = Guid.NewGuid().ToString();
            var author = Guid.NewGuid().ToString();
            var title = Guid.NewGuid().ToString();

            var expected = new Book(isbn, author, title);
            var actual = bookFactory.Create(isbn, author, title);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void CreateBookWrapperTestThrowsExceptionWhenBookIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => bookFactory.Create(null, new[] {"someProvider"}));
        }

        [Test]
        public void CreateBookWrapperTestThrowsExceptionWhenProvidersListIsNull()
        {
            var book = new Book("isbn", "author", "title");
            Assert.Throws<ArgumentNullException>(() => bookFactory.Create(book, null));
        }

        [Test]
        public void CreateBookWrapperTestThrowsExceptionWhenProvidersListIsEmpty()
        {
            var book = new Book("isbn", "author", "title");
            var providers = new string[0];
            Assert.Throws<ArgumentException>(() => bookFactory.Create(book, providers));
        }

        [Test]
        public void CreateBookWrapperTestNormalBehavior()
        {
            var book = new Book("isbn", "author", "title");
            var providers = new[] {"provider1", "provider2"};

            var expected = new BookWrapper(book, providers);
            var actual = bookFactory.Create(book, providers);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
