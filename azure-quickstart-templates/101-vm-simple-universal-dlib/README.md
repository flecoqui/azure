# Deployment of a VM (Linux or Windows) installing DLIB pre-requisites (g++,python)

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-simple-universal-dlib%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-simple-universal-dlib%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>

The objective of this template is to automate the creation of a VM (Linux or Windows) to generate DLIB components. </p>
Once the VM is deployed the DLIB source code is avaialble under:</p>
/git/dlib  (Linux)</p>
c:\git\dlib (Windows)</p>
So far this template allows you to deploy a simple VM running: </p>
#### Debian: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Ubuntu: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
The objective is to support soon : </p>
#### Centos: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Red Hat: Apache, g++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Windows Server 2016: IIS, VC++, cmake, Anaconda3, DLIB source code and samples (C++,Python),
#### Nano Server 2016: IIS, VC++, cmake, Anaconda3, DLIB source code and samples (C++,Python)
This will deploy in the region associated with Resource Group and the VM Size is one of the parameter.</p>
So far only the Debian and Ubuntu version have been tested.
Once the VM is deployed you can use the following bash files under /git/bash: </p>
#### buildDLIB.sh: build DLIB library(C++),
#### buildDLIBCPPSamples.sh: build DLIB C++ samples (C++),
#### buildDLIBPythonSamples.sh: build DLIB Python samples (Python),
#### runTests.sh: run unit tests (C++,Python)
</p>
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
Once the VM has been deployed, you can check the Web page hosted on the VM.</p>
For the Linux VM:</p>
http://<VM DNS Name>/index.php
For the Windows VM:</p>
http://<VM DNS Name>/index.html

</p>
For instance for Linux VM:

     http://vmubus001.eastus2.cloudapp.azure.com/index.php 

for Windows VM:

     http://vmnanos001.eastus2.cloudapp.azure.com/index.html 

</p>
Finally, you can open a remote session with the VM.

For instance for Linux VM:

     ssh VMAdmin@vmubus001.eastus2.cloudapp.azure.com

For Windows Server VM:

     mstsc /admin /v:vmwins001.eastus2.cloudapp.azure.com

For Nano Server VM:

     Set-Item WSMan:\\localhost\\Client\\TrustedHosts vmnanos001.eastus2.cloudapp.azure.com </p>
     Enter-PSSession -ComputerName vmnanos001.eastus2.cloudapp.azure.com </p>

</p>
Once you are connected with ssh, you can use the following bash files under /git/bash to: </p>
- buildDLIB.sh: build DLIB library(C++),</p>
- buildDLIBCPPSamples.sh: build DLIB C++ samples (C++),</p>
- buildDLIBPythonSamples.sh: build DLIB Python samples (Python),</p>
- runTests.sh: run unit tests (C++,Python)</p>
</p>
By default the bash files are available under /git/bash folder.</p>

</p>
If you want to test DLIB with the python samples, you'll find under /git/dish/python_examples several python files.
You can for instance run the following sample ./svm_rank.py to check the python configuration:
Keep in mind before running this test, you need to build the DLIB library and the python samples.

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/test1.png)


If you want to test python samples which require GUI, you need to install an X11 terminal on your client. If you are using a Windows PC, you could install Putty and XMing to support X11 on your PC:</p>
PuTTy from http://www.chiark.greenend.org.uk/~sgtatham/putty/ </p>
Xming from http://sourceforge.net/project/downloading.php?group_id=156984&filename=Xming-6-9-0-31-setup.exe </p>


Install Xming following the installation screenshots below:

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/xming1.jpg)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/xming2.jpg)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/xming3.jpg)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/xming4.jpg)

Configure Putty following the installation screenshots below:</p>

Enter the dns name of your server :</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty1.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty2.jpg)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty3.jpg)


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty4.jpg)


The server is already configured to support X11 over SSH:
The file /etc/ssh/ssh_config has been updated:</p>
ForwardAgent yes</p>
ForwardX11 yes</p>
ForwardX11Trusted yes </p>

The file /etc/ssh/sshd_config has been updated:</p>
X11Forwarding yes</p>


Before launching Putty to open an SSH session with your VM check that XMing is running on your local Windows Machine:
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/xming5.png)


Once you are connected with Putty,

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty5.png)

Enter the following commands: </p>
gnome-session </p>
xclock </p>

The Clock should be displayed on your Windows machine:

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty6.png)

Now you can test the python samples requiring GUI:</p>
For instance under /git/dish/python_examples run the following command:</p>
 ./face_landmark_detection.py ../../dlib-models/shape_predictor_68_face_landmarks.dat ../examples/faces</p>

 and check that the picture is displayed on your local machine:</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-simple-universal-dlib/Docs/putty7.png)





## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete dlibgrpeu2 eastus2
