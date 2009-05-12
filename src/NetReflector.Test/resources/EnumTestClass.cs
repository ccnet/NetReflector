using Exortech.NetReflector.Test.Util;
using NUnit.Framework;

namespace Exortech.NetReflector.Test
{
	[ReflectorType("enum-test")]
	internal class EnumTestClass
	{
		[ReflectorProperty("enum", Required=false)]
		public ReflectorMemberTest.TestEnum TestEnum = ReflectorMemberTest.TestEnum.A;

		public static string GetXml()
		{
			return @"<enum-test enum=""B"" />";
		}

		public static void AssertEquals(EnumTestClass actual)
		{
			Assert.AreEqual(ReflectorMemberTest.TestEnum.B, actual.TestEnum);
		}
	}
}
