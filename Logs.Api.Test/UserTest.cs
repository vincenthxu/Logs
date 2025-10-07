using Logs.Api.Models;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Logs.Api.Test
{
    [TestFixture]
    public class UserTest
    {
        private User? user;
        private DateTime today;
        [SetUp]
        public void SetUp()
        {
            user = null;
            today = DateTime.Today;
        }
        [Test]
        public void DateOfBirth_AfterToday_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                user = new(name: "Test", dateOfBirth: new DateOnly(today.Year, today.Month, today.Day + 1))
            );
            Assert.That(user, Is.Null);
        }

        [Test]
        public void DateOfBirth_Today_InstantiatesUser()
        {
            user = new(name: "Test", dateOfBirth: new DateOnly(today.Year, today.Month, today.Day));
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void DateOfBirth_BeforeToday_InstantiatesUser()
        {
            user = new(name: "Test", dateOfBirth: new DateOnly(today.Year, today.Month, today.Day - 1));
            Assert.That(user, Is.Not.Null);
        }
    }
}