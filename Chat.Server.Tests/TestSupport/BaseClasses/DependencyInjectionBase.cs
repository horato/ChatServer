using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chat.Core;
using Chat.Core.DependencyInjection;

namespace Chat.Server.Tests.TestSupport.BaseClasses
{
    public abstract class DependencyInjectionBase
    {
        static DependencyInjectionBase()
        {
            DI.InstallFrom(GetAllUsedAssemblies());
        }

        protected static IEnumerable<Assembly> GetAllUsedAssemblies()
        {
            return new[]
            {
                CoreAssemblyDefiningType.Assembly,
                ServerAssemblyDefiningType.Assembly,
                ServerTestsAssemblyDefiningType.Assembly
            };
        }

    }
}
