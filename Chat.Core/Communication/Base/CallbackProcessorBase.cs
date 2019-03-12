using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Communication.Base
{
    public abstract class CallbackProcessorBase<TCallback> : ICallbackProcessor where TCallback : CallbackBase
    {
        public void Process(ICallback callback)
        {
            if (!(callback is TCallback))
                throw new InvalidOperationException($"Invalid callback type. Got {callback?.GetType()}, expected {typeof(TCallback)}");

            ProcessInternal((TCallback)callback);
        }

        protected abstract void ProcessInternal(TCallback callback);
    }
}
