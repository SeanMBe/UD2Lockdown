echo processing: adding cms to hosts file
set hostsFile="%systemroot%\system32\drivers\etc\hosts"
echo %hostsFile%
echo 127.0.0.1      cms > newFile.txt
type %hostsFile% >> newFile.txt
type newFile.txt > %hostsFile%



set certutil=certutil
echo processing: deleting client/server certificates and root authority
echo processing: delete certificate authority
%certutil% -delstore root 553c235c5bbeeea4482a21ebfe58ea3e

echo processing: delete client cert
%certutil% -delstore my 8468ee1574a3de7cdea89cc41c667c0d3c5d0089

echo processing: delete server cert
%certutil% -delstore my 5d656cd4f27cc0b427a88bb7cba134fb22f047ea

echo processing: adding client/server and root authority
echo processing: add certificate authority
%certutil% -addstore -f root "shawCA.cer"

echo processing: add client certificate
%certutil% -p password -importPFX cmsClient.pfx

echo processing: add server certificate
%certutil% -p password -importPFX cmsServer.pfx

set netsh=C:\windows\System32\netsh.exe

%netsh% http delete ssl ipport=127.0.0.1:443
%netsh% http delete ssl ipport=0.0.0.0:443
%netsh% http delete urlacl url=https://cms:443/

%netsh% http add sslcert ipport=127.0.0.1:443 appid={214124cd-d05b-4309-9af9-9caa44b2b74a} certhash=5d656cd4f27cc0b427a88bb7cba134fb22f047ea
%netsh% http add urlacl url=https://cms:443/ user=Everyone

