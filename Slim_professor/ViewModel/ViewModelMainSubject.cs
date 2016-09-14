using MVVMBase.Command;
using MVVMBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Slim_professor.Model;
using Slim_professor.View;
using System.Windows.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Slim_professor.ViewModel
{
    class ViewModelMainSubject : ViewModelBase
    {
        private DBManager dbManager;
        public static ViewModelMainSubject MainSubjectObject;
        private DB_OnetimeProgram dbOneTime;
        private DB_AllProgram dbAllProgram;
        private SubjectList _subjectlist;
        private TextBox _temp;

        #region Algorithm component
        private PerformanceCounter cpu_Counter; // cpu 점유율
        private IntPtr handle;//활성화 윈도우를 담는 그릇
        private uint pid; // processID 얻어오기위한 그릇
        private Process ps; // pid로 프로세스 검색하기 위한 변수
        #region WindowProcessID
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWMAXIMIZED = 3;
        #endregion
        #endregion

        public ViewModelMainSubject(SubjectList subjectlist, TextBox temp)
        {
            dbManager = new DBManager();
            dbOneTime = new DB_OnetimeProgram(dbManager);
            dbAllProgram = new DB_AllProgram(dbManager);
            makeRedGreenList();
            _temp = temp;
            _ItemRedList = new List<object[]>();
            _ItemGreenList = new List<object[]>();
            _subjectlist = subjectlist;
            MainSubjectObject = this;

            Clock();
            cpu_Counter = new PerformanceCounter("Process", "% User Time", Process.GetCurrentProcess().ProcessName);
        }

        #region ItemRedList
        private List<object[]> _ItemRedList;
        public List<object[]> ItemRedList
        {
            get { return _ItemRedList; }
            set { _ItemRedList = value; }
        }
        #endregion


        #region ItemRedList
        private List<object[]> _ItemGreenList;
        public List<object[]> ItemGreenList
        {
            get { return _ItemGreenList; }
            set { _ItemGreenList = value; }
        }
        #endregion

        #region ItemListOneTime
        private List<object[]> _ItemListOneTime;
        public List<object[]> ItemListOneTime
        {
            get { return _ItemListOneTime; }
            set { _ItemListOneTime = value; }
        }
        #endregion

        #region makeList
        public void makeRedGreenList()
        {

            object[] subItems = PageMainSubject.SubjectInfo;
            int sub_id = Convert.ToInt32(subItems[(int)DB_Subject.FIELD.sub_id]);

            ItemRedList = dbAllProgram.SelectAllProgramList(Convert.ToInt32(sub_id), 0);
            ItemGreenList = dbAllProgram.SelectAllProgramList(Convert.ToInt32(sub_id), 1);



        }
        #endregion



        #region FrameSource
        private Uri _FrameSource;
        public Uri FrameSource
        {
            get { return _FrameSource; }
            set
            {
                if (_FrameSource != value)
                {
                    _FrameSource = value;
                    OnPropertyChanged("FrameSource");
                }
            }
        }
        #endregion

        //<Button Background="#FFEC5C5C" Width="24" Height="24" Canvas.Left="1158" Canvas.Top="9" Content="X" Foreground="White" FontWeight="Bold" Command="{Binding CCloseWindowCommand}"/>
        #region CloseWindowCommand
        private ICommand _CloseWindowCommand;
        public ICommand CCloseWindowCommand
        {
            get { return _CloseWindowCommand ?? (_CloseWindowCommand = new AppCommand(CloseWindowFunc)); }
        }

        private void CloseWindowFunc(Object o)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            MainFrame.Frame.Close();
        }
        #endregion

        #region GoStudentState
        private ICommand _GoStudentState;
        public ICommand GoStudentState
        {
            get { return _GoStudentState ?? (_GoStudentState = new AppCommand(GoStudentStateFunc)); }
        }

        private void GoStudentStateFunc(Object o)
        {
            _FrameSource = new Uri("PageStudentState.xaml", UriKind.Relative);
            Console.WriteLine(_FrameSource.OriginalString);
            OnPropertyChanged("FrameSource");
        }
        #endregion

        #region GoHiddenTalkCommand
        private ICommand _GoHiddenTalk;
        public ICommand GoHiddenTalk
        {
            get { return _GoHiddenTalk ?? (_GoHiddenTalk = new AppCommand(GoHiddenTalkFunc)); }
        }

        private void GoHiddenTalkFunc(Object o)
        {
            _FrameSource = new Uri("PageHiddenTalk.xaml", UriKind.Relative);
            Console.WriteLine(_FrameSource.OriginalString);
            OnPropertyChanged("FrameSource");
        }
        #endregion

        #region GoSubjectStatistic
        private ICommand _GoSubjectStatistic;
        public ICommand GoSubjectStatistic
        {
            get { return _GoSubjectStatistic ?? (_GoSubjectStatistic = new AppCommand(GoSubjectStatisticFunc)); }
        }

        private void GoSubjectStatisticFunc(Object o)
        {
            _FrameSource = new Uri("PageSubjectStatistic.xaml", UriKind.Relative);
            OnPropertyChanged("FrameSource");
        }
        #endregion


        #region GoNotice
        private ICommand _GoNotice;
        public ICommand GoNotice
        {
            get { return _GoNotice ?? (_GoNotice = new AppCommand(GoNoticeFunc)); }
        }

        private void GoNoticeFunc(Object o)
        {
            _FrameSource = new Uri("PageNotice.xaml", UriKind.Relative);
            OnPropertyChanged("FrameSource");
        }
        #endregion

        #region GoHome
        private MainFrame mainFrame;
        private ICommand _GoHome;
        public ICommand GoHome
        {
            get { return _GoHome ?? (_GoHome = new AppCommand(GoHomeFunc)); }
        }
        private void GoHomeFunc(Object o)
        {
            mainFrame = MainFrame.thisMainFrame();
            mainFrame.NavigationService.Navigate(_subjectlist);
        }
        #endregion
        /*
        #region MinimizeCommand
        private ICommand _MinimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                return _MinimizeCommand ?? (_MinimizeCommand = new AppCommand(MinimizeCommandFunc));
            }
        }
        public void MinimizeCommandFunc(Object o)
        {
            Widget widget = new Widget();
            widget.Show();
            MainFrame.Frame.Hide();
        }
        #endregion

        */

        #region Algorithm
        public void Clock()
        {
            System.Windows.Threading.DispatcherTimer TimerClock =
                new System.Windows.Threading.DispatcherTimer();

            TimerClock.Interval = new TimeSpan(0, 0, 0, 1);
            TimerClock.IsEnabled = true;
            TimerClock.Tick += new EventHandler(TimerClock_Tick);
        }

        public void TimerClock_Tick(object sender, EventArgs e)
        {
            handle = GetForegroundWindow();        // 활성화 윈도우
            GetWindowThreadProcessId(handle, out pid); // 핸들로 프로세스아이디 얻어옴 
            ps = Process.GetProcessById((int)pid); // 프로세스아이디로 프로세스 검색

            //if (cpu_Counter.NextValue() >= 2)
            //{
                //임시 박스에 임시적으로...!
                //_temp.AppendText(ps.ProcessName + Environment.NewLine);

                //현재 무슨 Process가 활성화 되는지 유저에게 인터페이스 제공
                _temp.Text = ps.ProcessName;

                
            //}

            //현재 활성화 되어있는 process의 이름으로 cpu_Counter InstanceName에 대입
            cpu_Counter.InstanceName = ps.ProcessName;

        }

        #endregion

        #region Profile
        public string UserGroup
        {
            get { return (string)MainFrame.UserInfo[(int)DB_User.FIELD.group]; }
        }

        public string UserName
        {
            get { return (string)MainFrame.UserInfo[(int)DB_User.FIELD.user_name]; }
        }
        #endregion

    }
}