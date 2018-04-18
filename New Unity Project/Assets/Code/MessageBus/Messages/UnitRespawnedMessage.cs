using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Messaging
{
    public class UnitRespawnedMessage: IMessage
    {

        public Unit DeadUnit { get; private set; }

        /// <summary>
        /// Message that is used when unit is respawned
        /// </summary>
        /// <param name="unit"> Defines which unit has respawned </param>
        public UnitRespawnedMessage( Unit unit)
        {
            DeadUnit = unit;
        }
    }
}
