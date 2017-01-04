using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using UpdateManager.Entity;
using System.Linq;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UpdateManager.Core
{
    public  static class WorkWithDriver
    {
        public static List<DataGridEntity> dataGridEntity = new List<DataGridEntity>();
        public static void getDriversForUpdate(DataGrid dataGrid)
        {
            InfoDrivers data = new InfoDrivers();            

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
                        
            foreach (var val in obj.data)
            {
                if (val.drivers.Count > 0)
                {
                    var temp = new Driver();
                    temp.deviceName = val.drivers[0].name;
                    temp.link = val.drivers[0].link;
                    var arrTime = (val.drivers[0].date).Split('-');
                    temp.date = new DateTime(Convert.ToInt32(arrTime[0]), Convert.ToInt32(arrTime[1]), Convert.ToInt32(arrTime[2]));
                    temp.version = val.drivers[0].version;
                    temp.directory = val.drivers[0].directory;
                    temp.inf = val.drivers[0].inf;

                    dataGridEntity.Add(new DataGridEntity(temp));
                }
            }
            dataGrid.ItemsSource = dataGridEntity;
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

        public static List<DataGridEntity> getListWithDriversForDownload()
        {
            var driversForDownload = dataGridEntity.Where(x => x.isCheck == true).ToList();
            return driversForDownload;
        }
        public static void updateDriversFull(List<DataGridEntity> driversForDownload, List<ProgressBar> progressBars, List<Label> labels)
        {
            var folderPath = String.Format(@"{0}\{1}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "UpdateManager");
            if (!(File.Exists(folderPath)))
                Directory.CreateDirectory(folderPath);
            var i = 0;
            var downloadQueue = new DownloadQueue(driversForDownload.Select(x => new DriverUI(x.driver, progressBars[i], labels[i++])));
            downloadQueue.startDownload();

            //Process installProcess = new Process();
            //installProcess.StartInfo.UseShellExecute = true;
            //installProcess.StartInfo.FileName = String.Format("{0}{1}", t.driver.directory, t.driver.inf);            
            //installProcess.StartInfo.Arguments = String.Format("C:\\WINDOWS\\System32\\rundll32.exe setupapi, InstallHinfSection");
            //installProcess.StartInfo.Verb = "Install";
            //installProcess.Start();
            //installProcess.WaitForExit();           
        }
    }
}
