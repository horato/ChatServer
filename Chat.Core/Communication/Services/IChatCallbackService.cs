using System.ServiceModel;
using Chat.Core.Communication.Base;

namespace Chat.Core.Communication.Services
{
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))]
    public interface IChatCallbackService
    {
        [OperationContract(IsOneWay = true)]
        void Callback(ICallback callback);

        void RegisterCallbackProcessor(ICallbackProcessor processor);
    }
}
