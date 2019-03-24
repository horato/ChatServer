using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class RoomBuilder : EntityBuilderBase<Room>
    {
        private string _name = "TEST_ROOM";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<UserRoomMembership> _users = new List<UserRoomMembership>();
        private IEnumerable<MessageHistory> _messageHistories = new List<MessageHistory>();

        public RoomBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public RoomBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public RoomBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public RoomBuilder WithUsers(IEnumerable<UserRoomMembership> users)
        {
            _users = users ?? new List<UserRoomMembership>();
            return this;
        }

        public RoomBuilder WithMessageHistories(IEnumerable<MessageHistory> messageHistories)
        {
            _messageHistories = messageHistories ?? new List<MessageHistory>();
            return this;
        }

        public override Room Build()
        {
            var entity = new Room(_name, _created, _updated);

            entity.AddMessageHistories(_messageHistories);
            entity.AddUserRoomMemberships(_users);

            return entity;
        }
    }
}
