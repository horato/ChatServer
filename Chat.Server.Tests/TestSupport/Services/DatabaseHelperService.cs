using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chat.Server.Database;
using NHibernate;

namespace Chat.Server.Tests.TestSupport.Services
{
    public class DatabaseHelperService : IDatabaseHelperService
    {
        private readonly ISessionFactory _sessionFactory;

        public DatabaseHelperService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public T RunInTransaction<T>(Func<T> action)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            using (new SessionContainer(session))
            {
                try
                {
                    var result = action();
                    transaction.Commit();
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void RunInTransaction(Action action)
        {
            RunInTransaction<object>(() =>
            {
                action();
                return null;
            });
        }
    }
}
