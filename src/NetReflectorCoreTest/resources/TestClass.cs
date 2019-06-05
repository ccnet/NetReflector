using System;
using NetReflectorCore;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[ReflectorType("reflectTest", Description="Class used for unit testing NetReflector.")]
	internal class TestClass
	{
		private string name;
		private int count;
		private string message;
		private DateTime date;
		private TestInnerClass innerClass;

		[ReflectorProperty("name", Description="name of the test class")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		[ReflectorProperty("count")]
		public int Count
		{
			get { return count; }
			set { count = value; }
		}

		[ReflectorProperty("message")]
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		[ReflectorProperty("date")]
		public DateTime DateTime
		{
			get { return date; }
			set { date = value; }
		}

		[ReflectorProperty("inner", InstanceTypeKey="classType")]
		public TestInnerClass InnerClass
		{
			get { return innerClass; }
			set { innerClass = value; }
		}

		[ReflectorProperty("field", Required=false)]
		public string Field = "foo";

		public static TestClass Create(DateTime date)
		{
			TestClass target = new TestClass();
			target.Count = 54;
			target.DateTime = date;
			target.InnerClass = TestInnerClass.Create();
			target.Field = "field";
			target.Message = "message";
			target.Name = "foo";
			return target;
		}

		public static string GetXml(DateTime date, string innerTestClassXml)
		{
			return string.Format(@"<reflectTest><count>54</count><date>{0}</date><field>field</field>{1}<message>message</message><name>foo</name></reflectTest>",
				date, innerTestClassXml);		
		}

		public static string GetXml(DateTime date)
		{
			return GetXml(date, string.Format(@"<inner classType=""inner"">{0}</inner>", TestInnerClass.GetInnerXml()));
		}

		public static TestClass CreateWithSubClass()
		{
			TestClass target = Create(DateTime.Now);
			target.InnerClass = TestSubClass.Create();
			return target;
		}

		public static string GetXmlWithSubClass(DateTime date)
		{
			return TestClass.GetXml(date, @"<inner classType=""sub""><name>sub</name><present>here</present><subzero>-40</subzero></inner>");
		}

		public static void AssertEquals(TestClass expected, TestClass actual)
		{
			Assert.AreEqual(expected.Count, actual.Count);
			Assert.AreEqual(expected.DateTime.ToString(), actual.DateTime.ToString());
			Assert.AreEqual(expected.Field, actual.Field);
			Assert.AreEqual(expected.Message, actual.Message);
			Assert.AreEqual(expected.Name, actual.Name);
		}
	}
}
