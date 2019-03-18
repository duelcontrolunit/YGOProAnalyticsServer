namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Pendulum monster Card.
    /// </summary>
    public class PendulumMonsterCard
    {
        /// <summary>
        /// Create pendulum monster card.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <param name="monsterCard"><see cref="DbModels.MonsterCard"/>.</param>
        /// <returns>New instance of <see cref="PendulumMonsterCard"/>.</returns>
        public static PendulumMonsterCard Create(
            int scale,
            MonsterCard monsterCard)
        {
            return new PendulumMonsterCard(scale)
            {
                MonsterCard = monsterCard
            };
        }

        /// <summary>
        /// Create pendulum monster card.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        protected PendulumMonsterCard(int scale)
        {
            Scale = scale;
        }

        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Scale value. Only pendulum monsters should have it.
        /// </summary>
        public int Scale { get; protected set; }

        /// <summary>
        /// Associated monster card id.
        /// </summary>
        public int MonsterCardId { get; set; }

        /// <summary>
        /// Associated monster card.
        /// </summary>
        public MonsterCard MonsterCard { get; set; }
    }
}
