using LogMonitoringApp.Services;
using System.Windows;

namespace LogMonitoringApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ToastService 초기화
            ToastService.Initialize(ToastContainer);
        }
    }
}