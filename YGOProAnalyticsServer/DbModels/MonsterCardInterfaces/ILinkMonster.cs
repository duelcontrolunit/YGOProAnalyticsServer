using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels.MonsterCardInterfaces
{
    /// <summary>
    /// Provide methods for initialize all monster properties associated with link monsters.
    /// </summary>
    public interface ILinkMonster
    {
        /// <summary>
        /// Set up properties for link monster cards.
        /// </summary>
        /// <param name="linkValue">Link value.</param>
        /// <param name="topLeftLinkMarker">Top-Left link marker.</param>
        /// <param name="topLinkMarker">Top link marker.</param>
        /// <param name="topRightLinkMarker">Top-Right link marker.</param>
        /// <param name="middleLeftLinkMarker">Middle-Left link marker.</param>
        /// <param name="middleRightLinkMarker">Middle-Right link marker.</param>
        /// <param name="bottomLeftLinkMarker">Bottom-Left link marker.</param>
        /// <param name="bottomLinkMarker">Bottom link marker.</param>
        /// <param name="bottomRightLinkMarker">Bottom-Right link marker.</param>
        void SetOrUpdateLinkElements(
            int linkValue,
            bool topLeftLinkMarker, 
            bool topLinkMarker, 
            bool topRightLinkMarker, 
            bool middleLeftLinkMarker, 
            bool middleRightLinkMarker,
            bool bottomLeftLinkMarker,
            bool bottomLinkMarker,
            bool bottomRightLinkMarker);
    }
}
