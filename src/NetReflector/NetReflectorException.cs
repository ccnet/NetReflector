using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Exortech.NetReflector
{
	[Serializable]
	public class NetReflectorException : ApplicationException
	{
		public NetReflectorException(string message) : base(message)
		{
		}

		public NetReflectorException(string message, Exception ex) : base(message, ex)
		{
		}

		protected NetReflectorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class NetReflectorTypeLoadException : NetReflectorException
	{
		public NetReflectorTypeLoadException(Assembly loadedAssembly, ReflectionTypeLoadException ex) : base(CreateMessage(loadedAssembly, ex))
		{
		}

		protected NetReflectorTypeLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		private static string CreateMessage(Assembly loadedAssembly, ReflectionTypeLoadException ex)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("Unable to load types from assembly {0}:", loadedAssembly.GetName()).Append(Environment.NewLine);
			builder.AppendFormat("Failed to load {0} of the {1} types defined in the assembly.", ex.LoaderExceptions.Length, ex.Types.Length).Append(Environment.NewLine);
			builder.Append("Exceptions: ").Append(Environment.NewLine);
			foreach (Exception exception in ex.LoaderExceptions)
			{
				TypeLoadException loaderException = exception as TypeLoadException;
				if (loaderException != null)
				{
					builder.AppendFormat("- Unable to load type: {0}", loaderException.TypeName).Append(Environment.NewLine);
				}
				builder.Append("	Exception: ").Append(exception.ToString());
			}
			return builder.ToString();
		}
	}
}