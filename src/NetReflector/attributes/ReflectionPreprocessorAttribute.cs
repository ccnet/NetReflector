using System;
using System.Xml;
using System.Reflection;

namespace Exortech.NetReflector
{
    /// <summary>
    /// Defines a method for preprocessing a node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ReflectionPreprocessorAttribute
        : Attribute
    {
        #region Public methods
        #region Invoke()
        /// <summary>
        /// Invokes the preprocessor method.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="method"></param>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        public static XmlNode Invoke(object parent, XmlNode inputNode)
        {
            var result = inputNode;
            foreach (var method in parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.GetCustomAttributes(typeof(ReflectionPreprocessorAttribute), true).Length > 0)
                {
                    result = method.Invoke(parent, new object[]{ inputNode }) as XmlNode;
                }
            }
            return result;
        }
        #endregion
        #endregion
    }
}
