using System;
using System.Runtime.Serialization;

namespace Inquisition.Exceptions
{
	public class InquisitionLogException : Exception
	{
		public InquisitionLogException()
		{
		}

		public InquisitionLogException(string message) : base(message)
		{
		}

		public InquisitionLogException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionLogException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
