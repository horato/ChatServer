using System.Reflection;

namespace Chat.Server
{
    public class ServerAssemblyDefiningType
    {
        public static Assembly Assembly => typeof(ServerAssemblyDefiningType).Assembly;
    }
}
