using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities.Base;

namespace Chat.Server.Domain.Entities
{
    public class UserRoomMembership : VersionedEntity
    {
        public virtual User User { get; protected set; }
        public virtual Room Room { get; protected set; }
        public virtual AuditInfo Created { get; protected set; }
        public virtual AuditInfo Updated { get; protected set; }

        private readonly ISet<UserRoomRestriction> _restrictions = new HashSet<UserRoomRestriction>();
        public virtual IEnumerable<UserRoomRestriction> Restrictions => _restrictions.AsEnumerable();

        private readonly ISet<Role> _roles = new HashSet<Role>();
        public virtual IEnumerable<Role> Roles => _roles.AsEnumerable();

        protected UserRoomMembership()
        {

        }

        public UserRoomMembership(AuditInfo created, AuditInfo updated)
        {
            Created = created;
            Updated = updated;
        }

        public virtual void LinkToRoom(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));
            if (Room != null)
                throw new InvalidOperationException("Room is already set.");

            Room = room;
        }

        public virtual void LinkToUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (User != null)
                throw new InvalidOperationException("User is already set.");

            User = user;
        }

        public virtual void AddUserRoomRestrictions(IEnumerable<UserRoomRestriction> restrictions)
        {
            if (restrictions == null)
                throw new ArgumentNullException(nameof(restrictions));

            foreach (var restriction in restrictions)
            {
                AddUserRoomRestriction(restriction);
            }
        }

        public virtual void AddUserRoomRestriction(UserRoomRestriction restriction)
        {
            if (restriction == null)
                throw new ArgumentNullException(nameof(restriction));

            restriction.LinkToUserRoomMembership(this);
            _restrictions.Add(restriction);
        }

        public virtual void AddRoles(IEnumerable<Role> roles)
        {
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));

            foreach (var role in roles)
            {
                AddRole(role);
            }
        }

        public virtual void AddRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            role.LinkToUserRoomMembership(this);
            _roles.Add(role);
        }
    }
}
