using System.Collections.Generic;
using System.Runtime.Serialization;

namespace @ServiceModelNamespace@.Version100.Types
{
	[CollectionDataContract(Namespace = "http://schemas.ddnglobal.com/types/", ItemName = "Property")]
	public class Properties : List<Property>
	{
		public Properties() { }
		public Properties(IEnumerable<Property> collection) : base(collection) { }
	}
}