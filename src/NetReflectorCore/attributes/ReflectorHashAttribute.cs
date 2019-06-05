using System;
using NetReflectorCore.Util;

namespace NetReflectorCore
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class ReflectorHashAttribute : ReflectorPropertyAttribute
	{
		private string key;

		public ReflectorHashAttribute(string name) : base(name)
		{}

		public ReflectorHashAttribute(string name, string key) : this(name)
		{
			this.key = key;
		}

		public string Key
		{
			get { return key; }
			set { key = value; }
		}

		public override IXmlSerialiser CreateSerialiser(ReflectorMember member)
		{
			return new XmlDictionarySerialiser(member, this);
		}
	}
}