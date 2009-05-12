using System;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public class XmlArraySerialiser : XmlCollectionSerialiser
	{
		protected readonly ReflectorTypeConverter converter;

		public XmlArraySerialiser(ReflectorMember member, ReflectorPropertyAttribute attribute) : base(member, attribute)
		{
			converter = new ReflectorTypeConverter();
		}

		protected override object Read(XmlNode node, Type instanceType, NetReflectorTypeTable table)
		{
			Type elementType = instanceType.GetElementType();
			// null check
			XmlElementList nodes = new XmlElementList(node.ChildNodes);
			Array array = Array.CreateInstance(elementType, nodes.Count);
			for (int i = 0; i < array.Length; i++)
			{
                try
                {
                    object value = converter.Convert(elementType, ReadValue(nodes[i], table));
                    if (value == null)
                    {
                        throw new NetReflectorConverterException(
                            string.Format("Unable to convert element '{0}' to '{1}'",
                            nodes[i].Name,
                            elementType.FullName));
                    }
                    array.SetValue(value, i);
                }
                catch (NetReflectorConverterException error)
                {
                    throw new NetReflectorException(
                        string.Format("Unable to load array item '{0}' - {1}" + Environment.NewLine +
                                        "Xml: {2}",
                            nodes[i].Name,
                            error.Message,
                            nodes[i].OuterXml), error);
                }
			}
			return array;
		}
	}
}