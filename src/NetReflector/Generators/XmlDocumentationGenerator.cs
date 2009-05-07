using System;
using System.IO;
using System.Xml;

namespace Exortech.NetReflector.Generators
{
	public class XmlDocumentationGenerator
	{
		private NetReflectorTypeTable table;
		private XmlMemberDocumentationGenerator memberGenerator;

		public XmlDocumentationGenerator(NetReflectorTypeTable table) : this(table, new XmlMemberDocumentationGenerator()) { }

		public XmlDocumentationGenerator(NetReflectorTypeTable table, XmlMemberDocumentationGenerator memberGenerator)
		{
			this.table = table;
			this.memberGenerator = memberGenerator;
		}

		public void Write(TextWriter writer)
		{
			Write(new XmlTextWriter(writer));
		}

		public void WriteIndented(TextWriter writer)
		{
			XmlTextWriter xmlWriter = new XmlTextWriter(writer);
			xmlWriter.Formatting = Formatting.Indented;
			Write(xmlWriter);
		}

		public void Write(XmlWriter writer)
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("netreflector");
			try
			{
				WriteTypes(writer);
			}
			finally
			{
				writer.WriteEndDocument();
			}
		}

		private void WriteTypes(XmlWriter writer)
		{
			foreach (IXmlTypeSerialiser typeSerialiser in table)
			{
				writer.WriteStartElement("reflectortype");
				writer.WriteElementString("name", typeSerialiser.Type.Name);
				writer.WriteElementString("namespace", typeSerialiser.Type.Namespace);
				writer.WriteElementString("reflectorName", typeSerialiser.Attribute.Name);
				WriteIfNotNull(writer, "description", typeSerialiser.Attribute.Description);
				memberGenerator.Write(writer, typeSerialiser);
				writer.WriteEndElement();
			}
		}

		public static void WriteIfNotNull(XmlWriter writer, string elementName, string output)
		{
			if (output != null)	writer.WriteElementString(elementName, output);
		}

		public static void WriteIfNotNull(XmlWriter writer, string elementName, Type type)
		{
			if (type != null)	writer.WriteElementString(elementName, type.Name);
		}
	}

	public class XmlMemberDocumentationGenerator
	{
		public virtual void Write(XmlWriter writer, IXmlTypeSerialiser typeSerialiser)
		{
			writer.WriteStartElement("members");
			foreach (IXmlMemberSerialiser memberSerialiser in typeSerialiser.MemberSerialisers)
			{
				writer.WriteStartElement("member");
				writer.WriteElementString("name", memberSerialiser.ReflectorMember.Name);
				writer.WriteElementString("reflectorName", memberSerialiser.Attribute.Name);
				XmlDocumentationGenerator.WriteIfNotNull(writer, "description", memberSerialiser.Attribute.Description);
				writer.WriteElementString("required", memberSerialiser.Attribute.Required.ToString());
				XmlDocumentationGenerator.WriteIfNotNull(writer, "instanceType", memberSerialiser.Attribute.InstanceType);
				XmlDocumentationGenerator.WriteIfNotNull(writer, "instanceTypeKey", memberSerialiser.Attribute.InstanceTypeKey);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}
}
