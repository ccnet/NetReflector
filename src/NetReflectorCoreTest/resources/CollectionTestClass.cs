using System.Collections;
using System.Xml;
using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[ReflectorType("collectiontest")]
	internal class CollectionTestClass
	{
		private ArrayList list;

		[ReflectorCollection("elements", Required=false)]
		public ArrayList ElementList = new ArrayList();

		[ReflectorCollection("list", InstanceType=typeof(ArrayList), Required=false)]
		public IList List
		{
			get { return list; }
			set { list = (ArrayList)value; }
		}

		[ReflectorProperty("arrayList", InstanceType=typeof(ArrayList), Required=false)]
		public object Stuff;

		public static CollectionTestClass Create()
		{
			CollectionTestClass target = new CollectionTestClass();
			target.ElementList = new ArrayList(new ElementTestClass[] { ElementTestClass.Create("1"), ElementTestClass.Create("2"), ElementTestClass.Create("3") });
			target.List = new ArrayList(new string[] { "1", "2", "3" });
			return target;
		}

		public static CollectionTestClass CreateCollectionContainingNulls()
		{
			CollectionTestClass target = new CollectionTestClass();
			target.ElementList = new ArrayList(new ElementTestClass[] { ElementTestClass.Create("1"), null, ElementTestClass.Create("3") });
			target.List = new ArrayList(new string[] { "1", null, "3" });
			return target;
		}

		public static void AssertEquals(CollectionTestClass expected, CollectionTestClass actual)
		{
			Assert.AreEqual(expected.ElementList.Count, actual.ElementList.Count);
			Assert.AreEqual(((ElementTestClass)expected.ElementList[0]).ID, ((ElementTestClass)actual.ElementList[0]).ID);
			Assert.AreEqual(((ElementTestClass)expected.ElementList[1]).ID, ((ElementTestClass)actual.ElementList[1]).ID);
			Assert.AreEqual(((ElementTestClass)expected.ElementList[2]).ID, ((ElementTestClass)actual.ElementList[2]).ID);
			Assert.AreEqual(expected.List.Count, actual.List.Count);
			Assert.AreEqual(expected.List[0], actual.List[0]);
			Assert.AreEqual(expected.List[1], actual.List[1]);
			Assert.AreEqual(expected.List[2], actual.List[2]);
		}

		public static string GetXml()
		{
			return "<collectiontest><elements><element><id>1</id></element><element><id>2</id></element><element><id>3</id></element></elements>" +
				"<list><string>1</string><string>2</string><string>3</string></list></collectiontest>";
		}

		public static string GetXmlWithMissingNullElements()
		{
			return "<collectiontest><elements><element><id>1</id></element><element><id>3</id></element></elements>" +
				"<list><string>1</string><string>3</string></list></collectiontest>";
		}

		public static string GetXmlWithEmbeddedComment()
		{
			return GetXml().Replace("<id>3</id>", "<!-- foo --><id>3</id>");
		}

		public static XmlNode GetXmlNodeWithEmbeddedComment()
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(GetXmlWithEmbeddedComment());
			return document.DocumentElement;
		}
	}
}
