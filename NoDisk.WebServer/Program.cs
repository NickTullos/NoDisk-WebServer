using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Configuration;
using NoDisk.Utils.interfaces;
using NoDisk.Utils;
using System.Reflection;

namespace NoDisk
{

    
    

    public static class Program
    {
    
    
        
        private const Int32 DEFAULT_PORT = 80, DEFAULT_NUM_CONNECTIONS = 10000, DEFAULT_BUFFER_SIZE = Int16.MaxValue;
        
        public static void Main(String[] args)
        {
            
            //Setup configuration information
            Int32 port = configuration<int>("PORT", DEFAULT_PORT);
            Int32 numConnections = configuration<int>("DEFAULT_NUM_CONNECTIONS", DEFAULT_NUM_CONNECTIONS);
            Int32 bufferSize = configuration<int>("DEFAULT_BUFFER_SIZE", DEFAULT_BUFFER_SIZE);
            Utils.Constants.Path_Root_Server = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\","");
            Utils.Constants.Path_Root_PHP = configuration<string>("PHP_DIRECTORY", Utils.Constants.Path_Root_Server+@"\www\cgi\php\");
            Utils.Constants.Path_Root_Site = configuration<string>("WWW_DIRECTORY", Utils.Constants.Path_Root_Server+@"\www\");

            //create a list of request Processors
            List<IProcessor> Processors = new List<IProcessor>();
            //new-up all the processors then add them to the list.
            NoDisk.FileSystem.KVDictionary.Processor KVDict = new FileSystem.KVDictionary.Processor();
            NoDisk.Processors.FileNotFound.Processor FileNotFound = new Processors.FileNotFound.Processor();
            NoDisk.Processors.FileServer.Processor FileServer = new Processors.FileServer.Processor();
            NoDisk.Processors.PHP_CGI.Processor PHP_CGI = new Processors.PHP_CGI.Processor();
            NoDisk.Processors.TestData.Processor TestProcessor = new NoDisk.Processors.TestData.Processor();
            
            //configure KV files system
            KVDict.IngoreDirectory.Add("CGI"); //ignore all files in CGI directory
            KVDict.Load(Constants.Path_Root_Site);


            //Processors.Add(TestProcessor.ProcessRequest); this will always return hello world
            Processors.Add(PHP_CGI);
            Processors.Add(FileServer);
            Processors.Add(FileNotFound);
            

            try
            {
            
                SocketListener sl = new SocketListener(numConnections, bufferSize,Processors);                
                sl.Start(port);

                Console.WriteLine("\r\nServer listening on port {0}. Press any key to terminate the server process...", port);
                Console.Read();
                sl.Stop();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// This function request the configuration information if exists otherwise uses defaultval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultval"></param>
        /// <returns></returns>
        private static T configuration<T>(string key, T defaultval )
        {
            
            try
            {

                var value = ConfigurationSettings.AppSettings[key];
                if (value == null)
                {
                    Console.WriteLine(string.Format("Warning:  {0} not found in configation file", key));
                    return defaultval;
                }
                else
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                //need to log exc
                Console.WriteLine(ex);
                return defaultval;
            }

        }
        private static Int32 GetValue(String[] args, Int32 index, Int32 defaultValue)
        {
            Int32 value = 0;

            if (args.Length <= index || !Int32.TryParse(args[index], out value))
            {
                return defaultValue;
            }

            return value;
        }

    }
}
