using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Messaging.Tests.Services
{
	[DataContract]
	public class UnRetryableFail
	{
		[DataMember]
		public string Name { get; set; }
	}

	[DataContract]
	public class UnRetryableFailResponse
	{
		[DataMember]
		public string Result { get; set; }
	}

	public class UnRetryableFailService
		: AsyncServiceBase<UnRetryableFail>
	{
		public int TimesCalled { get; set; }
		public string Result { get; set; }

		protected override object Run(UnRetryableFail request)
		{
			this.TimesCalled++;

			throw new UnRetryableMessagingException(
				"This request should not get retried",
				new NotSupportedException("This service always fails"));
		}
	}

}