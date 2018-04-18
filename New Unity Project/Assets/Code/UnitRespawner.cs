using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Messaging;

namespace TankGame
{
    public class UnitRespawner: MonoBehaviour
    {
        [SerializeField]
        private float _enemyRespawnTime;
        [SerializeField]
        private float _playerRespawnTime;

        private ISubscription<UnitDiedMessage> _unitDiedSubscription;

        private List<Unit> _deadUnits;

        private List<float> _deadUnitTimers;

        // Use this for initialization
        void Start()
        {
            _unitDiedSubscription = GameManager.Instance.MessageBus.Subscribe<UnitDiedMessage>(OnUnitDied);
            _deadUnits = new List<Unit>();
            _deadUnitTimers = new List<float>();
        }
        
        /// <summary>
        /// Deadunits and deadUnitTimers both belongs to same unit, Deadunits contains reference to the Unit component
        /// and deadUnitTimers contains the remaining time before the unit can be respawned. 
        /// </summary>
        void Update()
        {
            for(int i = _deadUnits.Count - 1; i >= 0; i--)
            {
                _deadUnitTimers[i] -= Time.deltaTime;
                if(_deadUnitTimers[i] <= 0)
                {
                    _deadUnits[i].gameObject.SetActive(true);
                    _deadUnits[i].Respawn();
                    // sends a message to healthUIitems that this unit has been respawned.
                    GameManager.Instance.MessageBus.Publish(new UnitRespawnedMessage(_deadUnits[i]));
                    _deadUnits.RemoveAt(i);
                    _deadUnitTimers.RemoveAt(i);
                } else
                {
                    // sends a message to healthUIitems with information about this unit and how long till the respawn.
                    GameManager.Instance.MessageBus.Publish(new UnitRespawnTimerMessage(_deadUnits[i],_deadUnitTimers[i]));
                }
            }
        }

        protected void OnDestroy()
        {
            UnregisterEventListeners();
        }

        /// <summary>
        /// Adds the unit contained in the message to deadUnits and adds respawn time to deadUnitTimers
        /// the respawn time depends if the unit that died was player or enemy
        /// </summary>
        /// <param name="msg"></param>
        private void OnUnitDied( UnitDiedMessage msg )
        {
            if(msg.DeadUnit is PlayerUnit)
            {
                _deadUnits.Add(msg.DeadUnit);
                _deadUnitTimers.Add(_playerRespawnTime);
            } else
            {
                _deadUnits.Add(msg.DeadUnit);
                _deadUnitTimers.Add(_enemyRespawnTime);
            }
        }

        private void UnregisterEventListeners()
        {
            GameManager.Instance.MessageBus.UnSubscribe(_unitDiedSubscription);
        }
    }
}
