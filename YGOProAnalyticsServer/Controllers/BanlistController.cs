using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanlistController : ControllerBase
    {
        readonly IBanlistService _banlistService;

        public BanlistController(IBanlistService banlistService)
        {
            _banlistService = banlistService ?? throw new ArgumentNullException(nameof(banlistService));
        }

        [HttpGet("ListOfBanlistsWithIdAndNames")]
        public async Task<IActionResult> GetListOfBanlistsWithIdAndNames()
        {
            return Ok(await _banlistService.GetListOfBanlistsNamesAndIdsAsNoTrackingFromCache());
        }
    }
}