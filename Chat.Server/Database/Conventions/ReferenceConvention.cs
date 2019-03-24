using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Chat.Server.Database.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.ForeignKey($"FK_{instance.EntityType.Name}_{instance.Name}");
            instance.Index($"IDX_{instance.EntityType.Name}_{instance.Property.Name}");
        }
    }
}
