using System;

namespace YGOProAnalyticsServer.Exceptions
{
    /// <summary>
    /// Should be used only when something is not properly initialized.
    /// </summary>
    public class NotProperlyInitializedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotProperlyInitializedException"/> class.
        /// </summary>
        public NotProperlyInitializedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotProperlyInitializedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotProperlyInitializedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotProperlyInitializedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public NotProperlyInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
