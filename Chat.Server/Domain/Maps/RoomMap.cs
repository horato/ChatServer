using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Maps.Base;

namespace Chat.Server.Domain.Maps
{
    public class RoomMap : VersionedEntityMap<Room>
    {
        public RoomMap()
        {
            Map(x => x.Name).Not.Nullable().Length(100);
            Component(x => x.Created).ColumnPrefix("Created");
            Component(x => x.Updated).ColumnPrefix("Updated");
            HasMany(x => x.Users).Inverse().Fetch.Join().Cascade.SaveUpdate();
            HasMany(x => x.MessageHistories).Inverse().Fetch.Join().Cascade.SaveUpdate();
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