using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[ReflectorType("enum-test")]
	internal class EnumTestClass
	{
		[ReflectorProperty("enum", Required=false)]
		public Util.ReflectorMemberTest.TestEnum TestEnum = Util.ReflectorMemberTest.TestEnum.A;

		public static string GetXml()
		{
			return @"<enum-test enum=""B"" />";
		}

		public static void AssertEquals(EnumTestClass actual)
		{
			Assert.AreEqual(Util.ReflectorMemberTest.TestEnum.B, actual.TestEnum);
		}
	}
}
