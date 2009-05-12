using NUnit.Framework;

namespace Exortech.NetReflector.Test
{
	[ReflectorType("inner")]
	internal class TestInnerClass : ITestClass
	{
		[ReflectorProperty("name")]
		public string InnerName;

		[ReflectorProperty("missing", Required=false)]
		public string Missing;

		[ReflectorProperty("present", Required=false)]
		public string Present = "wuz here";

		public static TestInnerClass Create()
		{
			TestInnerClass target = CreateMinimal();
			target.Missing = "missing";
			target.Present = "present";
			return target;
		}

		public static TestInnerClass CreateMinimal()
		{
			TestInnerClass target = new TestInnerClass();
			target.InnerName = "innerName";
			return target;
		}

		public static string GetOuterXml()
		{
			return string.Format("<inner>{0}</inner>", GetInnerXml());
		}

		public static string GetInnerXml()
		{
			return "<name>innerName</name><missing>missing</missing><present>present</present>";
		}

		public static string GetOuterXmlWithEmbeddedComment()
		{
			return string.Format("<inner><!-- foo -->{0}</inner>", GetInnerXml());
		}

		public static string GetOuterXmlMissingName()
		{
			return GetOuterXml().Replace("<name>innerName</name>", string.Empty);
		}

		public static string GetOuterXmlIncludingOnlyRequired()
		{
			return "<inner><name>innerName</name></inner>";
		}

		public static void AssertEquals(TestInnerClass expected, TestInnerClass actual)
		{
			Assert.AreEqual(expected.InnerName, actual.InnerName);
			Assert.AreEqual(expected.Missing, actual.Missing);
			Assert.AreEqual(expected.Present, actual.Present);
		}
	}
}