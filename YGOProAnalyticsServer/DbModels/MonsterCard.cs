using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    public class MonsterCard : Card
    {

        public string Attack { get; set; }

        public string Defence { get; set; }

        /// <summary>
        /// Only for XYZ monsters it is rank.
        /// For links it is always equal 0.
        /// </summary>
        public int LevelOrRank { get; set; }
        public string Scale { get; set; }
        public string LinkValue { get; set; }
        public string LinkMarkers { get; set; }
    }
    /**
     * 1. 
     */
} 
