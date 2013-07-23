using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoDisk.Utils.interfaces
{
    public interface ILogger
    {
         void WriteError(string info,Exception ex);
         void WriteWarning(string info,Exception ex);
         void WriteWarning(string info);
         void WriteError(Exception ex);
    }
}
