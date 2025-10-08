using Logs.Api.Models;
using System.Text.Json;

namespace Logs.Api.Test
{
    [TestFixture]
    public class EntryTests
    {
        private User user;
        private Entry entry;
        [SetUp]
        public void Setup()
        {
            user = new(name: "test", dateOfBirth: DateOnly.FromDateTime(DateTime.Today));
            entry = new(
                userId: user.Id,
                date: DateOnly.FromDateTime(DateTime.Today),
                time: TimeOnly.FromDateTime(DateTime.Now),
                bristolStoolScale: BristolStoolScale.Type4
            );
            entry.User = user;
        }

        [Test]
        public void SetId_NewId_SetsCorrectly()
        {
            int id = 420;
            Assert.That(entry.Id, Is.Not.EqualTo(id));

            entry.Id = id;
            Assert.That(entry.Id, Is.EqualTo(id));
        }

        [Test]
        public void ToString_OnInstance_ReturnsJsonSerializedString()
        {
            var deserializedObject = JsonSerializer.Deserialize<Entry>(entry.ToString());
            Assert.That(deserializedObject, Is.TypeOf(typeof(Entry)));
            Assert.That(deserializedObject.Id, Is.EqualTo(entry.Id));
            Assert.That(deserializedObject.UserId, Is.EqualTo(entry.UserId));
            Assert.That(deserializedObject.Date, Is.EqualTo(entry.Date));
            Assert.That(deserializedObject.Time, Is.EqualTo(entry.Time));
        }
    }
}