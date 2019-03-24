using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.DependencyInjection;
using Chat.Server.Database;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Tests.TestSupport.BaseClasses;
using Chat.Server.Tests.TestSupport.Builders;
using Chat.Server.Tests.TestSupport.Services;
using Unity;

namespace Chat.Server.Tests.Tests.Database.PersistenceTests
{
    public abstract class EntityPersistenceTestBase<TEntity> : DatabaseAndDependencyInjectionBase where TEntity : Entity
    {
        public abstract void EntityShouldBePersisted_Test();
        protected void EntityShouldBePersisted_Test_Internal()
        {
            DI.Container.Resolve<IDatabaseHelperService>().RunInTransaction(() =>
            {
                var entity = GetEntityBuilder().BuildAndWriteToDatabase();
                if (entity.IsTransient)
                    throw new InvalidOperationException($"Failed to save entity {entity} with id {entity.Id}");

                SessionContainer.Session.Clear();

                var loadedEntity = SessionContainer.Session.Get<TEntity>(entity.Id);
                AssertEquality(entity, loadedEntity);
            });
        }

        protected abstract EntityBuilderBase<TEntity> GetEntityBuilder();
        protected abstract void AssertEquality(TEntity savedEntity, TEntity loadedEntity);
    }
}
