Configuration RRAS
{
  param ($MachineName)

  Node $MachineName
  {
    #Install the Routing Role
     WindowsFeature Routing
    {
        Name = "Routing"
        Ensure = "Present"
    }
  }
} 