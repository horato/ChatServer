using Chat.Core.Communication.Base;

namespace Chat.Server.Communication.ServerActions.Base
{
    public interface IServerAction
    {
        IResponse Execute(IRequest request);
    }
}
