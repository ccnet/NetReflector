using System;
using Exortech.NetReflector.Test.Resources;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Serialisers
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

		[Test, Ignore("needed?")]
		public void WriteClassThatIsNotSerializable()
		{
			DateTime now = DateTime.Now;
			string xml = NetReflector.Write(TestClass.Create(now));
			Assert.AreEqual(TestClass.GetXml(now), xml);
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
			TestInstantiator instantiator = new TestInstantiator();
			table = NetReflectorTypeTable.CreateDefault(instantiator);
			ReadCollectionTestClass();
			Assert.AreEqual(4, instantiator.instantiateCallCount);
		}
	}
}