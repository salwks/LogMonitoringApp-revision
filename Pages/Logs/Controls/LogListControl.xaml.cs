using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LogMonitoringApp.Pages.Logs.Controls
{
    public partial class LogListControl : UserControl
    {
        public ObservableCollection<LogEntry> LogEntries { get; set; }
        private bool _isUpdatingCheckState = false;
        private readonly string _jsonFilePath = "Data/SampleLogs.json";

        public LogListControl()
        {
            InitializeComponent();

            // ObservableCollection 초기화
            LogEntries = new ObservableCollection<LogEntry>();

            // JSON 파일에서 데이터 로드
            LoadLogsFromJson();

            // DataGrid에 데이터 바인딩
            LogDataGrid.ItemsSource = LogEntries;
        }

        private void LoadLogsFromJson()
        {
            try
            {
                // 실행 파일 경로 기준으로 JSON 파일 경로 구성
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(basePath, _jsonFilePath);

                // 파일 존재 확인
                if (!File.Exists(fullPath))
                {
                    MessageBox.Show($"JSON 파일을 찾을 수 없습니다: {_jsonFilePath}", "파일 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadSampleData(); // 기본 샘플 데이터 로드
                    return;
                }

                // JSON 파일 읽기
                string jsonContent = File.ReadAllText(fullPath);

                // JSON 파싱 및 LogEntry 객체로 변환
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // 속성 이름 대소문자 무시
                };

                var logEntries = JsonSerializer.Deserialize<LogEntry[]>(jsonContent, options);

                if (logEntries == null || logEntries.Length == 0)
                {
                    MessageBox.Show("JSON 파일에서 로그 데이터를 로드할 수 없습니다.", "데이터 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LoadSampleData(); // 기본 샘플 데이터 로드
                    return;
                }

                // ObservableCollection에 로그 항목 추가
                LogEntries.Clear();
                foreach (var entry in logEntries)
                {
                    entry.IsSelected = false; // 기본적으로 체크 해제 상태
                    entry.PropertyChanged += LogEntry_PropertyChanged; // 이벤트 구독
                    LogEntries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"로그 데이터 로드 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadSampleData(); // 기본 샘플 데이터 로드
            }
        }

        // JSON 로드 실패 시 사용할 기본 샘플 데이터
        private void LoadSampleData()
        {
            LogEntries.Clear();

            // 샘플 데이터 생성 (각 로그 구분 타입 2개씩 포함)
            var sampleEntries = new LogEntry[]
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
                new LogEntry { IsSelected = false, Timestamp = "12/10 14:18:33", Severity = "정보", LogCategory = "보안", LogId = "SA908689", Message = "전사용 종료되었습니다.", User = "홍길동" }
            };

            foreach (var entry in sampleEntries)
            {
                entry.PropertyChanged += LogEntry_PropertyChanged;
                LogEntries.Add(entry);
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
                SelectAllCheckBox.IsChecked = allChecked;
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