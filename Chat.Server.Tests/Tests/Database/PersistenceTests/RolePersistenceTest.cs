using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;
using Chat.Server.Tests.TestSupport.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Server.Tests.Tests.Database.PersistenceTests
{
    public class RolePersistenceTest : EntityPersistenceTestBase<Role>
    {
        private UserRoomMembership _userRoomMembership = new UserRoomMembershipBuilder().Build();
        private string _name = "aa";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<Permission> _permissions = new List<Permission>
        {
            new PermissionBuilder().WithRole(null).Build(),
            new PermissionBuilder().WithRole(null).Build()
        };

        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<Role> GetEntityBuilder()
        {
            return new RoleBuilder()
                .WithUserRoomMembership(_userRoomMembership)
                .WithName(_name)
                .WithCreated(_created)
                .WithUpdated(_updated)
                .WithPermissions(_permissions);
        }

        protected override void AssertEquality(Role savedEntity, Role loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.UserRoomMembership, loadedEntity.UserRoomMembership);
            Assert.AreEqual(savedEntity.Name, loadedEntity.Name);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
            Assert.AreEqual(savedEntity.Permissions.Count(), loadedEntity.Permissions.Count());
        }
    }
}
