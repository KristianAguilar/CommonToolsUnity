using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Localization
{
    public class LocalizationService : MonoBehaviour
    {
        #region Singleton
        public static LocalizationService instance { get; private set; }

        private void Awake()
        {
            if (instance != null  && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        [SerializeField] private List<LanguageConfig> languageConfigs;

        private LanguageConfig currentLanguage;

        public bool langSetup => currentLanguage != null;

        public UnityAction OnLanguageChange;

        private void Start()
        {
            SetupSystemLanguge();
        }

        private void SetupSystemLanguge()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Spanish:
                    currentLanguage = GetLanguage(SystemLanguage.Spanish);
                    break;
                default:
                    currentLanguage = GetLanguage(SystemLanguage.English);
                    break;
            }
        }

        private LanguageConfig GetLanguage(SystemLanguage systemLanguage)
        {
            var lang = languageConfigs.Find(l => l.language == systemLanguage);
            if (lang != null) 
            {
                return lang;
            }
            else
            {
                Debug.LogWarning("The request language can't be found, use English words.");
                lang = languageConfigs.Find(l => l.language == SystemLanguage.English);
                return lang;
            }
        }

        public void ChangeLanguage(string stringLang)
        {
            SystemLanguage systemLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), stringLang);
            ChangeLanguage(systemLanguage);
        }

        public void ChangeLanguage(SystemLanguage newLang)
        {
            LanguageConfig requestLang = GetLanguage(newLang);
            if (requestLang == currentLanguage)
                return;

            currentLanguage = requestLang;
            OnLanguageChange?.Invoke();
        }


        /// <summary>
        /// Search and return the localized word for the given key.
        /// </summary>
        /// <param name="key">localization key value</param>
        /// <returns>translated word, return key if can't found it.</returns>
        public string GetWord(string key)
        {
            if (langSetup)
            {
                var word = currentLanguage.words.ToList().Find(w => w.key == key);
                if (word != null)
                {
                    return word.value;
                }
            }
            Debug.LogWarning("Language not setup yet or word not found.");
            return key;
        }

    }

}