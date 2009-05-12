using System.IO;
using Exortech.NetReflector;
using Exortech.NetReflector.Generators;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace NetReflectorDocumenterTask
{
	[TaskName("netreflector")]
	public class NetReflectorDocumenterTask : Task
	{
		private string outfile = "netreflector.xml";
		private string assembly;

		[TaskAttribute("outfile", Required=false)]
		public string FileName 
		{
			get { return outfile; }
			set { outfile = value; }
		}

		[TaskAttribute("assembly", Required=true)]
		public string Assembly 
		{
			get { return assembly; }
			set { assembly = value; }
		}

		protected override void ExecuteTask()
		{
			Log(Level.Info, "Loading assembly {0}.", assembly);
			NetReflectorTypeTable table = new NetReflectorTypeTable();
			table.Add(assembly);
			Log(Level.Info, "Loaded {0} types.", table.Count);

			Log(Level.Info, "Generating documentation. The output will be written to {0}", outfile);
			using (StreamWriter writer = new StreamWriter(outfile))
			{
				new XmlDocumentationGenerator(table).WriteIndented(writer);				
			}
		}
	}
}
