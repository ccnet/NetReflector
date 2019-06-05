using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using NetReflectorCore;
using NetReflectorCoreTest.Resources;
using NUnit.Framework;

namespace NetReflectorCoreTest
{
	[TestFixture]
	public class NetReflectorTypeTableTest
	{
		private const int NumberOfReflectorTypesInAssembly = 11;

		[Test]
		public void AddTypeToTypeTable()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(typeof (TestClass));

			Assert.AreEqual(1, table.Count);
			Assert.AreEqual(typeof (TestClass), table["reflectTest"].Type);
		}

		[Test]
		public void AddAssemblyToTypeTable()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(Assembly.GetExecutingAssembly());

			Assert.AreEqual(NumberOfReflectorTypesInAssembly, table.Count);
			Assert.AreEqual(typeof (TestClass), table["reflectTest"].Type);
			Assert.AreEqual(typeof (TestInnerClass), table["inner"].Type);
			Assert.AreEqual(typeof (TestSubClass), table["sub"].Type);
			Assert.AreEqual(typeof (ArrayTestClass), table["array-test"].Type);
			Assert.AreEqual(typeof (ElementTestClass), table["element"].Type);
			Assert.AreEqual(typeof (HashTestClass), table["hashtest"].Type);
			Assert.AreEqual(typeof (CollectionTestClass), table["collectiontest"].Type);
		}

		[Test]
		public void AddAssemblyTwice()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(Assembly.GetExecutingAssembly());
			table.Add(Assembly.GetExecutingAssembly());

			Assert.AreEqual(NumberOfReflectorTypesInAssembly, table.Count);
		}

		[Test]
		public void AddAllAssembliesInAppDomain()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(AppDomain.CurrentDomain);
			Assert.IsTrue(table.Count > 0);
		}

		[Test]
		public void AddAssemblyFromFilename()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
            var x = TestContext.CurrentContext.TestDirectory;

            //upgrading to nunit 3 changes the default working directory when running from Visual studio
            //table.Add("NetReflectorPlugin.Test.dll");
            table.Add(TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar + "NetReflectorCorePluginTest.dll");

            Assert.AreEqual(1, table.Count);
			Assert.IsNotNull(table["plugin"], "Plugin type not found");
		}

		[Test]
		public void LoadReflectorTypesByFilename_UnknownAssembly()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                NetReflectorTypeTable table = new NetReflectorTypeTable();
                table.Add("UnknownAssembly.Test.dll");
            });
        }

        [Test]
		public void LoadReflectorTypesByFilenameFilter()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(TestContext.CurrentContext.TestDirectory, "*Test.dll");
			Assert.AreEqual(NumberOfReflectorTypesInAssembly + 1, table.Count);
			Assert.IsNotNull(table["plugin"], "Plugin type not found");
			Assert.IsNotNull(table["array-test"], "Array-test type not found");
		}

        [Test]
		public void VerifyNetReflectorTypeLoadExceptionMessage()
		{
			ReflectionTypeLoadException innerException = new ReflectionTypeLoadException(new Type[] {typeof (TestClass), null}, new Exception[] {new TypeLoadException("Failed to load TestSubClass")});
			Assembly assembly = Assembly.GetCallingAssembly();
			NetReflectorTypeLoadException exception = new NetReflectorTypeLoadException(assembly, innerException);
			AssertContains(assembly.GetName().ToString(), exception.Message);
			AssertContains("Failed to load 1 of the 2 types defined in the assembly.", exception.Message);
			AssertContains("Failed to load TestSubClass", exception.Message);
		}

		[Test]
		public void ShouldUseCustomInstantiatorIfUsed()
		{
			TestInstantiator instantiator = new TestInstantiator();
			NetReflectorTypeTable table = new NetReflectorTypeTable(instantiator);
			table.Add(typeof (TestClass));

			Assert.AreEqual(instantiator, ((XmlTypeSerialiser) table["reflectTest"]).Instantiator);
		}


		private TypeBuilder CreateTypeWithReflectorTypeAttribute(ModuleBuilder moduleBuilder, string typeName, string reflectorName)
		{
			TypeBuilder tempType = moduleBuilder.DefineType(typeName);
			CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(typeof (ReflectorTypeAttribute).GetConstructor(new Type[] {typeof (string)}), new object[] {reflectorName});
			tempType.SetCustomAttribute(attributeBuilder);
			tempType.CreateType();
			return tempType;
		}

		private void AssertContains(string search, string target)
		{
			string message = string.Format("Search substring: {0} is not contained in target: {1}", search, target);
			Assert.IsTrue(target.IndexOf(search) >= 0, message);
		}
	}
}