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
                Device temp = new Device();

                temp.deviceId = object.Equals(obj.GetPropertyValue("DeviceID"), null) ? string.Empty : obj.GetPropertyValue("DeviceID").ToString();                
                var a = (string[])obj.GetPropertyValue("HardWareID");                
                temp.hardwareId = object.Equals(a, null) ? null : a.ToList();                
                temp.status = object.Equals(obj.GetPropertyValue("Status"), null) ? string.Empty : obj.GetPropertyValue("Status").ToString();
                temp.statusCode = (temp.status == "Error") ? "28" : "0";
                
                arr.Add(temp);                
            }            
            return arr;
        }
        OSinfo getInfoAboutOS()
        {
            ManagementObjectSearcher infoOC= new ManagementObjectSearcher("Select * FROM Win32_OperatingSystem");
            OSinfo temp = new OSinfo();
            foreach (ManagementObject obj in infoOC.Get())
            {
                temp.ver = object.Equals(obj.GetPropertyValue("Version"),null) ? null : obj.GetPropertyValue("Version").ToString().Remove(3);
                temp.arch = object.Equals(obj.GetPropertyValue("OSArchitecture"), null) ? null : obj.GetPropertyValue("OSArchitecture").ToString().Remove(2);
            }
            return temp;
        }
    }
}
