using System.Runtime.Serialization;
using Chat.Core.Communication.Base;

namespace Chat.Interface.Contracts.Test
{
    [DataContract]
    public class TestResponse : ResponseBase
    {
        [DataMember]
        public string Test { get; private set; }

        public TestResponse(string test)
        {
            Test = test;
        }
    }
}