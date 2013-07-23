using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.StatMonitorProcessor
{
    /// <summary>
    ///  This extention will give you some feedback to the servers performance. Its currently not in a good state.
    /// </summary>

     public class Processor : IProcessor
    {
         public byte[] Request(byte[] header)
        {

            string newheader = "HTTP/1.0 200 OK\r\nContent-Type: application/json\r\n\r\n";
        
            string processmsg = Encoding.UTF8.GetString(header);


            if (processmsg.Contains(@"/status/all"))
            {
                PerformanceCounter cpuCounter = new PerformanceCounter("Processor Information", "% Processor Time");
                PerformanceCounter ramCounter;

                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";
                cpuCounter.NextValue();

                ramCounter = new PerformanceCounter("Memory", "Available MBytes");



                string jsoncpu = "{\"cpu\" : \"" + cpuCounter.NextValue() + "%\"}";
                string jsonmem = "{\"memory\" : \"" + ramCounter.NextValue() + "MB\"}";

                string json = "[" + jsonmem + "," + jsoncpu + "]";
                return System.Text.Encoding.UTF8.GetBytes(newheader + json);
            }
            
            return null;
        }


         public byte[] ProcessRequest(IRequest request)
         {
             return Request(request.OriginalHeader);
         }


         public List<byte[]> KnownTypes
         {
             get
             {
                 return null;
             }
             set
             {

             }
         }
    }
}
