using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Database;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Tests.TestSupport.BaseClasses;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public abstract class EntityBuilderBase<TEntity>
        where TEntity : Entity
    {
        public TEntity BuildAndWriteToDatabase()
        {
            var entity = Build();
            if (!entity.IsTransient)
                throw new InvalidOperationException($"Entity {entity} is already persisted.");

            SessionContainer.Session.SaveOrUpdate(entity);
            SessionContainer.Session.Flush();

            return entity;
        }

        public abstract TEntity Build();
    }
}
