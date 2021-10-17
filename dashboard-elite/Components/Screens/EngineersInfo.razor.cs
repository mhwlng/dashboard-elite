using dashboard_elite.EliteData;
using Microsoft.AspNetCore.Components;

namespace dashboard_elite.Components.Screens
{
    public partial class EngineersInfo
    {
        [Parameter] public Data Data { get; set; }


        public EngineersInfo()
        {
            /*
            if (Data.EngineersList != null)
            {
                Engineer.EngineerBlueprints.TryGetValue(
                    Data.EngineersList[CurrentCard[(int)CurrentTab]].Faction,
                    out var blueprints);

                str =
                    Engine.Razor.Run("engineers.cshtml", null, new
                    {
                        CurrentTab = CurrentTab,
                        CurrentPage = _currentPage,
                        CurrentCard = CurrentCard[(int)CurrentTab],

                        Engineer = Data.EngineersList[CurrentCard[(int)CurrentTab]],

                        Blueprints = blueprints.Where(x => x.Type != "Suit" && x.Type != "Weapon"),

                        SuitWeaponBlueprints = blueprints.Where(x => x.Type == "Suit" || x.Type == "Weapon")

                    });
            }*/

        }

    }
}
