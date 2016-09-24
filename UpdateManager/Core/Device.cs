using System.Collections.Generic;

namespace UpdateManager.Core
{
    class Device
    {        
        public string deviceId { get; set; }
        public List<string> hardwareId { get; set; }
        public string status { get; set; }
        public byte statusCode { get; set; }
    }

    class OSinfo
    {
        public string arch { get; set; }
        public string ver { get; set; }
    }

    class InfoDrivers
    {
        public List<Device> devices { get; set; }
        public OSinfo windows { get; set; }
    }
}
