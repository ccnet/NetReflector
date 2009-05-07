using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public class XmlTypeSerialiser : IXmlTypeSerialiser
	{
		private Type type;
		private ReflectorTypeAttribute attribute;
		private IEnumerable serialisers;
		private IInstantiator instantiator;

		public XmlTypeSerialiser(Type type, ReflectorTypeAttribute attribute) : this(type, attribute, new DefaultInstantiator())
		{}

		public XmlTypeSerialiser(Type type, ReflectorTypeAttribute attribute, IInstantiator instantiator)
		{
			this.type = type;
			this.attribute = attribute;
			this.instantiator = instantiator;
		}

		public ReflectorTypeAttribute Attribute
		{
			get { return attribute; }
		}

		public Type Type
		{
			get { return type; }
		}

		public IInstantiator Instantiator
		{
			get { return instantiator; }
		}

		public IEnumerable MemberSerialisers
		{
			get
			{
				if (serialisers == null)
				{
					serialisers = InitialiseMemberSerialisers();
				}
				return serialisers;
			}
		}

		private IEnumerable InitialiseMemberSerialisers()
		{
			SortedList serialisers = new SortedList();
			foreach (MemberInfo member in type.GetMembers())
			{
				ReflectorPropertyAttribute attribute = ReflectorPropertyAttribute.GetAttribute(member);
				if (attribute != null)
				{
					serialisers.Add(member.Name, attribute.CreateSerialiser(ReflectorMember.Create(member)));
				}
			}
			return serialisers.Values;
		}

		public void Write(XmlWriter writer, object target)
		{
			writer.WriteStartElement(attribute.Name);
			WriteMembers(writer, target);
			writer.WriteEndElement();
		}

		public void WriteMembers(XmlWriter writer, object target)
		{
			foreach (IXmlSerialiser serialiser in MemberSerialisers)
			{
				serialiser.Write(writer, target);
			}
		}

		public object Read(XmlNode node, NetReflectorTypeTable table)
		{
			object instance = instantiator.Instantiate(type);
			ReadMembers(node, instance, table);
			return instance;
		}

		public void ReadMembers(XmlNode node, object instance, NetReflectorTypeTable table)
		{
			IList childNodes = new ArrayList();
			AddChildNodes(node.Attributes, childNodes, table);
			AddChildNodes(node.ChildNodes, childNodes, table);

			foreach (IXmlMemberSerialiser serialiser in MemberSerialisers)
			{
				XmlNode childNode = GetNodeByName(childNodes, serialiser.Attribute.Name);
                try
                {
                    object value = serialiser.Read(childNode, table);
                    if (value != null)
                    {
                        serialiser.SetValue(instance, value);
                    }
                    childNodes.Remove(childNode);
                }
                catch (NetReflectorItemRequiredException error)
                {
                    throw new NetReflectorException(
                        string.Format("{0}" + Environment.NewLine +
                                        "Xml: {1}",
                            error.Message,
                            node.OuterXml), error);
                }
			}

			foreach (XmlNode orphan in childNodes)
			{
                HandleUnusedNode(table, orphan);
			}
		}

        private void HandleUnusedNode(NetReflectorTypeTable table, XmlNode orphan)
        {
            // Ignore any XML schema instance nodes
            if (!string.Equals(orphan.NamespaceURI, "http://www.w3.org/2001/XMLSchema-instance"))
            {
                table.OnInvalidNode(new InvalidNodeEventArgs(orphan, "Unused node detected: " + orphan.OuterXml));
            }
        }

		private void AddChildNodes(IEnumerable nodes, IList childNodes, NetReflectorTypeTable table)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.NodeType == XmlNodeType.Comment) continue;

				if (GetNodeByName(childNodes, node.Name) != null)
				{
                    HandleUnusedNode(table, node);
				}
				else
				{
					childNodes.Add(node);					
				}
			}
		}

		private XmlNode GetNodeByName(IList nodes, string name)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.Name == name) return node;
			}
			return null;
		}
	}
}