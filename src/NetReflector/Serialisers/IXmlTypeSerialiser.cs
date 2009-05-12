using System;
using System.Collections;

namespace Exortech.NetReflector
{
	public interface IXmlTypeSerialiser : IXmlSerialiser
	{
		Type Type { get; }
		ReflectorTypeAttribute Attribute { get; }
		IEnumerable MemberSerialisers { get; }
	}
}
