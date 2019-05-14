using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YgoProServerActivityController : ControllerBase
    {
        readonly IServerActivityStatisticsService _activityStatisticsService;
        readonly IYgoProServerStatusService _serverStatusService;
        readonly IAdminConfig _adminConfig;
        readonly string _ygoProListOfRoomsURL;

        public YgoProServerActivityController(
            IServerActivityStatisticsService activityStatisticsService,
            IYgoProServerStatusService serverStatusService,
            IAdminConfig adminConfig)
        {
            _activityStatisticsService = activityStatisticsService ?? throw new ArgumentNullException(nameof(activityStatisticsService));
            _serverStatusService = serverStatusService ?? throw new ArgumentNullException(nameof(serverStatusService));
            _adminConfig = adminConfig ?? throw new ArgumentNullException(nameof(adminConfig));
            _ygoProListOfRoomsURL = _adminConfig.YgoProListOfRoomsUrl;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityStatistics()
        {
            return new JsonResult(await _activityStatisticsService.GetAllAsDtos());
        }

        [HttpGet("isOnline")]
        public async Task<IActionResult> IsOnline()
        {
            return new JsonResult(
                await _serverStatusService.IsOnlineBasedOnListOfRooms(_ygoProListOfRoomsURL)
            );
        }

        [HttpGet("ygoProServerActivityNow")]
        public async Task<IActionResult> YgoProActivityNow()
        {
            return new JsonResult(new YgoProActivityNowDTO() {
                NumberOfClosedRooms = await _serverStatusService
                                                .NumberOfClosedRooms(_ygoProListOfRoomsURL),
                NumberOfOpenRooms = await _serverStatusService
                                                .NumberOfOpenRooms(_ygoProListOfRoomsURL),
                NumberOfPlayersInLobby = await _serverStatusService
                                                .NumberOfPlayersInLobby(_ygoProListOfRoomsURL),
                NumberOfPlayersWhichPlayNow = await _serverStatusService
                                                .NumberOfPlayersWhichPlayNow(_ygoProListOfRoomsURL)
            });
        }
    }
}