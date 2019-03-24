using System;

namespace Chat.Server.Tests.TestSupport.Services
{
    public interface IDatabaseHelperService
    {
        T RunInTransaction<T>(Func<T> action);
        void RunInTransaction(Action action);
    }
}