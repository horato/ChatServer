using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;
using FluentNHibernate.Mapping;

namespace Chat.Server.Domain.Maps.Base
{
    public abstract class EntityMap<T> : ClassMap<T> where T : Entity
    {
        protected EntityMap()
        {
            Id(x => x.Id).Not.Nullable().GeneratedBy.GuidComb();

            Schema(GetSchema());
            Table(GetTablePrefix() + GetTableName());
        }

        protected abstract string GetSchema();
        protected abstract string GetTablePrefix();

        protected virtual string GetTableName()
        {
            return typeof(T).Name;
        }
    }
}
