# Deployment of N VMs running IIS (port 80) and iperf3 (port 5201) behind a Load Balancer and N VM in a backend network using the same IP address for the outbound traffic.

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-windows-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2F%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-windows-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy the following configuration:
An Azure Load Balancer which routes the inbound traffic towards an availability set of VMs running a Web Site and iperf3 in server mode in the frontend subnet.
Moreover, all the VMs in the backend subnet must use the same outbount IP address as the service they need to reach is protected with a whitelist of IP addresses.
A route table associated with the backend subnet will route all the outbound traffic towards a VM running RRAS between two NIC cards: one connected to the backend subnet and the second one connected a a public IP address.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/architecture.png)
</p>
### The main parameters are:</p>
- The VM oS Version (Windows 2008 R2, 2012, 2012 R2, 2016)</p>
- The VM size for the VM in the frontend subnet</p>
- The number of VM in the frontend subnet</p>
- The VM size for the VM in the backend subnet</p>
- The number of VM in the backend subnet</p>
- The VM size for the forward VM. As this VM needs to support 2 NIC card, you need to select at least D2 VMs.</p>


vmSize (Standard_AX, Standard_DX, Standard_DX_v2, Standard_FX, ...) : 

    "vmSize": {
      "type": "string",
      "metadata": {
        "description": "VM Size"
      },
      "defaultValue": "Standard_A1"
    },

vmCount (from 1 to 4): 

    "vmCount": {
      "type": "int",
      "metadata": {
        "description": "Number of VMs"
      },
      "defaultValue": 2,
      "allowedValues": [
        1,
        2,
        3,
        4
      ]
    },

osVersion (Windows 2008 R2, 2012, 2012 R2, 2016):

    "osVersion": {
      "type": "string",
      "metadata": {
        "description": "OS Version - Image SKU"
      },
      "defaultValue": "2016-Datacenter",
      "allowedValues": [
        "2008-R2-SP1",
        "2012-Datacenter",
        "2012-R2-Datacenter",
        "2016-Datacenter"
      ]
    },

The Load Balancer is configured to forward the following ports:
### RDP TCP 3389 (command line: "mstsc /admin /v:[PublicIPAddress:5000X]")
Load Balancer Public IP Address : port tcp 50000   ->   VM0 port tcp 3389</p>
Load Balancer Public IP Address : port tcp 50001   ->   VM1 port tcp 3389</p>

### iPerf TCP (command line: "iperf3 -c [PublicIPAddress] -p [5200X]")
Load Balancer Public IP Address : port tcp 52000   ->   VM0 port tcp 5201</p>
Load Balancer Public IP Address : port tcp 52001   ->   VM1 port tcp 5201</p>

### iPerf UDP (command line: "iperf3 -c [PublicIPAddress] -u -p [5200X]")
Load Balancer Public IP Address : port udp 52000   ->   VM0 port udp 5201</p>
Load Balancer Public IP Address : port udp 52001   ->   VM1 port udp 5201</p>

With Azure CLI you can deploy these VMs and the Load Balancer with 2 following command lines:

## CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create rraslbgrp eastus2

## DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create rraslbgrp deprraslbtest -f azuredeploy.json -e azuredeploy.parameters.json -vv


In order to complete the installation you need to install and configure manually the RRAS on the foward VM. So far, I didn't manage to get an automated installation step.
</p>

1. Once the deployment is done in the output section you can see the mstsc command line to establish a RDP session with the VMs in the frontend subnet.  
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/1-mstsc.png)
</p>
2. Launch the mstsc command to open a session with the first VM using port 50000. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/2-mstsc.png)
</p>
3. Once connected with the VM in the frontend subnet, open a RDP session with the Forward VM (10.0.2.10). 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/3-mstsc.png)
</p>
4. Add the role Remote Access on the Forward VM . 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/4-addrole.png)
</p>
5. Select Remote Access role in the list. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/5-remoteaccess.png)
</p>
6. Select the Direct Access service and the Routing service. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/6-routing.png)
</p>
7. Once the installation is done click on the "Close" button. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/7-install.png)
</p>
8. Now we need to configure the RRAS service: from the Start Menu select Windows Administrative Tools button. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/8-menu.png)
</p>
9. Select the Computer Management application. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/10-computermanagement.png)
</p>
10. Select Routing and Remote Access item in the tree and right-click, and select the popup menu Configure and Enable Routing and Remote Access. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/11-enableroutin.png)
</p>
11. Select the Network Address Translation configuration. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/12-nat.png)
</p>
12. If on the NAT Internet Connection page you don't see any Network Interface reboot the VM.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/13-failed.png)
</p>
13. On the NAT Internet Connection select the NIC card associated with IP address 10.0.2.10.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/14-nic.png)
</p>
14. When the configuration is done click on the Finish button to close the dialog box.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/15-config.png)
</p>
15. You should see the NAT item in the tree meaning the NAT service is running 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/16-check.png)
</p>
16. Now the RRAS is configured and running, open a RDP session with the VM in the backend subnet(10.0.1.10). 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/17-mstsc.png)
</p>
17. Launch the Web Browser. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/18-explorer.png)
</p>
17. Open a Web site which displays the public IP address associated with your VM. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/19-myip.png)
</p>
18. The IP address displayed should the same as the one associated with the public IP of the Forward VM on the Azure portal. 
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/Docs/20-myip.png)
</p>


## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete rraslbgrp eastus2
