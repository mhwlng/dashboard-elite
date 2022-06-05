using System;
using System.Threading;
using dashboard_elite.Audio;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using dashboard_elite.JsInterop;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Components.Buttons
{
    public partial class AlarmButton
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

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
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (focusChange)
            {
                InteropMouse.JsMouseUp();

                Thread.Sleep(100);
            }

            Common.PlaySound(ref _clickSound, ButtonData.ClickSound);

            switch (ButtonData.Function.ToLower())
            {
                case "selecthighestthreat":
                    CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].SelectHighestThreat, focusChange);
                    UnderAttack = false;
                    break;
                case "deploychaff":
                    CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].FireChaffLauncher, focusChange);
                    UnderAttack = false;
                    break;

                case "deployheatsink":
                    CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].DeployHeatSink, focusChange);
                    break;
                case "deployshieldcell":
                    CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].UseShieldCell, focusChange);
                    break;
            }
        }


    }
}
