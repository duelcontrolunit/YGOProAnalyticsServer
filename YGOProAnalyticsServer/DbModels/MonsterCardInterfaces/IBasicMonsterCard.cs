namespace YGOProAnalyticsServer.DbModels.MonsterCardInterfaces
{
    /// <summary>
    /// Provide methods for initialize all basic monster properties.
    /// </summary>
    public interface IBasicMonsterCard
    {
        /// <summary>
        /// Set up all basic properties for monster cards.
        /// </summary>
        /// <param name="passCode">Card identifier provided by api.</param>
        /// <param name="name">Name of the card.</param>
        /// <param name="description">Card description.</param>
        /// <param name="type">For example: normal monster, spell card.</param>
        /// <param name="race">For example: Spellcaster, Warrior.</param>
        /// <param name="imageUrl">Image URL.</param>
        /// <param name="smallImageUrl">Small image URL.</param>
        /// <param name="attack">Attack of the monster.</param>
        /// <param name="defence">Defence of the monster.</param>
        /// <param name="levelOrRank">Level or Ran of the monster (rank only for type which contain "XYZ").</param>
        /// <param name="attribute">Dark, Wind, Light, Water, Fire or Earth.</param>
        /// <param name="archetype">Archetype of the monster.</param>
        void SetOrUpdateMonsterCardBasicProperties(
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
            string attribute,
            Archetype archetype);
    }
}
