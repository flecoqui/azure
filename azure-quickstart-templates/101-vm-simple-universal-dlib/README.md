# Deployment of a VM (Linux or Windows) installing DLIB pre-requisites (g++,python)

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-simple-universal-dlib%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-simple-universal-dlib%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy a simple VM running: </p>
#### Debian: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Ubuntu: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Centos: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Red Hat: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Windows Server 2016: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Nano Server 2016: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python)
This will deploy in the region associated with Resource Group and the VM Size is one of the parameter.</p>
So far only the Debian and Ubuntu version have been tested.

With Azure CLI you can deploy this VM with 2 command lines:


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/1-architecture.png)



## CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create dlibgrpeu2 eastus2

## DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create dlibgrpeu2 depdlibtest -f azuredeploy.json -e azuredeploy.parameters.json -vv

Beyond login/password, the input parameters are :</p>
configurationSize (Small: F2 and 128 GB data disk, Medium: F4 and 256 GB data disk, Large: F8 and 512 GB data disk, XLarge: F16 and 1024 GB data disk) : 

    "configurationSize": {
      "type": "string",
      "defaultValue": "Small",
      "allowedValues": [
        "Small",
        "Medium",
        "Large",
        "XLarge"
      ],
      "metadata": {
        "description": "Configuration Size: VM Size + Disk Size"
      }
    }

configurationOS (debian, ubuntu, centos, redhat, nano server 2016, windows server 2016): 

    "configurationOS": {
      "type": "string",
      "defaultValue": "debian",
      "allowedValues": [
        "debian",
        "ubuntu",
        "centos",
        "redhat",
        "nanoserver2016",
        "windowsserver2016"
      ],
      "metadata": {
        "description": "The Operating System to be installed on the VM. Default value debian. Allowed values: debian,ubuntu,centos,redhat,nanoserver2016,windowsserver2016"
      }
    },



## TEST THE VM:
Once the VM has been deployed, you can open the Web page hosted on the VM.
For instance for Linux VM:

     http://vmubus001.eastus2.cloudapp.azure.com/index.php 

for Windows VM:

     http://vmnanos001.eastus2.cloudapp.azure.com/index.html 

</p>
You can also use the following bash files to: </p>

#### buildDLIB.sh: build DLIB library(C++),
#### buildDLIBCPPSamples.sh: build DLIB C++ samples (C++),
#### buildDLIBPythonSamples.sh: build DLIB Python samples (Python),
#### runTests.sh: run unit tests (C++,Python)
By default the bash files are available under /DLIB folder.</p>

</p>
Finally, you can open a remote session with the VM.

For instance for Linux VM:

     ssh VMAdmin@vmubus001.eastus2.cloudapp.azure.com

For Windows Server VM:

     mstsc /admin /v:vmwins001.eastus2.cloudapp.azure.com

For Nano Server VM:

     Set-Item WSMan:\\localhost\\Client\\TrustedHosts vmnanos001.eastus2.cloudapp.azure.com </p>
     Enter-PSSession -ComputerName vmnanos001.eastus2.cloudapp.azure.com </p>


## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete dlibgrpeu2 eastus2
