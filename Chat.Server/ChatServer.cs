using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Chat.Core;
using Chat.Core.Communication;
using Chat.Core.Communication.Services;
using Chat.Core.DependencyInjection;
using Chat.Interface;
using Chat.Server.Communication;
using Chat.Server.Database.Conventions;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using Unity;
using Unity.Lifetime;

namespace Chat.Server
{
    public class ChatServer
    {
        private ServiceHost _service;

        public ChatServer()
        {
            InitializeDependencyInjection();
            InitializeORM();
        }

        private void InitializeDependencyInjection()
        {
            DI.InstallFrom(new[]
            {
                CoreAssemblyDefiningType.Assembly,
                ServerAssemblyDefiningType.Assembly,
                InterfaceAssemblyDefiningType.Assembly
            });
        }

        private void InitializeORM()
        {
            var config = new Configuration();
            config.Configure();
            config.SetNamingStrategy(new NamingStrategy());

            //PostLoad listeners to buildup loaded entity if needed

            var factory = Fluently.Configure(config)
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<ServerAssemblyDefiningType>()
                .Conventions.AddFromAssemblyOf<ServerAssemblyDefiningType>())
                .BuildSessionFactory();

            DI.Container.RegisterInstance(typeof(ISessionFactory), factory, new ContainerControlledLifetimeManager());
        }

        public void Start()
        {
            if (_service != null)
                throw new InvalidOperationException("Service already initialized");

            var instance = DI.Container.Resolve<IChatService>();
            _service = new ServiceHost(instance);
            _service.Open();
        }

        public void Stop()
        {
            if (_service == null)
                return;

            _service.Close();
            _service = null;
        }
    }
}
