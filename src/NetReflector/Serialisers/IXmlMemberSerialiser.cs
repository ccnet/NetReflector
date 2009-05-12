using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public interface IXmlMemberSerialiser : IXmlSerialiser
	{
		ReflectorPropertyAttribute Attribute { get; }
		ReflectorMember ReflectorMember { get; }
		void SetValue(object instance, object value);
	}
}