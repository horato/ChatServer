using System;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Domain.Entities
{
    public class Permission : VersionedEntity
    {
        public virtual Role Role { get; protected set; }

        public virtual PermissionType PermissionType { get; protected set; }
        public virtual AuditInfo Created { get; protected set; }
        public virtual AuditInfo Updated { get; protected set; }

        protected Permission()
        {

        }

        public Permission(PermissionType permissionType, AuditInfo created, AuditInfo updated)
        {
            PermissionType = permissionType;
            Created = created;
            Updated = updated;
        }

        public virtual void LinkToRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (Role != null)
                throw new InvalidOperationException("Role is already set.");

            Role = role;
        }
    }
}