using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Slim_professor.ViewModel;
using Slim_professor.Model;
using System.Threading;
using System.Windows.Threading;
using System.Net;
using System.Net.Sockets;

namespace Slim_professor.View
{
    /// <summary>
    /// Interaction logic for PageMainSubject.xaml
    /// </summary>
    public partial class PageMainSubject : Page
    {       
        public static object[] SubjectInfo;
        public static Frame MainFrameObject;

        private DB_Subject dbSubject;
        private SubjectList _subjectlist;
        private int sec;//,min,hour;
        private Widget widget;

        private const int START_CLASS = 1;
        private const int FINISH_CLASS = 0;

        public PageMainSubject(object[] param, SubjectList subjectlist)
        {
            InitializeComponent();
            SubjectInfo = param;
            DataContext = new ViewModelMainSubject(subjectlist, processTextBox);

            MainFrameObject = FramePanel;
            ViewModelMainSubject.MainSubjectObject.FrameSource = new Uri("PageStudentState.xaml", UriKind.Relative);
            SubName.Text = SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_name).ToString();
            _subjectlist = subjectlist;
            widget = new Widget();

            // 수업 진행 중으로 바꿈
            dbSubject = new DB_Subject(new DBManager());
            Random random = new Random();
            int portNum = random.Next(1000) + 8124;
            dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), getMyIp, portNum);      // 수업 종료 DB 변경
            dbSubject.UpdateIsProcessing(Convert.ToInt32(SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), START_CLASS);  // START_CLASS를 넣으면 수업이 진행중이 됨   

            sec = 7200;
            Clock();
        }

        #region 소소한 기능들

        public void Clock()        
        {
            System.Windows.Threading.DispatcherTimer TimerClock = 
                new System.Windows.Threading.DispatcherTimer();

            // 200 milliseconds
            TimerClock.Interval = new TimeSpan(0, 0, 0, 1);
            TimerClock.IsEnabled = true;
            TimerClock.Tick += new EventHandler(TimerClock_Tick);
        }

        void TimerClock_Tick(object sender, EventArgs e)
        {
            sec -= 1;

            if (sec == 0)
            {
                widget.HT.Text = "00";
                widget.MT.Text = "00";
                widget.ST.Text = "00";
            }
            else
            {
                widget.HT.Text = (sec / 3600).ToString();
                widget.MT.Text = ((sec % 3600) / 60).ToString();
                widget.ST.Text = ((sec % 3600) % 60).ToString();
            }

        }

        /*
         * 자신 PC의 IP를 받아오는 프로퍼티
         */
        public string getMyIp
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }
        }
        #endregion

        private void WidgetBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            widget.Show();
            MainFrame.Frame.Hide();
        }
    }
}
