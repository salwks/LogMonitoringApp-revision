using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LogMonitoringApp.Pages.Logs.Controls
{
    public partial class LogListControl : UserControl
    {
        public ObservableCollection<LogEntry> LogEntries { get; set; }
        private bool _isUpdatingCheckState = false;

        public LogListControl()
        {
            InitializeComponent();

            // 샘플 데이터 생성 (각 로그 구분 타입 2개씩 포함)
            LogEntries = new ObservableCollection<LogEntry>
            {
                // 시스템 로그 2개
                new LogEntry { IsSelected = false, Timestamp = "12/10 15:18:33", Severity = "정보", LogCategory = "시스템", LogId = "SS150472", Message = "장치를 초기화 하였습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 14:06:05", Severity = "에러", LogCategory = "시스템", LogId = "SH876585", Message = "Tank 출력 kV 파라미터가 110%에 도달 했습니다.", User = "홍길동" },
                
                // 사용자 로그 2개
                new LogEntry { IsSelected = false, Timestamp = "12/10 13:30:21", Severity = "정보", LogCategory = "사용자", LogId = "UE868996", Message = "영상 관리 화면으로 이동했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 12:43:19", Severity = "정보", LogCategory = "사용자", LogId = "UE087687", Message = "영상에 사각형 개체를 추가하였습니다.", User = "홍길동" },
                
                // 네트워크 로그 2개
                new LogEntry { IsSelected = false, Timestamp = "12/10 15:10:10", Severity = "정보", LogCategory = "네트워크", LogId = "NC657578", Message = "네트워크에 연결되었습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 13:38:17", Severity = "정보", LogCategory = "네트워크", LogId = "NC658485", Message = "DICOM 서버와 연결되었습니다.", User = "홍길동" },
                
                // 보안 로그 2개
                new LogEntry { IsSelected = false, Timestamp = "12/10 14:27:17", Severity = "정보", LogCategory = "보안", LogId = "SA996599", Message = "시스템에 로그인 하였습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 14:18:33", Severity = "정보", LogCategory = "보안", LogId = "SA908689", Message = "전사용 종료되었습니다.", User = "홍길동" },
                
                // 추가 로그들
                new LogEntry { IsSelected = false, Timestamp = "12/10 13:27:17", Severity = "에러", LogCategory = "시스템", LogId = "SS897886", Message = "데이터 베이스 정리에 실패했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 12:56:48", Severity = "에러", LogCategory = "시스템", LogId = "SH578578", Message = "Filament 전류가 설정보다 낮습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 12:35:21", Severity = "정보", LogCategory = "사용자", LogId = "UE789686", Message = "영상을 확대했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 12:30:21", Severity = "정보", LogCategory = "사용자", LogId = "UE906769", Message = "비교 영상을 선택했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 12:18:33", Severity = "성공", LogCategory = "사용자", LogId = "UE706890", Message = "Fluoro 영상을 촬영했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 11:39:48", Severity = "경고", LogCategory = "시스템", LogId = "SH567988", Message = "Capbank가 충전중입니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 11:33:48", Severity = "에러", LogCategory = "사용자", LogId = "UE956785", Message = "Fluoro 영상 촬영에 실패했습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 11:33:18", Severity = "성공", LogCategory = "시스템", LogId = "SS789680", Message = "파일 시스템을 검사하였습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 11:11:35", Severity = "정보", LogCategory = "사용자", LogId = "UE906599", Message = "DNR 설정을 변경하였습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 10:29:17", Severity = "정보", LogCategory = "네트워크", LogId = "UE808689", Message = "촬영 모드를 Fluoro로 변경하였습니다.", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 10:27:17", Severity = "정보", LogCategory = "보안", LogId = "UE987879", Message = "kV 값을 변경했습니다", User = "홍길동" },
                new LogEntry { IsSelected = false, Timestamp = "12/10 10:18:33", Severity = "에러", LogCategory = "시스템", LogId = "SH979677", Message = "Detector가 연결되지 않았습니다.", User = "홍길동" }
            };

            LogDataGrid.ItemsSource = LogEntries;

            // PropertyChanged 이벤트 구독
            foreach (var entry in LogEntries)
            {
                entry.PropertyChanged += LogEntry_PropertyChanged;
            }
        }

        // 모든 행 체크박스 상태가 변경될 때 호출
        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (_isUpdatingCheckState) return;

            _isUpdatingCheckState = true;
            if (sender is CheckBox headerCheckBox)
            {
                bool isChecked = headerCheckBox.IsChecked == true;
                foreach (var entry in LogEntries)
                {
                    entry.IsSelected = isChecked;
                }
            }
            _isUpdatingCheckState = false;
        }

        // 개별 행 체크박스 상태가 변경될 때 호출
        private void ItemCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateHeaderCheckBoxState();
        }

        // 헤더 체크박스 상태 업데이트
        private void UpdateHeaderCheckBoxState()
        {
            if (_isUpdatingCheckState) return;

            _isUpdatingCheckState = true;

            if (LogEntries.Count == 0)
            {
                SelectAllCheckBox.IsChecked = false;
            }
            else
            {
                bool allChecked = LogEntries.All(entry => entry.IsSelected);
                bool anyChecked = LogEntries.Any(entry => entry.IsSelected);

                SelectAllCheckBox.IsChecked = allChecked;
                // 부분 선택 상태를 표시하려면 아래 코드를 활성화
                // SelectAllCheckBox.IsThreeState = true;
                // SelectAllCheckBox.IsChecked = allChecked ? true : (anyChecked ? null : false);
            }

            _isUpdatingCheckState = false;
        }

        // LogEntry 속성 변경 이벤트 핸들러
        private void LogEntry_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LogEntry.IsSelected))
            {
                UpdateHeaderCheckBoxState();
            }
        }
    }

    public class LogEntry : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public string Timestamp { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string LogCategory { get; set; } = string.Empty;
        public string LogId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}