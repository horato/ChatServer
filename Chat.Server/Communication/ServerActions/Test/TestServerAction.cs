using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;
using Chat.Core.Communication.Services;
using Chat.Interface.Contracts;
using Chat.Interface.Contracts.Test;
using Chat.Server.Communication.ServerActions.Base;

namespace Chat.Server.Communication.ServerActions.Test
{
    public class TestServerAction : ServerActionBase<TestRequest, TestResponse>
    {
        protected override TestResponse ExecuteInternal(TestRequest request)
        {
            CurrentCallback.Callback(new TestCallback(request.Test));

            return new TestResponse(request.Test);
        }
    }
}
