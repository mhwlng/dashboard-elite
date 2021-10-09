using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;

namespace dashboard_elite.Services
{
    public class SvgCacheService
    {
        private readonly IWebHostEnvironment _env;

        private string _svgPath;

        private Dictionary<string, string> SvgCache { get; set; } = new Dictionary<string, string>();

        public SvgCacheService(IWebHostEnvironment env)
        {
            _env = env;

            _svgPath = Path.Combine(_env.WebRootPath, "img\\buttons\\");
        }

        public string ButtonIcon(string key)
        {
                if (!SvgCache.ContainsKey(key))
                {
                    var fileName = Path.Combine(_svgPath, key);

                    if (File.Exists(fileName))
                    {
                        SvgCache.Add(key, File.ReadAllText(Path.Combine(_svgPath, key)));
                    }
                    else
                    {
                        SvgCache.Add(key, File.ReadAllText(Path.Combine(_svgPath, "no-icon.svg")));
                    }
                }

                return SvgCache[key];
        }


    }
}
