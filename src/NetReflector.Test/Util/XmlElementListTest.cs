using System.Xml;
using Exortech.NetReflector.Util;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Util
{
	[TestFixture]
	public class XmlElementListTest
	{
		[Test]
		public void EnsureNodeListHandlesComments()
		{
			XmlNode node = XmlDocumentResource.ElementListWithComment;
			Assert.AreEqual(3, node.ChildNodes.Count);
			XmlElementList list = new XmlElementList(node.ChildNodes);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("hashitem1", list[0].Attributes["id"].Value);
			Assert.AreEqual("hashitem3", list[1].Attributes["id"].Value);
		}

		[Test]
		public void VerifyEnumeration()
		{
			foreach (XmlNode node in XmlElementList.Create(XmlDocumentResource.ElementListWithComment.ChildNodes))
			{
				Assert.IsNotNull(node);
			}
		}
	}
}