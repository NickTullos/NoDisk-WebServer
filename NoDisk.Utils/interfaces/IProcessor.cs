using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoDisk.Utils.Enums;

namespace NoDisk.Utils.interfaces
{
    /// <summary>
    /// Interface for request processor
    /// </summary>
    public interface IProcessor
    {
        byte[] ProcessRequest(IRequest request);
        List<byte[]> KnownTypes { get; set; }
        
    }
}
