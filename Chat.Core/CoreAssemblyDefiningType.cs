using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core
{
    public static class CoreAssemblyDefiningType
    {
        public static Assembly Assembly => typeof(CoreAssemblyDefiningType).Assembly;
    }
}
