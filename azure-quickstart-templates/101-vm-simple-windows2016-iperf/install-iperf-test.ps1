
#usage install-iperf-test.ps1 dnsname

param
(
      [string]$dnsName = $null,
)

if(!$dnsName) {
 throw "DNSName not specified"
}
#IIS installation

#iperf Installation


