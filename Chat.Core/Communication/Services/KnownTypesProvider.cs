using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;

namespace Chat.Core.Communication.Services
{
    public class KnownTypesProvider
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            var list = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (var assembly in assemblies)
            {
                list.AddRange(
                    assembly.GetTypes().Where(x => !x.IsAbstract && (typeof(IResponse).IsAssignableFrom(x))));
                list.AddRange(
                    assembly.GetTypes().Where(x => !x.IsAbstract && (typeof(IRequest).IsAssignableFrom(x))));
                list.AddRange(
                    assembly.GetTypes().Where(x => !x.IsAbstract && (typeof(ICallback).IsAssignableFrom(x))));

            }

            return list;
        }
    }
}
