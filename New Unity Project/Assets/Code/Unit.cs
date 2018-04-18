using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Persistance;
using TankGame.Messaging;

namespace TankGame
{
    public abstract class Unit: MonoBehaviour, IDamageReceiver
    {

        #region Statics

        private static int s_idCounter = 0;

        public static int GetNextId()
        {
            var allUnits = FindObjectsOfType<Unit>();
            foreach(var unit in allUnits)
            {
                if(unit.Id >= s_idCounter)
                {
                    s_idCounter = unit.Id + 1;
                }
            }
            return s_idCounter++;
        }

        #endregion Statics

        protected IMover _mover;

        [SerializeField, HideInInspector]
        private int _id = -1;

        [SerializeField]
        public float moveSpeed, turnSpeed;

        [SerializeField]
        private int _startingHealth;
        private Vector3 _spawnPosition;

        public Weapon Weapon
        {
            get;
            protected set;
        }

        public IMover Mover
        {
            get { return _mover; }
        }

        public int Id {
            get { return _id; }
            private set { _id = value; }
        }

        public Health Health { get; protected set; }

        protected void OnDestroy()
        {
            Health.UnitDied -= HandleUnitDied;
        }

        /// <summary>
        /// After unit respawns place it to the position it was in the start of the game and set it health to starting health.
        /// </summary>
        public void Respawn()
        {
            Health.SetHealth(_startingHealth);
            transform.position = _spawnPosition;
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
            _spawnPosition = transform.position;
        }

        public virtual void Clear()
        {

        }

        public void RequestId()
        {
            if(Id < 0)
            {
                Id = GetNextId();
            }
        }

        protected abstract void Update();

        public void TakeDamage( int amount )
        {
            Health.TakeDamage(amount);
        }

        protected virtual void HandleUnitDied(Unit unit)
        {
            GameManager.Instance.MessageBus.Publish( new UnitDiedMessage(this));
            gameObject.SetActive(false);
        }

        public virtual UnitData GetUnitData ()
        {
            return new UnitData
            {
                Health = Health.CurrentHealth,
                Position = transform.position,
                YRotation = transform.eulerAngles.y,
                Id = Id
            };
        }

        public virtual void SetUnitData(UnitData data)
        {
            Health.SetHealth(data.Health);
            transform.position = data.Position;
            transform.eulerAngles = new Vector3(0, data.YRotation, 0);

        }
    }
}
