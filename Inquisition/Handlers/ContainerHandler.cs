using Inquisition.IoC;

namespace Inquisition.Handlers
{
	public static class ContainerHandler
    {
		private static Container Container = new Container();

		public static void Register<TKey>(object value)
		{
			Container.Register<TKey>(value);
		}

		public static TKey Resolve<TKey>()
		{
			return Container.Resolve<TKey>();
		}
    }
}
