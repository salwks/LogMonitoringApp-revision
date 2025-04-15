using LogMonitoringApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LogMonitoringApp.Shell.Navigation
{
    public partial class TopNavBar : UserControl
    {
        // EventHandler<int>의 첫 번째 매개변수는 object?로 선언됨
        public event EventHandler<int>? TabButtonClicked;

        public TopNavBar()
        {
            InitializeComponent();
        }

        private void OnRadioTabChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && int.TryParse(rb.Tag?.ToString(), out int index))
            {
                // sender를 넘겨도 문제없음 (null이 아니기 때문에)
                TabButtonClicked?.Invoke(this, index);

                // 토스트 메시지 표시
                string pageName = rb.Content.ToString() ?? "해당";
                ToastService.ShowToast($"{pageName} 화면으로 이동했습니다.");
            }
        }
    }
}