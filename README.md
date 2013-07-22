NoDisk Web Server
================

NoDisk is a Windows Web Server that caches all files into memory using KV.  Using IOCP instead of thread per socket allows the server to server more concurrent connections than most web servers.  The server is designed to have very little overhead besides the cached files. 

================
Features
