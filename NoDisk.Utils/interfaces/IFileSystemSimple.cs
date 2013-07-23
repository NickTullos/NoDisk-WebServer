using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoDisk.Utils.interfaces
{
    /// <summary>
    /// simple file system interface
    /// </summary>
    public interface IFileSystemSimple
    {
        IRequest InsertFile(IRequest Request);
    }
}
