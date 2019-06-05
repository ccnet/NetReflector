using Exortech.NetReflector.Util;
using NUnit.Framework;
using System;

namespace Exortech.NetReflector.Test.Util
{
	[TestFixture]
	public class DefaultInstantiatorTest
	{
		private DefaultInstantiator instantiator;

		[SetUp]
		public void Setup()
		{
			instantiator = new DefaultInstantiator();
		}

		public void IsCommonType()
		{
			Assert.IsTrue(ReflectionUtil.IsCommonType(typeof(string)), "String member should be common type");
			Assert.IsTrue(ReflectionUtil.IsCommonType(typeof(int)), "int should be common type");
			Assert.IsTrue(ReflectionUtil.IsCommonType(typeof(DateTime)), "DateTime should be common type");
			Assert.IsTrue(ReflectionUtil.IsCommonType(typeof(ReflectorMemberTest.TestEnum)), "Enum should be common type");
		}

		[Test]
		public void CreateInstance()
		{
			TestClass testClass = (TestClass)instantiator.Instantiate(typeof(TestClass));
			Assert.IsNotNull(testClass);
		}
		
		[Test]
		public void CreateInstanceWithUnknownType()
        {
            Assert.Throws<NetReflectorException>(() =>
            {
                instantiator.Instantiate(typeof(string));
            });
        }
    }
}
