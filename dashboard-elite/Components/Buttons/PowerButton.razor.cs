using System;
using System.Threading;
using dashboard_elite.EliteData;
using dashboard_elite.JsInterop;
using dashboard_elite.Services;
using Elite;
using EliteJournalReader.Events;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.Logging;
using MudBlazor;
using Serilog;

namespace dashboard_elite.Components.Buttons
{
    public partial class PowerButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private int GetPip()
        {
            switch (ButtonData.Function.ToLower())
            {
                case "sys":
                    return Pips[0];
                case "eng":
                    return Pips[1];
                case "wep":
                    return Pips[2];
            }

            return 0;
        }

        private string AdjustSVG(string svg)
        {
            var pip = GetPip();

            switch (pip)
            {
                case 0:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "display:none;");
                    svg = svg.Replace("{5}", "display:none;");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 1:
                    svg = svg.Replace("{0}", "");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "display:none;");
                    svg = svg.Replace("{5}", "display:none;");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 2:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "display:none;");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 3:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "display:none;");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 4:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 5:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "");
                    svg = svg.Replace("{6}", "display:none;");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 6:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "");
                    svg = svg.Replace("{6}", "");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 7:
                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "");
                    svg = svg.Replace("{6}", "");
                    svg = svg.Replace("{7}", "display:none;");
                    break;
                case 8:

                    svg = svg.Replace("{0}", "display:none;");
                    svg = svg.Replace("{1}", "display:none;");
                    svg = svg.Replace("{2}", "display:none;");
                    svg = svg.Replace("{3}", "display:none;");

                    svg = svg.Replace("{4}", "");
                    svg = svg.Replace("{5}", "");
                    svg = svg.Replace("{6}", "");
                    svg = svg.Replace("{7}", "");
                    break;
            }

            return svg;
        }

        private string PrimaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon);
                return AdjustSVG(svg);
            }
        }

        private string SecondaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon);

                return AdjustSVG(svg); // 
            }
        }

        private string TertiaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.TertiaryIcon);

                return AdjustSVG(svg); // 
            }
        }

        private CachedSound _clickSound = null;
        

        private MudTripleIconButtonState PowerState
        {
            get
            {
                if (UnderAttack && (DateTime.Now - Data.LastUnderAttackEvent).Seconds > 20)
                {
                    UnderAttack = false;
                }

                var pip = GetPip();

                var state = MudTripleIconButtonState.Primary;

                if (pip == 8)
                {
                    state = MudTripleIconButtonState.Secondary;
                }

                if (ButtonData.Function.ToLower() == "sys" && pip < 8 && UnderAttack)
                {
                    state = MudTripleIconButtonState.Tertiary;
                }

                return state;
            }
        }


        private void AdjustPips(int index)
        {
            if (Pips[index] < 8)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i != index)
                    {
                        Pips[i]--;
                    }
                    else
                    {
                        Pips[i] += 2;
                    }

                    if (Pips[i] < 0)
                    {
                        Pips[i] = 0;
                    }
                    else if (Pips[i] > 8)
                    {
                        Pips[i] = 8;
                    }
                }
            }
        }

        private bool _underAttack;

        [Parameter]
        public bool UnderAttack
        {
            get => _underAttack;
            set
            {
                if (_underAttack == value) return;
                _underAttack = value;
                UnderAttackChanged.InvokeAsync(value);
            }
        }

        [Parameter] public EventCallback<bool> UnderAttackChanged { get; set; }


        private int[] _pips = new int[3];

        [Parameter]
        public int[] Pips
        {
            get => _pips;
            set
            {
                if (value == null) return;

                if (value[0] != _pips[0] || value[1] != _pips[1] || value[2] != _pips[2])
                {

                    value.CopyTo(_pips,0);

                    PipsChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<int[]> PipsChanged { get; set; }


        private void ButtonClick()
        {
            InteropMouse.JsMouseUp();

            Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            Thread.Sleep(100);

            switch (ButtonData.Function.ToLower())
            {
                case "sys":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseSystemsPower_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseSystemsPower);
                    //AdjustPips(0);
                    break;
                case "eng":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseEnginesPower_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseEnginesPower);

                    //AdjustPips(1);
                    break;
                case "wep":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseWeaponsPower_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseWeaponsPower);
                    //AdjustPips(2);
                    break;
                case "rst":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ResetPowerDistribution_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ResetPowerDistribution);

                    //Pips[0] = 4;
                    //Pips[1] = 4;
                    //Pips[2] = 4;

                    break;
            }

        }

    }
}
