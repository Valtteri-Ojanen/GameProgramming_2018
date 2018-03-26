using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace TankGame.Localization
{
    public class LocalizationNotFoundException: FileNotFoundException
    {
        public LangCode Language { get;private set; }

        public LocalizationNotFoundException(LangCode language)
        {
            Language = language;
        }

        public override string Message
        {
            get
            {
                return "Localization Cannot be found for language" + Language;
            }
        }

    }
}
