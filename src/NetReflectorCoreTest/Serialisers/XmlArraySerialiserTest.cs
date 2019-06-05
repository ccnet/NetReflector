using System.IO;
using System.Text;
using System.Xml;
using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest.Serialisers
{
	[TestFixture]
	public class XmlArraySerialiserTest
	{
		private NetReflectorTypeTable table = NetReflectorTypeTable.CreateDefault();

		[Test]
		public void WriteArrayTestClass()
		{
			string xml = NetReflector.Write(ArrayTestClass.Create());
			Assert.AreEqual(ArrayTestClass.GetXml(), xml);
		}

		[Test]
		public void ReadArrayTestClass()
		{
			ArrayTestClass actual = (ArrayTestClass)NetReflector.Read(ArrayTestClass.GetXml(), table);
			ArrayTestClass.AssertEquals(ArrayTestClass.Create(), actual);
		}

		[Test]
		public void ReadArrayTestClassContainingEncodedMarkup()
		{
			string xml = ArrayTestClass.GetXml().Replace("2", XmlEncode(@"<foo><![CDATA[bar]]>"));
			ArrayTestClass actual = (ArrayTestClass)NetReflector.Read(xml, table);
			Assert.AreEqual(@"<foo><![CDATA[bar]]>", actual.StringArray[1]);
			Assert.AreEqual(@"<foo><![CDATA[bar]]>", actual.Elements[1].ID);
		}

		[Test]
		public void ReadArrayTestClassWithEnums()
		{
			ArrayTestClass actual = (ArrayTestClass)NetReflector.Read(ArrayTestClass.GetXmlForDays(), table);
			ArrayTestClass.AssertEquals(ArrayTestClass.CreateDays(), actual);
		}

		private void AssertXmlIsEquivalent(string expected, string actual)
		{
			
		}

		private string XmlEncode(string value)
		{
			StringBuilder buffer = new StringBuilder();
			new XmlTextWriter(new StringWriter(buffer)).WriteString(value);
			return buffer.ToString();
		}
	}
}