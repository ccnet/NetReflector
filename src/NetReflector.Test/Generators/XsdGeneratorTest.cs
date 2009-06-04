using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Exortech.NetReflector.Generators;
using NUnit.Framework;

namespace Exortech.NetReflector.Test.Generators
{
	[TestFixture]
	public class XsdGeneratorTest
	{
        [Test, Ignore("Work on schema generation is still in progress.")]
        public void ShouldGenerateASchemaToValidateTestClassXml()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(typeof(TestClass));
			table.Add(typeof(TestInnerClass));

			XsdGenerator generator = new XsdGenerator(table);
			XmlSchema schema = generator.Generate(true);
//			schema.Write(Console.Out);

			string xmlToValidate = TestClass.GetXml(DateTime.Today);
//			Console.Out.WriteLine("xmlToValidate = {0}", xmlToValidate);

			XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(new StringReader(xmlToValidate)));
			reader.Schemas.Add(schema);
			reader.ValidationType = ValidationType.Schema;
			while (reader.Read()) {}
		}

		[Test, Ignore("Work on schema generation is still in progress.")]
		public void ShouldGenerateASchemaToValidateTestSubClassXml()
		{
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(typeof(TestClass));
			table.Add(typeof(TestInnerClass));
			table.Add(typeof(TestSubClass));

			XsdGenerator generator = new XsdGenerator(table);
			XmlSchema schema = generator.Generate(true);
//			schema.Write(Console.Out);

			string xmlToValidate = TestClass.GetXmlWithSubClass(DateTime.Today);
//			Console.Out.WriteLine("xmlToValidate = {0}", xmlToValidate);

			XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(new StringReader(xmlToValidate)));
			reader.Schemas.Add(schema);
			reader.ValidationType = ValidationType.Schema;
			while (reader.Read()) {}
		}
	}
}
