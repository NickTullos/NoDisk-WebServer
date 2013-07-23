using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoDisk.Utils
{
    public  static class ByteArray
    {

        public class ByteArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] left, byte[] right)
            {
                if (left == null || right == null)
                {
                    return left == right;
                }
                return left.SequenceEqual(right);
            }
            public int GetHashCode(byte[] key)
            {
                if (key == null)
                    throw new ArgumentNullException("key");
                return key.Sum(b => b);
            }
        }
        
        static public string GenenericHTML400ResponseHeader = "HTTP/1.0 400 BAD REQUEST\r\n\r\n";
        static public string GenenericHTMLGetResponseHeader = "HTTP/1.0 200 OK\r\nServer: NoDisk" + Constants.SERVER_VERSION + "\r\n";
        static public string GenenericHTMLGetResponseHeaderWithContentType = "HTTP/1.0 200 OK\r\nContent-Type: text/html \r\n\r\n";
        static public byte[] RequestTerminator = new byte[] { (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n' };
        static public byte[] extPHP = new byte[] { (byte)'.', (byte)'P', (byte)'H', (byte)'P' };
        static public byte[] extHTM = new byte[] { (byte)'.', (byte)'H', (byte)'T', (byte)'M' };
        static public byte[] extHTML = new byte[] { (byte)'.', (byte)'H', (byte)'T', (byte)'M', (byte)'L' };
        static public byte[] extJPG = new byte[] { (byte)'.', (byte)'J', (byte)'P', (byte)'G' };
        static public byte[] extJPEG = new byte[] { (byte)'.', (byte)'J', (byte)'P', (byte)'E', (byte)'G' };
        static public byte[] extPNG = new byte[] { (byte)'.', (byte)'P', (byte)'N', (byte)'G' };
        static public byte[] extBMP = new byte[] { (byte)'.', (byte)'B', (byte)'M', (byte)'P' };
        static public byte[] extJS = new byte[] { (byte)'.', (byte)'J', (byte)'S'};
        //byte search
        //http://social.msdn.microsoft.com/Forums/vstudio/en-US/15514c1a-b6a1-44f5-a06c-9b029c4164d7/searching-a-byte-array-for-a-pattern-of-bytes

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind,int startpos = 0,int stoppos =-1)
        {
            if (stoppos == -1)
            { stoppos = arrayToSearchThrough.Length; }

            if (startpos < 0)
                return -1;
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = startpos; i < stoppos - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void Uppercase(byte[] array,int lenght)
        {
            for (int i = 0; i < lenght;i++)
            {
                if ((array[i] >= 97) & (array[i] <= 122))
                {
                    array[i] = (byte) (array[i] - (byte)32);
                }
           
            }
        }
    }
}
