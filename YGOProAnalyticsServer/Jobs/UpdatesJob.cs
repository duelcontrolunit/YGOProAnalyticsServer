using CronScheduler.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        readonly IServiceScopeFactory _scopeFactory;
        readonly IAdminConfig _adminConfig;

        public UpdatesJob(IServiceScopeFactory scopeFactory, IAdminConfig adminConfig)
        {
            _scopeFactory = scopeFactory;
            _adminConfig = adminConfig;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    await _adminConfig.LoadConfigFromFile(AdminConfig.path);
                    var banlistUpdater = scope.ServiceProvider.GetRequiredService<IBanlistDataToBanlistUpdater>();
                    var cardsAndArchetypesUpdater = scope.ServiceProvider.GetRequiredService<ICardsDataToCardsAndArchetypesUpdater>();
                    var adminConfig = scope.ServiceProvider.GetRequiredService<IAdminConfig>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await cardsAndArchetypesUpdater.UpdateCardsAndArchetypes(adminConfig.CardApiURL);
                    var newBanlists = await banlistUpdater.UpdateBanlists(adminConfig.BanlistApiURL);
                    await mediator.Publish(new CardsRelatedUpdatesCompleted(newBanlists));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                }
            }
        }
    }
}
