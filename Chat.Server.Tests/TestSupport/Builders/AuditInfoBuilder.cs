using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Server.Domain.Entities;

namespace Chat.Server.Tests.TestSupport.Builders
{
    public class AuditInfoBuilder
    {
        private DateTime _on = new DateTime(2154, 5, 6);
        private string _by = "a user";

        public AuditInfoBuilder WithOn(DateTime on)
        {
            _on = on;
            return this;
        }

        public AuditInfoBuilder WithBy(string by)
        {
            _by = by;
            return this;
        }

        public AuditInfo Build()
        {
            var entity = new AuditInfo(_on, _by);

            return entity;
        }
    }
}
