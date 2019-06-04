using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class ArchetypeService : IArchetypeService
    {
        readonly YgoProAnalyticsDatabase _db;

        public ArchetypeService(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<ArchetypeIdAndNameDTO>> GetArchetypeListWithIdsAndNamesAsNoTracking()
        {
            return await _db
                .Archetypes
                .AsNoTracking()
                .Select(x => new ArchetypeIdAndNameDTO(x.Id, x.Name))
                .ToListAsync();
        }
    }
}
