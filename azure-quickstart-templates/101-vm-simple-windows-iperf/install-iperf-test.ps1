# This script install :
# - iperf3 on a computer running Windows
# - Create a basic HTML home page
#
#usage install-iperf-test.ps1 dnsname

param
(
      [string]$dnsName = $null
)

set-executionpolicy remotesigned -Force

#Create Source folder
$source = 'C:\source' 
If (!(Test-Path -Path $source -PathType Container)) {New-Item -Path $source -ItemType Directory | Out-Null} 
function WriteLog($msg)
{
Write-Host $msg
$msg >> c:\source\test.log
}

if(!$dnsName) {
 WriteLog "DNSName not specified" 
 throw "DNSName not specified"
}

 WriteLog "Downloading iperf3"  
 $url = 'https://iperf.fr/download/windows/iperf-3.1.3-win64.zip' 
 $webClient = New-Object System.Net.WebClient  
 $webClient.DownloadFile($url,$source + "\iperf3.zip" )  
 
 
 WriteLog "Installing iperf3"  
 # Function to unzip file contents 
 function Expand-ZIPFile($file, $destination) 
 { 
     $shell = new-object -com shell.application 
     $zip = $shell.NameSpace($file) 
     foreach($item in $zip.items()) 
     { 
         # Unzip the file with 0x14 (overwrite silently) 
         $shell.Namespace($destination).copyhere($item, 0x14) 
     } 
 } 
Expand-ZIPFile -file "$source\iperf3.zip" -destination $source 
WriteLog "iperf3 Installed"  



WriteLog "Configuring firewall" 
function Add-FirewallRules
{
netsh advfirewall firewall add rule name="iperfudp" dir=in action=allow protocol=UDP localport=5201
netsh advfirewall firewall add rule name="iperftcp" dir=in action=allow protocol=TCP localport=5201
netsh advfirewall firewall add rule name="http" dir=in action=allow protocol=TCP localport=80
netsh advfirewall firewall add rule name="http" dir=in action=allow protocol=TCP localport=3389
}
Add-FirewallRules
WriteLog "Firewall configured" 


WriteLog "Creating Home Page" 
If (!(Test-Path -Path C:\inetpub -PathType Container)) {New-Item -Path C:\inetpub -ItemType Directory | Out-Null} 
If (!(Test-Path -Path C:\inetpub\wwwroot -PathType Container)) {New-Item -Path C:\inetpub\wwwroot -ItemType Directory | Out-Null} 
$content = @'
<html>
  <head>
    <title>Sample "Hello from {0}" </title>
  </head>
  <body bgcolor=white>
    <table border="0" cellpadding="10">
      <tr>
        <td>
          <h1>Hello from {0}</h1>
        </td>
      </tr>
    </table>

    <p>This is the home page for the iperf3 test on Azure VM</p>
    <p>Launch the command line from your client: </p>
    <p>     iperf3 -c {0} -p 5201 --parallel 32  </p>
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
'@
$content = $content -replace "\{0\}",$dnsName
$content | Out-File -FilePath C:\inetpub\wwwroot\index.html -Encoding utf8
WriteLog "Creating Home Page done" 

WriteLog "Installing IPERF3 as a service" 
sc.exe create ipef3 binpath= "cmd.exe /c c:\source\iperf-3.1.3-win64\iperf3.exe -s -D" type= own start= auto DisplayName= "IPERF3"
WriteLog "IPERF3 Installed" 

WriteLog "Initialization completed !" 
WriteLog "Rebooting !" 
Restart-Computer -Force       
