using Inquisition.Exceptions;

namespace Inquisition.Logging
{
	internal class DbLog : LogBase
	{
		public override void Log<T>(params T[] value)
		{
			throw new InquisitionLogException("No implemented");
		}
	}
}