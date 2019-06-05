using Exortech.NetReflector.Test.Resources;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Serialisers
{
	[TestFixture]
	public class XmlDictionarySerialiserTest
	{
		private NetReflectorTypeTable table;

		[SetUp]
		public void Setup()
		{
			table = NetReflectorTypeTable.CreateDefault();
		}

		[Test]
		public void WriteTestHashClassContainingElementTestClasses()
		{
			string xml = NetReflector.Write(HashTestClass.CreateHashtableWithElements());
			Assert.AreEqual(HashTestClass.GetXmlForHashtableWithElements(), xml);
		}

		[Test]
		public void WriteTestHashClassContainingStrings()
		{
			string xml = NetReflector.Write(HashTestClass.CreateHashtableWithStrings());
			Assert.AreEqual(HashTestClass.GetXmlForHashtableWithStrings(), xml);
		}

		[Test]
		public void ReadTestHashClassContainingStrings()
		{
			HashTestClass actual = (HashTestClass)NetReflector.Read(HashTestClass.GetXmlForHashtableWithStrings(), table);
			HashTestClass.AssertEquals(HashTestClass.CreateHashtableWithStrings(), actual);
		}
	
		[Test]
		public void ReadTestHashClassContainingElements()
		{
			HashTestClass actual = (HashTestClass)NetReflector.Read(HashTestClass.GetXmlForHashtableWithElements(), table);
			HashTestClass.AssertEquals(HashTestClass.CreateHashtableWithElements(), actual);
		}

		[Test]
		public void ShouldUseInstantiatorThatHasBeenSet()
		{
			TestInstantiator instantiator = new TestInstantiator();
			table = NetReflectorTypeTable.CreateDefault(instantiator);
			ReadTestHashClassContainingElements();
			Assert.AreEqual(4, instantiator.instantiateCallCount);
		}
	}
}
