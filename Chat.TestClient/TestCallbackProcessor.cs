using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Chat.Core.Communication.Base;
using Chat.Interface.Contracts.Test;

namespace Chat.TestClient
{
    public class TestCallbackProcessor : CallbackProcessorBase<TestCallback>
    {
        protected override void ProcessInternal(TestCallback callback)
        {
            MessageBox.Show(callback.Test, "Callback");
        }
    }
}
