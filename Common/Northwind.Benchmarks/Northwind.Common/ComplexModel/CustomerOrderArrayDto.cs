using System.Linq;
using Northwind.Common.ServiceModel;
using Platform.Text;
using ProtoBuf;

namespace Northwind.Common.ComplexModel
{
	[ProtoContract(ImplicitFields = ImplicitFields.AllPublic, InferTagFromName = true)]
	[TextRecord]
	public class CustomerOrderArrayDto
	{
		public CustomerOrderArrayDto()
		{
			this.Orders = new FullOrderDto[0];
		}

		[TextField]
		public CustomerDto Customer { get; set; }

		[TextField]
		public FullOrderDto[] Orders { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as CustomerOrderArrayDto;
			if (other == null) return false;

			var i = 0;
			return this.Customer.Equals(other.Customer)
			       && this.Orders.All(x => x.Equals(other.Orders[i++]));
		}
	}
}