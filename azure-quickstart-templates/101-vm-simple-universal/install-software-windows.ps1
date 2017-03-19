
#usage install-iperf-test.ps1 dnsname

param
(
      [string]$dnsName = $null,
	  [string]$adminUser
)


#Create Source folder
$source = 'C:\source' 
If (!(Test-Path -Path $source -PathType Container)) {New-Item -Path $source -ItemType Directory | Out-Null} 
function WriteLog($msg)
{
Write-Host $msg
$msg >> c:\source\install.log
}
function WriteDateLog
{
date >> c:\source\install.log
}
if(!$dnsName) {
 WriteLog "DNSName not specified" 
 throw "DNSName not specified"
}

WriteDateLog
WriteLog "Downloading iperf3" 
$url = 'https://iperf.fr/download/windows/iperf-3.1.3-win64.zip' 
$webClient = New-Object System.Net.WebClient  
$webClient.DownloadFile($url,$source + "\iperf3.zip" )  

WriteLog "Installing IIS"  
Install-WindowsFeature -Name "Web-Server"
WriteLog "Installing IIS: done"
 
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
New-NetFirewallRule -Name "IPERFUDP" -DisplayName "IPERF on UDP/5201" -Protocol UDP -LocalPort 5201 -Action Allow -Enabled True
New-NetFirewallRule -Name "IPERFTCP" -DisplayName "IPERF on TCP/5201" -Protocol TCP -LocalPort 5201 -Action Allow -Enabled True
New-NetFirewallRule -Name "HTTP" -DisplayName "HTTP" -Protocol TCP -LocalPort 80 -Action Allow -Enabled True
New-NetFirewallRule -Name "HTTPS" -DisplayName "HTTPS" -Protocol TCP -LocalPort 443 -Action Allow -Enabled True
New-NetFirewallRule -Name "WINRM1" -DisplayName "WINRM TCP/5985" -Protocol TCP -LocalPort 5985 -Action Allow -Enabled True
New-NetFirewallRule -Name "WINRM2" -DisplayName "WINRM TCP/5986" -Protocol TCP -LocalPort 5986 -Action Allow -Enabled True
New-NetFirewallRule -Name "RDP" -DisplayName "RDP TCP/3389" -Protocol TCP -LocalPort 3389 -Action Allow -Enabled True
}
Add-FirewallRules
WriteLog "Firewall configured" 


WriteLog "Creating Home Page" 
$ExternalIP = Invoke-RestMethod http://ipinfo.io/json | Select -exp ip
$LocalIP = Get-NetIPAddress -InterfaceAlias 'Ethernet 2' -AddressFamily IPv4
$OSInfo = Get-WmiObject Win32_OperatingSystem | Select-Object Caption, Version, ServicePackMajorVersion, OSArchitecture, CSName, WindowsDirectory, NumberOfUsers, BootDevice
$EditionId = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID').EditionId
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
		  <p>OS {1} Version {2} Architecture {3} </p>
		  <p>Local IP Address: {4} </p>
		  <p>External IP Address: {5} </p>
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
$content = $content -replace "\{1\}",$OSInfo.Caption
$content = $content -replace "\{2\}",$OSInfo.Version
$content = $content -replace "\{3\}",$OSInfo.OSArchitecture
$content = $content -replace "\{4\}",$LocalIP.IPAddress
$content = $content -replace "\{5\}",$ExternalIP

$content | Out-File -FilePath C:\inetpub\wwwroot\index.html -Encoding utf8
WriteLog "Creating Home Page done" 

WriteLog "Installing IPERF3 as a service" 
sc.exe create ipef3 binpath= "cmd.exe /c c:\source\iperf-3.1.3-win64\iperf3.exe -s -D" type= own start= auto DisplayName= "IPERF3"
WriteLog "IPERF3 Installed" 

WriteLog "Initialization completed !" 
WriteLog "Rebooting !" 
Restart-Computer -Force       
