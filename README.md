NoDisk Web Server
================

NoDisk is a High Performance Windows Web Server that caches all files into memory using KV.  Using IOCP instead of thread per socket allows the server to server more concurrent connections than most web servers.  The server is designed to have very little overhead besides the cached files. 

================
Features

1. All files are cached in memory. No file IO peroid.
2. IOCP for kernel based async communications. 
3. Max of 10,000 concurrent connections.
3. PHP support, but limited and CGI.
4. Will even on WinXP
5. Use Plug-in based modules
6. Written in C#

================

