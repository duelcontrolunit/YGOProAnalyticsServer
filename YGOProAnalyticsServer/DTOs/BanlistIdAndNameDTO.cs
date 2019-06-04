using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistIdAndNameDTO
    {
        public BanlistIdAndNameDTO(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
