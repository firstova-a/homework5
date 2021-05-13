using System;
using System.Collections.Generic;
using System.Text;

namespace homework5Server
{
	class ArtOrder
	{
		public int Id { get; set; }

		public string Description { get; set; }
		public string Content { get; set; }
		public decimal Price { get; set; }
		public Status Done { get; set; }
		public DateTime DeadLine { get; set; }
		public List<Type> Types { get; set; }
		public PaymentType PaymentType { get; set; }

		public List<Art> Arts { get; set; }

		public enum Status
		{
			accepted,
			rejected,
			inProgress,
			done
		}
	}
}
