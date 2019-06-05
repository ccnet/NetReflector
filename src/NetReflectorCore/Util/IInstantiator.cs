using System;

namespace NetReflectorCore.Util
{
	public interface IInstantiator
	{
		object Instantiate(Type type);
	}
}
