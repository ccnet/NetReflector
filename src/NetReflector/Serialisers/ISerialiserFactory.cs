using Exortech.NetReflector.Util;

namespace Exortech.NetReflector
{
	public interface ISerialiserFactory
	{
		IXmlMemberSerialiser Create(ReflectorMember memberInfo, ReflectorPropertyAttribute attribute);
	}
}