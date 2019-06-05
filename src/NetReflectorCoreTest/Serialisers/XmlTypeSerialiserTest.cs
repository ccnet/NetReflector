using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest.Serialisers
{
	[TestFixture]
	public class XmlTypeSerialiserTest		// Rename to ReflectorTypeAttributeTest
	{
		[Test]
		public void GetReflectorTypeAttribute()
		{
			ReflectorTypeAttribute attribute = ReflectorTypeAttribute.GetAttribute(typeof (TestClass));
			Assert.IsNotNull(attribute);
			Assert.AreEqual("reflectTest", attribute.Name);
		}

		[Test]
		public void ShouldReturnNullForTypeWithoutNetReflectorAttribute()
		{
			Assert.IsNull(ReflectorTypeAttribute.GetAttribute(typeof (string)));
		}
	}
}