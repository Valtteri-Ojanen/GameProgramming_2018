using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.UI
{
    public class HealthUI: MonoBehaviour
    {
        [SerializeField]
        private HealthUIItem _healthUIItemPrefab;

        public void Init ()
        {
            Debug.Log("Health UI Initialized");
        }

        public void AddUnit(Unit unit)
        {
            var healthitem = Instantiate(_healthUIItemPrefab, transform);
            healthitem.Init(unit);
            healthitem.gameObject.SetActive(true);
        }
    }
}
