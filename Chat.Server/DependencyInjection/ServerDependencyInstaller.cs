using Chat.Core.Communication;
using Chat.Core.Communication.Services;
using Chat.Core.DependencyInjection;
using Chat.Server.Communication;
using Chat.Server.Communication.Processing;
using Chat.Server.Communication.ServerActions;
using Unity;

namespace Chat.Server.DependencyInjection
{
    public class ServerDependencyInstaller : IDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            container.RegisterSingleton<IChatService, ChatService>();
            container.RegisterType<IServerActionProvider, ServerActionProvider>();
            container.RegisterType<IExceptionProcessor, ExceptionProcessor>();
            container.RegisterType<ILoggerProcessor, LoggerProcessor>();
            container.RegisterType<ITransactionProcessor, TransactionProcessor>();
            container.RegisterType<IServerActionProcessor, ServerActionProcessor>();
        }
    }
}
