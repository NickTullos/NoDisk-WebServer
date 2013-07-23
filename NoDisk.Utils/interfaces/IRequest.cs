using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoDisk.Utils.Enums;

namespace NoDisk.Utils.interfaces
{
    public interface IRequest
    {
         ILogger Logger { get; set; }
        /// <summary>
        /// return the complete request from the browser as the software sees it.
        /// </summary>
        byte[] OriginalHeader { get; set; }
        /// <summary>
        /// returns just the url i.e. \www\files\index.html and no parameters
        /// </summary>
         byte[] URL { get; set; }
        /// <summary>
        /// return the url and parameters i.e. \www\cgi\php\list.php?&name=nicktullos&adddress=blanchard
        /// </summary>
         byte[] URL_Parameters { get; set; }
        /// <summary>
        /// returns just the parameters i.e. &name=nicktullos&address=blanchard
        /// </summary>
         byte[] Parameters { get; set; }
        /// <summary>
        /// returns the html method i.e. GET, POST,PUT,DELETE
        /// </summary>
        HTML_METHOD METHOD { get; set; }
        /// <summary>
        /// File data pulled from the FileSystem Interface 
        /// </summary>
        byte[] FileData { get; set; }
        /// <summary>
        /// File extention including periond i.e. .PHP and .HTML
        /// </summary>
        byte[] FileExt { get; set; }

    }
}
