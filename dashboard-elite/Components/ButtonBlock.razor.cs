using System;
using System.Threading;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;

namespace dashboard_elite.Components
{
    public partial class ButtonBlock
    {
        [Inject] private ButtonCacheService ButtonCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

    }
}
