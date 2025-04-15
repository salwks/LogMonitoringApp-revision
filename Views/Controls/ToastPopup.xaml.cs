using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LogMonitoringApp.Views.Controls
{
    public partial class ToastPopup : UserControl
    {
        public ToastPopup()
        {
            InitializeComponent();
        }

        // 메시지 속성
        public string Message
        {
            get { return MessageText.Text; }
            set { MessageText.Text = value; }
        }

        // 팝업 보여주기
        public void Show(int durationMs = 2000)
        {
            // 페이드 인 애니메이션 시작
            var fadeIn = (Storyboard)FindResource("FadeIn");
            fadeIn.Begin(this);

            // 지정된 시간 후 페이드 아웃
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(durationMs);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                var fadeOut = (Storyboard)FindResource("FadeOut");
                fadeOut.Completed += (s2, e2) =>
                {
                    // 페이드 아웃 완료 후 제거
                    if (this.Parent is Panel panel)
                    {
                        panel.Children.Remove(this);
                    }
                };
                fadeOut.Begin(this);
            };
            timer.Start();
        }
    }
}