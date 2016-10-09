using System;
using BookStore.Implementation;
using NUnit.Framework;

namespace BookStore.Tests.Implementation
{
    public class BookWrapperTest : UnitTestBase
    {
        public void CtorTestThrowsWhenBookIsNull()
        {
            var providers = new[] {"provider1", "provider2"};
            Assert.Throws<ArgumentNullException>(() => new BookWrapper(null, providers));
        }

        public void CtorTestThrowsWhenProviderListIsNull()
        {
            var book = new Book("isbn", "author", "title");
            Assert.Throws<ArgumentNullException>(() => new BookWrapper(book, null));
        }

        public void CtorTestThrowsWhenProviderListIsEmpty()
        {
            var book = new Book("isbn", "author", "title");
            var providers = new string[0];
            Assert.Throws<ArgumentNullException>(() => new BookWrapper(book, providers));
        }
    }
}
