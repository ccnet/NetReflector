using System.Collections;
using System.Globalization;
using System.Text;

namespace Exortech.NetReflector.Util
{
	public sealed class StringUtil
	{
		// Static utility class
		private StringUtil() { }

		public static bool EqualsIgnoreCase(string a, string b)
		{
			return CaseInsensitiveComparer.Default.Compare(a, b) == 0;
		}

		public static string Join(string separator, object[] objects)
		{
			StringBuilder builder = new StringBuilder();
			foreach (object obj in objects)
			{
				if (obj == null) continue;
				if (builder.Length > 0) builder.Append(separator);
				builder.Append(obj.ToString());
			}
			return builder.ToString();
		}

		public static string Format(string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}
	}
}
