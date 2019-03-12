using System.Runtime.Serialization;
using Chat.Core.Communication.Base;

namespace Chat.Interface.Contracts.Test
{
    [DataContract]
    public class TestRequest : RequestBase<TestResponse>
    {
        [DataMember]
        public string Test { get; private set; }

        public TestRequest(string test)
        {
            Test = test;
        }
    }
}
