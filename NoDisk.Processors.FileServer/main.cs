using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.Processors.FileServer
{

    
  

/// <summary>
/// Returns the data from the FileSystem
/// </summary>
    public class Processor : IProcessor
    {

        public byte[] ProcessRequest(IRequest request)
        {
            return request.FileData;
        }
        
        public List<byte[]> KnownTypes
        {
            get
            {
                return null;
            }
            set { }
        }
    }
}
