using System;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Domain.Entities
{
    public class MessageHistory : VersionedEntity
    {
        public virtual string Message { get; protected set; }
        public virtual MessageHistoryType MessageType { get; protected set; }
        public virtual User From { get; protected set; }
        public virtual Room Room { get; protected set; }
        public virtual User To { get; protected set; }
        public virtual DateTime CreatedOn { get; protected set; }

        protected MessageHistory()
        {

        }

        public MessageHistory(string message, MessageHistoryType messageType, DateTime createdOn)
        {
            Message = message;
            MessageType = messageType;
            CreatedOn = createdOn;
        }

        public virtual void LinkToRoom(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));
            if (Room != null)
                throw new InvalidOperationException("Room is already set.");

            Room = room;
        }

        public virtual void LinkToUserTo(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (To != null)
                throw new InvalidOperationException("To is already set.");

            To = user;

        }

        public virtual void LinkToUserFrom(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (From != null)
                throw new InvalidOperationException("From is already set.");

            From = user;
        }
    }
}
