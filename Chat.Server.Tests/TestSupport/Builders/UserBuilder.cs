using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class UserBuilder : EntityBuilderBase<User>
    {
        private static int _nameCounter = 1;
        private static int _loginCounter = 1;

        private string _name = $"TEST_USER{_nameCounter++}";
        private string _login = $"TEST_LOGIN{_loginCounter++}";
        private string _password = "TEST_PASSWORD";
        private AuditInfo _created = new AuditInfoBuilder().Build();
        private AuditInfo _updated = new AuditInfoBuilder().Build();
        private IEnumerable<UserRoomMembership> _rooms = new List<UserRoomMembership>();
        private IEnumerable<MessageHistory> _messageHistories = new List<MessageHistory>();

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithLogin(string login)
        {
            _login = login;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public UserBuilder WithCreated(AuditInfo created)
        {
            _created = created;
            return this;
        }

        public UserBuilder WithUpdated(AuditInfo updated)
        {
            _updated = updated;
            return this;
        }

        public UserBuilder WithRooms(IEnumerable<UserRoomMembership> rooms)
        {
            _rooms = rooms ?? new List<UserRoomMembership>();
            return this;
        }

        public UserBuilder WithMessageHistories(IEnumerable<MessageHistory> messageHistories)
        {
            _messageHistories = messageHistories ?? new List<MessageHistory>();
            return this;
        }

        public override User Build()
        {
            var entity = new User(_name, _login, _password, _created, _updated);

            entity.AddUserRoomMemberships(_rooms);
            entity.AddMessageHistories(_messageHistories);

            return entity;
        }
    }
}
