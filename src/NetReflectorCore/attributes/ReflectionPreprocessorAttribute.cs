using System;
using System.Xml;
using System.Reflection;

namespace NetReflectorCore
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
        /// <param name="parent">The parent.</param>
        /// <param name="typeTable">The type table.</param>
        /// <param name="inputNode">The input node.</param>
        /// <returns></returns>
        public static XmlNode Invoke(object parent, NetReflectorTypeTable typeTable, XmlNode inputNode)
        {
            var result = inputNode;
            foreach (var method in parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.GetCustomAttributes(typeof(ReflectionPreprocessorAttribute), true).Length > 0)
                {
                    result = method.Invoke(parent, new object[]{ typeTable, inputNode }) as XmlNode;
                }
            }
            return result;
        }
        #endregion
        #endregion
    }
}
