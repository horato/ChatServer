using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication;
using Chat.Core.Communication.Base;

namespace Chat.Server.Communication.Processing
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        private readonly ILoggerProcessor _next;

        public ExceptionProcessor(ILoggerProcessor next)
        {
            _next = next;
        }

        public IResponse Process(IRequest request)
        {
            try
            {
                return _next.Process(request);
            }
            catch (Exception e)
            {
                throw new FaultException<RequestFailData>(new RequestFailData(e));
            }
        }
    }
}
