using System.Xml;
using Exortech.NetReflector.Util;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Attributes
{
	[TestFixture]
	public class ReflectorPropertyTest
	{
		private ReflectorMember member;

		[SetUp]
		protected void SetUp()
		{
			member = ReflectorMember.Create(typeof (TestClass).GetProperty("Name"));
		}

		[Test]
		public void ShouldUseCustomSerialiserFactory()
		{
			string xml = @"<customSerialiser><foo>3</foo></customSerialiser>";
			TestCustomSerialiser test = (TestCustomSerialiser) NetReflector.Read(xml);
			Assert.AreEqual(2, test.Foo);
		}

		[Test]
		public void ShouldUseDefaultSerialiserFactory()
		{
			ReflectorPropertyAttribute attribute = new ReflectorPropertyAttribute("foo");
			IXmlSerialiser serialiser = attribute.CreateSerialiser(member);
			Assert.IsNotNull(serialiser);
		}
	}

	internal class CustomSerialiserFactory : ISerialiserFactory
	{
		public IXmlMemberSerialiser Create(ReflectorMember member, ReflectorPropertyAttribute attribute)
		{
			return new CustomSerialiser(member, attribute);
		}
	}

	internal class CustomSerialiser : XmlMemberSerialiser
	{
		public CustomSerialiser(ReflectorMember member, ReflectorPropertyAttribute attribute) : base(member, attribute)
		{}

		public override object Read(XmlNode node, NetReflectorTypeTable table)
		{
			return 2;
		}
	}

	[ReflectorType("customSerialiser")]
	internal class TestCustomSerialiser
	{
		[ReflectorProperty("foo", typeof (CustomSerialiserFactory))]
		public int Foo;
	}
}