using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Maps.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class UserRoomMembershipMap : VersionedEntityMap<UserRoomMembership>
    {
        public UserRoomMembershipMap()
        {
            References(x => x.User).Not.Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            References(x => x.Room).Not.Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Component(x => x.Created).ColumnPrefix("Created");
            Component(x => x.Updated).ColumnPrefix("Updated");
            HasMany(x => x.Restrictions).Inverse().Fetch.Join().Cascade.SaveUpdate();
            HasMany(x => x.Roles).Inverse().Fetch.Join().Cascade.SaveUpdate();
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
