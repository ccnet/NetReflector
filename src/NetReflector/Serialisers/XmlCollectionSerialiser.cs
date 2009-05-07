using System;
using System.Collections;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public class XmlCollectionSerialiser : XmlMemberSerialiser
	{
		private static readonly string elementName = "string";

		public XmlCollectionSerialiser(ReflectorMember member, ReflectorPropertyAttribute attribute) : base(member, attribute)
		{}

		protected override void WriteValue(XmlWriter writer, object value)
		{
			foreach (object element in ((IEnumerable) value))
			{
				if (element == null) continue;

				ReflectorTypeAttribute attribute = ReflectorTypeAttribute.GetAttribute(element);
				if (attribute == null)
				{
					writer.WriteElementString(elementName, element.ToString());
				}
				else
				{
					// make more concise?
					IXmlSerialiser serialiser = attribute.CreateSerialiser(element.GetType());
					serialiser.Write(writer, element);
				}
			}
		}

		/// Todo: convert to element type
		protected override object Read(XmlNode node, Type instanceType, NetReflectorTypeTable table)
		{
			IList list = Instantiator.Instantiate(instanceType) as IList;
			// null check
			foreach (XmlNode child in XmlElementList.Create(node.ChildNodes))
			{
                object value = base.ReadValue(child, table);
                try
                {
                    list.Add(value);
                }
                catch (InvalidCastException error)
                {
                    throw new NetReflectorConverterException(
                        string.Format("Unable to convert element '{0}' to its required type{2}XML: {1}",
                        child.Name,
                        node.OuterXml,
                        Environment.NewLine), error);
                }
			}
			return list;
		}
	}
}