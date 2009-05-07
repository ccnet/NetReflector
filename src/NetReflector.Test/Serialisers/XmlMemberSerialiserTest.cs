using NUnit.Framework;
using System;

namespace Exortech.NetReflector.Test.Serialisers
{
	[TestFixture]
	public class	XmlMemberSerialiserTest
	{
		private NetReflectorTypeTable table = NetReflectorTypeTable.CreateDefault();

		[Test]
		public void WriteTestInnerClass()
		{
			TestInnerClass target = TestInnerClass.Create();
			string xml = NetReflector.Write(target);
			Assert.AreEqual(TestInnerClass.GetOuterXml(), xml);
		}

		[Test]
		public void WriteTestClass()
		{
			TestClass target = TestClass.Create(DateTime.Now);
			string xml = NetReflector.Write(target);
			Assert.AreEqual(TestClass.GetXml(target.DateTime), xml);
		}

		[Test]
		public void WriteSubClass()
		{
			TestClass target = TestClass.CreateWithSubClass();
			string xml = NetReflector.Write(target);
			Assert.AreEqual(TestClass.GetXmlWithSubClass(target.DateTime), xml);
		}

		[Test]
		public void WriteSubClassWithXmlMarkupInProperty()
		{
			TestClass target = TestClass.CreateWithSubClass();
			target.DateTime = DateTime.Now;
			target.InnerClass.InnerName = "<![CDATA[<message/>]]><!foo>&quot;";

			string xml = NetReflector.Write(target);
			Assert.AreEqual(TestClass.GetXml(target.DateTime, @"<inner classType=""sub""><name>&lt;![CDATA[&lt;message/&gt;]]&gt;&lt;!foo&gt;&amp;quot;</name><present>here</present><subzero>-40</subzero></inner>"), xml);
		}

		[Test]
		public void ReadTestInnerClass()
		{
			TestInnerClass result = (TestInnerClass)NetReflector.Read(TestInnerClass.GetOuterXml(), table);
			TestInnerClass.AssertEquals(TestInnerClass.Create(), result);
		}

		[Test]
		public void ReadTestClass()
		{
			DateTime now = DateTime.Now;
			TestClass root = (TestClass)NetReflector.Read(TestClass.GetXml(now), table);
			TestClass.AssertEquals(TestClass.Create(now), root);
		}

		[Test]
		public void ReadInnerClassWithComment()
		{
			TestInnerClass root = (TestInnerClass)NetReflector.Read(TestInnerClass.GetOuterXmlWithEmbeddedComment(), table);
			TestInnerClass.AssertEquals(TestInnerClass.Create(), root);
		}

		[Test]
		public void ReadSubClass()
		{
			TestClass testClass = (TestClass)NetReflector.Read(TestClass.GetXmlWithSubClass(DateTime.Now), table);
			TestSubClass.AssertEquals(TestSubClass.Create(), (TestSubClass)testClass.InnerClass);
		}

		[Test]
		public void ReadSubClassWithEncodedXmlInProperty()
		{
			string xml = TestClass.GetXml(DateTime.Now, @"<inner classType=""sub""><subzero>-40</subzero><name>&lt;![CDATA[&lt;message/&gt;]]&gt;&lt;!foo&gt;&amp;quot;</name><present>here</present></inner>");
			TestClass testClass = (TestClass)NetReflector.Read(xml, table);
			Assert.AreEqual(@"<![CDATA[<message/>]]><!foo>&quot;", testClass.InnerClass.InnerName);
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void ReadSubClassWhereTypeIsInvalid()
		{
			string xml = TestClass.GetXml(DateTime.Now, @"<inner classType=""nonexistent""><subzero>-40</subzero><name>&lt;![CDATA[&lt;message/&gt;]]&gt;&lt;!foo&gt;&amp;quot;</name><present>here</present></inner>");
			NetReflector.Read(xml, table);
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void ReadTestInnerClassThatIsMissingRequiredName()
		{
			NetReflector.Read(TestInnerClass.GetOuterXmlMissingName(), table);
		}

		[Test]
		public void ReadShouldNotOverwriteDefaultValues()
		{
			TestInnerClass innerClass = (TestInnerClass)NetReflector.Read(TestInnerClass.GetOuterXmlIncludingOnlyRequired());
			TestInnerClass.AssertEquals(TestInnerClass.CreateMinimal(), innerClass);
		}

		[Test, Ignore("yet to be implemented")]
		public void OnWriteCheckRequiredKeyword()
		{
		}
		
		[Test, ExpectedException(typeof(NetReflectorException))]
		public void ReadXmlWithMissingTypeOnAPropertyShouldThrowNetReflectorException()
		{
			string xml = TestClass.GetXmlWithSubClass(DateTime.Now).Replace(@"classType=""sub""", @"classType=""foo""");
			NetReflector.Read(xml, table);
		}

		[Test]
		public void ReadXmlWithEnum()
		{
			string xml = EnumTestClass.GetXml();
			EnumTestClass testEnum = (EnumTestClass) NetReflector.Read(xml);
			EnumTestClass.AssertEquals(testEnum);
		}

		[Test]
		public void ShouldIgnoreAnyMembersWithInstanceTypesThatAreNotSetToANetReflectorType()
		{
			IgnoreNonReflectorTypeTestClass target = new IgnoreNonReflectorTypeTestClass();
			target.MyComplexMember = new NonNetReflectorComplexMemberType();

			string serializedForm = NetReflector.Write(target);

			Assert.AreEqual(@"<ignorenonreflectortype />", serializedForm);
		}
	}
}
