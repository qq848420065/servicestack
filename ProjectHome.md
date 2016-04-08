# Service Stack .NET has moved to Git Hib! #

Service Stack has moved over to [GitHub](https://github.com/ServiceStack) at the main project site at https://github.com/ServiceStack/ServiceStack
### Service Stack Sub Projects now have their own homes as well ###
  * https://github.com/ServiceStack/ServiceStack.Text - JSV and JSON Fast text serializers
  * https://github.com/ServiceStack/ServiceStack.Redis - C# Redis Client
  * https://github.com/ServiceStack/ServiceStack.RedisWebServices - A Service Stack (i.e. XML, JSON, JSV, SOAP) Web Services layer around Redis
  * https://github.com/ServiceStack/ServiceStack.OrmLite - Simple, Fast, lightweight POCO ORM for Sqlite and SQL Server
  * https://github.com/ServiceStack/ServiceStack.Logging - Service Stack's implementation and dependency logging interface with adapters to log4net v1.2.10+, log4net 1.2.9, EventLog, Console, Debug, etc.

For the latest source code and binary releases please refer to the above active projects.
This project site is now in-active but will be kept alive for historical purposes.



&lt;hr/&gt;


<br />
<br />

# Previous Home Page #

_Latest Version of Service Stack released: [available here](https://github.com/ServiceStack/ServiceStack.Examples/downloads)._

Service Stack is a high-performance .NET web services framework _(including a number of high-performance sub-components: see below)_ that simplifies the development of XML, JSON, JSV and WCF SOAP [Web Services](ServiceStackWebServices.md). For more info check out [servicestack.net](http://www.servicestack.net).


# Simple web service example #
```
[DataContract]
public class GetFactorial
{
	[DataMember] 
	public long ForNumber { get; set; }
}

[DataContract]
public class GetFactorialResponse
{
	[DataMember] 
	public long Result { get; set; }
}

public class GetFactorialService : IService<GetFactorial>
{
	public object Execute(GetFactorial request)
	{
		return new GetFactorialResponse { Result = GetFactorial(request.ForNumber) };
	}

	static long GetFactorial(long n)
	{
		return n > 1 ? n * GetFactorial(n - 1) : 1;
	}
}
```

### Calling the service from any C#/.NET Client ###
```
//no code-gen required, can re-use above DTO's
var serviceClient = new XmlServiceClient("http://localhost/ServiceStack.Examples.Host.Web/Public/");
var response = this.ServiceClient.Send<GetFactorialResponse>(new GetFactorial { ForNumber = 3 });
Console.WriteLine("Result: {0}", response.Result);
```

### Calling the service from a Java Script client i.e. Ajax ###
```
var serviceClient = new JsonServiceClient("http://localhost/ServiceStack.Examples.Host.Web/Public/");
serviceClient.getFromService("GetFactorial", { ForNumber: 3 }, function(e) {
  alert(e.Result);
});
```

That's all the application code required to create a simple web service.

Preview links using just the above code sample with (live demo running on Linux):
[XML](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Xml/SyncReply/GetFactorial?ForNumber=3),
[JSON](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Json/SyncReply/GetFactorial?ForNumber=3),
[JSV](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Jsv/SyncReply/GetFactorial?ForNumber=3&debug)
Check out the [live demo](http://www.servicestack.net/ServiceStack.Examples.Clients/Default.htm) with [full source code](http://code.google.com/p/servicestack/source/browse/#svn/trunk/ServiceStack.Examples).

# Download #
To start developing web services with Service Stack we recommend starting with the ServiceStack.Examples project (includes ServiceStack.dlls):
  * http://servicestack.googlecode.com/files/ServiceStack.Examples.zip

If you already have ServiceStack and just want to download the latest release binaries get them at:
  * http://servicestack.googlecode.com/files/ServiceStack.zip

## Getting Started ##
An online tutorial that walks you through developing and calling web services is available here:
  * http://www.servicestack.net/monotouch/remote-info/

<br />

# Features of a modern web services framework #

Developed in the modern age, Service Stack provides an alternate, cleaner POCO-driven way of creating web services, featuring:

### Define your web services in a code-first approach using DSL-like POCO's ###
Service Stack was designed with a top-down view, i.e. we identified the minimum amount of effort required to implement a web service and ensured it remained that way.

What are the minimum number or artefacts required to implement a web service? From our point of view it is the `Request` and `Response` DTO's (which on their own define the web service contract or interface) and the actual implementation of the web service which would take in a `Request` and in most cases return the `Response`. As it turns out these remain the only classes required to create a functional web service in Service Stack.

The Request and Response DTO's are standard `DataContract` POCO's while the implementation just needs to inherit from a testable and dependency-free `IService<TRequestDto>`. As a bonus for keeping your DTO's in a separate dependency-free .dll, you're able to re-use them in your C#/.NET clients providing a strongly-typed API without any code-gen what-so-ever. Also your DTO's **define everything** Service Stack does not pollute your web services with any additional custom artefacts or markup.

Service Stack re-uses the custom artefacts above and with zero-config and without imposing any extra burden on the developer adds discover-ability and provides hosting of your web service on a number of different physical end-points which as of today includes: XML (+REST), JSON (+REST), JSV (+REST) and SOAP 1.1 / SOAP 1.2.

### Full support for unit and integration tests ###
Your application logic should not be tied to a third party vendor's web service host platform.
In Service Stack they're not, your web service implementations are host and end-point ignorant, dependency-free and can be unit-tested independently of ASP.NET, Service Stack or its IOC.

Without any code changes unit tests written can be re-used and run as integration tests simply by switching the IServiceClient used to point to a configured end-point host.

### Built-in Funq IOC container ###
Configured to auto-wire all of your web services with your registered dependencies.
[Funq](http://funq.codeplex.com) was chosen for it's high-performance, low footprint and intuitive full-featured minimalistic API.

### Encourages development of message-style, re-usable and batch-full web services ###
Entire POCO types are used to define the request and response DTO's to promote the creation well-defined coarse-grained web services. Message-based interfaces are best-practices when dealing with out-of-process calls as they are can batch more work using less network calls and are ultimately more re-usable as the same operation can be called using different calling semantics. This is in stark contrast to WCF's Operation or Service contracts which encourage RPC-style, application-specific web services by using method signatures to define each operation.

As it stands in general-purpose computing today, there is nothing more expensive you can do than a remote network call. Although easier for the newbie developer, by using _methods_ to define web service operations, WCF is promoting bad-practices by encouraging them to design and treat web-service calls like normal function calls even though they are millions of times slower. Especially at the app-server tier, nothing hurts performance and scalability of your client and server than multiple dependent and synchronous web service calls.

Batch-full, message-based web services are ideally suited in development of SOA services as they result in fewer, richer and more re-usable web services that need to be maintained. RPC-style services normally manifest themselves from a **client perspective** that is the result of the requirements of a single applications data access scenario. Single applications come and go over time while your data and services are poised to hang around for the longer term. Ideally you want to think about the definition of your web service from a **services and data perspective** and how you can expose your data so it is more re-usable by a number of your clients.

### Cross Platform Web Services Framework ###
With Mono on Linux now reaching full-maturity, Service Stack runs on .NET or Linux with Mono and can either be hosted inside an ASP.NET Web Application, Windows service or Console application running in or independently of a Web Server.

### Low Coupling for maximum accessibility and testability ###
No coupling between the transport's endpoint and your web service's payload. You can re-use your existing strongly-typed web service DTO's with any .NET client using the available Soap, Xml and Json Service Clients - giving you a strongly-typed API while at the same time avoiding the need for any generated code.

  * The most popular web service endpoints are configured by default. With no extra effort, each new web service created is immediately available and [discoverable](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Metadata) on the following end points:
    * [XML (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Xml/Metadata?op=GetFactorial)
    * [JSON (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Json/Metadata?op=GetFactorial)
    * [JSV (+REST)](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Jsv/Metadata?op=GetFactorial)
    * [SOAP 1.1](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Soap11/Metadata?op=GetFactorial)
    * [SOAP 1.2](http://www.servicestack.net/ServiceStack.Examples.Host.Web/Public/Soap12/Metadata?op=GetFactorial)
  * View the [Service Stack endpoints page](ServiceStackWebServices.md) for our recommendations on which endpoint to use and when.

# High Performance Sub Projects #
Also included in ServiceStack are libraries that are useful in the development of high performance web services:

  * [JsonSerializer](http://www.servicestack.net/mythz_blog/?p=344) - The fastest JSON Serializer for .NET. Over 3 times faster than other .NET JSON serialisers.
  * TypeSerializer - A fast, compact text serializer that is very resilient to schema changes and is:
    * 3.5x quicker and 2.6x smaller than the .NET XML DataContractSerializer and
    * 5.3x quicker and 1.3x smaller than the .NET JSON DataContractSerializer - _[view the detailed benchmarks](http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.1000000-times.2010-02-06.html)_

  * [ServiceStack.Redis](ServiceStackRedis.md) - An API complete C# [Redis](http://code.google.com/p/redis/) client with native support for persisting C# POCO objects.
    * You can download the latest [Windows build for the Redis Server here](RedisWindowsDownload.md).
    * [Redis Admin UI](http://www.servicestack.net/mythz_blog/?p=381) - An Ajax GUI admin tool to help visualize your Redis data.

  * OrmLite - A convention-based, configuration free lightweight ORM that uses attributes from DataAnnotations to infer the table schema. Currently supports both Sqlite and SqlServer.

  * [Caching](Caching.md) - A common interface for caching with providers for:
    * Memcached
    * In Memory Cache
    * Redis


## Find out More ##
  * Twitter: to get updates of ServiceStack, follow [@ServiceStack on twitter](http://twitter.com/ServiceStack).
  * Join the [ServiceStack Google+ community](https://plus.google.com/u/0/communities/112445368900682590445)

## Future Roadmap ##
Service Stack is under continuous improvement and is always adding features that are useful for high-performance, scalable and resilient web service scenarios. This is the current road map but is open to change.
If you have suggestions for new features or want to prioritize the existing ones below: [you can leave feedback here](http://code.google.com/p/servicestack/issues/entry).
  * Add an opt-in durable Message Queue service processing of [AsyncOneWay](AsyncOneWay.md) requests (with In Memory, Redis and RabbitMQ implementations)
  * Enable [ProtoBuf.NET](http://code.google.com/p/protobuf-net/), TypeSerializer and CSV (for tabular datasets) web service endpoints
  * Integrate the Spark View Engine and enable a HTML endpoint so web services can also return a HTML human-friendly view
  * New REST web service endpoint developed in the 'Spirit of REST' with partitioning of GET / POST / PUT / DELETE requests and utilizing 'Accept mimetype' HTTP request header to determine the resulting content type.
  * Code generated proxy classes for Objective-C, Java Script / Action Script (and Java or C++ if there's enough interest) clients.
  * Develop a completely managed HTTP Web Service Host (at the moment looking at building on top of [Kayak HTTP](http://kayakhttp.com))
  * Add support for 'Web Sockets' protocol

## Similar open source projects ##
Similar Open source .NET projects for developing or accessing web services include:

  * Open Rasta - REST-ful web service framwork:
    * http://trac.caffeine-it.com/openrasta

  * Rest Sharp - an open source REST client for .NET
    * http://restsharp.org




---


Runs on both Mono and .NET 3.5. _(Live preview hosted on Mono / CentOS)_