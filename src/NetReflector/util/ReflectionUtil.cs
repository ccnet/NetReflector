using System;

namespace Exortech.NetReflector.Util
{
	public sealed class ReflectionUtil
	{
		// Static utility class
		private ReflectionUtil() { }

		public static bool IsCommonType(Type t)
		{
			return (t.IsPrimitive || t == typeof(string) || t == typeof(DateTime) || t.IsSubclassOf(typeof(Enum)));
		}
	}
}
