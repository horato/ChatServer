using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Maps.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class PermissionMap : VersionedEntityMap<Permission>
    {
        public PermissionMap()
        {
            References(x => x.Role).Not.Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Map(x => x.PermissionType).Not.Nullable();
            Component(x => x.Created).ColumnPrefix("Created");
            Component(x => x.Updated).ColumnPrefix("Updated");
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