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


        public bool IsEnemy { get { return _unit != null && _unit is EnemyUnit; } }

        protected void OnDestroy()
        {
            UnregisterEventListeners();
        }

        public void Init (Unit unit)
        {
            _unit = unit;
            _text = GetComponentInChildren<Text>();
            _text.color = IsEnemy ? Color.red : Color.green;
            _unit.Health.HealthChanged += OnUnitHealthChanged;
            //_unit.Health.UnitDied += OnUnitDied;
            _unitDiedSubscription = GameManager.Instance.MessageBus.Subscribe<UnitDiedMessage>(OnUnitDied);
            SetText(_unit.Health.CurrentHealth);
        }

        private void OnUnitDied( UnitDiedMessage msg )
        {
            if(msg.DeadUnit == _unit)
            {
                UnregisterEventListeners();
                gameObject.SetActive(false);
            }
        }

        private void UnregisterEventListeners()
        {
            _unit.Health.HealthChanged -= OnUnitHealthChanged;
            GameManager.Instance.MessageBus.UnSubscribe(_unitDiedSubscription);
            //_unit.Health.UnitDied -= OnUnitDied;
        }

        private void OnUnitHealthChanged( Unit unit, int health )
        {
            SetText(health);
        }

        private void SetText(int health)
        {
            string unitKey = IsEnemy ? "Enemy" : "Player";
            string unitTranslation = L10n.CurrentLanguage.GetTranslation(unitKey);
            _text.text = string.Format("{0} {1} : {2}", unitTranslation, Localization.Localization.CurrentLanguage.GetTranslation("Health"), health);
        }
    }
}
