using System;

namespace NetReflectorCore
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class ReflectorCollectionAttribute : ReflectorPropertyAttribute
	{
		public ReflectorCollectionAttribute(string name) : base(name)
		{}
	}
}