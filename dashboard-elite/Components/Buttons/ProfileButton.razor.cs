using System.Collections.Generic;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Components.Buttons
{
    public partial class ProfileButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public KeyValuePair<string,ProfileData> ProfileData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ProfileData.Value.PrimaryIcon); // Off

        private string SecondaryIcon => SvgCacheService.ButtonIcon(ProfileData.Value.SecondaryIcon); // On

        private CachedSound _clickSound = null;

        private string _activeButtonBlock;

        [Parameter]
        public string ActiveButtonBlock
        {
            get => _activeButtonBlock;
            set
            {
                if (_activeButtonBlock == value) return;
                _activeButtonBlock = value;
                ActiveButtonBlockChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> ActiveButtonBlockChanged { get; set; }


        private MudTripleIconButtonState ProfileState => ActiveButtonBlock == ProfileData.Key ? MudTripleIconButtonState.Secondary : MudTripleIconButtonState.Primary;

        private void ButtonClick()
        {
            Program.PlaySound(ref _clickSound, ProfileData.Value.ClickSound);

            ActiveButtonBlock = ProfileData.Key;
        }


    }
}
