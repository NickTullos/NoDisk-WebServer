using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoDisk.Utils;

namespace NoDisk.Server
{
 
    /// <summary>
    /// This class monitors the pool of open sockets and closes sockets based on a configured timeout.
    /// </summary>
    public class ThreadMonitor
    {
        private const int THEAD_LOOP_DELAY = 5000; //loop the thread every 5 seconds

        private List<SocketAsyncEventArgs> _poolUsed;
        private object lockobj = new object();
        private bool runthread= true;
        private int _timeout;
        private Action<SocketAsyncEventArgs> _CloseClientSocket;
        public ThreadMonitor(List<SocketAsyncEventArgs> poolUsed, Action<SocketAsyncEventArgs> CloseClientSocket,int Timeout)
        {
            _poolUsed = poolUsed;
            _CloseClientSocket = CloseClientSocket;
            _timeout = Timeout;
        }

        /// <summary>
        /// stop the thread monitor
        /// </summary>
        public void stop()
        {
            lock (lockobj)
            {
                runthread = true;

            }
        }
        /// <summary>
        /// Start the thread monitor to find sockets from clients who are lingering taking up resources and kill their connection.
        /// </summary>
        public void start()
        {
            
            while (runthread)
            {
            Thread.Sleep(THEAD_LOOP_DELAY);
            int timeout = Environment.TickCount - _timeout;
            lock (this._poolUsed)
            {
                var list = _poolUsed.ToArray();
                for(int i=0;i<list.Length;i++)
                {
                    var token = (Token)list[i].UserToken;
                    if (token.LastActive < timeout)
                    {
                        //kill the connection
                        _CloseClientSocket(list[i]);


                    }
                }
            }
            }
        }
    }
}
