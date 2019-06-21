using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class ArchetypeWithHowManyWinsAndHowManyWasUsed
    {
        public ArchetypeWithHowManyWinsAndHowManyWasUsed(
            int id, 
            string name, 
            int howManyWon, 
            int howManyWasUsed)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HowManyWon = howManyWon;
            HowManyWasUsed = howManyWasUsed;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HowManyWon { get; set; }
        public int HowManyWasUsed { get; set; }
    }
}
