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
    public class UserRoomRestrictionPersistenceTest : EntityPersistenceTestBase<UserRoomRestriction>
    {
        private UserRoomMembership _userRoomMembership = new UserRoomMembershipBuilder().Build();
        private UserRestrictionType _restrictionType = UserRestrictionType.Mute;
        private bool _isPermanent = true;
        private DateTime? _effectiveTo = new DateTime(1111, 1, 1);
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();

        [TestMethod]
        public override void EntityShouldBePersisted_Test()
        {
            EntityShouldBePersisted_Test_Internal();
        }

        protected override EntityBuilderBase<UserRoomRestriction> GetEntityBuilder()
        {
            return new UserRoomRestrictionBuilder()
                .WithUserRoomMembership(_userRoomMembership)
                .WithRestrictionType(_restrictionType)
                .WithIsPermanent(_isPermanent)
                .WithEffectiveTo(_effectiveTo)
                .WithCreated(_created)
                .WithUpdated(_updated);
        }

        protected override void AssertEquality(UserRoomRestriction savedEntity, UserRoomRestriction loadedEntity)
        {
            Assert.AreEqual(savedEntity, loadedEntity);

            Assert.AreEqual(savedEntity.UserRoomMembership, loadedEntity.UserRoomMembership);
            Assert.AreEqual(savedEntity.RestrictionType, loadedEntity.RestrictionType);
            Assert.AreEqual(savedEntity.IsPermanent, loadedEntity.IsPermanent);
            Assert.AreEqual(savedEntity.EffectiveTo, loadedEntity.EffectiveTo);
            Assert.AreEqual(savedEntity.Created, loadedEntity.Created);
            Assert.AreEqual(savedEntity.Created.On, loadedEntity.Created.On);
            Assert.AreEqual(savedEntity.Created.By, loadedEntity.Created.By);
            Assert.AreEqual(savedEntity.Updated, loadedEntity.Updated);
            Assert.AreEqual(savedEntity.Updated.On, loadedEntity.Updated.On);
            Assert.AreEqual(savedEntity.Updated.By, loadedEntity.Updated.By);
        }
    }
}
