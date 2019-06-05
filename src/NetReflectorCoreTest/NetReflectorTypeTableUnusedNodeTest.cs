using System;
using System.Collections;
using System.Reflection;
using NetReflectorCore;
using NetReflectorCore.Util;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[TestFixture]
	public class NetReflectorTypeTableUnusedNodeTest
	{
		private IList nodes;
		private XmlTypeSerialiser serialiser;
		private NetReflectorTypeTable table;

		[SetUp]
		protected void SetUp()
		{
			nodes = new ArrayList();			
			serialiser = new XmlTypeSerialiser(typeof(TestClass), ReflectorTypeAttribute.GetAttribute(typeof(TestClass)));
			table = NetReflectorTypeTable();
		}

		[Test]
		public void RaiseEventIfXmlContainsUnnecessaryNodes()
		{
			string xml = TestClass.GetXml(DateTime.Now, string.Format(@"<inner fooz=""bar""><baz/><yaz />{0}</inner>", TestInnerClass.GetInnerXml()));
			table.InvalidNode += new InvalidNodeEventHandler(HandleUnusedNode);
			serialiser.Read(XmlUtil.ReadNode(xml), table);

			Assert.AreEqual(3, nodes.Count);
			Assert.AreEqual("fooz", InvalidNodeName(0));
			Assert.AreEqual("baz", InvalidNodeName(1));
			Assert.AreEqual("yaz", InvalidNodeName(2));
		}

		private string InvalidNodeName(int index)
		{
			return ((InvalidNodeEventArgs)nodes[index]).Node.Name;
		}

		[Test]
		public void RaiseEventIfXmlContainsDuplicateNodes()
		{
			string xml = TestClass.GetXml(DateTime.Now, string.Format(@"<inner name=""dupe"">{0}<present>dupe</present></inner>", TestInnerClass.GetInnerXml()));
			table.InvalidNode += new InvalidNodeEventHandler(HandleUnusedNode);
			serialiser.Read(XmlUtil.ReadNode(xml), table);

			Assert.AreEqual(2, nodes.Count);
			Assert.AreEqual("name", InvalidNodeName(0));
			Assert.AreEqual("present", InvalidNodeName(1));
		}

		[Test]
		public void DoNotRaiseEventIfXmlContainsComments()
		{
			string xml = TestClass.GetXml(DateTime.Now, string.Format(@"<inner>{0}<!-- foo --></inner>", TestInnerClass.GetInnerXml()));
			table.InvalidNode += new InvalidNodeEventHandler(HandleUnusedNode);
			serialiser.Read(XmlUtil.ReadNode(xml), table);

			Assert.AreEqual(0, nodes.Count);
		}

		[Test]
		public void DoNotRaiseEventIfXmlDoesNotContainUnnecessaryNodes()
		{
			string xml = TestClass.GetXmlWithSubClass(DateTime.Now);
			table.InvalidNode += new InvalidNodeEventHandler(HandleUnusedNode);
			serialiser.Read(XmlUtil.ReadNode(xml), table);

			Assert.AreEqual(0, nodes.Count);
		}

		[Test]
		public void ShouldHandleRaisingEventsIfNoHandlerHasBeenRegistered()
		{
			table.OnInvalidNode(null);
		}

		private void HandleUnusedNode(InvalidNodeEventArgs args)
		{
			nodes.Add(args);
		}

		private NetReflectorTypeTable NetReflectorTypeTable()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(Assembly.GetExecutingAssembly());
			return table;
		}
	}
}
