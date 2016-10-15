using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Management;
using System.Net;
using UpdateManager.Core;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace UpdateManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            InfoDrivers data = new InfoDrivers();
            data.devices = getDrivers();
            data.windows = getInfoAboutOS();
            data.limit = (byte)5;

            var stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(InfoDrivers));
            ser.WriteObject(stream1, data);

            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            stream1 = new MemoryStream(Encoding.UTF8.GetBytes((PostMethod(sr.ReadToEnd(), "http://api.drp.su/api/select"))));

            stream1.Position = 0;
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = serializer.Deserialize(new StreamReader(stream1).ReadToEnd(), typeof(object));

            var drivers = new List<Driver>();
            foreach (var val in obj.data)
            {
                if (val.drivers.Count > 0)
                {
                    var temp = new Driver();
                    temp.device = val.drivers[0].name;
                    temp.link = val.drivers[0].link;
                    temp.date = val.drivers[0].date;
                    temp.version = val.drivers[0].version;
                    drivers.Add(temp);
                }
            }            
        }
        public static string PostMethod(string postedData, string postUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(postedData);

            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response != null)
            {
                var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var responseToString = strreader.ReadToEnd();
                return responseToString;
            }
            return "";
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
        OSinfo getInfoAboutOS()
        {
            OSinfo os = new OSinfo();
            ManagementObjectSearcher infoOS = new ManagementObjectSearcher("Select * FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in infoOS.Get())
            {
                os.ver = object.Equals(obj.GetPropertyValue("Version"), null) ? null : obj.GetPropertyValue("Version").ToString().Remove(3);
                os.arch = object.Equals(obj.GetPropertyValue("OSArchitecture"), null) ? null : obj.GetPropertyValue("OSArchitecture").ToString().Remove(2);
            }
            return os;
        }

    }
}
