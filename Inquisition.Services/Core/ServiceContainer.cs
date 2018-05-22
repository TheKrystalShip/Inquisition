using Inquisition.IoC;

namespace Inquisition.Services.Core
{
	/// <summary>
	/// Container storing running services
	/// </summary>
	public static class ServiceContainer
    {
		private static Container Container = new Container();

		/// <summary>
		/// Add a service to the container
		/// </summary>
		/// <typeparam name="TKey">Type of service</typeparam>
		/// <param name="value">Instance of service</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static void Register<TKey>(object value)
		{
			Container.Register<TKey>(value);
		}

		/// <summary>
		/// Retrieve a service from the container
		/// </summary>
		/// <typeparam name="TKey">Type of service</typeparam>
		/// <returns>Instance of service</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static TKey Resolve<TKey>()
		{
			return Container.Resolve<TKey>();
		}
    }
}
