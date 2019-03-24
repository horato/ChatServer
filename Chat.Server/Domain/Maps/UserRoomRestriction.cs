using System;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Entities.Enums;
using Chat.Server.Domain.Maps.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class UserRoomRestrictionMap : VersionedEntityMap<UserRoomRestriction>
    {
        public UserRoomRestrictionMap()
        {
            References(x => x.UserRoomMembership).Not.Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Map(x => x.RestrictionType).Not.Nullable();
            Map(x => x.IsPermanent).Not.Nullable();
            Map(x => x.EffectiveTo).Nullable();
            Component(x => x.Created).ColumnPrefix("Created");
            Component(x => x.Updated).ColumnPrefix("Updated");
        }

        protected override string GetSchema()
        {
            return SchemaNames.ChatServer;
        }

        protected override string GetTablePrefix()
        {
            return TablePrefixes.Main;
        }
    }
}
