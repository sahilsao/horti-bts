using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HortiBts.Client.MultiLanguage
{
    public class AppComponentBase : ComponentBase, IDisposable
    {
        [Inject] protected LanguageService Lang { get; set; } = default!;

        protected MudForm? Form { get; set; }

        protected override void OnInitialized()
        {
            Lang.OnLanguageChanged += HandleLanguageChanged;
        }

        private async void HandleLanguageChanged()
        {
            if (Form is not null)
                await Form.ValidateAsync();

            StateHasChanged();
        }

        public void Dispose()
        {
            Lang.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}