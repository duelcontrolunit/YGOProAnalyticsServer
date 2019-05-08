using CronScheduler.AspNetCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

namespace YGOProAnalyticsServer.Jobs
{
    public class UpdatesJob : IScheduledJob
    {
        public string CronSchedule { get; } = "30 6 * * *";

        public string CronTimeZone { get; } = null;

        public bool RunImmediately { get; } = false;

        readonly IBanlistDataToBanlistUpdater _banlistUpdater;
        readonly ICardsDataToCardsAndArchetypesUpdater _cardsAndArchetypesUpdater;
        readonly IAdminConfig _adminConfig;
        readonly IMediator _mediator;

        public UpdatesJob(
            IBanlistDataToBanlistUpdater banlistUpdater, 
            ICardsDataToCardsAndArchetypesUpdater cardsAndArchetypesUpdater,
            IAdminConfig adminConfig)
        {
            _banlistUpdater = banlistUpdater ?? throw new ArgumentNullException(nameof(banlistUpdater));
            _cardsAndArchetypesUpdater = cardsAndArchetypesUpdater ?? throw new ArgumentNullException(nameof(cardsAndArchetypesUpdater));
            _adminConfig = adminConfig ?? throw new ArgumentNullException(nameof(adminConfig));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _banlistUpdater.UpdateBanlists(_adminConfig.BanlistApiURL);
            await _cardsAndArchetypesUpdater.UpdateCardsAndArchetypes(_adminConfig.CardApiURL);
            await _mediator.Publish(new CardsRelatedUpdatesCompleted());
        }
    }
}
