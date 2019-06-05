using NetReflectorCore;
using NUnit.Framework;
using System;
using System.IO;
using System.Xml;

namespace NetReflectorCoreTest
{
	[TestFixture]
	public class NetReflectorTest
	{
		private NetReflectorTypeTable table = NetReflectorTypeTable.CreateDefault();

		[Test]
		public void WriteWithNullTextWriter()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Write((TextWriter)null, "foo");
            });
        }

        [Test]
		public void WriteWithNullXmlWriter()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Write((XmlWriter)null, "foo");
            });
        }

        [Test]
		public void WriteWithNullTarget()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Write(null);
            });
        }

        [Test]
		public void WriteWithUnknownType()
        {
            Assert.Throws<NetReflectorException>(() =>
            {
                NetReflector.Write("string");
            });
        }

        [Test]
		public void ReadXmlWhereRootNodeDoesNotMatchReflectorType()
        {
            Assert.Throws<NetReflectorException>(() =>
            {
                NetReflector.Read("<foo/>", table);
            });
        }

        [Test]
		public void ReadNullXmlString()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Read((string)null, table);
            });
        }

        [Test]
		public void ReadNullTextReader()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Read((TextReader)null, table);
            });
        }

        [Test]
		public void ReadNullXmlReader()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Read((XmlReader)null, table);
            });
        }

        [Test]
		public void ReadNullXmlNode()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NetReflector.Read((XmlNode)null, table);
            });
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
