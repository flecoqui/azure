
#usage install-iperf-test.ps1 dnsname

param
(
      [string]$dnsName = $null
)

if(!$dnsName) {
 throw "DNSName not specified"
}
#IIS installation

#iperf Installation

$source = 'C:\source' 
 
If (!(Test-Path -Path $source -PathType Container)) {New-Item -Path $source -ItemType Directory | Out-Null} 

Write-Host "Downloading iperf3" 
$url = 'https://iperf.fr/download/windows/iperf-3.1.3-win64.zip'
$webClient = New-Object System.Net.WebClient 
$webClient.DownloadFile($url,$source + "\iperf3.zip" ) 

Write-Host "Installing iperf3" 
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
#Add-Type -assembly “system.io.compression.filesystem”
#[io.compression.zipfile]::ExtractToDirectory($source + "\iperf3.zip",$source)




Write-Host "Configuring firewall" 
function Add-FirewallRules
{
netsh advfirewall firewall add rule name="iperfudp" dir=in action=allow protocol=UDP localport=5201
netsh advfirewall firewall add rule name="iperftcp" dir=in action=allow protocol=TCP localport=5201
netsh advfirewall firewall add rule name="http" dir=in action=allow protocol=TCP localport=80
}
Add-FirewallRules
#Import-Module NetSecurity
#New-NetFirewallRule -Name http -DisplayName “Allow http” -Description “Allow http” -Protocol TCP -LocalPort 80 -Enabled True -Profile Any -Action Allow 
#New-NetFirewallRule -Name iperf3tcp -DisplayName “Allow iperf3 tcp” -Description “Allow iperf3 tcp” -Protocol TCP -LocalPort 5201 -Enabled True -Profile Any -Action Allow 
#New-NetFirewallRule -Name iperf3udp -DisplayName “Allow iperf3 udp” -Description “Allow iperf3 udp” -Protocol UDP -LocalPort 5201 -Enabled True -Profile Any -Action Allow 

function bg() {Start-Job -scriptblock { @args }}

Write-Host "Launching iperf3" 
$exe = $source + "\iperf-3.1.3-win64\iperf3.exe" 
bg $exe "-s" 

Write-Host "Creating Home Page" 
$dnsName="toot.ttott.com"
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
