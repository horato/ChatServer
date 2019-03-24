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
    public class RoomPersistenceTest : EntityPersistenceTestBase<Room>
    {
        private string _name = "aa";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();

        private IEnumerable<UserRoomMembership> _users = new List<UserRoomMembership>
        {
            new UserRoomMembershipBuilder().WithRoom(null).Build(),
            new UserRoomMembershipBuilder().WithRoom(null).Build(),
        };

        private IEnumerable<MessageHistory> _messageHistories = new List<MessageHistory>
        {
            new MessageHistoryBuilder().WithRoom(null).Build(),
            new MessageHistoryBuilder().WithRoom(null).Build(),
            new MessageHistoryBuilder().WithRoom(null).Build(),
        };

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<Room> GetEntityBuilder()
        {
            return new RoomBuilder()
                .WithName(_name)
                .WithCreated(_created)
                .WithUpdated(_updated)
                .WithUsers(_users)
                .WithMessageHistories(_messageHistories);
        }

        protected override void AssertEquality(Room savedEntity, Room loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.Name, loadedEntity.Name);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
            Assert.AreEqual(savedEntity.Users.Count(), loadedEntity.Users.Count());
            Assert.AreEqual(savedEntity.MessageHistories.Count(), loadedEntity.MessageHistories.Count());
        }
    }
}
