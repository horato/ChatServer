using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;
using Chat.Server.Database;
using NHibernate;

namespace Chat.Server.Communication.Processing
{
    public class TransactionProcessor : ITransactionProcessor
    {
        private readonly IServerActionProcessor _next;
        private readonly ISessionFactory _sessionFactory;

        public TransactionProcessor(IServerActionProcessor next, ISessionFactory sessionFactory)
        {
            _next = next;
            _sessionFactory = sessionFactory;
        }

        public IResponse Process(IRequest request)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            using (new SessionContainer(session))
            {
                try
                {
                    var response = _next.Process(request);
                    transaction.Commit();
                    return response;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
