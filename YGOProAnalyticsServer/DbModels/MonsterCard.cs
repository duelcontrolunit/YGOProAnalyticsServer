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
    /// <para>If you want to create an instance, use <see cref="MonsterCardBuilder"/></para>
    /// </summary>
    public class MonsterCard : Card, IPendulumMonster, ILinkMonster, IBasicMonsterCard
    {
        /// <summary>
        /// Initialize monster card
        /// </summary>
        protected MonsterCard(
            int passCode, 
            string name, 
            string description, 
            string type, 
            string race, 
            string imageUrl, 
            string smallImageUrl) : base(passCode, name, description, type, race, imageUrl, smallImageUrl)
        {
        }

        /// <summary>
        /// Attack of the monster
        /// </summary>
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
        /// Scale value. Only pendulum monsters should have it.
        /// </summary>
        public int Scale { get; protected set; }

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
        /// This should be called only by <see cref="MonsterCardBuilder"/>
        /// </summary>
        /// <returns>Returns empty monster card. DO NOT CALL THIS METHOD</returns>
        public static MonsterCard GetInstanceForBuilder()
        {
            return new MonsterCard(
               default(int),
               default(string),
               default(string),
               default(string),
               default(string),
               default(string),
               default(string)
            );
        }

        void ILinkMonster.SetOrUpdateLinkElements(
            int linkValue, 
            bool topLeftLinkMarker, 
            bool topLinkMarker, 
            bool topRightLinkMarker, 
            bool middleLeftLinkMarker, 
            bool middleRightLinkMarker, 
            bool bottomLeftLinkMarker, 
            bool bottomLinkMarker, 
            bool bottomRightLinkMarker)
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

        void IBasicMonsterCard.SetOrUpdateMonsterCardBasicProperties(
            int passCode, 
            string name, 
            string description, 
            string type, 
            string race, 
            string imageUrl, 
            string smallImageUrl, 
            string attack, 
            string defence, 
            int levelOrRank, 
            Archetype archetype)
        {
            PassCode = passCode;
            Name = name;
            Description = description;
            Type = type;
            Race = race;
            ImageUrl = imageUrl;
            SmallImageUrl = smallImageUrl;
            Attack = attack;
            Defence = defence;
            LevelOrRank = levelOrRank;
            Archetype = archetype;
        }

        void IPendulumMonster.SetOrUpdateScale(int scale)
        {
            Scale = scale;
        }
    }
} 
