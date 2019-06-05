using System;
using System.Xml;

namespace NetReflectorCore
{
	public delegate void InvalidNodeEventHandler(InvalidNodeEventArgs args);

	public class InvalidNodeEventArgs : EventArgs
	{
		public readonly XmlNode Node;
		public readonly string Message;

		public InvalidNodeEventArgs(XmlNode node, string message)
		{
			Node = node;
			Message = message;
		}

		public override string ToString()
		{
			return string.Format("{0}.  Node={1}", Message, Node.OuterXml);
		}
	}
}