using System;

namespace Inquisition.Logging
{
	internal class ConsoleLog : LogBase
	{
		public override void Log<T>(params T[] value)
		{
			string data = "";
			foreach (T t in value)
				data += t.ToString() + "	";

			Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} {data}");
		}
	}
}
