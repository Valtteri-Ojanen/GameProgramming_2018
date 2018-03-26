using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.UI
{
    public class LocalizedLabel: MonoBehaviour
    {

        [SerializeField]
        private Text _text;
        [SerializeField]
        private string _key;

        private void Awake()
        {
            Localization.Localization.LanguageLoaded += OnLanguageLoaded;
        }

        private void Start()
        {
            OnLanguageLoaded();
        }

        private void OnLanguageLoaded()
        {
            _text.text =
                Localization.Localization.CurrentLanguage.GetTranslation(_key);
        }
    }
}
