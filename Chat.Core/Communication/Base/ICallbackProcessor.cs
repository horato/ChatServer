using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Communication.Base
{
    public interface ICallbackProcessor
    {
        void Process(ICallback callback);
    }
}
