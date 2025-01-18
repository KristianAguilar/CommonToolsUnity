using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(fileName = "Eng_Lang", menuName = "Config/LanguageConfig", order = 1)]
    public class LanguageConfig : ScriptableObject
    {
        public Word[] words;
        public SystemLanguage language;
    }

    [System.Serializable]
    public class Word
    {
        public string key;
        public string value;
    }
}
