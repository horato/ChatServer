using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;
using Chat.Core.Extensions;
using Chat.Server.Communication.ServerActions.Base;
using Unity;

namespace Chat.Server.Communication.ServerActions
{
    public class ServerActionProvider : IServerActionProvider
    {
        /// <summary> Request type - Server action type </summary>
        private readonly IDictionary<Type, Type> _serverActions = new Dictionary<Type, Type>();
        private readonly IUnityContainer _container;

        public ServerActionProvider(IUnityContainer container)
        {
            _container = container;
            LoadFrom(new[] { ServerAssemblyDefiningType.Assembly });
        }

        private void LoadFrom(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => !x.IsAbstract && x.IsInheritedFromGenericBaseClass(typeof(ServerActionBase<,>)));
                foreach (var type in types)
                {
                    var requestType = type.GetGenericBaseClassArgument(typeof(ServerActionBase<,>), 0);
                    if (requestType == null)
                        throw new InvalidOperationException($"Request type could not be determined from {type}");
                    if (_serverActions.ContainsKey(requestType))
                        throw new InvalidOperationException($"Server action for type {requestType} already exists.");

                    _serverActions.Add(requestType, type);
                }
            }
        }

        public IServerAction ProvideServerAction(IRequest request)
        {
            if (!_serverActions.ContainsKey(request.GetType()))
                throw new InvalidOperationException($"No ServerAction found for type {request.GetType()}");

            var serverActionType = _serverActions[request.GetType()];
            var instance = _container.Resolve(serverActionType);
            var serverActionInstance = instance as IServerAction;
            if (serverActionInstance == null)
                throw new InvalidOperationException($"Failed to cast {instance} to {typeof(IServerAction)}");

            return serverActionInstance;
        }
    }
}
