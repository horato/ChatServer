using System.ServiceModel;
using Chat.Core.Communication.Base;
using Chat.Core.Communication.Services;
using Chat.Server.Communication.Processing;

namespace Chat.Server.Communication
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        private readonly IExceptionProcessor _processor;

        public ChatService(IExceptionProcessor processor)
        {
            _processor = processor;
        }

        public IResponse SendRequest(IRequest request)
        {
            return _processor.Process(request);
        }
    }
}