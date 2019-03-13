using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    public class LinkMonsterCard
    {
        public static LinkMonsterCard Create(int linkValue,
            bool topLeftLinkMarker,
            bool topLinkMarker,
            bool topRightLinkMarker,
            bool middleLeftLinkMarker,
            bool middleRightLinkMarker,
            bool bottomLeftLinkMarker,
            bool bottomLinkMarker,
            bool bottomRightLinkMarker,
            MonsterCard monsterCard)
        {
            return new LinkMonsterCard(
                linkValue,
                topLeftLinkMarker,
                topLinkMarker,
                topRightLinkMarker,
                middleLeftLinkMarker,
                middleRightLinkMarker,
                bottomLeftLinkMarker,
                bottomLinkMarker,
                bottomRightLinkMarker)
            {
                MonsterCard = monsterCard
            };
        }
        protected LinkMonsterCard(
            int linkValue,
            bool topLeftLinkMarker, 
            bool topLinkMarker, 
            bool topRightLinkMarker, 
            bool middleLeftLinkMarker, 
            bool middleRightLinkMarker,
            bool bottomLeftLinkMarker, 
            bool bottomLinkMarker, 
            bool bottomRightLinkMarker
        )
        {
            LinkValue = linkValue;
            TopLeftLinkMarker = topLeftLinkMarker;
            TopLinkMarker = topLinkMarker;
            TopRightLinkMarker = topRightLinkMarker;
            MiddleLeftLinkMarker = middleLeftLinkMarker;
            MiddleRightLinkMarker = middleRightLinkMarker;
            BottomLeftLinkMarker = bottomLeftLinkMarker;
            BottomLinkMarker = bottomLinkMarker;
            BottomRightLinkMarker = bottomRightLinkMarker;
        }
        public int Id { get; protected set; }

        /// <summary>
        /// Link value. Only link monsters should have it.
        /// </summary>
        public int LinkValue { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool TopLeftLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool TopLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool TopRightLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool MiddleLeftLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool MiddleRightLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool BottomLeftLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool BottomLinkMarker { get; protected set; }

        /// <summary>
        /// Only link monsters should have it.
        /// </summary>
        public bool BottomRightLinkMarker { get; protected set; }

        public int MonsterCardId { get; set; }
        public MonsterCard MonsterCard { get; set; }
    }
}
