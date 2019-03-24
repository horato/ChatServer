using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests
{
    public static class ServerTestsAssemblyDefiningType
    {
        public static Assembly Assembly => typeof(ServerTestsAssemblyDefiningType).Assembly;
    }
}
