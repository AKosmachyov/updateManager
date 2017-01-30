using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UpdateManager.Entity
{
    class DriverUI
    {
        public DriverUI(Driver d, ProgressBar p, Label l) {
            _driver = d;
            _progress = p;
            _label = l;
            folderPath = String.Format(@"{0}\UpdateManager\{1}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                 _driver.deviceName.Replace(" ", string.Empty));
        }

        public Driver _driver ;
        ProgressBar _progress;
        WebClient _downloader = null;
        public Label _label;
        string folderPath;
        public async Task downloaderAsync(Func<Task> next)
        {
            if (File.Exists(String.Format(@"{0}.zip", folderPath)))
            {
                _label.Content = "100%";
                _progress.Value = 100;
                await next();
                return;                
            }
            var link = String.Format("http://download.drp.su/driverpacks/repack{0}", _driver.link);
            var filePath = String.Format(@"{0}.zip", folderPath);
            try
            {
                using (_downloader = new WebClient())
                {
                    //_downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressChanged);                    
                    await _downloader.DownloadFileTaskAsync(new Uri(link), filePath);
                    await next();
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
        public async void installDriver(Func<Task> next)
        {
            var t = String.Format(@"{0}.zip", folderPath);
            var tt = String.Format(@"{0}", folderPath);
            await Task.Run(() => ZipFile.ExtractToDirectory(String.Format(@"{0}.zip", folderPath), String.Format(@"{0}", folderPath)));
            var filePath = String.Format(@"{0}\{1}{2}", folderPath, _driver.directory, _driver.inf);
            if (!(File.Exists(filePath)))
            {
                await Task.Run(() => {
                    Directory.Delete(folderPath, true);
                    File.Delete(String.Format(@"{0}.zip", folderPath));
                });
                _label.Content = "Ошибка установки";
                await next();
                return;
            }

            //Process installProcess = new Process();
            //installProcess.StartInfo.UseShellExecute = true;
            //installProcess.StartInfo.FileName = filePath;
            //installProcess.StartInfo.Arguments = String.Format("C:\\WINDOWS\\System32\\rundll32.exe setupapi, InstallHinfSection");
            //installProcess.StartInfo.Verb = "Install";
            //installProcess.Start();
            //await Task.Run(() => installProcess.WaitForExit());
            await Task.Run(() => {
                Directory.Delete(folderPath, true);
                File.Delete(String.Format(@"{0}.zip", folderPath));
            });            
            await next();
        }
    }
}
