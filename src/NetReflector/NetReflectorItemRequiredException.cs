using System;
using System.Runtime.Serialization;

namespace Exortech.NetReflector
{
    /// <summary>
    /// NetReflector has been unable to find a required item.
    /// </summary>
	[Serializable]
    public class NetReflectorItemRequiredException
        : Exception
    {
        #region Constructors
        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorItemRequiredException"/>.
        /// </summary>
        public NetReflectorItemRequiredException()
            : base()
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorItemRequiredException"/>.
        /// </summary>
        public NetReflectorItemRequiredException(string message)
            : base(message)
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorItemRequiredException"/>.
        /// </summary>
        public NetReflectorItemRequiredException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Starts a new instance of <see cref="NetReflectorItemRequiredException"/>.
        /// </summary>
        protected NetReflectorItemRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        #endregion
    }
}
