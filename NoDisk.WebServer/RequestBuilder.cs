using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoDisk.Utils.interfaces;

namespace NoDisk.Server
{
    /// <summary>
    /// This class process the header byte[] into a IRequest.
    /// </summary>
    public class RequestBuilder
    {
        IFileSystemSimple _Filesystem = null;

        public RequestBuilder(IFileSystemSimple FileSystem)
        {
        _Filesystem = FileSystem;
        }
        

        public IRequest CreateRequestObj(byte[] header)
        {
            IRequest RequestObj = new Request();
            RequestObj.OriginalHeader = header;
            RequestObj.METHOD = GetRequest(header);
            
            //WTH is this request?
            if (RequestObj.METHOD == Utils.Enums.HTML_METHOD.UNKNOWN)
            {
                return RequestObj;
            }

            RequestObj = ParseHeader(header, RequestObj);

            //we need to check for post parameters, but we are only taking php parameters right now
            if (RequestObj.METHOD == Utils.Enums.HTML_METHOD.POST)
            {
                var str = Encoding.UTF8.GetString(RequestObj.OriginalHeader);
                var term = "\r\n\r\n";
                if (str.Contains(term) == true)
                {
                    var termPos = str.IndexOf(term);
                    if (termPos != -1)
                    {
                        var nullpos = str.IndexOf('\0');
                        if (nullpos != -1)
                        {
                            var values = str.Substring(termPos + term.Length, nullpos - termPos - term.Length);

                          RequestObj.Parameters=  Encoding.UTF8.GetBytes(values);
                        }
                    }
                }
            }


            if (RequestObj.URL != null)
            {
                RequestObj =  _Filesystem.InsertFile(RequestObj);
                //file ext will get inserted in insert file IF the request is for a "\"
                if (RequestObj.FileExt == null)
                {
                    RequestObj.FileExt = GetFileExt(RequestObj);
                }
            }



            return RequestObj;
        }

        /// <summary>
        /// This function will uppercase the request and remove the terminator "\r\n\r\n"
        /// </summary>
        /// <param name="RequestHeader"></param>
        /// <returns></returns>
        public IRequest ParseHeader(byte[] RequestHeader, IRequest Request)
            //byte[] URL_Parameters,  byte[] URL,  byte[] Parameters)
        {
             

            int len = NoDisk.Utils.ByteArray.IndexOf(RequestHeader, NoDisk.Utils.ByteArray.RequestTerminator);

            byte[] Question = new byte[1] { 63 };

            int QuestionMarkPos = NoDisk.Utils.ByteArray.IndexOf(RequestHeader, Question,0,len);

            int uppercasemax = len;

            if (QuestionMarkPos != -1)
            {
                if (QuestionMarkPos < len)
                {
                    uppercasemax = QuestionMarkPos;
                }
            }


            NoDisk.Utils.ByteArray.Uppercase(RequestHeader, uppercasemax);
            // length of method name
            var Methodlength = Request.METHOD.ToString().Length - 2;
            //get the complete URL with worthless browser info
            byte[] temp = new byte[len - Methodlength];
            Array.Copy(RequestHeader, 0, temp, 0, len - Methodlength);
            //now remove worthess browser info
            Request.URL_Parameters = GetFullURL(temp,Request);
            //now split up the URL and Parameters
            //we must get the question mark again because the position has changed
            QuestionMarkPos = NoDisk.Utils.ByteArray.IndexOf(Request.URL_Parameters, Question);
            if (QuestionMarkPos == -1)
            {
                Request.URL = Request.URL_Parameters;
                Request.Parameters = null;
            }
            else
            {
                //we create a new byte array that doesnt contain the ?
                Request.URL = new byte[QuestionMarkPos ];
                Array.Copy(Request.URL_Parameters, 0, Request.URL, 0, Request.URL.Length);

                Request.Parameters = new byte[Request.URL_Parameters.Length - QuestionMarkPos - 1];
                Array.Copy(Request.URL_Parameters, QuestionMarkPos+1, Request.Parameters, 0, Request.Parameters.Length);
            }


            return Request;
        }

        //TODO
        /// <summary>
        /// This function will return the enum type of request i.e. GET,POST,PUT,DELETE
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public NoDisk.Utils.Enums.HTML_METHOD GetRequest(byte[] buffer)
        {
            byte[] Method = new byte[5];
            byte[] GET = new byte[]{ (byte)'G',(byte)'E',(byte)'T'};
            byte[] POST = new byte[]{ (byte)'P',(byte)'O',(byte)'S',(byte)'T'};

            Array.Copy(buffer,Method,Method.Length);
             if (NoDisk.Utils.ByteArray.IndexOf(Method,GET) !=-1 )
               return NoDisk.Utils.Enums.HTML_METHOD.GET;

             if (NoDisk.Utils.ByteArray.IndexOf(Method, POST) != -1)
                 return NoDisk.Utils.Enums.HTML_METHOD.POST;

             return Utils.Enums.HTML_METHOD.UNKNOWN;
        }
        /// <summary>
        /// This function gets the lasts 4 bytes of the URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private byte[] GetFileExt(IRequest request)
        {
          
          
            //-5 is used do we get towards the back of the array and look for .XXXX
            int pos = NoDisk.Utils.ByteArray.IndexOf(request.URL, new byte[]{(byte)'.'}, request.URL.Length - 5);

            if (pos != -1)
            {
                byte[] FileExt =new byte[request.URL.Length-pos];
                Array.Copy(request.URL,pos,FileExt,0,FileExt.Length);
                return FileExt;
            }

            return null;

        }

        /// <summary>
        /// This will return the URL including parameters if they exist
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] GetFullURL(byte[] buffer,IRequest Request)
        {
            var methodlen = Request.METHOD.ToString().Length+1;
            //first the first terminator
            byte[] tmp = new byte[1] { 32 };
            var pos = NoDisk.Utils.ByteArray.IndexOf(buffer, tmp, methodlen);

            if (pos != -1)
            {
                byte[] temp = new byte[pos - methodlen];
                Array.Copy(buffer, methodlen, temp, 0, pos - methodlen);

                return temp;
            }

            return buffer;
        }
    }
}
