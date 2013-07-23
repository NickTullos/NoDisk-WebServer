using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoDisk.Utils.interfaces;

namespace NoDisk.FileSystem.KVDictionary
{

    
     public class DictInfo
     {
     public byte[] data;
     public string filename;
     public string extention;
     public bool IsServerSideScript;// we arent going to add the headers to scripts with server side execution
     public bool ScriptIsProcessed;  
     }
    /// <summary>
    /// File System implemented using KV.  The url uppercased is the key and the results is a preprocessed html request unless its a script of course.
    /// </summary>
     public class Processor : IFileSystemSimple
    {
        public List<string> IngoreDirectory = new List<string>();
        static private Dictionary<byte[], DictInfo> _kvdict = new Dictionary<byte[], DictInfo>(new NoDisk.Utils.ByteArray.ByteArrayComparer());
         
         /******* this pattern isnt working? but the file extention sercurity keep them from getting served. still needs to be fixed to save memory****/
         //dont serve up files that start with a "." the files must have atleast 1 char before and after the period.
        public string SearchPattern = "*?.?*";




        public IRequest InsertFile(IRequest request )
        {
            var rtnRequest = request;
            var FullPath = request.URL;
            DictInfo info = null;


            //most likely the request is good.
            if (_kvdict.TryGetValue(FullPath, out info) == true)
            {
                rtnRequest.FileData = info.data;
                return rtnRequest;
            }

            //check for a /
            if (FullPath[FullPath.Length - 1] == (byte)'/')
            {
                //check for safest option first
                byte[] indx = new byte[]{(byte)'I',(byte)'N',(byte)'D',(byte)'E',(byte)'X',(byte)'.',(byte)'H',(byte)'T',(byte)'M',(byte)'L'};
                byte[] index = new byte[FullPath.Length + "index.htm".Length];
                Array.Copy(FullPath, index, FullPath.Length);
                Array.Copy(indx,0, index, FullPath.Length,indx.Length-1);

                //INDEX.HTM
                if (_kvdict.TryGetValue(index, out info) == true)
                {
                    rtnRequest.FileData = info.data;
                    rtnRequest.FileExt = NoDisk.Utils.ByteArray.extHTM;
                    return rtnRequest;
                }


                index = new byte[FullPath.Length + "index.html".Length];
                Array.Copy(FullPath, index, FullPath.Length);
                Array.Copy(indx, 0, index, FullPath.Length, indx.Length);
                //INDEX.HTML
                if (_kvdict.TryGetValue(index, out info) == true)
                {
                    rtnRequest.FileData = info.data;
                    rtnRequest.FileExt = NoDisk.Utils.ByteArray.extHTML;
                    return rtnRequest;
                }

                //INDEX.PHP
                indx[indx.Length-2] = (byte)'P';
                indx[indx.Length-3] = (byte)'H';
                indx[indx.Length-4] = (byte)'P';
                index = new byte[FullPath.Length + "index.php".Length];
                Array.Copy(FullPath, index, FullPath.Length);
                Array.Copy(indx, 0, index, FullPath.Length, indx.Length-1);

                if (_kvdict.TryGetValue(index, out info) == true)
                {
                    rtnRequest.FileData = info.data;
                    rtnRequest.FileExt = NoDisk.Utils.ByteArray.extPHP;
                    return rtnRequest;

                }



            }


            rtnRequest.FileData = null;
            return rtnRequest;
        }

         public void Load(string path)
        {
            Console.WriteLine("Loading Files into cache: ");
            DirectoryInfo dir = new DirectoryInfo(path);        
            DirectoryWalk(dir, @"" ); //first directory is root
            Console.WriteLine("Finished Loading Files into cache: ");
         }

        //from steve's project
        private void DirectoryWalk(DirectoryInfo dir, string keyFix)
        {
            if (IngoreDirectory.Contains(dir.Name.ToUpper()) == true)
            {
                Console.WriteLine(" /" + dir.Name.ToUpper() + " - Ignoring Directory ");
                return;
            }

            FileInfo[] files = dir.EnumerateFiles(SearchPattern).ToArray();
            foreach (FileInfo file in files)
            {
                string key = keyFix + "/" + file.Name;

               
                DictInfo info = ProcessFile(file);


                var keyarray = Encoding.UTF8.GetBytes(key);
                NoDisk.Utils.ByteArray.Uppercase(keyarray, keyarray.Length);

                _kvdict.Add(keyarray, info);
                //_kvdict.Add(key.ToLower(), info);

               


                Console.WriteLine(" "+Encoding.UTF8.GetString(keyarray));

            }
            DirectoryInfo[] dirs = dir.EnumerateDirectories().ToArray();
            foreach (DirectoryInfo ndir in dirs)
            {
                DirectoryWalk(ndir, keyFix + "/" + ndir.Name);
            }
        }
        
        /// <summary>
        /// If the contentype is set to string.empty then no content type will be return.
        /// </summary>
        /// <param name="contenttype"></param>
        /// <returns></returns>
        private byte[] addheader(string contenttype)
        {
            if (contenttype == string.Empty)
            {
                return Encoding.UTF8.GetBytes(Utils.ByteArray.GenenericHTMLGetResponseHeader);
            }
            var header = "HTTP/1.0 200 OK\r\nContent-Type: " + contenttype + "\r\n\r\n";
            var headerut8 = Encoding.UTF8.GetBytes(header);
            return headerut8;
        }

        private DictInfo  ProcessFile(FileInfo file)
        {
            
             DictInfo info = new DictInfo();
                
                info.data = ReadFileBinary(file);
                info.filename = file.Name.ToLower();
                info.extention = file.Extension.ToLower();
                //info.IsServerSideScript = false;

            byte[] header = null;
            switch(info.extention){
                case (".php"):
                    info.IsServerSideScript = true;
                    break;
                case (".bmp"):
                    info.IsServerSideScript = false;
                    header = addheader("image/bmp");
                    break;
                case(".jpg"):
                case(".jpeg"):
                    info.IsServerSideScript = false;
                    header = addheader("image/jpeg");
                    break;
                case(".png"):
                    info.IsServerSideScript = false;
                    header = addheader("image/png");
                    break;
                case (".css"):
                    info.IsServerSideScript = false;
                    header = addheader("text/css");
                    break;
                case(".htm"):
                case (".js"):
                case(".html"):
                    info.IsServerSideScript = false;
                    header = addheader("text/html");
                    break;
                default://unknown file type
                    info.IsServerSideScript = false;
                    header = addheader("text/html");
                    info.data = Encoding.UTF8.GetBytes("<html>server error - unknown file type "+file.Extension+" </html>");
                    break;

            }


            //combine header and data for completedly processed response
            if (info.IsServerSideScript == false)
            {
                byte[] total = new byte[header.Length + info.data.Length];
                Array.Copy(header, total, header.Length);
                Array.Copy(info.data, 0, total, header.Length, info.data.Length);
                info.data = total;
            }


            return info;

        }

        private byte[] ReadFileBinary(FileInfo f)
        {
            try
            {

                FileStream fs = new FileStream(f.FullName,
                            FileMode.Open, FileAccess.Read,
              FileShare.Read);
                
                BinaryReader reader = new BinaryReader(fs);
                byte[] bytes = new byte[fs.Length];
                reader.Read(bytes,0,bytes.Length);
                reader.Close();
                fs.Close();

                return bytes; 

            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }



    }
}
