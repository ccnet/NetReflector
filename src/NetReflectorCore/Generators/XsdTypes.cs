using System;
using System.Collections;
using System.Xml;

namespace NetReflectorCore.Generators
{
	public class XsdTypes
	{
		private Hashtable map = new Hashtable();

		public XsdTypes()
		{
			map[typeof (string)] = "string";
			map[typeof (int)] = "integer";
			map[typeof (byte)] = "byte";
			map[typeof (decimal)] = "decimal";
			map[typeof (double)] = "double";
			map[typeof (bool)] = "boolean";
			map[typeof (float)] = "float";
			map[typeof (long)] = "long";
			map[typeof (short)] = "short";
			map[typeof (DateTime)] = "dateTime";
		}

		private string this[Type type]
		{
			get
			{
				object value = map[type];
				if (value == null) throw new NetReflectorException(string.Format("Unable to find Xsd type for {0}", type));
				return value.ToString();
			}
		}

		public XmlQualifiedName ConvertToSchemaTypeName(Type type)
		{
			return new XmlQualifiedName(this[type].ToString(), "http://www.w3.org/2001/XMLSchema");
		}
	}
}