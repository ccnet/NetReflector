using System;
using NUnit.Framework;

namespace Exortech.NetReflector.Test
{
	[ReflectorType("array-test")]
	internal class ArrayTestClass
	{
		private ElementTestClass[] elements = new ElementTestClass[0];

		[ReflectorArray("stringArray", Required=false)]
		public string[] StringArray = new string[0];

		[ReflectorArray("elementArray", Required=false)]
		public ElementTestClass[] Elements
		{
			get { return elements; }
			set { elements = value; }
		}

		[ReflectorArray("days", Required=false)]
		public DayOfWeek[] Days = new DayOfWeek[0];

		public static ArrayTestClass Create()
		{
			ArrayTestClass target = new ArrayTestClass();
			target.StringArray = new string[] { "1", "2", "3" };
			target.Elements = new ElementTestClass[] { ElementTestClass.Create("1"), ElementTestClass.Create("2"), ElementTestClass.Create("3") };
			return target;
		}

		public static ArrayTestClass CreateDays()
		{
			ArrayTestClass testClass = new ArrayTestClass();
			testClass.Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
			return testClass;
		}

		public static string GetXml()
		{
			return "<array-test><days /><elementArray><element><id>1</id></element><element><id>2</id></element><element><id>3</id></element></elementArray><stringArray><string>1</string><string>2</string><string>3</string></stringArray></array-test>";
		}

		public static string GetXmlForDays()
		{
			return "<array-test><days><day>Monday</day><day>Wednesday</day><day>Friday</day></days></array-test>";
		}

		public static void AssertEquals(ArrayTestClass expected, ArrayTestClass actual)
		{
			Assert.AreEqual(expected.Elements.Length, actual.Elements.Length);
			for (int i = 0; i < expected.Elements.Length; i++)
			{
				Assert.AreEqual(expected.Elements[i], actual.Elements[i]);
			}
			Assert.AreEqual(expected.StringArray.Length, actual.StringArray.Length);
			for (int i = 0; i < expected.StringArray.Length; i++)
			{
				Assert.AreEqual(expected.StringArray[i], actual.StringArray[i]);
			}
			Assert.AreEqual(expected.Days.Length, actual.Days.Length);
			for (int i = 0; i < expected.Days.Length; i++)
			{
				Assert.AreEqual(expected.Days[i], actual.Days[i]);
			}
		}
	}
}
