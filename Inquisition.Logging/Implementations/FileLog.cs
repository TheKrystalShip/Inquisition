using Inquisition.Exceptions;

namespace Inquisition.Logging
{
	internal class FileLog : LogBase
	{
		public override void Log<T>(params T[] value)
		{
			throw new InquisitionLoggingException("Not implemented");
		}
	}
}