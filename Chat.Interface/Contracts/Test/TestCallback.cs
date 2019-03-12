using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Chat.Core.Communication.Base;

namespace Chat.Interface.Contracts.Test
{
    [DataContract]
    public class TestCallback : CallbackBase
    {
        [DataMember]
        public string Test { get; protected set; }

        public TestCallback(string test)
        {
            Test = test;
        }
    }
}
