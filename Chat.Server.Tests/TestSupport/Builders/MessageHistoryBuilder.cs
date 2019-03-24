using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class MessageHistoryBuilder : EntityBuilderBase<MessageHistory>
    {
        private string _message = "A test message";
        private MessageHistoryType _messageType = MessageHistoryType.Public;
        private User _from = new UserBuilder().Build();
        private Room _room = new RoomBuilder().Build();
        private User _to = new UserBuilder().Build();
        private DateTime _createdOn = new DateTime(2020, 5, 14);

        public MessageHistoryBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public MessageHistoryBuilder WithMessageType(MessageHistoryType messageType)
        {
            _messageType = messageType;
            return this;
        }

        public MessageHistoryBuilder WithFrom(User from)
        {
            _from = from;
            return this;
        }

        public MessageHistoryBuilder WithRoom(Room room)
        {
            _room = room;
            return this;
        }

        public MessageHistoryBuilder WithTo(User to)
        {
            _to = to;
            return this;
        }

        public MessageHistoryBuilder WithCreatedOn(DateTime createdOn)
        {
            _createdOn = createdOn;
            return this;
        }


        public override MessageHistory Build()
        {
            var entity = new MessageHistory(_message, _messageType, _createdOn);
            if (_from != null)
                entity.LinkToUserFrom(_from);
            if (_to != null)
                entity.LinkToUserTo(_to);
            if (_room != null)
                entity.LinkToRoom(_room);

            return entity;
        }
    }
}
