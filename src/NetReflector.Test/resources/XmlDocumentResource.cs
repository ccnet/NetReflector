using System;
using System.Xml;

namespace Exortech.NetReflector.Test
{
	public class XmlDocumentResource
	{
		public static readonly DateTime DefaultDate = new DateTime(2002, 12, 24);

		public static string Xml
		{
			get
			{	 
				return String.Format(
@"<reflectTest name=""rori"" count=""100"">
	<message>Hello World!</message>
	<date>{0}</date>
	<inner name=""newo"" present=""i'm here"" subzero=""-274"" classType=""sub"" />
</reflectTest>"
					, DefaultDate.ToShortDateString());
			}
		}

		public static XmlDocument Document
		{
			get 
			{
				return CreateDocument(Xml);
			}
		}

		public static XmlElement DocumentElement
		{
			get { return Document.DocumentElement; }
		}

		public static XmlNode DateElement
		{
			get { return Document.SelectSingleNode("/reflectTest/date"); }
		}

		public static XmlNode InnerElement
		{
			get { return Document.SelectSingleNode("/reflectTest/inner"); }
		}

		public static XmlNode InnerElementWithComment
		{
			get 
			{
				return CreateDocumentElement(
@"<inner name=""newo"">
	<present>i'm here</present>
	<!-- <missing>i'm not</missing> -->
</inner>"); }
		}

		public static XmlNode StringArrayElement
		{
			get { return CreateDocumentElement(
@"<stringArray>
		<item>Item 3</item>
		<include>Item 4<ignore nan=""nan""/></include>
</stringArray>"); }
		}

		public static XmlNode ObjectArrayElement
		{
			get 
			{
				return CreateDocumentElement(
@"<elementArray>
		<element id=""e1""/>
		<element id=""e2""/>
</elementArray>"); }
		}

		public static XmlNode ObjectArrayElement_BadElement
		{
			get 
			{
				return CreateDocumentElement(
@"<elementArray>
		<element id=""e1""/>
		<inner name=""newo""/>
</elementArray>"); }
		}

		public static XmlNode ObjectArrayElementWithComment
		{
			get 
			{
				return CreateDocumentElement(
@"<elementArray>
		<element id=""e1""/>
		<!-- <element id=""e2""/> -->
		<element id=""e3""/>
</elementArray>"); }
		}

		public static XmlNode ElementListElement
		{
			get
			{
				return CreateDocumentElement(
@"<elements>
	<element id=""hashitem1"" />
	<element id=""hashitem2"" />
	<element id=""hashitem3"" />
</elements>");
			}
		}

		public static XmlNode ElementListElement_MissingKey
		{
			get
			{
				return CreateDocumentElement(
@"<elements>
	<element id=""hashitem1"" />
	<element/>
</elements>");
			}
		}

		public static XmlNode StringListElement
		{
			get
			{
				return CreateDocumentElement(
@"<list>
	<a>test</a>
	<b>test2</b>
	<c/>
</list>");
			}
		}

		public static XmlNode ElementListWithComment
		{
			get
			{
				return CreateDocumentElement(
@"<elements>
	<element id=""hashitem1"" />
	<!--  <element id=""hashitem2"" /> COMMENT -->
	<element id=""hashitem3"" />
</elements>");
			}
		}

		public static XmlDocument CreateDocument(string xml)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);
			return document;
		}

		public static XmlNode CreateDocumentElement(string xml)
		{
			return CreateDocument(xml).DocumentElement;
		}
	}
}
