using NUnit.Framework;
using System;
using System.IO;
using System.Xml;

namespace Exortech.NetReflector.Test
{
	[TestFixture]
	public class NetReflectorTest
	{
		private NetReflectorTypeTable table = NetReflectorTypeTable.CreateDefault();

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void WriteWithNullTextWriter()
		{
			NetReflector.Write((TextWriter)null, "foo");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void WriteWithNullXmlWriter()
		{
			NetReflector.Write((XmlWriter)null, "foo");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void WriteWithNullTarget()
		{
			NetReflector.Write(null);
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void WriteWithUnknownType()
		{
			NetReflector.Write("string");
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void ReadXmlWhereRootNodeDoesNotMatchReflectorType()
		{
			NetReflector.Read("<foo/>", table);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ReadNullXmlString()
		{
			NetReflector.Read((string)null, table);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ReadNullTextReader()
		{
			NetReflector.Read((TextReader)null, table);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ReadNullXmlReader()
		{
			NetReflector.Read((XmlReader)null, table);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ReadNullXmlNode()
		{
			NetReflector.Read((XmlNode)null, table);
		}

		[Test]
		public void ReadNullReflectorTypeTable()
		{
			NetReflector.Read(@"<inner name=""foo"" />");
		}

		[Test]
		public void ReadMembers()
		{
			TestInnerClass inner = new TestInnerClass();
			NetReflector.Read(@"<foo name=""foo"" />", inner);
			Assert.AreEqual("foo", inner.InnerName);
		}
	}
}
