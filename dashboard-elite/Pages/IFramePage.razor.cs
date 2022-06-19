using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;



namespace dashboard_elite.Pages
{
    public partial class IFramePage
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private ProtectedSessionStorage ProtectedLocalStorage { get; set; }
 

        public int WindowCount { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                WindowCount = (await ProtectedLocalStorage.GetAsync<int>("WindowCount")).Value;
                if (WindowCount < 2)
                {
                    NavigationManager.NavigateTo("/information/commander");
                }
                else
                {
                    StateHasChanged();

                }


            }

        }


    }
}
