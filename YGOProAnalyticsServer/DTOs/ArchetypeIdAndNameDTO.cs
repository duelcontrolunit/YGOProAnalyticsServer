using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class ArchetypeIdAndNameDTO
    {
        public ArchetypeIdAndNameDTO(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
