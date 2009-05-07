using System;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector.Test.Resources
{
	public class TestInstantiator : IInstantiator
	{
		public int instantiateCallCount = 0;
		private readonly DefaultInstantiator realInstantiator;

		public TestInstantiator()
		{
			realInstantiator = new DefaultInstantiator();
		}

		public object Instantiate(Type type)
		{
			instantiateCallCount++;
			return realInstantiator.Instantiate(type);
		}
	}
}