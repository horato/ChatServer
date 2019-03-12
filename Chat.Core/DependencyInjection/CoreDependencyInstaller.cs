using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication;
using Chat.Core.Communication.Services;
using Unity;

namespace Chat.Core.DependencyInjection
{
    public class CoreDependencyInstaller : IDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            container.RegisterSingleton<IChatCallbackService, ChatCallbackService>();
            container.RegisterType<IRequestSender, RequestSender>();
        }
    }
}
