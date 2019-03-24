using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Enums;
using Chat.Server.Tests.TestSupport.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Server.Tests.Tests.Database.PersistenceTests
{
    [TestClass]
    public class PermissionPersistenceTest : EntityPersistenceTestBase<Permission>
    {
        private Role _role = new RoleBuilder().Build();
        private PermissionType _permissionType = PermissionType.All;
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<Permission> GetEntityBuilder()
        {
            return new PermissionBuilder()
                .WithRole(_role)
                .WithPermissionType(_permissionType)
                .WithCreated(_created)
                .WithUpdated(_updated);
        }

        protected override void AssertEquality(Permission savedEntity, Permission loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.Role, loadedEntity.Role);
            Assert.AreEqual(savedEntity.PermissionType, loadedEntity.PermissionType);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
        }
    }
}
