using System.Xml;

namespace NetReflectorCore
{
	public interface IXmlSerialiser
	{
		void Write(XmlWriter writer, object target);
		object Read(XmlNode node, NetReflectorTypeTable table);
	}
}