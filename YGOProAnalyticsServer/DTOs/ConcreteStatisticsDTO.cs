using System;
using System.Collections.Generic;

namespace YGOProAnalyticsServer.DTOs
{
    public class ConcreteArchetypeDTO
    {
        public ConcreteArchetypeDTO(
            int id,
            string name,
            bool isPureArchetype,
            DeckDTO cardsInArchetype,
            IEnumerable<ArchetypeStatisticsDTO> statistics)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsPureArchetype = isPureArchetype;
            CardsInArchetype = cardsInArchetype ?? throw new ArgumentNullException(nameof(cardsInArchetype));
            Statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPureArchetype { get; set; }
        public DeckDTO CardsInArchetype { get; set; }
        public IEnumerable<ArchetypeStatisticsDTO> Statistics { get; set; }
    }
}