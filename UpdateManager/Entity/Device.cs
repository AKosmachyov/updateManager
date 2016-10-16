using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UpdateManager.Entity
{
    [DataContract]
    public class Device
    {       
        [DataMember]
        public string deviceId { get; set; }
        [DataMember]
        public List<string> hardwareId { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public byte statusCode { get; set; }
    }
}