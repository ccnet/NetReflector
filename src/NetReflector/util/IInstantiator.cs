using System;

namespace Exortech.NetReflector.Util
{
	public interface IInstantiator
	{
		object Instantiate(Type type);
	}
}
