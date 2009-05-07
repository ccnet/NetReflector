using System.Xml;
using System.Xml.Schema;
using Exortech.NetReflector.Util;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Text;

namespace Exortech.NetReflector.Generators
{
	public class XsdGenerator
	{
        private readonly Regex capitalFinder = new Regex("[A-Z][^A-Z]*", RegexOptions.Compiled);
		private readonly NetReflectorTypeTable table;
		private XsdTypes typeMap = new XsdTypes();
        private List<Type> enumTypes = new List<Type>();
        private List<Type> baseTypes = new List<Type>();
        private Dictionary<Type, ReferencedType> loadedTypes = new Dictionary<Type, ReferencedType>();
        private Dictionary<XmlSchemaChoice, Type> itemArrays = new Dictionary<XmlSchemaChoice, Type>();

		public XsdGenerator(NetReflectorTypeTable table)
		{
			this.table = table;
		}

        /// <summary>
        /// The namespace URI to use.
        /// </summary>
        public string NamespaceUri { get; set; }

		public XmlSchema Generate(bool generateElements)
		{
            // Find all the base types - these will be abstract XML types, but will need to be
            // inherited from
            foreach (IXmlTypeSerialiser serialiser in table)
            {
                FindBaseTypes(serialiser);
            }

            XmlSchema schema = InitialiseSchema();

            // Generate all the types - complex, enum and base (abstract)
            foreach (IXmlTypeSerialiser serialiser in table)
			{
                if (generateElements) GenerateSchemaElementForType(schema, serialiser);
                GenerateComplexType(schema, serialiser);
			}
            foreach (var enumType in enumTypes)
            {
                GenerateEnum(schema, enumType);
            }
            foreach (var baseType in baseTypes)
            {
                GenerateBase(schema, baseType);
            }

            // Link the required types together
            foreach (var item in itemArrays)
            {
                foreach (var loadedType in loadedTypes)
                {
                    if ((loadedType.Value.Extends == item.Value) ||
                        ((loadedType.Value.Extends == null) &&
                        item.Value.IsAssignableFrom(loadedType.Key)))
                    {
                        item.Key.Items.Add(loadedType.Value.GenerateElement());
                    }
                }
            }
            return schema;
		}

        private XmlSchema InitialiseSchema()
        {
            XmlSchema schema = new XmlSchema
            {
                AttributeFormDefault = XmlSchemaForm.Unqualified,
                ElementFormDefault = XmlSchemaForm.Qualified
            };
            if (!string.IsNullOrEmpty(NamespaceUri))
            {
                schema.Namespaces.Add(string.Empty, NamespaceUri);
                schema.TargetNamespace = NamespaceUri;
            }
            return schema;
        }

        private void FindBaseTypes(IXmlTypeSerialiser serialiser)
        {
            foreach (IXmlMemberSerialiser memberSerialiser in serialiser.MemberSerialisers)
            {
                var memberType = memberSerialiser.ReflectorMember.MemberType;
                if (memberType.IsArray) memberType = memberType.GetElementType();
                if (!ReflectionUtil.IsCommonType(memberType))
                {
                    if (!baseTypes.Contains(memberType)) baseTypes.Add(memberType);
                }
            }
        }

		private void GenerateSchemaElementForType(XmlSchema schema, IXmlTypeSerialiser serialiser)
		{
			XmlSchemaElement element = new XmlSchemaElement();
			element.Name = serialiser.Attribute.Name;
            element.SchemaTypeName = new XmlQualifiedName(GenerateItemName(serialiser.Type), NamespaceUri);
			schema.Items.Add(element);
        }

        private void GenerateComplexType(XmlSchema schema, IXmlTypeSerialiser serialiser)
        {
			XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            complexType.Name= GenerateItemName(serialiser.Type);
			var particle = GenerateSchemaElementsForMembers(serialiser);

            if (serialiser.Attribute.Extends != null)
            {
                var extension = new XmlSchemaComplexContentExtension
                {
                    BaseTypeName = new XmlQualifiedName(GenerateItemName(serialiser.Attribute.Extends), NamespaceUri),
                    Particle = particle
                };
                complexType.ContentModel = new XmlSchemaComplexContent
                {
                    Content = extension
                };
                if (!baseTypes.Contains(serialiser.Attribute.Extends)) baseTypes.Add(serialiser.Attribute.Extends);
            }
            else
            {
                complexType.Particle = particle;
            }

			schema.Items.Add(complexType);
            if (baseTypes.Contains(serialiser.Type)) baseTypes.Remove(serialiser.Type);
            loadedTypes.Add(serialiser.Type, new ReferencedType
            {
                Extends = serialiser.Attribute.Extends,
                NamespaceUri = NamespaceUri,
                TypeName = complexType.Name,
                ElementName = serialiser.Attribute.Name,
                Description = serialiser.Attribute.Description
            });
		}

		private XmlSchemaParticle GenerateSchemaElementsForMembers(IXmlTypeSerialiser typeSerialiser)
		{
			XmlSchemaAll group = new XmlSchemaAll();
			foreach (IXmlMemberSerialiser memberSerialiser in typeSerialiser.MemberSerialisers)
			{
                var item = new XmlSchemaElement
                {
                    MaxOccurs = 1,
                    MinOccurs = memberSerialiser.Attribute.Required ? 1 : 0
                };
                AddDocumentation(memberSerialiser.Attribute.Description, item);
				item.Name = memberSerialiser.Attribute.Name;
                GenerateElementType(memberSerialiser.ReflectorMember.MemberType, item, memberSerialiser.Attribute.HasCustomFactory);
				group.Items.Add(item);				
			}
			return group;
		}

        private static void AddDocumentation(string description, XmlSchemaElement item)
        {
            if (!string.IsNullOrEmpty(description))
            {
                item.Annotation = new XmlSchemaAnnotation();
                var document = new XmlDocument();
                item.Annotation.Items.Add(new XmlSchemaDocumentation
                {
                    Markup = new XmlNode[]{
                        document.CreateTextNode(description)
                    }
                });
            }
        }

        private void GenerateElementType(Type itemType, XmlSchemaElement item, bool isAny)
        {
            if (isAny)
            {
            }
            else if (itemType.IsEnum)
            {
                if (!enumTypes.Contains(itemType)) enumTypes.Add(itemType);
                item.SchemaTypeName = new XmlQualifiedName(GenerateItemName(itemType), NamespaceUri);
            }
            else if (itemType.IsArray)
            {
                var choice = new XmlSchemaChoice
                {
                    MinOccurs= 0,
                    MaxOccursString = "unbounded"
                };
                var complexType = new XmlSchemaComplexType
                {
                    Particle = choice
                };
                item.SchemaType = complexType;
                var arrayItemType = itemType.GetElementType();
                if (arrayItemType.IsEnum || ReflectionUtil.IsCommonType(arrayItemType))
                {
                    var arrayItem = new XmlSchemaElement();
                    // The item name is not specified in the attribute, nor does NetReflector care
                    // Therefore, make a guess at a valid item name
                    if (item.Name.EndsWith("s"))
                    {
                        arrayItem.Name = item.Name.Substring(0, item.Name.Length - 1);
                    }
                    else
                    {
                        var match = capitalFinder.Match(item.Name);
                        if (match.Success)
                        {
                            var nextMatch = match.NextMatch();
                            while (nextMatch.Success)
                            {
                                match = nextMatch;
                                nextMatch = match.NextMatch();
                            }
                            arrayItem.Name = item.Name.Substring(0, match.Index);
                        }
                        else
                        {
                            arrayItem.Name = item.Name;
                        }
                    }
                    GenerateElementType(arrayItemType, arrayItem, isAny);
                    choice.Items.Add(arrayItem);
                }
                else
                {
                    itemArrays.Add(choice, arrayItemType);
                }
            }
            else if (ReflectionUtil.IsCommonType(itemType))
            {
                item.SchemaTypeName = typeMap.ConvertToSchemaTypeName(itemType);
            }
            else
            {
                item.SchemaTypeName = new XmlQualifiedName(GenerateItemName(itemType), NamespaceUri);
            }
        }

        private void GenerateEnum(XmlSchema schema, Type enumType)
        {
            var restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            foreach (var enumValue in Enum.GetNames(enumType))
            {
                restriction.Facets.Add(
                    new XmlSchemaEnumerationFacet
                    {
                        Value = enumValue
                    });
            }
            var simpleType = new XmlSchemaSimpleType
            {
                Name = GenerateItemName(enumType),
                Content = restriction
            };

            schema.Items.Add(simpleType);
            if (baseTypes.Contains(enumType)) baseTypes.Remove(enumType);
        }

        private void GenerateBase(XmlSchema schema, Type baseType)
        {
            var complexType = new XmlSchemaComplexType
            {
                Name = GenerateItemName(baseType),
                IsAbstract = true
            };

            schema.Items.Add(complexType);
        }

        private string GenerateItemName(Type value)
        {
            if (value.FullName.Contains("+"))
            {
                var dotPos = value.FullName.LastIndexOf('.');
                var name = value.FullName.Substring(dotPos + 1).Replace('+', '.');
                return name;
            }
            else
            {
                return value.Name;
            }
        }

        private class ReferencedType
        {
            public string NamespaceUri { get; set; }
            public string TypeName { get; set; }
            public string ElementName { get; set; }
            public string Description { get; set; }
            public Type Extends { get; set; }

            public XmlSchemaElement GenerateElement()
            {
                var element = new XmlSchemaElement
                {
                    Name = ElementName,
                    SchemaTypeName = new XmlQualifiedName(TypeName, NamespaceUri)
                };
                XsdGenerator.AddDocumentation(Description, element);
                return element;
            }
        }
    }
}
