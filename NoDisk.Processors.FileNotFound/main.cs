using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.Processors.FileNotFound
{
    /// <summary>
    /// Last chance processor, it always returns a 404 aka file not found.
    /// </summary>
    public class Processor : IProcessor
    {
        public byte[] ProcessRequest(IRequest info)
        {
            string FNF = "HTTP/1.0 404 OK\r\nContent-Type: text/html \r\n\r\n<html>File not found</html>";
            return Encoding.UTF8.GetBytes(FNF);
        }

/// <summary>
/// FNF always returns null because it takes all
/// </summary>

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
