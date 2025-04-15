using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LogMonitoringApp.Pages.Logs; // LogsPage 사용

namespace LogMonitoringApp.Shell
{
    public class NavigationService
    {
        private Frame _frame;
        private Dictionary<string, Page> _pageCache;

        public NavigationService(Frame frame)
        {
            _frame = frame;
            _pageCache = new Dictionary<string, Page>();
        }

        public void NavigateTo(string pageName, bool useCache = true)
        {
            Page page;
            if (useCache && _pageCache.ContainsKey(pageName))
            {
                page = _pageCache[pageName];
            }
            else
            {
                page = CreatePage(pageName);
                if (useCache)
                    _pageCache[pageName] = page;
            }
            _frame.Navigate(page);
        }

        private Page CreatePage(string pageName)
        {
            switch (pageName)
            {
                // 기존 "LogMonitoringPage"을 "LogsPage"로 변경
                case "LogsPage":
                    return new LogsPage();
                default:
                    throw new ArgumentException($"Unknown page: {pageName}");
            }
        }
    }
}
