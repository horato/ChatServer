using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps
{
    public class AuditInfoMap : ComponentMap<AuditInfo>
    {
        public AuditInfoMap()
        {
            Map(x => x.On).Not.Nullable();
            Map(x => x.By).Not.Nullable();
        }
    }
}
