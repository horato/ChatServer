using System.ServiceModel;
using Chat.Core.Communication.Base;

namespace Chat.Core.Communication.Services
{
    [ServiceContract(CallbackContract = typeof(IChatCallbackService), SessionMode = SessionMode.Required)]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))]
    public interface IChatService
    {
        [OperationContract]
        [FaultContract(typeof(RequestFailData))]
        IResponse SendRequest(IRequest request);
    }
}