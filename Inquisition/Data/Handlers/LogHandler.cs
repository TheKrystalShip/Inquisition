using System;

namespace Inquisition.Data.Handlers
{
	public class LogHandler
	{
		public static void WriteLine<T>(params T[] value) where T: class
		{
			string data = "";
			foreach (T t in value)
				data += t.ToString() + "	";

			Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} {data}");
		}
	}
}
