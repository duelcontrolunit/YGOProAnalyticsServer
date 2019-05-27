using System.Collections.Generic;

namespace YGOProAnalyticsServer.DTOs.Interfaces
{
    public interface ITrapCardsContainer
    {
        List<CardDTO> TrapCards { get; set; }
    }
}
