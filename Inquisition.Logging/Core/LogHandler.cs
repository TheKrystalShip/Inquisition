namespace Inquisition.Logging
{
	public static class LogHandler
	{
		public static void WriteLine(LogTarget target, params object[] value)
		{
			LogBase logBase;

			switch (target)
			{
				case LogTarget.File:
					logBase = new FileLog();
					logBase.Log(value);
					break;
				case LogTarget.Database:
					logBase = new DbLog();
					logBase.Log(value);
					break;
				case LogTarget.Event:
					logBase = new EventLog();
					logBase.Log(value);
					break;
				case LogTarget.Console:
					logBase = new ConsoleLog();
					logBase.Log(value);
					break;
				case LogTarget.All:
					logBase = new FileLog();
					logBase.Log(value);
					logBase = new DbLog();
					logBase.Log(value);
					logBase = new EventLog();
					logBase.Log(value);
					logBase = new ConsoleLog();
					logBase.Log(value);
					break;
				default:
					break;
			}
		}
	}
}
