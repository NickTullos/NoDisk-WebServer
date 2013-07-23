using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoDisk.Utils.Enums;
using NoDisk.Utils.interfaces;

namespace NoDisk.Server
{
    public class Request : IRequest
    {
        private ILogger _Logger;
        private byte[] _OriginalHeader;
        private HTML_METHOD _METHOD;
        private byte[] _URL;
        private byte[] _Parameters;
        private byte[] _FileData;
        private byte[] _FileExt;


        public byte[] FileExt
        {
            get { return _FileExt; }
            set { _FileExt = value; }
        }
        


        public byte[] FileData
        {
            get { return _FileData; }
            set { _FileData = value; }
        }
        


        public ILogger Logger
        {
            get { return _Logger; }
            set { _Logger = value; }
        }

        public byte[] OriginalHeader
        {
            get { return _OriginalHeader; }
            set { _OriginalHeader = value; }
        }


        public byte[] URL
        {
            get { return _URL; }
            set { _URL = value; }
        }


        public byte[] Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        private byte[] _URL_Parameters;

        public byte[] URL_Parameters
        {
            get { return _URL_Parameters; }
            set { _URL_Parameters = value; }
        }
        


        public HTML_METHOD METHOD
        {
            get { return _METHOD; }
            set { _METHOD = value; }
        }


    }
}
