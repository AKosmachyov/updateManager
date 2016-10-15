using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UpdateManager.Core
{
    [DataContract]
    class Device
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
    [DataContract]
    class OSinfo
    {
        [DataMember]
        public string arch { get; set; }
        [DataMember]
        public string ver { get; set; }
    }
    [DataContract]
    class InfoDrivers
    {
        [DataMember]
        public List<Device> devices { get; set; }
        [DataMember]
        public OSinfo windows { get; set; }
        [DataMember]
        public byte limit { get; set; }
    }

    class Driver
    {
        public string device { get; set; }
        public string version { get; set; }
        public string link { get; set; }
        public string date { get; set; }

    }    
}
