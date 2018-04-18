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
            Localization.Localization.LanguageLoaded += SetText;
        }

        private void Start()
        {
            SetText();
        }

        private void SetText()
        {
            _text.text =
                Localization.Localization.CurrentLanguage.GetTranslation(_key);
        }

        private void OnDestroy()
        {
            Localization.Localization.LanguageLoaded -= SetText;
        }
    }
}
