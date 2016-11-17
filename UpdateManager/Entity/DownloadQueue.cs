using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdateManager.Entity
{
   //delegate void 
    class DownloadQueue
    {
        delegate void NextOperation();
        public DownloadQueue(Queue<DriverUI> q)
        {
            _queue = q;                    
        }

        public Queue<DriverUI> _queue;

        public void startDownload()
        {
            if(this._queue.Count > 0)
            {

                this._queue.Dequeue().downloaderAsync(next).Wait();
            }
        }

        public void next(object sender, AsyncCompletedEventArgs e)
        {
            //object sender, DownloadDataCompletedEventArgs e
            if (this._queue.Count > 0)
            {
                this._queue.Dequeue().downloaderAsync(next).Wait();
            }
        }
    }
}
