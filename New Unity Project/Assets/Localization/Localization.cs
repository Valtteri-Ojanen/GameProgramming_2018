using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace TankGame.Localization
{
    public enum LangCode
    {
        NA = 0,
        EN = 1,
        FI = 2
    }

    public static class Localization
    {
        public const string LocalizationFolderName = "Localization";
        public const string FileExtension = ".json";

        public static Action LanguageLoaded;

        public static string LocalizationPath
        {
            get { return Path.Combine(Application.streamingAssetsPath, LocalizationFolderName); }
        }

        public static Language CurrentLanguage { get; private set; }

        public static string GetLocalizationFilePath(LangCode langCode)
        {
            return Path.Combine(LocalizationPath, langCode.ToString()) + FileExtension;
        }

        public static void SaveCurrentLanguage()
        {
            if(CurrentLanguage.LanguageCode == LangCode.NA || CurrentLanguage == null)
            {
                return;
            }

            if(!Directory.Exists(LocalizationPath))
            {
                Directory.CreateDirectory(LocalizationPath);
            }

            string path = GetLocalizationFilePath(CurrentLanguage.LanguageCode);
            string SerializedLanguage = JsonUtility.ToJson(CurrentLanguage);
            File.WriteAllText(path, SerializedLanguage, Encoding.UTF8);
        }

        public static void LoadLanguage(LangCode langCode)
        {
            var path = GetLocalizationFilePath(langCode);
            if(File.Exists(path))
            {
                string jsonLanguage = File.ReadAllText(path);
                CurrentLanguage = JsonUtility.FromJson<Language>(jsonLanguage);
            }
            else
            {
                //throw new LocalizationNotFoundException(langCode);
                CreateLanguage(langCode);
            }

            if(LanguageLoaded != null)
            {
                LanguageLoaded();
            }
        }

        public static void CreateLanguage (LangCode langCode)
        {
            CurrentLanguage = new Language(langCode);
        }
    }
}
