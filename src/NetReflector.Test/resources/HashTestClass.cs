using System.Collections;
using NUnit.Framework;

namespace Exortech.NetReflector.Test
{
	[ReflectorType("hashtest")]
	internal class HashTestClass
	{
		private Hashtable list;

		[ReflectorHash("elements", "id", Required=false)]
		public SortedList ElementHash;

		[ReflectorHash("list", InstanceType=typeof(SortedList), Required=false)]
		public IDictionary List
		{
			get { return list; }
			set { list = (Hashtable)value; }
		}

		public static HashTestClass CreateHashtableWithStrings()
		{
			SortedList list = new SortedList();
			list.Add("1", "a");
			list.Add("2", "b");
			list.Add("3", "c");

			HashTestClass hash = new HashTestClass();
			hash.ElementHash = list;
			return hash;
		}

		public static HashTestClass CreateHashtableWithElements()
		{
			SortedList list = new SortedList();
			list.Add("1", ElementTestClass.Create("1"));
			list.Add("2", ElementTestClass.Create("2"));
			list.Add("3", ElementTestClass.Create("3"));

			HashTestClass hash = new HashTestClass();
			hash.ElementHash = list;
			return hash;
		}

		public static void AssertEquals(HashTestClass expected, HashTestClass actual)
		{
			Assert.AreEqual(expected.ElementHash.Count, actual.ElementHash.Count);
			foreach (object key in expected.ElementHash.Keys)
			{
				Assert.IsTrue(actual.ElementHash.ContainsKey(key));
				Assert.AreEqual(expected.ElementHash[key], actual.ElementHash[key]);
			}
		}

		public static string GetXmlForHashtableWithStrings()
		{
			return @"<hashtest><elements><string id=""1"">a</string><string id=""2"">b</string><string id=""3"">c</string></elements></hashtest>";
		}

		public static string GetXmlForHashtableWithElements()
		{
			return @"<hashtest><elements><element id=""1""><id>1</id></element><element id=""2""><id>2</id></element><element id=""3""><id>3</id></element></elements></hashtest>";
		}		
	}
}
