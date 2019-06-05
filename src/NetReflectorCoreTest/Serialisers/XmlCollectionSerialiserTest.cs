using System;
using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest.Serialisers
{
	[TestFixture]
	public class XmlCollectionSerialiserTest
	{
		private NetReflectorTypeTable table;

		[SetUp]
		public void Setup()
		{
			table = NetReflectorTypeTable.CreateDefault();
		}

		[Test]
		public void WriteCollectionTestClass()
		{
			string xml = NetReflector.Write(CollectionTestClass.Create());
			Assert.AreEqual(CollectionTestClass.GetXml(), xml);
		}

		[Test]
		public void WriteCollectionTestClassContainingNulls()
		{
			string xml = NetReflector.Write(CollectionTestClass.CreateCollectionContainingNulls());
			Assert.AreEqual(CollectionTestClass.GetXmlWithMissingNullElements(), xml);
		}

		[Test]
		public void ReadCollectionTestClass()
		{
			CollectionTestClass actual = (CollectionTestClass) NetReflector.Read(CollectionTestClass.GetXml(), table);
			CollectionTestClass.AssertEquals(CollectionTestClass.Create(), actual);
		}

		[Test]
		public void ShouldUseInstantiatorThatHasBeenSet()
		{
			Resources.TestInstantiator instantiator = new Resources.TestInstantiator();
			table = NetReflectorTypeTable.CreateDefault(instantiator);
			ReadCollectionTestClass();
			Assert.AreEqual(4, instantiator.instantiateCallCount);
		}
	}
}