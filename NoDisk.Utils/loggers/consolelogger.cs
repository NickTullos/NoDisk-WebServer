using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.Utils.loggers
{
    public class consolelogger : ILogger
    {

        public void WriteError(string info, Exception ex)
        {
         
            Console.WriteLine(info);
            Console.WriteLine(ex.ToString());
        }

        public void WriteWarning(string info, Exception ex)
        {
            Console.WriteLine(info);
            Console.WriteLine(ex.ToString());
        }

        public void WriteWarning(string info)
        {
            Console.WriteLine(info);
        }

        public void WriteError(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
