using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Messaging
{
    public class UnitGotHitMessage: IMessage
    {
        public Unit DamagedUnit
        {
            get;
            private set;
        }

        public UnitGotHitMessage( Unit unit)
        {
            DamagedUnit = unit;
        }
    }
}
