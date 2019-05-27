using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    public class AnalysisMetadata
    {
        public int Id { get; set; }
        public DateTime LastDuelLogDateAnalyzed { get; set; } = new DateTime();
        public DateTime LastDecklistsPackDate { get; set; } = new DateTime();
    }
}
