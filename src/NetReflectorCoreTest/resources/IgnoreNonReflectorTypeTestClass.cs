using NetReflectorCore;
using System;

namespace NetReflectorCoreTest
{
	[ReflectorType("ignorenonreflectortype")]
	internal class IgnoreNonReflectorTypeTestClass
	{
		private IComplexMemberType myComplexMember;

		[ReflectorProperty("myComplexMember", InstanceTypeKey="type", Required=false)]
		public IComplexMemberType MyComplexMember
		{
			get { return myComplexMember; }
			set { myComplexMember = value;}
		}
	}

	internal interface IComplexMemberType
	{
		string Foo {get; }
	}

	[ReflectorType("netreflectorcomplexmembertype")]
	internal class NetReflectorComplexMemberType : IComplexMemberType
	{
		public string Foo
		{
			get { return "NetReflectorType"; }
		}
	}

	internal class NonNetReflectorComplexMemberType : IComplexMemberType
	{
		public string Foo
		{
			get { return "NonNetReflectorType"; }
		}
	}
}
