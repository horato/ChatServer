using Chat.Core.Communication.Base;
using Chat.Server.Communication.ServerActions.Base;

namespace Chat.Server.Communication.ServerActions
{
    public interface IServerActionProvider
    {
        IServerAction ProvideServerAction(IRequest request);
    }
}