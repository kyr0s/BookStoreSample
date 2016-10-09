using System;
using BookStore.Implementation;
using BookStore.Implementation.DataProviders;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Implementation
{
    public class BookIndexBuilderTest : UnitTestBase
    {
        private IBookIndex bookIndex;
        private ISpecificBookDataProvider provider1;
        private ISpecificBookDataProvider provider2;
        private BookIndexBuilder bookIndexBuilder;
        private IBookFactory bookFactory;

        protected override void SetUp()
        {
            base.SetUp();

            bookIndex = NewMock<IBookIndex>();
            bookFactory = NewMock<IBookFactory>();
            provider1 = NewMock<ISpecificBookDataProvider>();
            provider2 = NewMock<ISpecificBookDataProvider>();
            bookIndexBuilder = new BookIndexBuilder(bookIndex, bookFactory, new [] {provider1, provider2});
        }

        [Test]
        public void BuildTestWhenProviderThrowsException()
        {
            const string providerName1 = "Name1";
            const string providerName2 = "Name2";

            var book1 = new Book("1", "1", "1");
            var book2 = new Book("2", "2", "2");

            var bookWrapper1 = new BookWrapper(book1, new []{providerName2});
            var bookWrapper2 = new BookWrapper(book2, new []{providerName2});

            using (MocksRecord())
            {
                provider1.Stub(p => p.Name).Return(providerName1);
                provider2.Stub(p => p.Name).Return(providerName2);

                provider1.Expect(p => p.SelectAll()).Throw(new Exception("FATAL CRASH!!111"));
                provider2
                    .Expect(p => p.SelectAll())
                    .Return(new[] {book1, book2});

                bookFactory.Expect(f => f.Create(book1, new[] {providerName2})).Return(bookWrapper1);
                bookFactory.Expect(f => f.Create(book2, new[] {providerName2})).Return(bookWrapper2);

                bookIndex.Expect(i => i.Rebuild(new[] {bookWrapper1, bookWrapper2}));
            }

            bookIndexBuilder.Build();
        }

        [Test]
        public void BuildTestUniqueBooksFromProviders()
        {
            const string providerName1 = "Name1";
            const string providerName2 = "Name2";

            var book1 = new Book("1", "1", "1");
            var book2 = new Book("2", "2", "2");

            var bookWrapper1 = new BookWrapper(book1, new[] {providerName1});
            var bookWrapper2 = new BookWrapper(book2, new[] {providerName2});

            using (MocksRecord())
            {
                provider1.Stub(p => p.Name).Return(providerName1);
                provider2.Stub(p => p.Name).Return(providerName2);

                provider1.Expect(p => p.SelectAll()).Return(new[] {book1});
                provider2.Expect(p => p.SelectAll()).Return(new[] {book2});

                bookFactory.Expect(f => f.Create(book1, new[] {providerName1})).Return(bookWrapper1);
                bookFactory.Expect(f => f.Create(book2, new[] {providerName2})).Return(bookWrapper2);

                bookIndex.Expect(i => i.Rebuild(new[] {bookWrapper1, bookWrapper2}));
            }

            bookIndexBuilder.Build();
        }

        [Test]
        public void BuildTestNotUniqueBooksFromProviders()
        {
            const string providerName1 = "Name1";
            const string providerName2 = "Name2";

            var book1 = new Book("1", "1", "1");
            var book2 = new Book("2", "2", "2");
            var book3 = new Book("3", "3", "3");

            var bookWrapper1 = new BookWrapper(book1, new[] {providerName1});
            var bookWrapper2 = new BookWrapper(book2, new[] {providerName1, providerName2});
            var bookWrapper3 = new BookWrapper(book3, new[] {providerName2});

            using (MocksRecord())
            {
                provider1.Stub(p => p.Name).Return(providerName1);
                provider2.Stub(p => p.Name).Return(providerName2);

                provider1.Expect(p => p.SelectAll()).Return(new[] {book1, book2});
                provider2.Expect(p => p.SelectAll()).Return(new[] {book2, book3});

                bookFactory.Expect(f => f.Create(book1, new[] {providerName1})).Return(bookWrapper1);
                bookFactory.Expect(f => f.Create(book2, new[] {providerName1, providerName2})).Return(bookWrapper2);
                bookFactory.Expect(f => f.Create(book3, new[] {providerName2})).Return(bookWrapper3);

                bookIndex.Expect(i => i.Rebuild(new[] {bookWrapper1, bookWrapper2, bookWrapper3}));
            }

            bookIndexBuilder.Build();
        }
    }
}
