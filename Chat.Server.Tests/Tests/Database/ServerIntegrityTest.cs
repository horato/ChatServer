using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.DependencyInjection;
using Chat.Core.Extensions;
using Chat.Server.Domain.Entities.Base;
using Chat.Server.Domain.Maps.Base;
using Chat.Server.Tests.TestSupport.BaseClasses;
using Chat.Server.Tests.TestSupport.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace Chat.Server.Tests.Tests.Database
{
    [TestClass]
    public class ServerIntegrityTest : DatabaseAndDependencyInjectionBase
    {
        [TestMethod]
        public void AllDependenciesCanBeResolved()
        {
            var registeredTypes = DI.Container.Registrations.Select(x => x.RegisteredType);
            foreach (var registeredType in registeredTypes)
            {
                var instance = DI.Container.Resolve(registeredType);
                Assert.IsNotNull(instance, $"Failed to resolve {registeredType}");
            }
        }

        [TestMethod]
        public void AllEntitiesHaveBuilder()
        {
            var sb = new StringBuilder();
            var entities = GetAllUsedAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && typeof(Entity).IsAssignableFrom(x)).ToList();
            var builders = GetAllUsedAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && x.IsInheritedFromGenericBaseClass(typeof(EntityBuilderBase<>))).ToList();
            foreach (var entity in entities)
            {
                var entityBuilders = builders.Where(x => x.BaseType?.GetGenericArguments()[0] == entity).ToList();
                if (entityBuilders.Count > 1)
                    sb.AppendLine($"Entity {entity} has more than 1 builder ({string.Join(", ", entityBuilders)}).");
                if (entityBuilders.Count == 0)
                    sb.AppendLine($"Entity {entity} does not have any builder.");
            }

            Assert.IsTrue(string.IsNullOrWhiteSpace(sb.ToString()), $"AllEntitiesHaveBuilder test failed:{Environment.NewLine}{sb}");
        }

        [TestMethod]
        public void AllEntitiesHaveMapping()
        {
            var sb = new StringBuilder();
            var entities = GetAllUsedAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && typeof(Entity).IsAssignableFrom(x)).ToList();
            var entityMaps = GetAllUsedAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && x.IsInheritedFromGenericBaseClass(typeof(EntityMap<>))).ToList();
            foreach (var entity in entities)
            {
                var maps = entityMaps.Where(x => !x.IsAbstract && x.BaseType?.GetGenericArguments()[0] == entity).ToList();
                if (maps.Count > 1)
                {
                    sb.AppendLine($"Entity {entity} has more than 1 map ({string.Join(", ", maps)}).");
                }
                else if (maps.Count == 0)
                {
                    sb.AppendLine($"Entity {entity} does not nave any map.");
                }
                else
                {
                    var map = maps.Single();
                    if (typeof(VersionedEntity).IsAssignableFrom(entity) && !map.IsInheritedFromGenericBaseClass(typeof(VersionedEntityMap<>)))
                        sb.AppendLine($"Entity {entity} is {nameof(VersionedEntity)} but does not derive from {typeof(VersionedEntityMap<>)}");
                }
            }

            Assert.IsTrue(string.IsNullOrWhiteSpace(sb.ToString()), $"AllEntitiesHaveBuilder test failed:{Environment.NewLine}{sb}");
        }
    }
}
