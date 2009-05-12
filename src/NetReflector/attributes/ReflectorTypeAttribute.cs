using System;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class ReflectorTypeAttribute : Attribute, IReflectorAttribute
	{
		private string name;
		private string description;

		public ReflectorTypeAttribute(string name)
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

        public Type Extends { get; set; }

        public bool HasCustomFactory
        {
            get { return false; }
        }

		public IXmlSerialiser CreateSerialiser(Type type)
		{
			return new XmlTypeSerialiser(type, this);
		}

		public IXmlSerialiser CreateSerialiser(Type type, IInstantiator instantiator)
		{
			return new XmlTypeSerialiser(type, this, instantiator);
		}

		public void Write(XmlWriter writer, object target)
		{
			CreateSerialiser(target.GetType()).Write(writer, target);
		}

		public static ReflectorTypeAttribute GetAttribute(object target)
		{
			return GetAttribute(target.GetType());
		}

		public static ReflectorTypeAttribute GetAttribute(Type type)
		{
			object[] attributes = type.GetCustomAttributes(typeof(ReflectorTypeAttribute), false);
			return (attributes.Length == 0) ? null : (ReflectorTypeAttribute)attributes[0];
		}
	}
}
