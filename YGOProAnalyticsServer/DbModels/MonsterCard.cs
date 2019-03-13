using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels.MonsterCardInterfaces;
using YGOProAnalyticsServer.Services.Builders;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// YuGiOh! card. Can be used to store and manage monster card.
    /// <para>If you want store or manage trap/spell card use <see cref="Card"/></para> 
    /// <para>If you want to create an instance, use <see cref="CardBuilder"/></para>
    /// </summary>
    public class MonsterCard
    {
        public MonsterCard(string attack, string defence, int levelOrRank, string attribute)
        {
            Attack = attack;
            Defence = defence;
            LevelOrRank = levelOrRank;
            Attribute = attribute;
        }

        public static MonsterCard Create(
            string attack,
            string defence,
            int levelOrRank,
            string attribute,
            Card card
            )
        {
            return new MonsterCard(
                attack,
                defence,
                levelOrRank,
                attribute)
            {
                Card = card
            };
        }

        /// <summary>
        /// Initialize monster card
        /// </summary>

        public int Id { get; protected set; }
        /// <summary>
        /// Attack of the monster
        /// </summary>
        [Required]
        public string Attack { get; protected set; }

        /// <summary>
        /// Defence of the monster. Ignore that property for link monsters.
        /// </summary>
        public string Defence { get; protected set; }

        /// <summary>
        /// Only for XYZ monsters it is rank.
        /// For links it is always equal 0.
        /// </summary>
        public int LevelOrRank { get; protected set; }

        /// <summary>
        /// Monster attribute
        /// </summary>
        [Required]
        public string Attribute { get; protected set; }

        public int? PendulumMonsterCardId { get; set; }

        public PendulumMonsterCard PendulumMonsterCard { get; set; }
        public int? LinkMonsterCardId { get; set; }
        public LinkMonsterCard LinkMonsterCard { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }

        ///// <summary>
        ///// This should be called only by <see cref="MonsterCardBuilder"/>
        ///// </summary>
        ///// <returns>Returns empty monster card. DO NOT CALL THIS METHOD</returns>
        //public static MonsterCard GetInstanceForBuilder()
        //{
        //    return new MonsterCard(
        //       default(int),
        //       default(string),
        //       default(string),
        //       default(string),
        //       default(string),
        //       default(string),
        //       default(string)
        //    );
        //}

        //    /// <summary>
        //    /// Set up properties for link monster cards.
        //    /// </summary>
        //    /// <param name="linkValue">Link value.</param>
        //    /// <param name="topLeftLinkMarker">Top-Left link marker.</param>
        //    /// <param name="topLinkMarker">Top link marker.</param>
        //    /// <param name="topRightLinkMarker">Top-Right link marker.</param>
        //    /// <param name="middleLeftLinkMarker">Middle-Left link marker.</param>
        //    /// <param name="middleRightLinkMarker">Middle-Right link marker.</param>
        //    /// <param name="bottomLeftLinkMarker">Bottom-Left link marker.</param>
        //    /// <param name="bottomLinkMarker">Bottom link marker.</param>
        //    /// <param name="bottomRightLinkMarker">Bottom-Right link marker.</param>
        //    void ILinkMonster.SetOrUpdateLinkElements(
        //        int linkValue, 
        //        bool topLeftLinkMarker, 
        //        bool topLinkMarker, 
        //        bool topRightLinkMarker, 
        //        bool middleLeftLinkMarker, 
        //        bool middleRightLinkMarker, 
        //        bool bottomLeftLinkMarker, 
        //        bool bottomLinkMarker, 
        //        bool bottomRightLinkMarker)
        //    {
        //        LinkValue = linkValue;
        //        TopLeftLinkMarker = topLeftLinkMarker;
        //        TopLinkMarker = topLinkMarker;
        //        TopRightLinkMarker = topRightLinkMarker;
        //        MiddleLeftLinkMarker = middleLeftLinkMarker;
        //        MiddleRightLinkMarker = middleRightLinkMarker;
        //        BottomLeftLinkMarker = bottomLeftLinkMarker;
        //        BottomLinkMarker = bottomLinkMarker;
        //        BottomRightLinkMarker = bottomRightLinkMarker;
        //    }

        //    /// <summary>
        //    /// Set up all basic properties for monster cards.
        //    /// </summary>
        //    /// <param name="passCode">Card identifier provided by api.</param>
        //    /// <param name="name">Name of the card.</param>
        //    /// <param name="description">Card description.</param>
        //    /// <param name="type">For example: normal monster, spell card.</param>
        //    /// <param name="race">For example: Spellcaster, Warrior.</param>
        //    /// <param name="imageUrl">Image URL.</param>
        //    /// <param name="smallImageUrl">Small image URL.</param>
        //    /// <param name="attack">Attack of the monster.</param>
        //    /// <param name="defence">Defence of the monster.</param>
        //    /// <param name="levelOrRank">Level or Ran of the monster (rank only for type which contain "XYZ").</param>
        //    /// <param name="attribute">Dark, Wind, Light, Water, Fire or Earth.</param>
        //    /// <param name="archetype">Archetype of the monster.</param>
        //    void IBasicMonsterCard.SetOrUpdateMonsterCardBasicProperties(
        //        int passCode, 
        //        string name, 
        //        string description, 
        //        string type, 
        //        string race, 
        //        string imageUrl, 
        //        string smallImageUrl, 
        //        string attack, 
        //        string defence, 
        //        int levelOrRank,
        //        string attribute,
        //        Archetype archetype)
        //    {
        //        PassCode = passCode;
        //        Name = name;
        //        Description = description;
        //        Type = type;
        //        Race = race;
        //        ImageUrl = imageUrl;
        //        SmallImageUrl = smallImageUrl;
        //        Attack = attack;
        //        Defence = defence;
        //        LevelOrRank = levelOrRank;
        //        Attribute = attribute;
        //        Archetype = archetype;
        //    }

        //    /// <summary>
        //    /// Set up properties for pendulum cards.
        //    /// </summary>
        //    /// <param name="scale">Scale value.</param>
        //    void IPendulumMonster.SetOrUpdateScale(int scale)
        //    {
        //        Scale = scale;
        //    }
        //}
    }
} 
