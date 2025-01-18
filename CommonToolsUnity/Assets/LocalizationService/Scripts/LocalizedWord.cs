using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedWord : MonoBehaviour
    {
        [SerializeField] private string key;

        private TMP_Text tmpText;

        private bool wordSetup = false;

        private void Awake()
        {
            tmpText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (!wordSetup)
            {
                if (LocalizationService.instance != null && LocalizationService.instance.langSetup)
                {
                    SetupWord();
                    wordSetup = true;
                    LocalizationService.instance.OnLanguageChange += SetupWord;
                }
            }
        }

        private void SetupWord()
        {
            string value = LocalizationService.instance.GetWord(key);
            if (value != null)
                tmpText.text = value;
        }
    }
}
