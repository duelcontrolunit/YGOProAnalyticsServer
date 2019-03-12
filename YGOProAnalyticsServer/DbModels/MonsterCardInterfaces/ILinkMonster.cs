using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels.MonsterCardInterfaces
{
    public interface ILinkMonster
    {
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
