using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels.MonsterCardInterfaces
{
    /// <summary>
    /// Provide methods for initialize all monster properties associated with pendulum monsters.
    /// </summary>
    public interface IPendulumMonster
    {
        /// <summary>
        /// Set up properties for pendulum cards.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        void SetOrUpdateScale(int scale);
    }
}
