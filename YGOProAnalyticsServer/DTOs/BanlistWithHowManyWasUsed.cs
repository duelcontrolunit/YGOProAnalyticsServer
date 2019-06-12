using System;

namespace YGOProAnalyticsServer.DTOs
{
    public class BanlistWithHowManyWasUsed
    {
        public BanlistWithHowManyWasUsed(
            int id,
            string name,
            string format,
            DateTime releaseDate,
            int howManyTimeWasUsed)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Format = format ?? throw new ArgumentNullException(nameof(format));
            ReleaseDate = releaseDate;
            HowManyTimeWasUsed = howManyTimeWasUsed;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int HowManyTimeWasUsed { get; set; }
    }
}