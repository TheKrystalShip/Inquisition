using System;
using System.Runtime.Serialization;

namespace Inquisition.Exceptions
{
	public class InquisitionNotFoundException : Exception
	{
		public InquisitionNotFoundException()
		{
		}

		public InquisitionNotFoundException(string message) : base(message)
		{
		}

		public InquisitionNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
