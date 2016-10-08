using System;
using BookStore.Implementation;
using NUnit.Framework;

namespace BookStore.Tests.Implementation
{
    public class BookTest : UnitTestBase
    {
        [Test]
        public void EqualsTestWhenIsbnEqualsOnly()
        {
            var isbn = Guid.NewGuid().ToString();
            var book1 = new Book(isbn, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var book2 = new Book(isbn, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsTrue(book1.Equals(book2));
        }

        [Test]
        public void EqualsTestCaseInsensitive()
        {
            var isbn = Guid.NewGuid().ToString();
            var book1 = new Book(isbn.ToUpper(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var book2 = new Book(isbn.ToLower(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsTrue(book1.Equals(book2));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CtorTestThrowsWhenIsbnIsNull(string isbn)
        {
            Assert.Throws<ArgumentNullException>(() => new Book(isbn, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CtorTestThrowsWhenAuthorIsNull(string author)
        {
            Assert.Throws<ArgumentNullException>(() => new Book(Guid.NewGuid().ToString(), author, Guid.NewGuid().ToString()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void CtorTestThrowsWhenTitleIsNull(string title)
        {
            Assert.Throws<ArgumentNullException>(() => new Book(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), title));
        }
    }
}
