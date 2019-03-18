using System.ComponentModel.DataAnnotations;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Link monster card.
    /// </summary>
    public class LinkMonsterCard
    {
        /// <summary>
        /// Create link monster card.
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
        /// <param name="monsterCard"><see cref="DbModels.MonsterCard"/>.</param>
        /// <returns>New instence of <see cref="LinkMonsterCard"/>.</returns>
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

        /// <summary>
        /// Create link monster card.
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

        /// <summary>
        /// Identifier.
        /// </summary>
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

        /// <summary>
        /// Monster Card Id.
        /// </summary>
        public int MonsterCardId { get; set; }

        /// <summary>
        /// Navigation to associated <see cref="DbModels.MonsterCard"/>.
        /// </summary>
        [Required]
        public MonsterCard MonsterCard { get; set; }
    }
}
