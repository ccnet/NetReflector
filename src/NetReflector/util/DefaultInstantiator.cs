using System;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector.Util
{
	public class DefaultInstantiator : IInstantiator
	{
		public object Instantiate(Type type)
		{
			try
			{
				return type.Assembly.CreateInstance(type.FullName);
			}
			catch (Exception e)
			{
				string message = StringUtil.Format("Unable to create an instance of reflected type '{0}'.  Please verify that this object has a default constructor.", type);
				throw new NetReflectorException(message, e);
			}
		}
	}
}
