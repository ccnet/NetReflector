using NetReflectorCore.Util;

namespace NetReflectorCore
{
	public interface IXmlMemberSerialiser : IXmlSerialiser
	{
		ReflectorPropertyAttribute Attribute { get; }
		ReflectorMember ReflectorMember { get; }
		void SetValue(object instance, object value);
	}
}