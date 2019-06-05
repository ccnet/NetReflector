using System.Xml;

namespace NetReflectorCore.Util
{
	public class XmlUtil
	{
		public static XmlNode GetChildNode(XmlNode node, string name)
		{
			XmlNode attribute = node.Attributes[name];
			return (attribute == null) ? node[name] : attribute;
		}

		public static XmlNode ReadNode(string xml)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);
			return document.DocumentElement;
		}

		public static XmlNode ReadNode(XmlReader reader)
		{
			XmlDocument document = new XmlDocument();
			document.Load(reader);
			return document.DocumentElement;
		}	
	}
}
