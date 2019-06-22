using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchetypeController : ControllerBase
    {
        readonly IArchetypeService _archetypeService;
        readonly IArchetypeToDtoConverter _archetypeToDtoConverter;

        public ArchetypeController(
            IArchetypeService archetypeService,
            IArchetypeToDtoConverter archetypeToDtoConverter)
        {
            _archetypeService = archetypeService ?? throw new ArgumentNullException(nameof(archetypeService));
            _archetypeToDtoConverter = archetypeToDtoConverter ?? throw new ArgumentNullException(nameof(archetypeToDtoConverter));
        }

        [HttpGet("ArchetypeListWithIdsAndNames")]
        public async Task<IActionResult> GetArchetypeListWithIdsAndNames()
        {
            return Ok(await _archetypeService.GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var archetype = await _archetypeService.GetDataForConcreteArchetypePage(id);
            if(archetype == null)
            {
                return NotFound($"Archetype with id equal {id} not found.");
            }

            var dto = _archetypeToDtoConverter.Convert(archetype);

            return Ok(dto);
        }
    }
}