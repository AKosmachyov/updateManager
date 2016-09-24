using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using UpdateManager.Core;

namespace UpdateManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        
        private void button1_Click(object sender, EventArgs e)
        {
            InfoDrivers data = new InfoDrivers();
            data.devices = getDrivers();
            data.windows = getInfoAboutOS();
            string ad = JsonConvert.SerializeObject(data);            
        }
        List<Device> getDrivers()
        {
            List<Device> arr = new List<Device>();
            ManagementObjectSearcher drivers = new ManagementObjectSearcher("Select * FROM Win32_PnPEntity");

            foreach (ManagementObject obj in drivers.Get())
            {                
                Device device = new Device();

                device.deviceId = object.Equals(obj.GetPropertyValue("DeviceID"), null) ? string.Empty : obj.GetPropertyValue("DeviceID").ToString();
                var hardWareId = obj.GetPropertyValue("HardWareID");                
                if(object.Equals(hardWareId, null))
                {
                    var temphardw = device.deviceId.Split('\\');
                    device.hardwareId = new[]{(temphardw[0] + temphardw[1])}.ToList();
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
        OSinfo getInfoAboutOS()
        {
            OSinfo os = new OSinfo();
            ManagementObjectSearcher infoOS= new ManagementObjectSearcher("Select * FROM Win32_OperatingSystem");                      

            foreach (ManagementObject obj in infoOS.Get())
            {
                os.ver = object.Equals(obj.GetPropertyValue("Version"),null) ? null : obj.GetPropertyValue("Version").ToString().Remove(3);
                os.arch = object.Equals(obj.GetPropertyValue("OSArchitecture"), null) ? null : obj.GetPropertyValue("OSArchitecture").ToString().Remove(2);
            }
            return os;
        }
    }
}
