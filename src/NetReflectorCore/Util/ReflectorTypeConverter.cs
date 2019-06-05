using System;
using System.ComponentModel;

namespace NetReflectorCore.Util
{
	public class ReflectorTypeConverter
	{
		private bool IsCompatibleType(Type to, object from)
		{
			return (from == null || to.IsInstanceOfType(from));
		}

		public object Convert(Type to, object from)
		{
			if (IsCompatibleType(to, from)) return from;
			try
			{
				return TypeDescriptor.GetConverter(to).ConvertFrom(from);
			}
			catch (Exception ex)
			{
                throw new NetReflectorConverterException(string.Format
					("Cannot convert from type {0} to {1} for object with value: \"{2}\"", from.GetType(), to, from), ex);
			}
		}
	}
}