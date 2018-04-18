using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using L10N = TankGame.Localization.Localization;

namespace TankGame
{
    public class Health
    {
        private int _currentHealth;

        public event Action<Unit> UnitDied;
        public event Action<Unit, int> HealthChanged;

        public int CurrentHealth {
            get { return _currentHealth; }
            private set {
                _currentHealth = value;
                if(HealthChanged != null)
                {
                    HealthChanged(Owner, _currentHealth);
                }
            }
        }
        public Unit Owner { get; private set; }

        public Health(Unit owner, int startingHealth)
        {
            Owner = owner;
            CurrentHealth = startingHealth;
            L10N.LanguageLoaded += LanguageChanged;
        }

        private void OnDestroy()
        {
            L10N.LanguageLoaded -= LanguageChanged;
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

        public void SetHealth(int health)
        {
            CurrentHealth = health;
        }

        private void LanguageChanged ()
        {
            if(HealthChanged != null)
            {
                HealthChanged(Owner, _currentHealth);
            }
        }
    }
}
