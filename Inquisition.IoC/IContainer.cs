namespace Inquisition.IoC
{
	/// <summary>
	/// Inversion of Control container interface
	/// </summary>
	public interface IContainer
	{
		void Register<TKey>(object value);
		TKey Resolve<TKey>();
	}
}