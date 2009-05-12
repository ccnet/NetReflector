using System;
using System.Collections;
using System.Xml;

namespace Exortech.NetReflector.Util
{
	public class XmlElementList : XmlNodeList
	{
		private ArrayList list = new ArrayList();

		public XmlElementList(XmlNodeList nodes)
		{
			FillList(nodes);
		}

		private void FillList(XmlNodeList nodes)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					list.Add(node);
				}
			}
		}

		public override int Count
		{
			get { return list.Count; }
		}

		public override IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public override XmlNode Item(int index)
		{
			return (XmlNode)list[index];
		}

		public static XmlElementList Create(XmlNodeList nodes)
		{
			return new XmlElementList(nodes);
		}
	}
}
