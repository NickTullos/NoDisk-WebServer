using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.Processors.TestData
{
    /// <summary>
    ///  This processor is for max performance testing and debugging issues.  It always returns "Hello World, no matter what the request"
    /// </summary>
    public class Processor : IProcessor
    {


      static private byte[] fakedata =  Encoding.UTF8.GetBytes("HTTP/1.0 200 OK\r\nContent-Type: text/html  \r\n\r\n <html> Hello World, meet NoDisk speeds  </html>");
      static private byte[] htmlheader = Encoding.UTF8.GetBytes("HTTP/1.0 200 OK\r\nContent-Type: text/html  \r\n\r\n<html><b>ECHO TEST:</b></br>");
      static private byte[] htmlfooter = Encoding.UTF8.GetBytes("</html>");
      /// <summary>
      ///// This processor is for max performance testing and debugging issues.  It always returns "Hello World, no matter what the request"
      /// </summary>
      /// <param name="header"></param>
      /// <returns></returns>
       public byte[] StartProcessorMaxPerformanceTest(IRequest request)
      {
          return fakedata;

      }

       public byte[] StartProcessorEchoTest(IRequest request)
      {
          var headerlength = NoDisk.Utils.ByteArray.IndexOf(request.OriginalHeader, NoDisk.Utils.ByteArray.RequestTerminator);

          byte[] rv = new byte[htmlheader.Length + headerlength + htmlfooter.Length];
          System.Buffer.BlockCopy(htmlheader, 0, rv, 0, htmlheader.Length);
          System.Buffer.BlockCopy(request.OriginalHeader, 0, rv, htmlheader.Length, headerlength);
          System.Buffer.BlockCopy(htmlfooter, 0, rv, htmlheader.Length + headerlength, htmlfooter.Length);


          return (rv);
      }


       public byte[] ProcessRequest(IRequest request)
       {
           return request.URL;
       }



       public List<byte[]> KnownTypes
       {
           get
           {
               throw new NotImplementedException();
           }
           set
           {
               throw new NotImplementedException();
           }
       }
    }
}
