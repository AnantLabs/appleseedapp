using System.Runtime.Serialization;

namespace System.Configuration.Provider
{
    /// <summary>
    /// Summary description for NotSupportedByProviderException.
    /// </summary>
    [Serializable]
    public class NotSupportedByProviderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NotSupportedByProviderException"/> class.
        /// </summary>
        public NotSupportedByProviderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NotSupportedByProviderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotSupportedByProviderException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NotSupportedByProviderException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected NotSupportedByProviderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NotSupportedByProviderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public NotSupportedByProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}