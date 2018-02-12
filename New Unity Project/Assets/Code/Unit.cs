using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public abstract class Unit: MonoBehaviour, IDamageReceiver
    {
        protected IMover _mover;

        [SerializeField]
        public float moveSpeed, turnSpeed;

        [SerializeField]
        private int _startingHealth;

        public Weapon Weapon
        {
            get;
            protected set;
        }

        public IMover Mover
        {
            get { return _mover; }
        }

        public Health Health { get; protected set; }

        protected void Awake()
        {
            Init();
        }

        protected void OnDestroy()
        {
            Health.UnitDied -= HandleUnitDied;
        }

        public virtual void Init()
        {
            _mover = gameObject.GetOrAddComponent<TransformMover>();

            Mover.Init(moveSpeed, turnSpeed);

            Weapon = GetComponentInChildren<Weapon>();
            if( Weapon != null)
            {
                Weapon.Init(this);
            }

            Health = new Health(this, _startingHealth);
            Health.UnitDied += HandleUnitDied;
        }

        public virtual void Clear()
        {

        }

        protected abstract void Update();

        public void TakeDamage( int amount )
        {
            Health.TakeDamage(amount);
        }

        protected virtual void HandleUnitDied(Unit unit)
        {
            gameObject.SetActive(false);
        }
    }
}
