using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Messaging
{
    public class UnitRespawnTimerMessage: IMessage
    {

        public Unit DeadUnit { get; private set; }

        public float RespawnTimer { get; private set; }

        /// <summary>
        /// Message that is used when unit is currently waiting for respawn
        /// to have the timer shown in the UI
        /// </summary>
        /// <param name="unit"> defines which unit is waiting for respawn </param>
        /// <param name="respawnTimer"> defines the time how long till the unit respawns </param>
        public UnitRespawnTimerMessage( Unit unit, float respawnTimer )
        {
            DeadUnit = unit;
            RespawnTimer = respawnTimer;
        }
    }
}
