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
    public class ArchetypeController : ControllerBase
    {
        readonly IArchetypeService _archetypeService;

        public ArchetypeController(IArchetypeService archetypeService)
        {
            _archetypeService = archetypeService ?? throw new ArgumentNullException(nameof(archetypeService));
        }

        [HttpGet("ArchetypeListWithIdsAndNames")]
        public async Task<IActionResult> GetArchetypeListWithIdsAndNames()
        {
            return Ok(await _archetypeService.GetArchetypeListWithIdsAndNamesAsNoTracking());
        }
    }
}