using System;
using Exortech.NetReflector;

namespace NetReflectorPluginTest
{
	[ReflectorType("plugin")]
	public class Plugin
	{
		private string name;
		private int count;
		private string[] things;

		[ReflectorProperty("name")]
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

		[ReflectorArray("things")]
		public string[] Things
		{
			get { return things; }
			set { things = value; }
		}
	}
}
