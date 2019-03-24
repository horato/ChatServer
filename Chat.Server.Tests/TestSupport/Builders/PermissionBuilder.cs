using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class PermissionBuilder : EntityBuilderBase<Permission>
    {
        private Role _role = new RoleBuilder().Build();
        private PermissionType _permissionType = PermissionType.All;
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();

        public PermissionBuilder WithRole(Role role)
        {
            _role = role;
            return this;
        }

        public PermissionBuilder WithPermissionType(PermissionType permissionType)
        {
            _permissionType = permissionType;
            return this;
        }

        public PermissionBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public PermissionBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public override Permission Build()
        {
            var entity = new Permission(_permissionType, _created, _updated);
            if (_role != null)
                entity.LinkToRole(_role);

            return entity;
        }
    }
}
