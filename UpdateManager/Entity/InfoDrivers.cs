using System.Collections.Generic;
using System.Management;
using System.Runtime.Serialization;
using System.Linq;

namespace UpdateManager.Entity
{   
    [DataContract]
    public class InfoDrivers
    {
        public InfoDrivers()
        {
            devices = getDrivers();
            windows = new OSinfo();
            limit = (byte)5;
        }

        private List<Device> getDrivers()
        {
            List<Device> arr = new List<Device>();
            ManagementObjectSearcher drivers = new ManagementObjectSearcher("Select * FROM Win32_PnPEntity");

            foreach (ManagementObject obj in drivers.Get())
            {
                Device device = new Device();

                device.deviceId = object.Equals(obj.GetPropertyValue("DeviceID"), null) ? string.Empty : obj.GetPropertyValue("DeviceID").ToString();
                var hardWareId = obj.GetPropertyValue("HardWareID");
                if (object.Equals(hardWareId, null))
                {
                    var temphardw = device.deviceId.Split('\\');
                    device.hardwareId = new[] { (temphardw[0] + temphardw[1]) }.ToList();
                }
                else
                {
                    device.hardwareId = ((string[])hardWareId).ToList();
                };
                device.status = object.Equals(obj.GetPropertyValue("Status"), null) ? string.Empty : obj.GetPropertyValue("Status").ToString();
                device.statusCode = (device.status == "Error") ? (byte)28 : (byte)0;

                arr.Add(device);
            }
            return arr;
        }

        [DataMember]
        public List<Device> devices { get; set; }
        [DataMember]
        public OSinfo windows { get; set; }
        [DataMember]
        public byte limit { get; set; }
    }
}