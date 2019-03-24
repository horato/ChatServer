using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.DependencyInjection;
using Chat.Server.Tests.TestSupport;
using Chat.Server.Tests.TestSupport.BaseClasses;
using Chat.Server.Tests.TestSupport.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace Chat.Server.Tests.Tests.Database.ChangeScripts
{
    [TestClass]
    public class ChangescriptsTest : DatabaseAndDependencyInjectionBase
    {
        [TestMethod]
        public void GeneratedDatabaseShouldMatchChangescriptedDatabase_Test()
        {
            //TODO: config
            string result;
            using (var refDb = new ReferenceDatabase("localhost\\SQL2017", "ServerDatabase_Ref", "server", "test"))
            {
                refDb.RemoveAllDatabaseContent();
                refDb.CreateDatabase();
                refDb.ApplyChangescripts();
                var service = DI.Container.Resolve<IDatabaseCompareService>();
                result = service.CompareDatabasesAndGenerateChangescripts("ServerDatabase_Ref", "ServerDatabase_Test");
            }

            Assert.IsTrue(string.IsNullOrEmpty(result), $"Test and reference database are different.{Environment.NewLine}{result}");
        }
    }
}
