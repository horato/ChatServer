using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities.Base;

namespace Chat.Server.Domain.Entities
{
    public class Room : VersionedEntity
    {
        public virtual string Name { get; protected set; }
        public virtual AuditInfo Created { get; protected set; }
        public virtual AuditInfo Updated { get; protected set; }

        private readonly ISet<UserRoomMembership> _users = new HashSet<UserRoomMembership>();
        public virtual IEnumerable<UserRoomMembership> Users => _users.AsEnumerable();

        private readonly ISet<MessageHistory> _messageHistories = new HashSet<MessageHistory>();
        public virtual IEnumerable<MessageHistory> MessageHistories => _messageHistories.AsEnumerable();

        protected Room()
        {

        }

        public Room(string name, AuditInfo created, AuditInfo updated)
        {
            Name = name;
            Created = created;
            Updated = updated;
        }

        public virtual void AddUserRoomMemberships(IEnumerable<UserRoomMembership> memberships)
        {
            if (memberships == null)
                throw new ArgumentNullException(nameof(memberships));

            foreach (var membership in memberships)
            {
                AddUserRoomMembership(membership);
            }
        }

        public virtual void AddUserRoomMembership(UserRoomMembership membership)
        {
            if (membership == null)
                throw new ArgumentNullException(nameof(membership));

            membership.LinkToRoom(this);
            _users.Add(membership);
        }

        public virtual void AddMessageHistories(IEnumerable<MessageHistory> histories)
        {
            if (histories == null)
                throw new ArgumentNullException(nameof(histories));

            foreach (var history in histories)
            {
                AddMessageHistory(history);
            }
        }

        public virtual void AddMessageHistory(MessageHistory history)
        {
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            history.LinkToRoom(this);
            _messageHistories.Add(history);
        }
    }
}