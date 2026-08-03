using Microsoft.AspNetCore.Components;

namespace HortiBts.Client.MultiLanguage
{
    public class LanguageService
    {
        private bool _isHindi = true;

        public event Action? OnLanguageChanged;

        public bool IsHindi => _isHindi;

        public void Toggle()
        {
            _isHindi = !_isHindi;
            OnLanguageChanged?.Invoke();
        }

        public void SetLanguage(bool isHindi)
        {
            _isHindi = isHindi;
            OnLanguageChanged?.Invoke();

        }
        public string Text(string english, string hindi)
        {
            return _isHindi ? hindi : english;
        }
    }
}
