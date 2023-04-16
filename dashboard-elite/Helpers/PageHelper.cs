using System;
using System.Linq;
using dashboard_elite.EliteData;

namespace dashboard_elite.Helpers
{
    public static class PageHelper
    {
        public enum Page
        {
            Commander,
            Engineers,
            Galnet,
            Poi,
            Mining,
            Powers,
            HWInfo,
            CurrentShip,
            StoredShips,
            StoredModules,
            Target,
            Navigation,
            Route,
            SystemMap,
            OdysseySettlements,
            ColoniaBridge,
            Missions,
            ChatLog,
            Materials,
            Cargo,
            EdEngineer,
            ShipLocker,
            Backpack,
            GalaxyMap
        }

        public static readonly int[] SubPages = { 0,  // commander
                                                  37, // engineers
                                                  0,  // galnet
                                                  8,  // poi
                                                  3,  // mining
                                                  11, // powers
                                                  0,  // hwinfo
                                                  0,  // currentship
                                                  0,  // storedships
                                                  0,  // storedmodules
                                                  0,  // target
                                                  0,  // navigation
                                                  0,  // route
                                                  0,  // systemmap
                                                  0,  // odysseysettlements
                                                  0,  // coloniabridge
                                                  0,  // missions
                                                  7,  // chatlog
                                                  3,  // materials
                                                  0,  // cargo
                                                  3,  // edengineer
                                                  4,  // shiplocker
                                                  4,  // backpack
                                                  0   // galaxymap
                                                  };

        private static readonly int[] CurrentPage = new int[100];

        public static int IncrementCurrentPage(string pageName, int currentPage, Ships Ships, Module Module)
        {
            Enum.TryParse(pageName, true, out Page pageType);

            switch(pageType)
            {
                case Page.StoredShips:
                    SubPages[(int)pageType] = Ships.ShipsList.Count(x => x.Stored);
                    break;
                case Page.StoredModules:
                    SubPages[(int)pageType] = Module.StoredModulesList.Values.Count;
                    break;
            }

            if (currentPage == SubPages[(int)pageType] - 1)
            {
                currentPage = 0;
            }
            else
            {
                currentPage++;
            }

            CurrentPage[(int)pageType] = currentPage;

            return currentPage;
        }

        public static int DecrementCurrentPage(string pageName, int currentPage, Ships Ships, Module Module)
        {
            Enum.TryParse(pageName, true, out Page pageType);

            if (currentPage == 0)
            {
                switch (pageType)
                {
                    case Page.StoredShips:
                        SubPages[(int)pageType] = Ships.ShipsList.Count(x => x.Stored);
                        break;
                    case Page.StoredModules:
                        SubPages[(int)pageType] = Module.StoredModulesList.Values.Count;
                        break;
                }

                currentPage = SubPages[(int)pageType] - 1;
            }
            else
            {
                currentPage--;
            }

            CurrentPage[(int)pageType] = currentPage;

            return currentPage;
        }


        public static void SetCurrentPage(Page pageType, int currentPage)
        {
            CurrentPage[(int)pageType] = currentPage;
        }

        public static void SetCurrentPage(string pageName, int currentPage)
        {
            Enum.TryParse(pageName, true, out Page pageType);

            SetCurrentPage(pageType, currentPage);
        }
        

        public static int GetCurrentPage(Page pageType)
        {
            return CurrentPage[(int)pageType];
        }

        public static int GetCurrentPage(string pageName)
        {
            Enum.TryParse(pageName, true, out PageHelper.Page pageType);

            return GetCurrentPage(pageType);
        }

        public static string SinceText(int agodec, DateTime updatedTime)
        {

            var ts = DateTime.Now - updatedTime.AddSeconds(-agodec);

            return ts.ToHumanReadableString();
        }


    }
}
