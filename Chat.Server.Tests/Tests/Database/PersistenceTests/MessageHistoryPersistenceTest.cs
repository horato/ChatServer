using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Enums;
using Chat.Server.Tests.TestSupport.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Server.Tests.Tests.Database.PersistenceTests
{
    [TestClass]
    public class MessageHistoryPersistenceTest : EntityPersistenceTestBase<MessageHistory>
    {
        private string _message = "aa";
        private MessageHistoryType _messageType = MessageHistoryType.Private;
        private User _from = new UserBuilder().Build();
        private Room _room = new RoomBuilder().Build();
        private User _to = new UserBuilder().Build();
        private DateTime _createdOn = new DateTime(1111, 1, 1);

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<MessageHistory> GetEntityBuilder()
        {
            return new MessageHistoryBuilder()
                .WithMessage(_message)
                .WithMessageType(_messageType)
                .WithFrom(_from)
                .WithRoom(_room)
                .WithTo(_to)
                .WithCreatedOn(_createdOn);
        }

        protected override void AssertEquality(MessageHistory savedEntity, MessageHistory loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.Message, loadedEntity.Message);
            Assert.AreEqual(savedEntity.MessageType, loadedEntity.MessageType);
            Assert.AreEqual(savedEntity.From, loadedEntity.From);
            Assert.AreEqual(savedEntity.Room, loadedEntity.Room);
            Assert.AreEqual(savedEntity.To, loadedEntity.To);
            Assert.AreEqual(savedEntity.CreatedOn, loadedEntity.CreatedOn);
        }
    }
}
