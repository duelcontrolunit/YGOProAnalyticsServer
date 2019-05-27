using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class LinkMonsterCardDTO : MonsterCardDTO
    {
        public int LinkValue { get; set; }
        public bool TopLeftLinkMarker { get; set; }
        public bool TopLinkMarker { get; set; }
        public bool TopRightLinkMarker { get; set; }
        public bool MiddleLeftLinkMarker { get; set; }
        public bool MiddleRightLinkMarker { get; set; }
        public bool BottomLeftLinkMarker { get; set; }
        public bool BottomLinkMarker { get; set; }
        public bool BottomRightLinkMarker { get; set; }
    }
}
