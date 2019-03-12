using Chat.Core.Communication.Base;

namespace Chat.Server.Communication.Processing
{
    public interface IRequestProcessor
    {
        IResponse Process(IRequest request);
    }
}
