using System;
using System.Runtime.Serialization;

namespace Inquisition.Exceptions
{
	public class InquisitionLoggingException : Exception
	{
		public InquisitionLoggingException()
		{
		}

		public InquisitionLoggingException(string message) : base(message)
		{
		}

		public InquisitionLoggingException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionLoggingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
