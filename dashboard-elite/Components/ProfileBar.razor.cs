using System.Threading;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;

namespace dashboard_elite.Components
{
    public partial class ProfileBar
    {
        [Inject] private ProfileCacheService ProfileCacheService { get; set; }

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
    }
}
