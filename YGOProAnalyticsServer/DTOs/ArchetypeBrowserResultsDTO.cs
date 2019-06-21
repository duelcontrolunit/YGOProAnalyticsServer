using System;
using System.Collections.Generic;

namespace YGOProAnalyticsServer.DTOs
{
    public class ArchetypeBrowserResultsDTO
    {
        public ArchetypeBrowserResultsDTO(
            int totalNumberOfPages,
            IEnumerable<ArchetypeWithHowManyWinsAndHowManyWasUsed> archetypes)
        {
            TotalNumberOfPages = totalNumberOfPages;
            Archetypes = archetypes ?? throw new ArgumentNullException(nameof(archetypes));
        }

        public int TotalNumberOfPages { get; set; }

        public IEnumerable<ArchetypeWithHowManyWinsAndHowManyWasUsed> Archetypes { get; set; }
    }
}
