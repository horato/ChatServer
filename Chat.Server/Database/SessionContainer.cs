using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace Chat.Server.Database
{
    public class SessionContainer : IDisposable
    {
        private static readonly ThreadLocal<ISession> _session = new ThreadLocal<ISession>();

        public static ISession Session => _session.Value;

        public SessionContainer(ISession session)
        {
            _session.Value = session;
        }

        public void Dispose()
        {
            Session.Flush();
            Session.Close();
            Session.Dispose();

            _session.Value = null;
        }
    }
}
