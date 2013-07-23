using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NoDisk.Utils;
using NoDisk.Utils.interfaces;

namespace NoDisk.Processors.PHP_CGI
{
    /// <summary>
    /// This is not the fastest way to execute a PHP request. 
    /// </summary>
    public class Processor : IProcessor
    {
        //this guy Needs access to the files
        public Func<IRequest, byte[]> RequestFile;


        private List<byte[]> _knowntypes = new List<byte[]>();
        public Processor ()
        {
            _knowntypes.Add(NoDisk.Utils.ByteArray.extPHP);
        }
        public List<byte[]> KnownTypes
        {
            get
            {
                return _knowntypes;
            }
            set
            {}
        }

        public byte[] ProcessRequest(IRequest request)
        {

            var File = request.FileData;

            if ((File == null) |(request.FileExt == null))
            {
            return null;
            }

            //manual cehck file type
            if (NoDisk.Utils.ByteArray.extPHP.SequenceEqual(request.FileExt) != true)
            {
                return null;
            }




            Process php = new Process();

            // Path to php-cgi
            php.StartInfo.FileName = NoDisk.Utils.Constants.Path_Root_PHP + @"\php-cgi.exe";

            // We cannot execute it through ShellExecute - need to redirect output stream
            php.StartInfo.UseShellExecute = false;
            php.StartInfo.RedirectStandardOutput = true;
            php.StartInfo.RedirectStandardInput = true;
            php.StartInfo.CreateNoWindow = false;

            php.StartInfo.EnvironmentVariables.Clear();
            //php.StartInfo.EnvironmentVariables.Add("GATEWAY_INTERFACE", "CGI/1.1");
            php.StartInfo.EnvironmentVariables.Add("CONTENT_TYPE", "text/html");
            php.StartInfo.EnvironmentVariables.Add("REQUESTED_METHOD", "GET");
            //if there are parameters then add them
            if (request.Parameters != null)
            {
                php.StartInfo.EnvironmentVariables.Add("QUERY_STRING", Encoding.UTF8.GetString(request.Parameters));
            }


            string str = "";
            php.Start();

//            php.StandardInput.Write("<?php $_GET[\"name\"]=1; ?> ");
            if (request.Parameters != null)
            {
                var qparam = Encoding.UTF8.GetString(request.Parameters);
                List<string> qparams = qparam.Split('&').ToList();
                qparams.ForEach(a=>a.Replace("&",""));

                foreach (string qpara_add in qparams)
                {
                    if (qpara_add != string.Empty)
                    {
                        if (qpara_add.Length > Constants.PHP_MAX_PARAMETER_SIZE)
                            continue;
                        string[] kv = qpara_add.Split('=');
                        if (kv.Length == 1)
                            continue;
                        //allow some % chars and convert them back
                        kv[1]=kv[1].Replace("%21", "!").Replace("%2E",".").Replace("%3F","?");

                        if (qpara_add.Contains("%") == false) 
                        {
                            //clears KV from injection
                            kv[0] = kv[0].Replace("+","");
                            kv[1] = kv[1].Replace("+", " ");
                            php.StandardInput.Write("<?php $_"+request.METHOD.ToString()+"[\""+kv[0]+"\"]='"+kv[1]+"'; ?> ");
                        }
                    }
                }
                
            }

            php.StandardInput.WriteLine(Encoding.UTF8.GetString(File));

            str = php.StandardOutput.ReadToEnd();

            php.WaitForExit();
            php.Close();

            return Encoding.UTF8.GetBytes(NoDisk.Utils.ByteArray.GenenericHTMLGetResponseHeader + str);


        }



         
    }
}
