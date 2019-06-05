using NetReflectorCore.Util;
using System;

namespace NetReflectorCoreTest.Resources
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