using System;

namespace dashboard_elite.Helpers
{
    public static class PageHelper
    {
        public enum Page
        {
            Commander,
            Engineers,
            Galnet,
            Poi
        }

        public static readonly int[] SubPages = { 0,  // commander
                                                  0,  // engineers
                                                  0,  // galnet
                                                  8   // poi
                                                  };

        private static readonly int[] CurrentPage = new int[100];

        public static int IncrementCurrentPage(string pageName, int currentPage)
        {
            Enum.TryParse(pageName, true, out Page pageType);

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

        public static int DecrementCurrentPage(string pageName, int currentPage)
        {
            Enum.TryParse(pageName, true, out Page pageType);

            if (currentPage == 0)
            {
                currentPage = SubPages[(int)pageType] - 1;
            }
            else
            {
                currentPage--;
            }

            CurrentPage[(int)pageType] = currentPage;

            return currentPage;
        }



        public static void SetCurrentPage(string pageName, int currentPage)
        {
            Enum.TryParse(pageName, true, out Page pageType);

            CurrentPage[(int)pageType] = currentPage;
        }

        public static void SetCurrentPage(Page pageType, int currentPage)
        {
            CurrentPage[(int)pageType] = currentPage;
        }

        public static int GetCurrentPage(Page pageType)
        {
            return PageHelper.CurrentPage[(int)pageType];
        }

        public static int GetCurrentPage(string pageName)
        {
            Enum.TryParse(pageName, true, out PageHelper.Page pageType);

            return PageHelper.CurrentPage[(int)pageType];
        }

    }
}
