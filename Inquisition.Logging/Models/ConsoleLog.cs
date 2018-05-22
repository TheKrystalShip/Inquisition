using System;

namespace Inquisition.Logging
{
	internal class ConsoleLog : LogBase
	{
		public override void Log(params object[] value)
		{
			string data = "";
			foreach (object t in value)
				data += t.ToString() + "	";

			Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} {data}");
		}
	}
}
