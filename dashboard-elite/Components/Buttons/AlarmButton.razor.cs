using System;
using System.Threading;
using dashboard_elite.EliteData;
using dashboard_elite.JsInterop;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Components.Buttons
{
    public partial class AlarmButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); // normal

        private string SecondaryIcon => SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon); // alarm

        private CachedSound _clickSound = null;

        private MudTripleIconButtonState AlarmState
        {
            get
            {
                var isAlarm = false;

                if (UnderAttack && (DateTime.Now - Data.LastUnderAttackEvent).Seconds > 20)
                {
                    UnderAttack = false;
                }

                switch (ButtonData.Function.ToLower())
                {
                    case "selecthighestthreat":
                        isAlarm = UnderAttack;
                        break;
                    case "deploychaff":
                        isAlarm = UnderAttack;
                        break;

                    case "deployheatsink":
                        isAlarm = Data.StatusData.Overheating;
                        break;

                    case "deployshieldcell":
                        isAlarm = !Data.StatusData.ShieldsUp;
                        break;
                }

                return isAlarm ? MudTripleIconButtonState.Secondary : MudTripleIconButtonState.Primary;
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


        private void ButtonClick()
        {

            InteropMouse.JsMouseUp();

            Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            Thread.Sleep(100);

            switch (ButtonData.Function.ToLower())
            {
                case "selecthighestthreat":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SelectHighestThreat);
                    UnderAttack = false;
                    break;
                case "deploychaff":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FireChaffLauncher);
                    UnderAttack = false;
                    break;

                case "deployheatsink":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].DeployHeatSink);
                    break;
                case "deployshieldcell":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UseShieldCell);
                    break;
            }
        }


    }
}
