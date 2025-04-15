using LogMonitoringApp.Views.Controls;
using System.Windows;
using System.Windows.Controls;

namespace LogMonitoringApp.Services
{
    public static class ToastService
    {
        private static Panel? _container;

        // 토스트 컨테이너 초기화 (메인 윈도우에서 호출)
        public static void Initialize(Panel container)
        {
            _container = container;
        }

        // 토스트 메시지 보여주기
        public static void ShowToast(string message, int durationMs = 2000)
        {
            if (_container == null)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                var toast = new ToastPopup
                {
                    Message = message,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 80) // 화면 하단에서 간격
                };

                _container.Children.Add(toast);
                toast.Show(durationMs);
            });
        }
    }
}