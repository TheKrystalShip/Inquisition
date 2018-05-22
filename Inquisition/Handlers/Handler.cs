using System;

namespace Inquisition.Handlers
{
	public abstract class Handler : IDisposable
	{
		public abstract void Dispose();
	}
}
