using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Serilog;

// ReSharper disable IdentifierTypo

namespace Elite
{
    public class GalnetRoot
    {
        [JsonProperty("data")]
        public List<GalnetDataItem> Data { get; set; }
    }

    public class GalnetDataItem
    {
        [JsonProperty("attributes")]
        public GalnetData Attributes { get; set; }

    }

    public class GalnetBodyItem
    {
        [JsonProperty("value")]
        public string Value { get; set; }

    }

    public class GalnetData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("field_galnet_date")]
        public string Date { get; set; }

        [JsonProperty("field_galnet_image")]
        public string Image { get; set; }

        [JsonProperty("imageList")]
        public List<string> ImageList { get; set; }

        [JsonProperty("body")]
        public GalnetBodyItem BodyItem { get; set; }

        [JsonProperty("field_galnet_body")]
        public string Body { get; set; }

    }


    public static class Galnet
    {
        public static Dictionary<string,List<GalnetData>>GalnetList = new Dictionary<string, List<GalnetData>>();

        public static Dictionary<string, List<GalnetData>> GetGalnet(string path)
        {
            try
            {
                path = Path.Combine(dashboard_elite.Program.ExePath, path);

                if (File.Exists(path))
                {
                    return JsonConvert.DeserializeObject<List<GalnetData>>(File.ReadAllText(path))
                        .GroupBy(x => x.Date)
                        .ToDictionary(x => x.Key, x => x.ToList())
                        .Take(10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

            return new Dictionary<string, List<GalnetData>>();

        }

        public static void GetGalnetImages(Dictionary<string, List<GalnetData>> GalnetList)
        {
            try
            {
                var directory = Path.Combine(dashboard_elite.Program.ExePath, "wwwroot\\img\\galnet");

                foreach (var l in GalnetList)
                {
                    foreach (var n in l.Value)
                    {
                        for (var i = 0; i < n.ImageList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(n.ImageList[i]))
                            {
                                var imageName = n.ImageList[i] + ".png";

                                var imgPath = Path.Combine(directory, imageName);

                                if (!File.Exists(imgPath))
                                {
                                    try
                                    {
                                        using (var client = new WebClient())
                                        {
                                            client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                                            var data = client.DownloadData("https://hosting.zaonce.net/elite-dangerous/galnet/" + imageName);

                                            File.WriteAllBytes(imgPath, data);
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        Log.Logger.Error("galnet image not found " + imageName + " " + ex.ToString());

                                        n.ImageList[i] = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.ToString());
            }

        }


    }
}
