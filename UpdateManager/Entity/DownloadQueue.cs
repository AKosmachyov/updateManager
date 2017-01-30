using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdateManager.Entity
{
    class DownloadQueue
    {
        public DownloadQueue(IEnumerable<DriverUI> el)
        {
            _queue = new Queue<DriverUI>(el);
            _installQueue = new Queue<DriverUI>(_queue);                        
        }

        public Queue<DriverUI> _queue;
        public Queue<DriverUI> _installQueue;

        public async void startDownload()
        {
            if(this._queue.Count > 0)
                await _queue.Dequeue().downloaderAsync(next);
        }

        public Task next()
        {            
            if (this._queue.Count > 0)            
                return this._queue.Dequeue().downloaderAsync(next);
            return null;
        }

        public async void startInstall()
        {
            if (this._installQueue.Count > 0)            
                await Task.Run(() => this._installQueue.Dequeue().installDriver(nextInstall));
        }

        public Task nextInstall()
        {
            if (this._queue.Count > 0)
                return Task.Run(() => this._queue.Dequeue().installDriver(nextInstall));
            return null;
        }
    }
}
