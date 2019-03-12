using Chat.Core.Communication.Base;

namespace Chat.Core.Communication
{
    public interface IRequestSender
    {
        TResponse SendRequest<TResponse>(RequestBase<TResponse> request) where TResponse : ResponseBase;
    }
}