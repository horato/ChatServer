using System.Runtime.Serialization;

namespace Chat.Core.Communication.Base
{
    [DataContract]
    public abstract class RequestBase<TResponse> : IRequest where TResponse : ResponseBase
    {

    }
}
