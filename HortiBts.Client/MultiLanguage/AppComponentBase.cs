using Microsoft.AspNetCore.Components;

namespace HortiBts.Client.MultiLanguage
{
    public class AppComponentBase : ComponentBase, IDisposable
    {
        [Inject] protected LanguageService Lang { get; set; } = default!;

        protected override void OnInitialized()
        {
            Lang.OnLanguageChanged += StateHasChanged;
        }

        public void Dispose()
        {
            Lang.OnLanguageChanged -= StateHasChanged;
        }
    }
}