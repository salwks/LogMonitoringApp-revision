using System.Windows.Controls;
using LogMonitoringApp.Shell.Navigation; // TopNavBar를 위한 네임스페이스

namespace LogMonitoringApp.Shell
{
    public partial class AppShell : UserControl
    {
        public AppShell()
        {
            InitializeComponent();
            // TopNavBarControl의 TabButtonClicked 이벤트 구독
            TopNavBarControl.TabButtonClicked += TopNavBarControl_TabButtonClicked;
        }

        // 매개변수를 수정하여 sender를 nullable로 지정
        private void TopNavBarControl_TabButtonClicked(object? sender, int tabIndex)
        {
            MainTabControl.SelectedIndex = tabIndex;
        }
    }
}