using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.DependencyInjection;
using Chat.Server.Database.Conventions;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using Unity;
using Unity.Lifetime;

namespace Chat.Server.Tests.TestSupport.BaseClasses
{
    public abstract class DatabaseAndDependencyInjectionBase : DependencyInjectionBase
    {
        static DatabaseAndDependencyInjectionBase()
        {
            var config = new Configuration();
            config.Configure();
            config.SetNamingStrategy(new NamingStrategy());

            var factory = Fluently.Configure(config)
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<ServerAssemblyDefiningType>()
                .Conventions.AddFromAssemblyOf<ServerAssemblyDefiningType>())
                .BuildSessionFactory();

            DI.Container.RegisterInstance(typeof(ISessionFactory), factory, new ContainerControlledLifetimeManager());
        }
    }
}
