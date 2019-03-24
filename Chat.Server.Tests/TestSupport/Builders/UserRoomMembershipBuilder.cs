using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class UserRoomMembershipBuilder : EntityBuilderBase<UserRoomMembership>
    {
        private User _user = new UserBuilder().Build();
        private Room _room = new RoomBuilder().Build();
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<UserRoomRestriction> _restrictions = new List<UserRoomRestriction>();
        private IEnumerable<Role> _roles = new List<Role>();

        public UserRoomMembershipBuilder WithUser(User user)
        {
            _user = user;
            return this;
        }

        public UserRoomMembershipBuilder WithRoom(Room room)
        {
            _room = room;
            return this;
        }

        public UserRoomMembershipBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public UserRoomMembershipBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public UserRoomMembershipBuilder WithRestrictions(IEnumerable<UserRoomRestriction> restrictions)
        {
            _restrictions = restrictions ?? new List<UserRoomRestriction>();
            return this;
        }

        public UserRoomMembershipBuilder WithRoles(IEnumerable<Role> roles)
        {
            _roles = roles ?? new List<Role>();
            return this;
        }

        public override UserRoomMembership Build()
        {
            var entity = new UserRoomMembership(_created, _updated);

            entity.AddRoles(_roles);
            entity.AddUserRoomRestrictions(_restrictions);

            if (_user != null)
                entity.LinkToUser(_user);
            if (_room != null)
                entity.LinkToRoom(_room);

            return entity;
        }
    }
}
