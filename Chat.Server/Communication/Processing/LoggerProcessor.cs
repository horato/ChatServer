using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;

namespace Chat.Server.Communication.Processing
{
    public class LoggerProcessor : ILoggerProcessor
    {
        private readonly ITransactionProcessor _next;

        public LoggerProcessor(ITransactionProcessor next)
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
                //TODO: log
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
