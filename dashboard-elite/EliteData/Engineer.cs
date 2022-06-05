using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EDEngineer.Models;
using EDEngineer.Utils;
using Newtonsoft.Json;
using Serilog; //using System.Windows.Forms;

namespace dashboard_elite.EliteData
{
    public class Engineer
    {
        public Task ShoppingListTask;

        private CancellationTokenSource _shoppingListTokenSource = new CancellationTokenSource();
        
        public Dictionary<(string, string, int?), Blueprint> Blueprints;

        public Dictionary<string, List<Blueprint>> EngineerBlueprints;

        public Dictionary<string, List<string>> IngredientTypes;

        public Dictionary<string,EntryData> EngineeringMaterials;

        public Dictionary<string, EntryData> EngineeringMaterialsByKey;

        public string CommanderName;

        public List<BlueprintShoppingListItem> BlueprintShoppingList = new List<BlueprintShoppingListItem>();

        public List<IngredientShoppingListItem> IngredientShoppingList = new List<IngredientShoppingListItem>();

        public (Dictionary<string,EntryData>,Dictionary<string, EntryData>) GetAllEngineeringMaterials(string path)
        {
            try
            {
                path = Path.Combine(dashboard_elite.Common.ExePath, path);

                if (File.Exists(path))
                {
                    var json = JsonConvert.DeserializeObject<List<EntryData>>(File.ReadAllText(path));

                    return (json.ToDictionary(x => x.Name, x => x),
                            json.ToDictionary(x => x.FormattedName.ToLower(), x => x));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

            return (new Dictionary<string, EntryData>(), new Dictionary<string, EntryData>());
        }

        public Dictionary<(string, string, int?), Blueprint> GetAllBlueprints(string path, Dictionary<string, EntryData> engineeringMaterials)
        {
            try
            {
                path = Path.Combine(dashboard_elite.Common.ExePath, path);

                if (File.Exists(path))
                {
                    var blueprintConverter = new BlueprintConverter(engineeringMaterials);

                    return JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText(path), blueprintConverter)
                        .Where(b => b.Ingredients.Any())
                        .ToDictionary(x => (x.BlueprintName, x.Type, x.Grade), x => x);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

            return new Dictionary<(string, string, int?), Blueprint>();
          

        }

        public Dictionary<string, List<Blueprint>> GetEngineerBlueprints(string path, Dictionary<string, EntryData> engineeringMaterials)
        {
            try
            {
                path = Path.Combine(dashboard_elite.Common.ExePath, path);

                if (File.Exists(path))
                {

                    var blueprintConverter = new BlueprintConverter(engineeringMaterials);

                    var blueprints = JsonConvert
                        .DeserializeObject<List<Blueprint>>(File.ReadAllText(path), blueprintConverter)
                        .Where(b => b.Ingredients.Any()).ToList();


                    var engineerBlueprints = blueprints
                        .Where(x => x.Category != BlueprintCategory.Experimental && x.Type != "Suit" &&
                                     x.Type != "Weapon" && x.Category != BlueprintCategory.Unlock)
                            .SelectMany(blueprint => blueprint.Engineers,
                                (blueprint, engineer) => new {blueprint, engineer})
                            .Where(x => !x.engineer.StartsWith("@"))
                            .GroupBy(x => x.engineer)
                            .ToDictionary(x => x.Key,
                                x => x.Select(z => (Blueprint) z.blueprint)
                                    .GroupBy(a => a.Type).Select(b => b.First(c => c.Grade == b.Max(d => d.Grade)))
                                    .OrderByDescending(e => e.Grade).ThenBy(e => e.Type).ToList())
                            .Concat(blueprints
                                .Where(y => y.Type == "Suit" || y.Type == "Weapon")
                                .SelectMany(blueprint => blueprint.Engineers,
                                    (blueprint, engineer) => new {blueprint, engineer})
                                .Where(y => !y.engineer.StartsWith("@"))
                                .GroupBy(y => y.engineer)
                                .ToDictionary(x => x.Key,
                                    x => x.Select(z => (Blueprint) z.blueprint)
                                        .GroupBy(a => a.Type + a.BlueprintName).Select(b => b.First(c => c.Grade == b.Max(d => d.Grade)))
                                        .OrderByDescending(e => e.Grade).ThenBy(e => e.Type + e.BlueprintName).ToList()))
                        .ToDictionary(s => s.Key, s => s.Value); 

                    return engineerBlueprints;

                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

            return new Dictionary<string, List<Blueprint>>();


        }

        public Dictionary<string, List<string>> GetIngredientTypes(string path, Dictionary<string, EntryData> engineeringMaterials)
        {
            try
            {
                path = Path.Combine(dashboard_elite.Common.ExePath, path);

                if (File.Exists(path))
                {
                    var blueprintConverter = new BlueprintConverter(engineeringMaterials);

                    return JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText(path), blueprintConverter)
                        .Where(b => b.Ingredients.Any())
                        .SelectMany(y => y.Ingredients, (a, b) => new {b.EntryData.Name, a.Type})
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key,
                            y => y.Select(z => z.Type).Distinct().ToList())
                        ;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

            return new Dictionary<string, List<string>>();

        }

        private  async Task<string> GetJson(string url)
        {
            try
            {
                var utf8 = await Common.WebClient.GetStringAsync(url);

                return utf8;

            }
            catch
            {
                // ignored
            }

            return null;
        }

        public async Task GetCommanderName()
        {
            var commanderData  = await GetJson("http://localhost:44405/commanders");

            if (string.IsNullOrEmpty(commanderData)) return;

            CommanderName = JsonConvert.DeserializeObject<List<string>>(commanderData)
                .FirstOrDefault();
        }

        public void GetShoppingList()
        {
            if (string.IsNullOrEmpty(CommanderName)) return;

            var systemDataToken = _shoppingListTokenSource.Token;

            ShoppingListTask = Task.Run(async () =>
            {
                if (systemDataToken.IsCancellationRequested)
                {
                    systemDataToken.ThrowIfCancellationRequested();
                }

                var shoppingListData = await GetJson("http://localhost:44405/" + CommanderName + "/shopping-list");

                if (string.IsNullOrEmpty(shoppingListData)) return;

                var bluePrintList =
                    JsonConvert.DeserializeObject<List<BlueprintShoppingListItem>>(shoppingListData);

                foreach (var item in bluePrintList)
                {
                    Blueprints.TryGetValue(
                        (item.Blueprint.BlueprintName, item.Blueprint.Type, item.Blueprint.Grade),
                        out var bluePrintData);

                    item.BluePrintData = bluePrintData;
                }

                BlueprintShoppingList = bluePrintList;

                IngredientShoppingList = bluePrintList.Select(
                        x => new
                        {
                            x.Blueprint.BlueprintName,
                            x.Blueprint.Type,
                            x.Blueprint.Grade,
                            Ingredients = x.BluePrintData.Ingredients.Select(y => new
                                BlueprintIngredient(y.EntryData, y.Size * x.Count))
                        })

                    .SelectMany(x => x.Ingredients)
                    .GroupBy(x => x.EntryData.Name)
                    .Select(x => new IngredientShoppingListItem
                    {
                        Name = x.Key,
                        RequiredCount = x.Sum(y => y.Size),
                        EntryData = x.First().EntryData

                    }).ToList();

            }, systemDataToken);

        }

    }
}
