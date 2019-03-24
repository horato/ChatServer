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
    [TestClass]
    public class UserRoomMembershipPersistenceTest : EntityPersistenceTestBase<UserRoomMembership>
    {
        private User _user = new UserBuilder().Build();
        private Room _room = new RoomBuilder().Build();
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<UserRoomRestriction> _restrictions = new List<UserRoomRestriction>
        {
            new UserRoomRestrictionBuilder().WithUserRoomMembership(null).Build(),
            new UserRoomRestrictionBuilder().WithUserRoomMembership(null).Build(),
        };
        private IEnumerable<Role> _roles = new List<Role>
        {
            new RoleBuilder().WithUserRoomMembership(null).Build(),
            new RoleBuilder().WithUserRoomMembership(null).Build(),
            new RoleBuilder().WithUserRoomMembership(null).Build(),
        };

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<UserRoomMembership> GetEntityBuilder()
        {
            return new UserRoomMembershipBuilder()
                .WithUser(_user)
                .WithRoom(_room)
                .WithCreated(_created)
                .WithUpdated(_updated)
                .WithRestrictions(_restrictions)
                .WithRoles(_roles);
        }

        protected override void AssertEquality(UserRoomMembership savedEntity, UserRoomMembership loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.User, loadedEntity.User);
            Assert.AreEqual(savedEntity.Room, loadedEntity.Room);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
            Assert.AreEqual(savedEntity.Restrictions.Count(), loadedEntity.Restrictions.Count());
            Assert.AreEqual(savedEntity.Roles.Count(), loadedEntity.Roles.Count());
        }
    }
}
