using Chat.Core.DependencyInjection;
using Chat.Server.Tests.TestSupport.Services;
using Unity;

namespace Chat.Server.Tests
{
    public class ServerTestsDependencyInstaller : IDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            container.RegisterType<IDatabaseCompareService, DatabaseCompareService>();
            container.RegisterType<IDatabaseHelperService, DatabaseHelperService>();
        }
    }
}
