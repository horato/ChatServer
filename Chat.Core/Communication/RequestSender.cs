using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;
using Chat.Core.Communication.Services;
using Chat.Core.DependencyInjection;
using Unity;

namespace Chat.Core.Communication
{
    public class RequestSender : IRequestSender
    {
        private readonly DuplexChannelFactory<IChatService> _channelFactory;
        private IChatService _channel;
        private ICommunicationObject _channelInfo;

        public RequestSender(IChatCallbackService callbackService)
        {
            _channelFactory = new DuplexChannelFactory<IChatService>(new InstanceContext(callbackService), "*");
            SetupChannel();
        }

        private void SetupChannel()
        {
            _channel = _channelFactory.CreateChannel(new EndpointAddress("net.tcp://localhost:9000/ChatService"));
            _channelInfo = (ICommunicationObject)_channel;
        }

        public TResponse SendRequest<TResponse>(RequestBase<TResponse> request) where TResponse : ResponseBase
        {
            EnsureValidChannel();
            return (TResponse)_channel.SendRequest(request);
        }

        private void EnsureValidChannel()
        {
            switch (_channelInfo.State)
            {
                // Channel is ok
                case CommunicationState.Opened:
                    return;

                // Channel is not ready yet and the call will probably fail, but it will be available soon.
                case CommunicationState.Created:
                case CommunicationState.Opening:
                    return;

                // Channel is dead, create new.
                case CommunicationState.Closing:
                case CommunicationState.Closed:
                case CommunicationState.Faulted:
                    SetupChannel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
