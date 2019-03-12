using System;
using System.ServiceModel;
using Chat.Core.Communication.Base;
using Chat.Core.Communication.Services;

namespace Chat.Server.Communication.ServerActions.Base
{
    public abstract class ServerActionBase<TRequest, TResponse> : IServerAction
        where TRequest : RequestBase<TResponse>
        where TResponse : ResponseBase
    {
        public IChatCallbackService CurrentCallback => OperationContext.Current.GetCallbackChannel<IChatCallbackService>();

        public IResponse Execute(IRequest request)
        {
            if (request == null)
                throw new InvalidOperationException("Request is null");
            if (!(request is TRequest))
                throw new InvalidOperationException($"Wrong request type. Expected {typeof(TRequest)}, got {request.GetType()}");

            return ExecuteInternal((TRequest)request);
        }

        protected abstract TResponse ExecuteInternal(TRequest request);
    }
}
