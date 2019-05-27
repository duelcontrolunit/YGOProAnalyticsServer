using System.Collections.Generic;

namespace YGOProAnalyticsServer.DTOs.Interfaces
{
    public interface ISpellCardsContainer
    {
        List<CardDTO> SpellCards { get; set; }
    }
}
