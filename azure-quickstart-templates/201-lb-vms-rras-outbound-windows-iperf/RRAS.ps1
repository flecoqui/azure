Configuration RRAS
{
  param ($MachineName)

  Node $MachineName
  {
    #Install the Routing Role
    WindowsFeature RemoteAccess
    {
        Name = "RemoteAccess"
        Ensure = "Present"
    }    
    WindowsFeature Routing
    {
        Name = "Routing"
        Ensure = "Present"
    }
    WindowsFeature DirectAccess-VPN
    {
        Name = "DirectAccess-VPN"
        Ensure = "Present"
    }
  }
} 