using System;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Entities.Enums;

namespace Chat.Server.Domain.Entities
{
    public class UserRoomRestriction : VersionedEntity
    {
        public virtual UserRoomMembership UserRoomMembership { get; protected set; }

        public virtual UserRestrictionType RestrictionType { get; protected set; }
        public virtual bool IsPermanent { get; protected set; }
        public virtual DateTime? EffectiveTo { get; protected set; }
        public virtual AuditInfo Created { get; protected set; }
        public virtual AuditInfo Updated { get; protected set; }

        protected UserRoomRestriction()
        {

        }

        public UserRoomRestriction(UserRestrictionType restrictionType, bool isPermanent, DateTime? effectiveTo, AuditInfo created, AuditInfo updated)
        {
            RestrictionType = restrictionType;
            IsPermanent = isPermanent;
            EffectiveTo = effectiveTo;
            Created = created;
            Updated = updated;
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
