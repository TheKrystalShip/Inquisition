namespace Inquisition.Logging
{
	internal abstract class LogBase
    {
		public abstract void Log<T>(params T[] value);
    }
}
