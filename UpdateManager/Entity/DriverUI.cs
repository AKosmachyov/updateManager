using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UpdateManager.Entity
{
    class DriverUI
    {
        public DriverUI(Driver d, ProgressBar p) {
            _driver = d;
            _progress = p;
        }

        Driver _driver ;
        ProgressBar _progress;
        WebClient _downloader = null;
        public async Task downloaderAsync(Action<object, AsyncCompletedEventArgs> next)
        {
            var link = String.Format("http://download.drp.su/driverpacks/repack{0}", _driver.link);
            var name = String.Format("{0}.zip", _driver.device);
            try
                {
                    using (_downloader = new WebClient())
                    {
                        _downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressChanged);
                        _downloader.DownloadFileCompleted += new AsyncCompletedEventHandler(next);
                        await _downloader.DownloadFileTaskAsync(new Uri(link), name);
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
            this._progress.Value = int.Parse(Math.Truncate(bytesIn / totalBytes * 100).ToString());
        }
    }
}
