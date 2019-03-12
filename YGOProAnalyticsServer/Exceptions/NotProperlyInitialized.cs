using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Exceptions
{
    public class NotProperlyInitializedException : Exception
    {
        public NotProperlyInitializedException() : base()
        {
        }

        public NotProperlyInitializedException(string message)
            : base(message)
        {
        }

        public NotProperlyInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
