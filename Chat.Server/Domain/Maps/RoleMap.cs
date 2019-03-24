using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Maps.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class RoleMap : VersionedEntityMap<Role>
    {
        public RoleMap()
        {
            References(x => x.UserRoomMembership).Not.Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Map(x => x.Name).Not.Nullable().Length(100);
            Component(x => x.Created).ColumnPrefix("Created");
            Component(x => x.Updated).ColumnPrefix("Updated");
            HasMany(x => x.Permissions).Inverse().Fetch.Join().Cascade.SaveUpdate();
        }

        protected override string GetSchema()
        {
            return SchemaNames.ChatServer;
        }

        protected override string GetTablePrefix()
        {
            return TablePrefixes.Security;
        }
    }
}
