using System;
using System.Collections.Concurrent;

namespace Inquisition.IoC
{
	/// <summary>
	/// Inversion of Control container
	/// </summary>
	public class Container : IContainer
	{
		/// <summary>
		/// Thread safe dictionary to store registered values
		/// </summary>
		private ConcurrentDictionary<Type, object> Source;

		public Container()
		{
			Source = new ConcurrentDictionary<Type, object>();
		}

		/// <summary>
		/// Register a value based on a type
		/// </summary>
		/// <typeparam name="TKey">Type to register</typeparam>
		/// <param name="value">Value to register</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="OverflowException"></exception>
		public void Register<TKey>(object value)
		{
			Source.TryAdd(typeof(TKey), value);
		}

		/// <summary>
		/// Retrieve a value of a given type
		/// </summary>
		/// <typeparam name="TKey">Type of value to retrieve</typeparam>
		/// <returns>An instance of the given type</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public TKey Resolve<TKey>()
		{
			if (!Source.ContainsKey(typeof(TKey)))
			{
				throw new InvalidOperationException(String.Format("Requested type {0} has not been registered", typeof(TKey).ToString()));
			}

			if (!Source.TryGetValue(typeof(TKey), out object value))
			{
				throw new InvalidOperationException(String.Format("Failed to retrieve type {0}", typeof(TKey).ToString()));
			}
			else
			{
				return (TKey) value;
			}
		}
    }
}
