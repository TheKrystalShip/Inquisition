using System;
using System.Runtime.Serialization;

namespace Inquisition.Exceptions
{
	public class InquisitionException : Exception
	{
		public InquisitionException()
		{
		}

		public InquisitionException(string message) : base(message)
		{
		}

		public InquisitionException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
