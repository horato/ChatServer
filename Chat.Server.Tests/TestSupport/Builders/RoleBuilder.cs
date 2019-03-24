using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class RoleBuilder : EntityBuilderBase<Role>
    {
        private UserRoomMembership _userRoomMembership = new UserRoomMembershipBuilder().Build();
        private string _name = "TEST_ROLE";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<Permission> _permissions = new List<Permission>();

        public RoleBuilder WithUserRoomMembership(UserRoomMembership membership)
        {
            _userRoomMembership = membership;
            return this;
        }

        public RoleBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public RoleBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public RoleBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public RoleBuilder WithPermissions(IEnumerable<Permission> permissions)
        {
            _permissions = permissions ?? new List<Permission>();
            return this;
        }

        public override Role Build()
        {
            var entity = new Role(_name, _created, _updated);

            entity.AddPermissions(_permissions);

            if (_userRoomMembership != null)
                entity.LinkToUserRoomMembership(_userRoomMembership);

            return entity;
        }
    }
}
