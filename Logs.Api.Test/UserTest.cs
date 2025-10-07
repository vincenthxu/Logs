using Logs.Api.Models;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Logs.Api.Test
{
    [TestFixture]
    public class UserTest
    {
        private User? user;
        private DateOnly today;
        [SetUp]
        public void SetUp()
        {
            user = null;
            today = DateOnly.FromDateTime(DateTime.Today);
        }
        [Test]
        public void DateOfBirth_AfterToday_ThrowsException()
        {
            DateOnly tomorrow = today.AddDays(1);
            Assert.Throws<InvalidOperationException>(() =>
                user = new(name: "Test", dateOfBirth: tomorrow)
            );
            Assert.That(user, Is.Null);
        }

        [Test]
        public void DateOfBirth_Today_InstantiatesUser()
        {
            user = new(name: "Test", dateOfBirth: today);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.DateOfBirth, Is.EqualTo(today));
        }

        [Test]
        public void DateOfBirth_BeforeToday_InstantiatesUser()
        {
            DateOnly yesterday = today.AddDays(-1);
            user = new(name: "Test", dateOfBirth: yesterday);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.DateOfBirth, Is.EqualTo(yesterday));
        }
    }
}