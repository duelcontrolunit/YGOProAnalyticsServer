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

        /// <summary>
        /// Gets or sets the last duel log date analyzed.
        /// </summary>
        /// <value>
        /// The last duel log date analyzed.
        /// </value>
        public DateTime LastDuelLogDateAnalyzed { get; set; } = new DateTime();
        /// <summary>
        /// Gets or sets the last decklists pack date.
        /// </summary>
        /// <value>
        /// The last decklists pack date.
        /// </value>
        public DateTime LastDecklistsPackDate { get; set; } = new DateTime();
    }
}
