using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Globalization;
using NoDisk.Utils.interfaces;
using NoDisk.Server;

namespace NoDisk
{
    delegate void ProcessData(SocketAsyncEventArgs args);

    /// <summary>
    /// Token for use with SocketAsyncEventArgs.
    /// </summary>
    internal sealed class Token : IDisposable
    {
        private Socket connection;

        private byte[] maxrequest;

        private Int32 currentIndex;

        public int LastActive;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connection">Socket to accept incoming data.</param>
        /// <param name="bufferSize">Buffer size for accepted data.</param>
        internal Token(Socket connection, Int32 bufferSize)
        {
            this.connection = connection;
            //this is used by the timeout monitor
            LastActive = Environment.TickCount;
         
        }

        /// <summary>
        /// Accept socket.
        /// </summary>
        internal Socket Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// Process data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        internal bool ProcessData(SocketAsyncEventArgs args, List<IProcessor> processors, Stack<byte[]> bufferlist,Func<byte[],IRequest> RequestBuilder)
        {

            byte[] GetTerminator = new byte[] { (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n' };

            ///** needs workd

            int terminatorPosition = NoDisk.Utils.ByteArray.IndexOf(args.Buffer, GetTerminator);
            //is this a proper request with a terminator?
            if (terminatorPosition != -1)
            {

                IRequest request = RequestBuilder(args.Buffer);
                //only process the request if the file extention is valid.
               
                //TODO
                //should only call the processor based on the Extention of the Request
                
                foreach (var process in processors)
                {
                    
                    var results = process.ProcessRequest(request);
                    if (results != null)
                    { 
                        args.SetBuffer(results, 0, results.Length);
                        break;
                    }
                }

                return true;
            }

            //var max_length = Encoding.UTF8.GetBytes(Utils.ByteArray.GenenericHTML400ResponseHeader +"No Processor found for extention or buffer was rejected due to size.");
            var max_length = Encoding.UTF8.GetBytes(Utils.ByteArray.GenenericHTML400ResponseHeader);
                
            args.SetBuffer(max_length,0,max_length.Length);
            return true;
         
        }


        #region IDisposable Members

        /// <summary>
        /// Release instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.connection.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                this.connection.Close();
            }
        }

        #endregion
    }
}
