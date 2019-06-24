using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Events
{
    public class CardsRelatedUpdatesCompleted : INotification
    {
        public IEnumerable<Banlist> NewBanlists { get; }

        public CardsRelatedUpdatesCompleted(IEnumerable<Banlist> banlists)
        {
            NewBanlists = banlists;
        }
    }
}
