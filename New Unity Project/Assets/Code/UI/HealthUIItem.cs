using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TankGame.Messaging;
using L10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
    public class HealthUIItem: MonoBehaviour
    {

        private Unit _unit;

        private Text _text;

        private ISubscription<UnitDiedMessage> _unitDiedSubscription;

        private ISubscription<UnitRespawnedMessage> _unitRespawnedSubscription;

        private ISubscription<UnitRespawnTimerMessage> _unitRespawnTimerSubscription;


        public bool IsEnemy { get { return _unit != null && _unit is EnemyUnit; } }
        
        protected void OnDestroy()
        {
            GameManager.Instance.MessageBus.UnSubscribe(_unitRespawnTimerSubscription);
            GameManager.Instance.MessageBus.UnSubscribe(_unitRespawnedSubscription);
            UnregisterEventListeners();
        }

        public void Init (Unit unit)
        {
            _unit = unit;
            _text = GetComponentInChildren<Text>();
            _text.color = IsEnemy ? Color.red : Color.green;
            _unitRespawnedSubscription = GameManager.Instance.MessageBus.Subscribe<UnitRespawnedMessage>(OnUnitRespawned);
            RegisterEventListeners();
        }

        /// <summary>
        /// Called when unit has died in the scene. unregisters listening events that are used by living unit
        /// starts listening unitrespawn timer so it can be shown in the UI
        /// </summary>
        /// <param name="msg"> contains information about which unit died in the scene </param>
        private void OnUnitDied( UnitDiedMessage msg )
        {
            if(msg.DeadUnit == _unit)
            {
                UnregisterEventListeners();
                _unitRespawnTimerSubscription = GameManager.Instance.MessageBus.Subscribe<UnitRespawnTimerMessage>(UnitRespawnTimer);
            }
        }
        
        private void OnUnitRespawned(UnitRespawnedMessage msg)
        {
            if(msg.DeadUnit == _unit)
            {
                RegisterEventListeners();
            }
        }

        /// <summary>
        /// Called when this items Unit has died and is waiting for respawn.
        /// </summary>
        /// <param name="msg"> contains information about which unit is waiting for respawn,
        /// and how long till it respawns </param>
        private void UnitRespawnTimer(UnitRespawnTimerMessage msg)
        {
            if(msg.DeadUnit == _unit)
            {
                SetRespawnTimerText(msg.RespawnTimer);
            }
        }

        /// <summary>
        /// Unregisters listening all events called by living unit
        /// </summary>
        private void UnregisterEventListeners()
        {
            _unit.Health.HealthChanged -= OnUnitHealthChanged;
            GameManager.Instance.MessageBus.UnSubscribe(_unitDiedSubscription);
        }

        /// <summary>
        /// Registers to listen all events called by living units.
        /// </summary>
        private void RegisterEventListeners()
        {
            GameManager.Instance.MessageBus.UnSubscribe(_unitRespawnTimerSubscription);
            _unit.Health.HealthChanged += OnUnitHealthChanged;
            _unitDiedSubscription = GameManager.Instance.MessageBus.Subscribe<UnitDiedMessage>(OnUnitDied);
            SetHealthText(_unit.Health.CurrentHealth);
        }

        /// <summary>
        /// Called when units health is changed
        /// </summary>
        /// <param name="unit"> Which unit this event is called from </param>
        /// <param name="health"> Current health of the unit </param>
        private void OnUnitHealthChanged( Unit unit, int health )
        {
            SetHealthText(health);
        }

        /// <summary>
        /// Sets health text in the UI for the unit this health UI belongs
        /// </summary>
        /// <param name="health"> current health of the unit </param>
        private void SetHealthText(int health)
        {
            string unitKey = IsEnemy ? "Enemy" : "Player";
            string unitTranslation = L10n.CurrentLanguage.GetTranslation(unitKey);
            _text.text = string.Format("{0} {1} : {2}", unitTranslation, L10n.CurrentLanguage.GetTranslation("Health"), health);
        }

        /// <summary>
        /// Sets respawn timer on the UI if the unit is currently waiting for respawn
        /// </summary>
        /// <param name="timer"> Time remaining for the respawn </param>
        private void SetRespawnTimerText(float timer)
        {
            float rounded = Mathf.Round(timer * 10.0f) / 10.0f;
            string unitKey = IsEnemy ? "Enemy" : "Player";
            string unitTranslation = L10n.CurrentLanguage.GetTranslation(unitKey);
            _text.text = string.Format("{0} {1} : {2}", unitTranslation, L10n.CurrentLanguage.GetTranslation("Respawn"), rounded);
        }
    }
}
