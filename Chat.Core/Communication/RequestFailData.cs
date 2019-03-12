using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Communication
{
    [DataContract]
    public class RequestFailData
    {
        [DataMember]
        public string ExceptionType { get; }

        [DataMember]
        public string ExceptionMessage { get; }

        [DataMember]
        public string ExceptionString { get; }

        public RequestFailData(Exception e)
        {
            ExceptionType = e.GetType().FullName;
            ExceptionMessage = e.Message;
            ExceptionString = e.ToString();
        }
    }
}
