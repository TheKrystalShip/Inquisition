using Inquisition.Exceptions;

namespace Inquisition.Logging
{
	internal class EventLog : LogBase
	{
		public override void Log<T>(params T[] value)
		{
			throw new InquisitionLoggingException("Not implemented");
		}
	}
}