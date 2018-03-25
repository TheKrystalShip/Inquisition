using System;
using System.Runtime.Serialization;

namespace Inquisition.Exceptions
{
	public class InquisitionDatabaseException : Exception
	{
		public InquisitionDatabaseException(string message) : base(message)
		{
		}

		public InquisitionDatabaseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
