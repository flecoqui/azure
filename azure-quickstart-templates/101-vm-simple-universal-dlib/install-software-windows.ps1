
#usage install-software-windows.ps1 dnsname

param
(
      [string]$dnsName = $null,
	  [string]$adminUser
)


#Create C:\var\log folder
$sourcevar = 'C:\var' 
If (!(Test-Path -Path $sourcevar -PathType Container)) {New-Item -Path $sourcevar -ItemType Directory | Out-Null} 
$sourcelog = 'C:\var\log' 
If (!(Test-Path -Path $sourcelog -PathType Container)) {New-Item -Path $sourcelog -ItemType Directory | Out-Null} 
#Create C:\git folder
$sourcegit = 'C:\git' 
If (!(Test-Path -Path $sourcegit -PathType Container)) {New-Item -Path $sourcegit -ItemType Directory | Out-Null} 
$sourcebash = 'C:\git\bash' 
If (!(Test-Path -Path $sourcebash -PathType Container)) {New-Item -Path $sourcebash -ItemType Directory | Out-Null} 

Break Script 

function WriteLog($msg)
{
Write-Host $msg
$msg >> c:\var\log\install.log
}
function WriteDateLog
{
date >> c:\var\log\install.log
}
if(!$dnsName) {
 WriteLog "DNSName not specified" 
 throw "DNSName not specified"
}
function DownloadAndUnzip($sourceUrl,$DestinationDir ) 
{
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
function Download($sourceUrl,$DestinationDir ) 
{
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
                $downloadedFileStream = [System.IO.FileStream]::new($DestinationDir, [System.IO.FileMode]::Create, [System.IO.FileAccess]::Write)
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
}

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
WriteDateLog

WriteLog "Downloading git" 
$url = 'https://github.com/git-for-windows/git/releases/download/v2.14.2.windows.1/Git-2.14.2-64-bit.exe' 
$EditionId = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID').EditionId
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
	Download $url $sourcebash 
	WriteLog "git downloaded" 
}
else
{
	$webClient = New-Object System.Net.WebClient  
	$webClient.DownloadFile($url,$sourcebash + "\Git-2.14.2-64-bit.exe" )  
	WriteLog "git downloaded"  
}

$url = 'https://aka.ms/vs/15/release/vs_community.exe' 
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
	Download $url $sourcebash 
	WriteLog "git downloaded" 
}
else
{
	$webClient = New-Object System.Net.WebClient  
	$webClient.DownloadFile($url,$sourcebash + "\vs_community.exe" )  
	WriteLog "git downloaded"  
}
$url = 'https://github.com/philr/bzip2-windows/releases/download/v1.0.6/bzip2-1.0.6-win-x64.zip'
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
	DownloadAndUnzip $url $sourcebash 
	WriteLog "bzip2 downloaded" 
}
else
{
	$webClient = New-Object System.Net.WebClient  
	$webClient.DownloadFile($url,$sourcebash + "\bzip2-1.0.6-win-x64.zip" )  
	# Function to unzip file contents 
	Expand-ZIPFile -file "$source\bzip2-1.0.6-win-x64.zip" -destination $source 
	WriteLog "bzip2 downloaded"  
}
$url = 'https://cmake.org/files/v3.9/cmake-3.9.3-win64-x64.zip'
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
	DownloadAndUnzip $url $sourcebash 
	WriteLog "cmake downloaded" 
}
else
{
	$webClient = New-Object System.Net.WebClient  
	$webClient.DownloadFile($url,$sourcebash + "\cmake-3.9.3-win64-x64.zip" )  
	# Function to unzip file contents 
	Expand-ZIPFile -file "$source\cmake-3.9.3-win64-x64.zip" -destination $source 
	WriteLog "cmake downloaded"  
}

function Install-IIS
{
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
}
WriteLog "Installing IIS"  
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
Install-IIS
}
else
{
Install-WindowsFeature -Name "Web-Server"
}
WriteLog "Installing IIS: done"


WriteLog "Configuring firewall" 
function Add-FirewallRulesNano
{
New-NetFirewallRule -Name "HTTP" -DisplayName "HTTP" -Protocol TCP -LocalPort 80 -Action Allow -Enabled True
New-NetFirewallRule -Name "HTTPS" -DisplayName "HTTPS" -Protocol TCP -LocalPort 443 -Action Allow -Enabled True
New-NetFirewallRule -Name "WINRM1" -DisplayName "WINRM TCP/5985" -Protocol TCP -LocalPort 5985 -Action Allow -Enabled True
New-NetFirewallRule -Name "WINRM2" -DisplayName "WINRM TCP/5986" -Protocol TCP -LocalPort 5986 -Action Allow -Enabled True
}
function Add-FirewallRules
{
New-NetFirewallRule -Name "HTTP" -DisplayName "HTTP" -Protocol TCP -LocalPort 80 -Action Allow -Enabled True
New-NetFirewallRule -Name "HTTPS" -DisplayName "HTTPS" -Protocol TCP -LocalPort 443 -Action Allow -Enabled True
New-NetFirewallRule -Name "RDP" -DisplayName "RDP TCP/3389" -Protocol TCP -LocalPort 3389 -Action Allow -Enabled True
}
if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
	Add-FirewallRulesNano
}
else
{
	Add-FirewallRules
}
WriteLog "Firewall configured" 


WriteLog "Creating Home Page" 
$ExternalIP = Invoke-RestMethod http://ipinfo.io/json | Select -exp ip

if (($EditionId -eq "ServerStandardNano") -or
    ($EditionId -eq "ServerDataCenterNano") -or
    ($EditionId -eq "NanoServer") -or
    ($EditionId -eq "ServerTuva")) {
$LocalIP = Get-NetIPAddress -InterfaceAlias 'Ethernet' -AddressFamily IPv4
$BuildNumber = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'CurrentBuild').CurrentBuild
$osstring = @'
OS {1} BuildNumber {2}
'@
$osstring = $osstring -replace "\{1\}",$EditionId
$osstring = $osstring -replace "\{2\}",$BuildNumber
}
else
{
$LocalIP = Get-NetIPAddress -InterfaceAlias 'Ethernet 2' -AddressFamily IPv4
$OSInfo = Get-WmiObject Win32_OperatingSystem | Select-Object Caption, Version, ServicePackMajorVersion, OSArchitecture, CSName, WindowsDirectory, NumberOfUsers, BootDevice
$osstring = @'
OS {1} Version {2} Architecture {3}
'@
$osstring = $osstring -replace "\{1\}",$OSInfo.Caption
$osstring = $osstring -replace "\{2\}",$OSInfo.Version
$osstring = $osstring -replace "\{3\}",$OSInfo.OSArchitecture
}
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
		  <p>{1}</p>
		  <p>Local IP Address: {2} </p>
		  <p>External IP Address: {3} </p>
        </td>
      </tr>
    </table>

    <p>This is the home page for the DLIB test on Azure VM</p>
    <p>Launch the following command line from your client to open an RDP session: </p>
    <p>     mstsc /admin /v:{0}  </p>
	<p>Launch the following commands in the RDP session from C:\GIT\BASH : </p>
    <p>     bash buildDLIB.sh: to build DLIB library </p> 
    <p>     bash buildDLIBCPPSamples.sh: to build DLIB C++ samples </p> 
    <p>     bash buildDLIBPythonSamples.sh: to build DLIB Python samples </p> 
    <p>     bash runDLIBTests.sh: to run DLIB tests </p> 
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
'@
$content = $content -replace "\{0\}",$dnsName
$content = $content -replace "\{1\}",$osstring
$content = $content -replace "\{2\}",$LocalIP.IPAddress
$content = $content -replace "\{3\}",$ExternalIP

$content | Out-File -FilePath C:\inetpub\wwwroot\index.html -Encoding utf8
WriteLog "Creating Home Page done" 
WriteLog "Starting IIS" 
net start w3svc

WriteLog "Installing git" 
c:\git\bash\Git-2.14.2-64-bit.exe
WriteLog "git Installed" 
WriteLog "Installing VS" 
c:\git\bash\vs_community.exe --add Microsoft.VisualStudio.Workload.NativeCrossPlat --add Microsoft.VisualStudio.Workload.NativeDesktop --add Microsoft.VisualStudio.Workload.Python --quiet --norestart
WriteLog "VS Installed" 

WriteLog "Downloading DLIB source code"
cd c:\git
git clone https://github.com/davisking/dlib.git
git clone https://github.com/davisking/dlib-models.git 
#uncompress models
c:\git\bash\bzip2.exe -d /git/dlib-models/dlib_face_recognition_resnet_model_v1.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/mmod_dog_hipsterizer.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/mmod_front_and_rear_end_vehicle_detector.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/mmod_human_face_detector.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/mmod_rear_end_vehicle_detector.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/resnet34_1000_imagenet_classifier.dnn.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/shape_predictor_5_face_landmarks.dat.bz2
c:\git\bash\bzip2.exe -d /git/dlib-models/shape_predictor_68_face_landmarks.dat.bz2
WriteLog "DLIB source code downloaded" 

WriteLog "Creating batch files"
echo cd c:\git\dlib > c:\git\bash\buildDLIB.bat
echo mkdir build >> c:\git\bash\buildDLIB.bat
echo cd build >> c:\git\bash\buildDLIB.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe .. >> c:\git\bash\buildDLIB.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe --build . --config Release >> c:\git\bash\buildDLIB.bat

echo cd c:\git\dlib\examples > c:\git\bash\buildDLIBCPPSamples.bat
echo mkdir build >> c:\git\bash\buildDLIBCPPSamples.bat
echo cd build >> c:\git\bash\buildDLIBCPPSamples.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe .. >> c:\git\bash\buildDLIBCPPSamples.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe --build .  >> c:\git\bash\buildDLIBCPPSamples.bat

echo cd c:\git\dlib > c:\git\bash\buildDLIBPythonSamples.bat
echo python setup.py install >> c:\git\bash\buildDLIBPythonSamples.bat

echo cd c:\git\dlib\dlib\test > c:\git\bash\runDLIBTests.bat
echo mkdir build >> c:\git\bash\runDLIBTests.bat
echo cd build >> c:\git\bash\runDLIBTests.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe .. >> c:\git\bash\runDLIBTests.bat
echo c:\git\bash\cmake-3.9.3-win64-x64\bin\cmake.exe --build . --config Release >> c:\git\bash\runDLIBTests.bat
echo dtest.exe --runall >> c:\git\bash\runDLIBTests.bat
WriteLog "Batch files created"


WriteLog "Initialization completed !" 
WriteLog "Rebooting !" 
Restart-Computer -Force       
