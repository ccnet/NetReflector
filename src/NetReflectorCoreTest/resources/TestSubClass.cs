using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[ReflectorType("sub")]
	internal class TestSubClass : TestInnerClass
	{
		private int subzero;

		[ReflectorProperty("subzero")]
		public int SubZero
		{
			get { return subzero; }
			set { subzero = value; }
		}

		public new static TestSubClass Create()
		{
			TestSubClass sub = new TestSubClass();
			sub.SubZero = -40;
			sub.InnerName = "sub";
			sub.Present = "here";
			return sub;
		}

		public static void AssertEquals(TestSubClass expected, TestSubClass actual)
		{
			Assert.AreEqual(expected.InnerName, actual.InnerName);
			Assert.AreEqual(expected.Missing, actual.Missing);
			Assert.AreEqual(expected.Present, actual.Present);
			Assert.AreEqual(expected.SubZero, actual.SubZero);
		}
	}
}
