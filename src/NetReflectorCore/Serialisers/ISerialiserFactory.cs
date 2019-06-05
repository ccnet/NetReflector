using NetReflectorCore.Util;

namespace NetReflectorCore
{
	public interface ISerialiserFactory
	{
		IXmlMemberSerialiser Create(ReflectorMember memberInfo, ReflectorPropertyAttribute attribute);
	}
}