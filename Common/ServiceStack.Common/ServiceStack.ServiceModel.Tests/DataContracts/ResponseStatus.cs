﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack.ServiceModel.Tests.DataContracts
{
	[DataContract(Namespace = "http://schemas.servicestack.net/types/")]
	public class ResponseStatus
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseStatus"/> class.
		/// 
		/// A response status without an errorcode == success
		/// </summary>
		public ResponseStatus()
		{
			this.Errors = new List<ResponseError>();
		}

		[DataMember]
		public string ErrorCode { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public List<ResponseError> Errors { get; set; }
	}
}