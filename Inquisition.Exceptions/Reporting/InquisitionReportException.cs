using System;
using System.Runtime.Serialization;

namespace Inquisition.Excepitions
{
	public class InquisitionReportException : Exception
	{
		public InquisitionReportException(string message) : base(message)
		{
		}

		public InquisitionReportException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InquisitionReportException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
