using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Exceptions
{
    public class NotProperlyInitialized : Exception
    {
        public NotProperlyInitialized() : base()
        {
        }

        public NotProperlyInitialized(string message)
            : base(message)
        {
        }

        public NotProperlyInitialized(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
