<html>
<h1>NoDisk Windows Web Server</h1>
<br>
<div style=" text-align:center">
    <img src="/NoDisk.WebServer/www/jackaloped.jpg">
  </div>
<br>
NoDisk is a High Performance Windows Web Server that caches all files into memory using a KV file system. Using IOCP instead of thread per socket allows the server to serve more concurrent connections than most web servers. The server is designed to have very little overhead besides the cached files. <br>
<br>
<b>Features</b><br>
 1.All files are cached in memory. No file IO peroid.<br>
 2.IOCP for kernel based async communications. <br>
 3.Max of 10,000 concurrent connections.<br>
 4.PHP support, but limited and CGI.<br>
 5.Will even on WinXP; .Net 4.0 <br>
 6.Use Plug-in based modules<br>
 7.Written in C#<br>
<br>
<b>Warnings</b><br>
1. Its POC state, use at your own risk.<br>
2. If you modify a file, you must restart the server since all files are cached in memory!<br>
3. Always fear the jackalope because he has a black gibus in his closet.<br>
<br>
<br>
Any questions can be emailed to me at tullostech@gmail<br>
<br>
Thanks<br>
Nick<br>
</html>
