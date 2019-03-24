using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Tests.TestSupport.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Server.Tests.Tests.Database.PersistenceTests
{
    [TestClass]
    public class UserPersistenceTest : EntityPersistenceTestBase<User>
    {
        private string _name = "aa";
        private string _login = "bb";
        private string _password = "cc";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<UserRoomMembership> _rooms = new List<UserRoomMembership>
        {
            new UserRoomMembershipBuilder().WithUser(null).Build(),
            new UserRoomMembershipBuilder().WithUser(null).Build(),
        };
        private IEnumerable<MessageHistory> _messageHistories = new List<MessageHistory>
        {
            new MessageHistoryBuilder().WithFrom(null).Build(),
            new MessageHistoryBuilder().WithFrom(null).Build(),
            new MessageHistoryBuilder().WithFrom(null).Build(),
        };

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<User> GetEntityBuilder()
        {
            return new UserBuilder()
                .WithName(_name)
                .WithLogin(_login)
                .WithPassword(_password)
                .WithCreated(_created)
                .WithUpdated(_updated)
                .WithRooms(_rooms)
                .WithMessageHistories(_messageHistories);
        }

        protected override void AssertEquality(User savedEntity, User loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.Name, loadedEntity.Name);
            Assert.AreEqual(savedEntity.Login, loadedEntity.Login);
            Assert.AreEqual(savedEntity.Password, loadedEntity.Password);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
            Assert.AreEqual(savedEntity.Rooms.Count(), loadedEntity.Rooms.Count());
            Assert.AreEqual(savedEntity.MessageHistories.Count(), loadedEntity.MessageHistories.Count());
        }
    }
}
