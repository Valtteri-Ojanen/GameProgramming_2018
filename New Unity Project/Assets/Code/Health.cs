using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TankGame
{
    public class Health
    {
        public event Action<Unit> UnitDied;

        public int CurrentHealth { get; private set; }
        public Unit Owner { get; private set; }

        public Health(Unit owner, int startingHealth)
        {
            Owner = owner;
            CurrentHealth = startingHealth;
        }

        public bool TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, CurrentHealth);
            bool didDie = CurrentHealth == 0;
            if(didDie && UnitDied != null)
            {
                UnitDied(Owner);
            }
            return didDie;
        }
    }
}
