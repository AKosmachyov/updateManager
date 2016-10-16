﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using UpdateManager.Entity;

namespace UpdateManager.Core
{
    public  static class WorkWithDriver
    {
        public static void getDriversForUpdate()
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
    }
}
