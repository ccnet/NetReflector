using Exortech.NetReflector.Util;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Exortech.NetReflector.Test.Util
{
	[TestFixture]
	public class ReflectorMemberTest
	{
		private MemberInfo GetMember(string name)
		{
			return typeof(TestClass).GetProperty(name);
		}

		[Test]
		public void SetValue()
		{
			ReflectorMember member = ReflectorMember.Create(GetMember("Name"));
			TestClass testClass = new TestClass();
			member.SetValue(testClass, "Hello");
			Assert.AreEqual("Hello", testClass.Name);
		}

		[Test]
		public void SetValueWithTypeConversion()
		{
			ReflectorMember member = ReflectorMember.Create(GetMember("Count"));
			TestClass testClass = new TestClass();
			member.SetValue(testClass, "99");
			Assert.AreEqual(99, testClass.Count);
		}

        [Test, ExpectedException(typeof(NetReflectorConverterException))]
		public void SetValueWithTypeConversionForInconvertibleTypes()
		{
			ReflectorMember member = ReflectorMember.Create(GetMember("Count"));
			TestClass testClass = new TestClass();
			member.SetValue(testClass, "inconvertible");
		}

		[Test]
		public void SetValueOfSubClass()
		{
			ReflectorMember member = ReflectorMember.Create(GetMember("InnerClass"));
			TestClass testClass = new TestClass();
			member.SetValue(testClass, new TestSubClass());
			Assert.IsNotNull(testClass.InnerClass);
			Assert.AreEqual(typeof(TestSubClass), testClass.InnerClass.GetType());
		}

		[Test]
		public void SetValueToNull()
		{
			ReflectorMember member = ReflectorMember.Create(GetMember("InnerClass"));
			TestClass testClass = new TestClass();
			member.SetValue(testClass, null);
			Assert.IsNull(testClass.InnerClass);
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void SettingValueThrowsException()
		{
			ReflectorMember member = ReflectorMember.Create(typeof(ExceptionTestClass).GetProperty("ExceptionProperty"));
			ExceptionTestClass testClass = new ExceptionTestClass();
			member.SetValue(testClass, null);
		}

		[Test, ExpectedException(typeof(NetReflectorException))]
		public void SetValueWhenNoSet()
		{
			PropertyInfo readonlyNameProperty = typeof(ReadOnlMemberClass).GetProperty("ReadOnlyName");
			ReflectorMember member = ReflectorMember.Create(readonlyNameProperty);
			ReadOnlMemberClass testClass = new ReadOnlMemberClass();
			member.SetValue(testClass, "Hello");
		} 
		
		class ReadOnlMemberClass : TestClass
		{		
			string readonlyname;
			
			[ReflectorProperty("readonlyname", Description="name of the test class (read only)", Required = false)]
			public string ReadOnlyName
			{
				get { return readonlyname; }
			} 
		}
		
		class ExceptionTestClass : TestClass
		{
			public string ExceptionProperty
			{
				set { throw new ArgumentException("always throws an exception"); }
			}
		}

		[Test]
		public void AutomaticallyParseStringIntoEnum()
		{
			ReflectorMember member = ReflectorMember.Create(typeof(EnumTestClass).GetField("EnumValue"));
			Assert.IsNotNull(member, "member not found");
			EnumTestClass testClass = new EnumTestClass();
			Assert.AreEqual(TestEnum.D, testClass.EnumValue);
			member.SetValue(testClass, TestEnum.A);		// set value as enum
			Assert.AreEqual(TestEnum.A, testClass.EnumValue);
			member.SetValue(testClass, "B");		// set value as string		
			Assert.AreEqual(TestEnum.B, testClass.EnumValue);
			member.SetValue(testClass, "c");		// set value as string		
			Assert.AreEqual(TestEnum.C, testClass.EnumValue);
		}

		class EnumTestClass
		{
			public TestEnum EnumValue = TestEnum.D;
		}

		public enum TestEnum
		{
			A, B, C, D
		}
	}
}
