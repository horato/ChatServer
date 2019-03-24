using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Maps.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class MessageHistoryMap : VersionedEntityMap<MessageHistory>
    {
        public MessageHistoryMap()
        {
            Map(x => x.Message).Not.Nullable().Length(10000); // nvarchar(max)
            Map(x => x.MessageType).Not.Nullable();
            References(x => x.From).Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            References(x => x.To).Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            References(x => x.Room).Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Map(x => x.CreatedOn).Not.Nullable();
        }

        protected override string GetSchema()
        {
            return SchemaNames.ChatServer;
        }

        protected override string GetTablePrefix()
        {
            return TablePrefixes.History;
        }
    }
}
