using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoDisk.Utils
{
    /// <summary>
    /// This might not be constant, but they are ready only
    /// </summary>
    /// 
    

    public class Constants
    {
        public const string SERVER_VERSION = "0.1 / B";
        public const int CLIENT_SOCKET_TIMEOUT = 7000; //7 seconds timeout.
        public const int QUERY_MAX_SIZE = 1000; // if an incomming request is larger then drop the socket
        public const int PHP_MAX_PARAMETER_SIZE = 500;
        static public string Path_Root_Site;
        static public string Path_Root_Server;
        static public string Path_Root_PHP;

    }
}
