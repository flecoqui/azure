
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

WriteLog "Installing iperf3" 

function DownloadAndUnzip($sourceUrl,$DestinationDir ) 
{
$EditionId = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID').EditionId

if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {

    $TempPath = [System.IO.Path]::GetTempFileName()
    if (($sourceUrl -as [System.URI]).AbsoluteURI -ne $null)
    {
        $handler = New-Object System.Net.Http.HttpClientHandler
        $client = New-Object System.Net.Http.HttpClient($handler)
        $client.Timeout = New-Object System.TimeSpan(0, 30, 0)
        $cancelTokenSource = [System.Threading.CancellationTokenSource]::new()
        $responseMsg = $client.GetAsync([System.Uri]::new($sourceUrl), $cancelTokenSource.Token)
        $responseMsg.Wait()
        if (!$responseMsg.IsCanceled)
        {
            $response = $responseMsg.Result
            if ($response.IsSuccessStatusCode)
            {
                $downloadedFileStream = [System.IO.FileStream]::new($TempPath, [System.IO.FileMode]::Create, [System.IO.FileAccess]::Write)
                $copyStreamOp = $response.Content.CopyToAsync($downloadedFileStream)
                $copyStreamOp.Wait()
                $downloadedFileStream.Close()
                if ($copyStreamOp.Exception -ne $null)
                {
                    throw $copyStreamOp.Exception
                }
            }
        }
    }
    else
    {
        throw "Cannot copy from $sourceUrl"
    }
    [System.IO.Compression.ZipFile]::ExtractToDirectory($TempPath, $DestinationDir)
    Remove-Item $TempPath
   }
}
DownloadAndUnzip $url $source 
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

 
function Install-IIS
{
#Install-PackageProvider NanoServerPackage
 #  Import-PackageProvider NanoServerPackage
  # Install-NanoServerPackage -Name Microsoft-NanoServer-Storage-Package
   #Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package
WriteLog "Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force"
Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force
WriteLog "Save-Module -Path "$env:programfiles\WindowsPowerShell\Modules\" -Name NanoServerPackage -minimumVersion 1.0.1.0"
Save-Module -Path "$env:programfiles\WindowsPowerShell\Modules\" -Name NanoServerPackage -minimumVersion 1.0.1.0
WriteLog "Import-PackageProvider NanoServerPackage"
Import-PackageProvider NanoServerPackage
WriteLog "Find-NanoServerPackage -name *"
Find-NanoServerPackage -name *
WriteLog "Find-NanoServerPackage *iis* | install-NanoServerPackage -culture en-us"
Find-NanoServerPackage *iis* | install-NanoServerPackage -culture en-us

# WriteLog "Installing IIS: Install-Module -Name NanoServerPackage -SkipPublisherCheck -force"
# Install-Module -Name NanoServerPackage -SkipPublisherCheck -force
 
# WriteLog "Installing IIS: Install-PackageProvider NanoServerPackage" 
# Install-PackageProvider NanoServerPackage
 
# WriteLog "Installing IIS:Set-ExecutionPolicy RemoteSigned -Scope Process" 
# Set-ExecutionPolicy RemoteSigned -Scope Process

# WriteLog "Installing IIS: Import-PackageProvider NanoServerPackage" 
# Import-PackageProvider NanoServerPackage

# WriteLog "Installing IIS: Find-NanoServerPackage –AllVersions -Name *IIS* -RequiredVersion 10.0.14393.0"
# Find-NanoServerPackage –AllVersions -Name *IIS* -RequiredVersion 10.0.14393.0

# WriteLog "Installing IIS: Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package -Culture en-us -RequiredVersion 10.0.14393.0"
# Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package -Culture en-us -RequiredVersion 10.0.14393.0

 WriteLog "Installing IIS: done"
# install-Module -Name NanoServerPackage -SkipPublisherCheck -force
# install-PackageProvider NanoServerPackage
# Set-ExecutionPolicy RemoteSigned -Scope Process
# Import-PackageProvider NanoServerPackage
# Install-NanoServerPackage -Name Microsoft-NanoServer-Storage-Package
# Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package
}

function Dump-Install-IIS
{
 "
 function WriteLog(`$msg) `r
 { `r
 Write-Host `$msg `r
 `$msg >> c:\source\installiis.log `r
 } `r
 function WriteDateLog `r
 { `r
 date >> c:\source\installiis.log `r
 } `r
 if (!(Test-Path -Path c:\source\iis.log)) `r
 {  `r
 WriteLog ""Installing IIS: Install-Module -Name NanoServerPackage -SkipPublisherCheck -force "" `r
 Install-Module -Name NanoServerPackage -SkipPublisherCheck -force `r
 WriteLog ""Installing IIS: Install-PackageProvider NanoServerPackage"" `r
 Install-PackageProvider NanoServerPackage `r 
 WriteLog ""Installing IIS:Set-ExecutionPolicy RemoteSigned -Scope Process""  `r
 Set-ExecutionPolicy RemoteSigned -Scope Process `r
 WriteLog ""Installing IIS: Import-PackageProvider NanoServerPackage"" `r 
 Import-PackageProvider NanoServerPackage `r
 WriteLog ""Installing IIS: Find-NanoServerPackage –AllVersions -Name *IIS* -RequiredVersion 10.0.14393.0"" `r
 Find-NanoServerPackage –AllVersions -Name *IIS* -RequiredVersion 10.0.14393.0 `r
 WriteLog ""Installing IIS: Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package -Culture en-us -RequiredVersion 10.0.14393.0"" `r
 Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package -Culture en-us -RequiredVersion 10.0.14393.0 `r
 WriteLog ""Installing IIS: done"" `r
 WriteLog ""Starting IIS""  `r
 net start w3svc `r
 WriteLog ""Starting IIS: done"" `r
 date >> c:\source\iis.log `r
 }`r
 "
}

function Install-script
{
 "
 #function bg() {Invoke-Command -scriptblock  { c:\source\iperf-3.1.3-win64\iperf3.exe -s -D --logfile iperflog.txt }}`r`n
 if (!(Test-Path -Path c:\source\iis.log)) `r`n
 {  `r`n
 echo Start installation script >> c:\source\iis.log `r`n 
 echo date >> c:\source\iis.log `r`n 
 echo Set-ExecutionPolicyRemoteSigned-ScopeProcess >> c:\source\iis.log `r`n 
 Set-ExecutionPolicy RemoteSigned -Scope Process `r`n
 Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force `r`n
 echo install-Module-NameNanoServerPackage-SkipPublisherCheck >> c:\source\iis.log `r`n 
 install-Module -Name NanoServerPackage -SkipPublisherCheck -force `r`n
 echo install-PackagePRoviderNanoServerPackage >> c:\source\iis.log `r`n 
 install-PackagePRovider NanoServerPackage `r`n
 echo Import-PackageProviderNanoServerPackage >> c:\source\iis.log `r`n 
 Import-PackageProvider NanoServerPackage `r`n
 echo Install-NanoServerPackage-NameMicrosoft-NanoServer-Storage-Package >> c:\source\iis.log `r`n 
 Install-NanoServerPackage -Name Microsoft-NanoServer-Storage-Package `r`n
 echo Install-NanoServerPackage-Name Microsoft-NanoServer-IIS-Package >> c:\source\iis.log `r`n 
 Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package `r`n
 echo Restart-Computer >> c:\source\iis.log `r`n 
 Restart-Computer -Force      `r`n 
 } `r`n
 else `r`n
 { `r`n
 echo Start installation script Nothing to do  >> c:\source\iis.log `r`n 
 echo date >> c:\source\iis.log `r`n 
 #bg `r`n
 } `r`n
 "
}

WriteDateLog
Install-IIS

WriteLog "Starting IIS" 
net start w3svc

#Install-script > c:\source\installiis.ps1
#WriteLog "Create file c:\source\installiis.ps1" 
#Dump-Install-IIS > c:\source\installiis.ps1

WriteDateLog
WriteLog "Installing IPERF3 as a service" 
sc.exe create ipef3 binpath= "cmd.exe /c c:\source\iperf-3.1.3-win64\iperf3.exe -s -D" type= own start= auto DisplayName= "IPERF3"
WriteLog "IPERF3 Installed"


#WriteLog "Create file c:\source\installiis.ps1" 
#Dump-Install-IIS > c:\source\installiis.ps1
#create scheduled task
#WriteLog "Create scheduled task for file c:\source\installiis.ps1" 
#$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-NoExit c:\source\installiis.ps1" 
#$trigger = New-ScheduledTaskTrigger -AtLogOn
#Register-ScheduledTask -TaskName "scriptiis" -Action $action -Trigger $trigger -RunLevel Highest -User $adminUser | Out-Null 



#echo "echo ""boot at"" >> c:\source\Startup.log"    > c:\source\Startup.cmd
#echo "date >> c:\source\Startup.log"    >> c:\source\Startup.cmd
#echo "ipconfig >> c:\source\Startup.log"   >> c:\source\Startup.cmd 
#schtasks /create /tn "Logboot" /tr c:\source\Startup.cmd /sc onstart /ru "System"

WriteLog "Initialization completed !" 
WriteDateLog
WriteLog "Rebooting !" 
Restart-Computer -Force  
WriteLog "Rebooting done!"      
WriteDateLog
