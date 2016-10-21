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
        private DB_OnetimeProgram dbOneTimeProgram;
        private DB_AllProgram dbAllProgram;
        private DB_Subject dbSubject;
        private DB_User dbUser;
        private DB_Attendance dbAttendance;

        private SubjectList _subjectlist;
        private TextBox processTextBox;

        private const int FINISH_CLASS = 0;
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

        public ViewModelMainSubject(SubjectList subjectlist, TextBox _processTextBox)
        {
            dbManager = new DBManager();
            dbOneTimeProgram = new DB_OnetimeProgram(dbManager);
            dbAllProgram = new DB_AllProgram(dbManager);
            dbSubject = new DB_Subject(dbManager);
            dbUser = new DB_User(dbManager);
            dbAttendance = new DB_Attendance(dbManager);

            /*
             * 해당 수업을 듣는 학생들 구분하기
             * 1. 모든 학생 리스트 출력(attendance 테이블에 선생님이 그날 첫 로그인을 하면 해당 학생들 전부 insert시키기)
             * 2. 기본적으로 오늘 날짜 출력
             * 3. 날짜 선택해서 들어가면 해당 학생의 당시 출결 여부 판단 가능
             * 4. (선택사항) 학생 더블클릭하면 해당 날짜의 학생 스텟 확인 가능(빨간불 몇회, 물음표 몇회)
             */


            #region 학생 출석리스트에 담기
            // 1. 모든 학생 리스트 출력(attendance 테이블에 선생님이 그날 첫 로그인을 하면 해당 학생들 전부 insert시키기)

            //TODO: 오늘 출석을 했으면 체크해서 다시 이 로직을 수행하지 않도록.. -> subject테이블 마지막 속성에 가장 마지막으로 접속한 날짜를 기록해두고 확인하는 방식으로.
            //if(선생님이 오늘 이 과목으로 로그인을 한번도 하지 않았으면)

            string month = "", day = "";
            if (DateTime.Now.Month < 10)
                month = "0" + DateTime.Now.Month;
            else
                month = DateTime.Now.Month.ToString();
            if (DateTime.Now.Day < 10)
                day = "0" + DateTime.Now.Day;
            else
                day = DateTime.Now.Day.ToString();


            string today = DateTime.Now.Year+"-"+month+"-"+day;
            string lastClassDate = Convert.ToString(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.last_class)).Split(' ')[0];
            if (lastClassDate != today)
            {
                int subId = Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id));
                dbSubject.UpdateLastClass(subId, DateTime.Now.Date);
                List<object[]> studentList = dbUser.SelectStudent();
                for (int i = 0; i < studentList.Count; i++)
                {
                    string[] subIds = studentList[i].ElementAt((int)DB_User.FIELD.sub_ids).ToString().Split('_');
                    bool isMyStudent = false;
                    for (int j = 0; j < subIds.Length; j++)
                    {
                        if (subId == Convert.ToInt32(subIds[j]))
                        {
                            isMyStudent = true;
                            break;
                        }
                    }
                    if (isMyStudent)
                    {
                        dbAttendance.InsertStudentAttendance(studentList[i].ElementAt((int)DB_User.FIELD.user_id).ToString(), subId, 0);
                    }
                }
            }
            #endregion

            //makeRedGreenList();
            processTextBox = _processTextBox;
            _ItemChcek = new List<object[]>();
            _ItemGreenList = new List<object[]>();
            _subjectlist = subjectlist;
            MainSubjectObject = this;

            Clock();
            cpu_Counter = new PerformanceCounter("Process", "% User Time", Process.GetCurrentProcess().ProcessName);
        }

        #region ItemCheck
        private List<object[]> _ItemChcek;
        public List<object[]> ItemChcek
        {
            get { return _ItemChcek; }
            set { _ItemChcek = value; }
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
        /*   public void makeRedGreenList()
           {

               object[] subItems = PageMainSubject.SubjectInfo;
               int sub_id = Convert.ToInt32(subItems[(int)DB_Subject.FIELD.sub_id]);

               ItemRedList = dbAllProgram.SelectAllProgramList(Convert.ToInt32(sub_id), 0);
               ItemGreenList = dbAllProgram.SelectAllProgramList(Convert.ToInt32(sub_id), 1);


           }  */
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

        #region ClosingWindowCommand
        private ICommand _ClosingWindowCommand;
        public ICommand ClosingWindowCommand
        {
            get { return _ClosingWindowCommand ?? (_ClosingWindowCommand = new AppCommand(ClosingWindowFunc)); }
        }

        private void ClosingWindowFunc(Object o)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            dbOneTimeProgram.DeleteOneTime(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), string.Empty, 0);      // 수업 종료 DB 변경
            dbSubject.UpdateIsProcessing(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), FINISH_CLASS);   // 수업 종료 DB 변경
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
            dbOneTimeProgram.DeleteOneTime(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), string.Empty, 0);      // 수업 종료 DB 변경
            dbSubject.UpdateIsProcessing(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), FINISH_CLASS);   // 수업 종료 DB 변경
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
            processTextBox.Text = ps.ProcessName;

                
            //}

            //현재 활성화 되어있는 process의 이름으로 cpu_Counter InstanceName에 대입
            cpu_Counter.InstanceName = ps.ProcessName;
            ItemChcek=dbOneTimeProgram.SelectOneTimeList(ps.ProcessName);



            if (ItemChcek == null)
            {
                dbOneTimeProgram.InsertOneTime(ps.ProcessName, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), Convert.ToString(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_name)), 1);
                dbOneTimeProgram.UpdateOneTime(ps.ProcessName, 0, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            }

          
            else if (ItemChcek.Count > 0)
            {
                dbOneTimeProgram.UpdateOneTime(ps.ProcessName, 1, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            }
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