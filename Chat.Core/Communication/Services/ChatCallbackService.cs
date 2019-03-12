using System;
using System.ServiceModel;
using Chat.Core.Communication.Base;

namespace Chat.Core.Communication.Services
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ChatCallbackService : IChatCallbackService
    {
        private ICallbackProcessor _callbackProcessor;

        public void Callback(ICallback callback)
        {
            _callbackProcessor?.Process(callback);
        }

        public void RegisterCallbackProcessor(ICallbackProcessor processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));
            if (_callbackProcessor != null)
                throw new InvalidOperationException("_callbackProcessor is already set.");

            _callbackProcessor = processor;
        }
    }
}
