using System.Management;
using System.Runtime.Serialization;

namespace UpdateManager.Entity
{    
    [DataContract]
    public class OSinfo
    {
        public OSinfo()
        {           
            ManagementObjectSearcher infoOS = new ManagementObjectSearcher("Select * FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in infoOS.Get())
            {
                ver = object.Equals(obj.GetPropertyValue("Version"), null) ? null : obj.GetPropertyValue("Version").ToString().Remove(3);
                arch = object.Equals(obj.GetPropertyValue("OSArchitecture"), null) ? null : obj.GetPropertyValue("OSArchitecture").ToString().Remove(2);
            }            
        }

        [DataMember]
        public string arch { get; set; }
        [DataMember]
        public string ver { get; set; }
    }
}