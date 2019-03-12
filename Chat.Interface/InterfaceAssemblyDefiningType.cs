using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Interface
{
    public class InterfaceAssemblyDefiningType
    {
        public static Assembly Assembly => typeof(InterfaceAssemblyDefiningType).Assembly;
    }
}
