using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace dashboard_elite.Services
{
    public class ProfileData
    {
        public string PrimaryIcon { get; set; }
        public string SecondaryIcon { get; set; }
        public string ClickSound { get; set; }

    }


    public class ProfileCacheService
    {
        private readonly IWebHostEnvironment _env;

        public Dictionary<string, ProfileData> ProfileCache { get; set; } = new Dictionary<string, ProfileData>();

        public ProfileCacheService(IWebHostEnvironment env)
        {
            _env = env;

            var profilePath = Path.Combine(dashboard_elite.Program.ExePath, "Data\\");

            ProfileCache =
                JsonConvert.DeserializeObject<Dictionary<string, ProfileData>>(
                    File.ReadAllText(Path.Combine(profilePath, "profiles.json")));

        }

    }



}
