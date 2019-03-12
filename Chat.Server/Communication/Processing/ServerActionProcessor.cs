using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;
using Chat.Server.Communication.ServerActions;

namespace Chat.Server.Communication.Processing
{
    public class ServerActionProcessor : IServerActionProcessor
    {
        private readonly IServerActionProvider _serverActionProvider;

        public ServerActionProcessor(IServerActionProvider serverActionProvider)
        {
            _serverActionProvider = serverActionProvider;
        }

        public IResponse Process(IRequest request)
        {
            var serverAction = _serverActionProvider.ProvideServerAction(request);
            return serverAction.Execute(request);
        }
    }
}
