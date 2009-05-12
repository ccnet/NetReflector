using System;
using System.IO;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public class NetReflectorWriter
	{
		private readonly XmlWriter writer;

		public NetReflectorWriter(TextWriter writer)
		{
			NetReflector.CheckNull(writer, "writer", typeof (TextWriter));
			this.writer = new XmlTextWriter(writer);
		}

		public NetReflectorWriter(XmlWriter writer)
		{
			NetReflector.CheckNull(writer, "writer", typeof (XmlWriter));
			this.writer = writer;
		}

		public void Write(object target)
		{
			NetReflector.CheckNull(target, "target", typeof (object));

			ReflectorTypeAttribute attribute = ReflectorTypeAttribute.GetAttribute(target);
			if (attribute == null)
			{
				throw new NetReflectorException(string.Format("Cannot serialise the object.  Target object type ({0}) is not marked with a ReflectorType attribute.", target.GetType()));
			}
			attribute.Write(writer, target);
		}
	}

	public class NetReflectorReader
	{
		private readonly NetReflectorTypeTable table;

		public NetReflectorReader() : this(NetReflectorTypeTable.CreateDefault())
		{}

		public NetReflectorReader(NetReflectorTypeTable table)
		{
			this.table = table;
		}

		public object Read(string xml)
		{
			return Read(new StringReader(xml));
		}

		public object Read(TextReader reader)
		{
			NetReflector.CheckNull(reader, "reader", typeof (TextReader));
			return Read(new XmlTextReader(reader));
		}

		public object Read(XmlReader reader)
		{
			NetReflector.CheckNull(reader, "reader", typeof (XmlReader));
			return Read(XmlUtil.ReadNode(reader));
		}

		public object Read(XmlNode node)
		{
			NetReflector.CheckNull(node, "node", typeof (XmlNode));
			IXmlSerialiser serialiser = table[node.Name];
			if (serialiser == null)
			{
				throw new NetReflectorException(string.Format("No loaded type is marked up with a ReflectorType attribute that matches the Xml node ({0}).  Xml Source: {1}", node.Name, node.OuterXml));
			}
			return serialiser.Read(node, table);
		}

		public void Read(string xml, object instance)
		{
			NetReflector.CheckNull(xml, "xml", typeof (string));
			Read(new StringReader(xml), instance);
		}

		public void Read(TextReader reader, object instance)
		{
			NetReflector.CheckNull(reader, "reader", typeof (TextReader));
			Read(new XmlTextReader(reader), instance);
		}

		public void Read(XmlReader reader, object instance)
		{
			NetReflector.CheckNull(reader, "reader", typeof (XmlReader));
			Read(XmlUtil.ReadNode(reader), instance);
		}

		public void Read(XmlNode node, object instance)
		{
			NetReflector.CheckNull(node, "node", typeof (XmlNode));
			NetReflector.CheckNull(instance, "instance", typeof (object));
			new XmlTypeSerialiser(instance.GetType(), new ReflectorTypeAttribute(instance.GetType().Name)).ReadMembers(node, instance, table);
		}
	}

//	[Obsolete("Use the instance methods instead")]
	public class NetReflector
	{
		public static string Write(object target)
		{
			StringWriter buffer = new StringWriter();
			new NetReflectorWriter(buffer).Write(target);
			return buffer.ToString();
		}

		public static void Write(TextWriter writer, object target)
		{
			new NetReflectorWriter(writer).Write(target);
		}

		public static void Write(XmlWriter writer, object target)
		{
			new NetReflectorWriter(writer).Write(target);
		}

		public static object Read(string xml)
		{
			return new NetReflectorReader().Read(xml);
		}

		public static object Read(string xml, NetReflectorTypeTable table)
		{
			return new NetReflectorReader(table).Read(xml);
		}

		public static object Read(TextReader reader)
		{
			return new NetReflectorReader().Read(reader);
		}

		public static object Read(TextReader reader, NetReflectorTypeTable table)
		{
			return new NetReflectorReader(table).Read(reader);
		}

		public static object Read(XmlReader reader)
		{
			return new NetReflectorReader().Read(reader);
		}

		public static object Read(XmlReader reader, NetReflectorTypeTable table)
		{
			return new NetReflectorReader(table).Read(reader);
		}

		public static object Read(XmlNode node)
		{
			return new NetReflectorReader().Read(node);
		}

		public static object Read(XmlNode node, NetReflectorTypeTable table)
		{
			return new NetReflectorReader(table).Read(node);
		}

		public static void Read(string xml, object instance)
		{
			new NetReflectorReader().Read(xml, instance);
		}

		public static void Read(string xml, object instance, NetReflectorTypeTable table)
		{
			new NetReflectorReader(table).Read(xml, instance);
		}

		public static void Read(XmlReader reader, object instance)
		{
			new NetReflectorReader().Read(reader, instance);
		}

		public static void Read(XmlReader reader, object instance, NetReflectorTypeTable table)
		{
			new NetReflectorReader(table).Read(reader, instance);
		}

		public static void Read(XmlNode node, object instance)
		{
			new NetReflectorReader().Read(node, instance);
		}

		public static void Read(XmlNode node, object instance, NetReflectorTypeTable table)
		{
			new NetReflectorReader(table).Read(node, instance);
		}

		protected internal static void CheckNull(object obj, string param, Type expected)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(param, String.Format
					("Supplied argument {0} is null", expected.Name));
			}
		}
	}
}