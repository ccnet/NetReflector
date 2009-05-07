using System;
using System.Collections;

namespace Exortech.NetReflector.Test
{
	[ReflectorType("element")]
	internal class ElementTestClass
	{
		private string id;

		[ReflectorProperty("id")]
		public string ID
		{
			get { return id; }
			set { id = value; }
		}

		public static ElementTestClass Create(string id)
		{
			ElementTestClass element = new ElementTestClass();
			element.ID = id;
			return element;
		}

		public override bool Equals(object obj)
		{
			return (obj is ElementTestClass) && ((ElementTestClass)obj).ID == ID;
		}

		public override string ToString()
		{
			return string.Format("ElementTestClass: {0}", ID);
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
