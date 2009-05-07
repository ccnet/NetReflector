using System;
using System.Collections;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public class XmlDictionarySerialiser : XmlMemberSerialiser
	{
		private ReflectorHashAttribute attribute;
		private string elementName = "string";

		public XmlDictionarySerialiser(ReflectorMember member, ReflectorHashAttribute attribute) : base(member, attribute)
		{
			this.attribute = attribute;
		}

		protected override void WriteValue(XmlWriter writer, object value)
		{
			IDictionary dictionary = (IDictionary) value;
			foreach (object key in dictionary.Keys)
			{
				object target = dictionary[key];

				// any way to refactor code block?
				ReflectorTypeAttribute typeAttribute = ReflectorTypeAttribute.GetAttribute(target);
				if (typeAttribute == null)
				{
					writer.WriteStartElement(elementName);
					writer.WriteAttributeString(attribute.Key, key.ToString());
					writer.WriteString(target.ToString());
					writer.WriteEndElement();
				}
				else
				{
					writer.WriteStartElement(typeAttribute.Name);
					writer.WriteAttributeString(attribute.Key, key.ToString());

					XmlTypeSerialiser serialiser = (XmlTypeSerialiser) typeAttribute.CreateSerialiser(target.GetType());
					serialiser.WriteMembers(writer, target);

					writer.WriteEndElement();
				}
			}
		}

		protected override object Read(XmlNode node, Type instanceType, NetReflectorTypeTable table)
		{
			IDictionary dictionary = Instantiator.Instantiate(instanceType) as IDictionary;
			// null check
			foreach (XmlNode child in XmlElementList.Create(node.ChildNodes))
			{
				object key = GetHashkey(child);
				object value = base.ReadValue(child, table);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private string GetHashkey(XmlNode node)
		{
			return XmlUtil.GetChildNode(node, attribute.Key).InnerText;
		}
	}
}