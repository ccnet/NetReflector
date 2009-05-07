using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using Exortech.NetReflector.Generators;
using Exortech.NetReflector.Util;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Generators
{
	[TestFixture]
	public class XmlDocumentationGeneratorTest
	{
		[Test]
		public void ShouldProduceXmlDocumentationOfReflectorTypes()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(typeof (TestClass));

			StringWriter writer = new StringWriter();
			XmlDocumentationGenerator generator = new XmlDocumentationGenerator(table, new XmlMemberDocumentationGeneratorExtension());
			generator.Write(writer);

			string expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?><netreflector><reflectortype><name>TestClass</name><namespace>Exortech.NetReflector.Test</namespace><reflectorName>reflectTest</reflectorName>" +
				"<description>Class used for unit testing NetReflector.</description></reflectortype></netreflector>";
			Assert.AreEqual(expectedXml, writer.ToString());
		}

		[Test]
		public void ShouldProduceXmlDocumentationOfReflectorTypesButNotIncludeEmptyDescriptions()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(typeof (TestSubClass));

			StringWriter writer = new StringWriter();
			XmlDocumentationGenerator generator = new XmlDocumentationGenerator(table, new XmlMemberDocumentationGeneratorExtension());
			generator.Write(writer);

			string expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?><netreflector><reflectortype><name>TestSubClass</name><namespace>Exortech.NetReflector.Test</namespace><reflectorName>sub</reflectorName></reflectortype></netreflector>";
			Assert.AreEqual(expectedXml, writer.ToString());
		}

		[Test]
		public void ShouldProduceDocumentationForTypeMembers()
		{
			StringWriter writer = new StringWriter();

			XmlMemberDocumentationGenerator generator = new XmlMemberDocumentationGenerator();
			generator.Write(new XmlTextWriter(writer), new TestXmlMemberSerialiser());

			string expectedXml = @"<members><member><name>Name</name><reflectorName>name</reflectorName><description>name of the test class</description><required>True</required></member>" +
				"<member><name>Count</name><reflectorName>count</reflectorName><required>True</required></member></members>";
			Assert.AreEqual(expectedXml, writer.ToString());
		}


		private class XmlMemberDocumentationGeneratorExtension : XmlMemberDocumentationGenerator
		{
			public override void Write(XmlWriter writer, IXmlTypeSerialiser typeSerialiser)
			{
				// do nothing
			}
		}

		private class TestXmlMemberSerialiser : IXmlTypeSerialiser
		{
			private IXmlMemberSerialiser CreateSerialiser(string name)
			{
				MemberInfo member = typeof (TestClass).GetMember(name)[0];
				return (IXmlMemberSerialiser) ReflectorPropertyAttribute.GetAttribute(member).CreateSerialiser(ReflectorMember.Create(member));
			}

			public Type Type
			{
				get { throw new NotImplementedException(); }
			}

			public ReflectorTypeAttribute Attribute
			{
				get { throw new NotImplementedException(); }
			}

			public IEnumerable MemberSerialisers
			{
				get { return new IXmlMemberSerialiser[] {CreateSerialiser("Name"), CreateSerialiser("Count")}; }
			}

			public void Write(XmlWriter writer, object target)
			{
				throw new NotImplementedException();
			}

			public object Read(XmlNode node, NetReflectorTypeTable table)
			{
				throw new NotImplementedException();
			}
		}
	}
}