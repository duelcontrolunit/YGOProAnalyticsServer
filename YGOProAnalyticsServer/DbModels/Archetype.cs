using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels
{
    /// <summary>
    /// Card archetype.
    /// </summary>
    public class Archetype
    {
        /// <summary>
        ///  Initialize archetype.
        /// </summary>
        /// <param name="id">Archetype identifier.</param>
        /// <param name="name">Name of the archetype.</param>
        /// <param name="isPureArchetype">
        ///     Pure archetype is archetype defined by Konami. 
        ///     Non-pure archetype is archetype created from two pure archatypes.
        /// </param>
        protected Archetype(
            int id, 
            string name, 
            bool isPureArchetype)
        {
            Id = id;
            Name = name;
            IsPureArchetype = isPureArchetype;
        }

        /// <summary>
        ///  Initialize archetype.
        /// </summary>
        /// <param name="name">Name of the archetype.</param>
        /// <param name="isPureArchetype">
        ///     Pure archetype is archetype defined by Konami. 
        ///     Non-pure archetype is archetype created from two pure archatypes.
        /// </param>
        public Archetype(
           string name,
           bool isPureArchetype)
        {
            Name = name;
            IsPureArchetype = isPureArchetype;
        }

        /// <summary>
        /// Archetype identifier.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Archetype name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Cards which belong to this archetype.
        /// </summary>
        public ICollection<Card> Cards { get; protected set; } = new List<Card>();

        /// <summary>
        /// Pure archetype is archetype defined by Konami.
        /// Non-pure archetype is archetype created from two pure archatypes.
        /// </summary>
        public bool IsPureArchetype { get; protected set; }

        /// <summary>
        /// Collection of statistics of this archetype.
        /// </summary>
        public ICollection<ArchetypeStatistics> Statistics { get; protected set; } = new List<ArchetypeStatistics>();
    }
}
