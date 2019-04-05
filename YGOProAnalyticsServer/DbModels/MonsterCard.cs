using System.ComponentModel.DataAnnotations;
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
        /// <summary>
        /// Initialize monster card.
        /// </summary>
        protected MonsterCard(string attack, string defence, int levelOrRank, string attribute)
        {
            Attack = attack;
            Defence = defence;
            LevelOrRank = levelOrRank;
            Attribute = attribute;
        }

        /// <summary>
        /// Initialize monster card.
        /// </summary>
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
        /// Identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Attack of the monster.
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
        /// Monster attribute.
        /// </summary>
        [Required]
        public string Attribute { get; protected set; }

        /// <summary>
        /// If card is not pendulum monster should be null.
        /// </summary>
        public PendulumMonsterCard PendulumMonsterCard { get; set; }

        /// <summary>
        /// If card is not link monster should be null.
        /// </summary>
        public LinkMonsterCard LinkMonsterCard { get; set; }

        /// <summary>
        /// Card identifier.
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// Navigation for card. 
        /// </summary>
        public Card Card { get; set; }
    }
} 
