using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Exceptions
{
    /// <summary>
    /// Should be used only when decklist contains no cards in Main, Extra and Side Deck.
    /// When this exception is thrown, check why is an empty decklist given as a parameter for analytics).
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class EmptyDecklistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyDecklistException"/> class.
        /// </summary>
        public EmptyDecklistException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyDecklistException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmptyDecklistException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBanlistException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public EmptyDecklistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
