namespace Inquisition.Logging
{
	public static class LogHandler
	{
		public static void WriteLine<T>(LogTarget target, params T[] value)
		{
			LogBase logBase;

			switch (target)
			{
				case LogTarget.File:
					logBase = new FileLog();
					logBase.Log<T>(value);
					break;
				case LogTarget.Database:
					logBase = new DbLog();
					logBase.Log<T>(value);
					break;
				case LogTarget.Event:
					logBase = new EventLog();
					logBase.Log<T>(value);
					break;
				case LogTarget.Console:
					logBase = new ConsoleLog();
					logBase.Log<T>(value);
					break;
				default:
					break;
			}
		}
	}
}
