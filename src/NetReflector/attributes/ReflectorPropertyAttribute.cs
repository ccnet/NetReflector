using System;
using System.Reflection;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class 
		ReflectorPropertyAttribute : Attribute, IReflectorAttribute
	{
		private string name;
		private string description;
		private bool required = true;
		private Type instanceType;
		private string instanceTypeKey;
		private ISerialiserFactory factory = new DefaultSerialiserFactory();

		public ReflectorPropertyAttribute(string name)
		{
			this.name = name;
		}

		public ReflectorPropertyAttribute(string name, Type factoryType) : this(name)
		{
			this.name = name;
            HasCustomFactory = true;
			this.factory = (ISerialiserFactory) Activator.CreateInstance(factoryType);
		}

        public bool HasCustomFactory { get; private set; }

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

		public bool Required
		{
			get { return required; }
			set { required = value; }
		}

		public Type InstanceType
		{
			get { return instanceType; }
			set { instanceType = value; }
		}

		public string InstanceTypeKey
		{
			get { return instanceTypeKey; }
			set { instanceTypeKey = value; }
		}

		public virtual IXmlSerialiser CreateSerialiser(ReflectorMember member)
		{
			return factory.Create(member, this); 
		}

		public static ReflectorPropertyAttribute GetAttribute(MemberInfo member)
		{
			object[] attributes = member.GetCustomAttributes(typeof(ReflectorPropertyAttribute), false);
			return (attributes.Length == 0) ? null : (ReflectorPropertyAttribute)attributes[0];
		}
	}
}