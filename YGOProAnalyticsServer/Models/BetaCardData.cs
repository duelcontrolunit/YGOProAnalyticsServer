using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Models
{
    /// <summary>
    /// BetaCardData for BetaCardData API
    /// </summary>
    public class BetaCardData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetaCardData"/> class.
        /// </summary>
        /// <param name="nameOfCard">The name of card.</param>
        /// <param name="officialPassCode">The official pass code.</param>
        /// <param name="betaPassCode">The beta pass code.</param>
        public BetaCardData(string nameOfCard, int officialPassCode, int betaPassCode)
        {
            Name = nameOfCard;
            this.OfficialPassCode = officialPassCode;
            this.BetaPassCode = betaPassCode;
        }
        /// <summary>
        /// Gets the name of the card.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the official pass code.
        /// </summary>
        /// <value>
        /// The official pass code.
        /// </value>
        public int OfficialPassCode { get; private set; }
        /// <summary>
        /// Gets the beta pass code.
        /// </summary>
        /// <value>
        /// The beta pass code.
        /// </value>
        public int BetaPassCode { get; private set; }
    }
}
