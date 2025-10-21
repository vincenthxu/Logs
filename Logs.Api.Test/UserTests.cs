using Logs.Api.Models;
using System.Text.Json;

namespace Logs.Api.Test
{
    [TestFixture]
    public class UserTests
    {
        private User? user;
        private DateOnly today;
        [SetUp]
        public void Setup()
        {
            user = null;
            today = DateOnly.FromDateTime(DateTime.Today);
        }

        [Test]
        public void SetDateOfBirth_AfterToday_SetsInvalidDateOfBirthFlag()
        {
            DateOnly tomorrow = today.AddDays(1);
            user = new(name: "Test", dateOfBirth: tomorrow, email: "a@a.com");
            Assert.That(user.HasInvalidDateOfBirth, Is.True);
        }

        [Test]
        public void SetDateOfBirth_Today_InstantiatesUser()
        {
            user = new(name: "Test", dateOfBirth: today, email: "a@a.com");
            Assert.That(user, Is.Not.Null);
            Assert.That(user.DateOfBirth, Is.EqualTo(today));
        }

        [Test]
        public void SetDateOfBirth_BeforeToday_InstantiatesUser()
        {
            DateOnly yesterday = today.AddDays(-1);
            user = new(name: "Test", dateOfBirth: yesterday, email: "a@a.com");
            Assert.That(user, Is.Not.Null);
            Assert.That(user.DateOfBirth, Is.EqualTo(yesterday));
        }

        [Test]
        public void GetAge_DateOfBirthIsThisYear_ReturnsZero()
        {
            user = new(name: "test", dateOfBirth: today, email: "a@a.com");
            Assert.That(user.Age, Is.Zero);
        }

        [Test]
        public void GetAge_DateOfBirthThisYearHasPassed_ReturnsYearsSinceYearOfBirth()
        {
            int years = 20;
            user = new(name: "test1", dateOfBirth: today.AddYears(-years), email: "a@a.com");
            Assert.That(user.Age, Is.EqualTo(years));
        }

        [Test]
        public void GetAge_DateOfBirthThisYearHasNotPassed_ReturnsYearsSinceYearOfBirthMinusOne()
        {
            int years = 20;
            user = new(name: "test2", dateOfBirth: today.AddYears(-years).AddDays(1), email: "a@a.com");
            Assert.That(user.Age, Is.EqualTo(years - 1));
        }

        [Test]
        public void ToString_OnUserInstance_ReturnsJsonSerializedString()
        {
            user = new(name: "test", dateOfBirth: today, email: "a@a.com");
            var deserializedObject = JsonSerializer.Deserialize<User>(user.ToString());
            Assert.That(deserializedObject, Is.TypeOf(typeof(User)));
            Assert.That(deserializedObject.Name, Is.EqualTo("test"));
            Assert.That(deserializedObject.DateOfBirth, Is.EqualTo(today));
        }
    }
}