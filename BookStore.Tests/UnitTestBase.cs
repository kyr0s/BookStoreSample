using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests
{
    [TestFixture]
    abstract class UnitTestBase
    {
        private MockRepository mocks;

        [SetUp]
        protected virtual void SetUp()
        {
            mocks = new MockRepository();
        }

        protected T NewMock<T>()
        {
            return mocks.StrictMock<T>();
        }

        protected IDisposable MocksRecord()
        {
            return mocks.Record();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            mocks.Record().Dispose();
            mocks.VerifyAll();
        }
    }
}
