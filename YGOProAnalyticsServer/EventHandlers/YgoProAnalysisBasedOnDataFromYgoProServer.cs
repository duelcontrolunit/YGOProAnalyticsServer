using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Events;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProAnalysisBasedOnDataFromYgoProServer : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        public Task Handle(DataFromYgoProServerRetrieved notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
