using System;
using System.Collections;

namespace NetReflectorCore
{
	public interface IXmlTypeSerialiser : IXmlSerialiser
	{
		Type Type { get; }
		ReflectorTypeAttribute Attribute { get; }
		IEnumerable MemberSerialisers { get; }
	}
}
