using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Exceptions
{
    /// <summary>
    /// Should be used only when we detect unnown banlist (what is really important for analysis,
    /// but cause we are dependent from api, we never can be sure API is up to date.
    /// When this exception is thrown, server admin should be notified about situation immediately).
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class UnknownBanlistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBanlistException"/> class.
        /// </summary>
        public UnknownBanlistException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBanlistException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnknownBanlistException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBanlistException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public UnknownBanlistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
