using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UpdateManager.Entity
{
    class DriverUI
    {
        public DriverUI(Driver d, ProgressBar p, Label l) {
            _driver = d;
            _progress = p;
            _label = l;
        }

        Driver _driver ;
        ProgressBar _progress;
        WebClient _downloader = null;
        Label _label;
        public async Task downloaderAsync(Action<object, AsyncCompletedEventArgs> next)
        {
            var folderPath = String.Format(@"{0}\UpdateManager", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));         
            if (File.Exists(String.Format(@"{0}\{1}.zip", folderPath, _driver.deviceName)))
            {
                _label.Content = "100%";
                _progress.Value = 100;
                next(null, null);
                return;                
            }
            var link = String.Format("http://download.drp.su/driverpacks/repack{0}", _driver.link);
            var filePath = String.Format(@"{0}\{1}.zip", folderPath, _driver.deviceName);
            try
                {
                    using (_downloader = new WebClient())
                    {
                        _downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressChanged);                        
                        _downloader.DownloadFileCompleted += new AsyncCompletedEventHandler(next);
                        await _downloader.DownloadFileTaskAsync(new Uri(link), filePath);
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Ошибка скачивания");
                }
        }
        private void progressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            var value = Math.Truncate(bytesIn / totalBytes * 100).ToString();
            this._label.Content = value + '%';
            this._progress.Value = int.Parse(value);
        }        
    }
}
