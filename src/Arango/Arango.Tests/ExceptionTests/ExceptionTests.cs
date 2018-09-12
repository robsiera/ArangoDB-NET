using Arango.Client;
using NUnit.Framework;
using System;

namespace Arango.Tests.ExceptionTests
{
    [TestFixture()]
    public class ExceptionTests : IDisposable
    {
        [Test()]
        public void Should_throw_exception()
        {
            // given
            ASettings.ThrowExceptions = true;

            // when
            var arangoException = Assert.Throws<AException>(() =>
            {
                var db = new ADatabase(Database.SystemAlias);
                var resultCreate = db.Create("*/-+");
            });

            // then
            Assert.IsNotNull(arangoException);
            Assert.AreEqual(400, arangoException.StatusCode);
            Assert.That(arangoException.Message, Is.Not.Null.And.Not.Empty);
        }

        public void Dispose()
        {
            ASettings.ThrowExceptions = false;
        }
    }
}
