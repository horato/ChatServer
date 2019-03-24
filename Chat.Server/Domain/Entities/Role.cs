using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities.Base;

namespace Chat.Server.Domain.Entities
{
    public class Role : VersionedEntity
    {
        public virtual UserRoomMembership UserRoomMembership { get; protected set; }

        public virtual string Name { get; protected set; }
        public virtual AuditInfo Created { get; protected set; }
        public virtual AuditInfo Updated { get; protected set; }

        private readonly ISet<Permission> _permissions = new HashSet<Permission>();
        public virtual IEnumerable<Permission> Permissions => _permissions.AsEnumerable();

        protected Role()
        {

        }

        public Role(string name, AuditInfo created, AuditInfo updated)
        {
            Name = name;
            Created = created;
            Updated = updated;
        }

        public virtual void AddPermissions(IEnumerable<Permission> permissions)
        {
            if (permissions == null)
                throw new ArgumentNullException(nameof(permissions));

            foreach (var permission in permissions)
            {
                AddPermission(permission);
            }
        }

        public virtual void AddPermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            permission.LinkToRole(this);
            _permissions.Add(permission);
        }

        public virtual void LinkToUserRoomMembership(UserRoomMembership userRoomMembership)
        {
            if (userRoomMembership == null)
                throw new ArgumentNullException(nameof(userRoomMembership));
            if (UserRoomMembership != null)
                throw new InvalidOperationException("UserRoomMembership is already set.");

            UserRoomMembership = userRoomMembership;
        }
    }
}
