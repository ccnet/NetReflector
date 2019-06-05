using System;
using System.Runtime.Serialization;

namespace NetReflectorCore
{
    /// <summary>
    /// NetReflector has been unable to convert from one type to another.
    /// </summary>
	[Serializable]
    public class NetReflectorConverterException
        : Exception
    {
        #region Constructors
        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorConverterException"/>.
        /// </summary>
        public NetReflectorConverterException()
            : base()
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorConverterException"/>.
        /// </summary>
        public NetReflectorConverterException(string message)
            : base(message)
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorConverterException"/>.
        /// </summary>
        public NetReflectorConverterException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorConverterException"/>.
        /// </summary>
        protected NetReflectorConverterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        #endregion
    }
}
