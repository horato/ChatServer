using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class UserRoomRestrictionBuilder : EntityBuilderBase<UserRoomRestriction>
    {
        private UserRoomMembership _userRoomMembership = new UserRoomMembershipBuilder().Build();
        private UserRestrictionType _restrictionType = UserRestrictionType.Ban;
        private bool _isPermanent = true;
        private DateTime? _effectiveTo = new DateTime(2022, 7, 15);
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();

        public UserRoomRestrictionBuilder WithUserRoomMembership(UserRoomMembership membership)
        {
            _userRoomMembership = membership;
            return this;
        }

        public UserRoomRestrictionBuilder WithRestrictionType(UserRestrictionType restrictionType)
        {
            _restrictionType = restrictionType;
            return this;
        }

        public UserRoomRestrictionBuilder WithIsPermanent(bool isPermanent)
        {
            _isPermanent = isPermanent;
            return this;
        }

        public UserRoomRestrictionBuilder WithEffectiveTo(DateTime? effectiveTo)
        {
            _effectiveTo = effectiveTo;
            return this;
        }

        public UserRoomRestrictionBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public UserRoomRestrictionBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public override UserRoomRestriction Build()
        {
            var entity = new UserRoomRestriction(_restrictionType, _isPermanent, _effectiveTo, _created, _updated);

            if (_userRoomMembership != null)
                entity.LinkToUserRoomMembership(_userRoomMembership);

            return entity;
        }
    }
}
