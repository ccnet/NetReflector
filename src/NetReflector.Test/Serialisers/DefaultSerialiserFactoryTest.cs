using System.Reflection;
using Exortech.NetReflector.Util;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Serialisers
{
	[TestFixture]
	public class DefaultSerialiserFactoryTest
	{
		private ReflectorPropertyAttribute attribute;
		private DefaultSerialiserFactory factory;

		[SetUp]
		protected void SetUp()
		{
			attribute = new ReflectorPropertyAttribute("foo");
			factory = new DefaultSerialiserFactory();
		}

		[Test]
		public void ShouldCreateArraySerialiserWhenArrayPropertyIsPassed()
		{
			IXmlSerialiser serialiser = factory.Create(ReflectorMember.Create(typeof (ArrayTestClass).GetProperty("Elements")), attribute);
			Assert.AreEqual(typeof(XmlArraySerialiser), serialiser.GetType());
		}

		[Test]
		public void ShouldCreateCollectionSerialiserWhenCollectionPropertyIsPassed()
		{
			IXmlSerialiser serialiser = factory.Create(ReflectorMember.Create(typeof (CollectionTestClass).GetProperty("List")), attribute);
			Assert.AreEqual(typeof(XmlCollectionSerialiser), serialiser.GetType());
		}

		[Test]
		public void ShouldCreateCollectionSerialiserWhenInstanceTypeIsCollection()
		{
			FieldInfo field = typeof (CollectionTestClass).GetField("Stuff");
			attribute = (ReflectorPropertyAttribute) field.GetCustomAttributes(false)[0];
			IXmlSerialiser serialiser = factory.Create(ReflectorMember.Create(field), attribute);
			Assert.AreEqual(typeof(XmlCollectionSerialiser), serialiser.GetType());
		}
	}
}