using System.Xml;

namespace Exortech.NetReflector
{
	public interface IXmlSerialiser
	{
		void Write(XmlWriter writer, object target);
		object Read(XmlNode node, NetReflectorTypeTable table);
	}
}