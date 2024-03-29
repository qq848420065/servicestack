using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ServiceStack.WebHost.Endpoints")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("ServiceStack.WebHost.Endpoints")]
[assembly: AssemblyCopyright("Copyright � Microsoft 2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("06704d66-af8e-411f-8260-8d05de5ce6ad")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

//Default DataContract namespace instead of tempuri.org
//Note: doesn't work for ilmerged assemblies
[assembly: ContractNamespace("http://schemas.servicestack.net/types", ClrNamespace = "ServiceStack.WebHost.Endpoints.Tests.Support.Operations")]
[assembly: ContractNamespace("http://schemas.servicestack.net/types", ClrNamespace = "ServiceStack.WebHost.Endpoints.Tests.Support.Types")]
