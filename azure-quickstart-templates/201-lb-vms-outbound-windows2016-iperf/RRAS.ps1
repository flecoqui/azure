Configuration RRAS
{
  param ($MachineName)

  Node $MachineName
  {
    #Install the IIS Role
    WindowsFeature RemoteAccess
    {
      Ensure = "Present"
      Name = "RemoteAccess"
    }

     WindowsFeature Routing
    {
        Name = "Routing"
        Ensure = "Present"
    }
  }
} 