﻿using System;
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
using System.Threading;
using System.Windows.Threading;

namespace Slim_professor.View
{
    /// <summary>
    /// Interaction logic for PageMainSubject.xaml
    /// </summary>
    public partial class PageMainSubject : Page
    {       
        public static object[] SubjectInfo;
        public static Frame MainFrameObject;

        private SubjectList _subjectlist;
        private int sec;//,min,hour;
        private Widget widget;
        public PageMainSubject(object[] param, SubjectList subjectlist)
        {
            InitializeComponent();
            DataContext = new ViewModelMainSubject(subjectlist);

            MainFrameObject = FramePanel;
            ViewModelMainSubject.MainSubjectObject.FrameSource = new Uri("PageStudentState.xaml", UriKind.Relative);
            SubjectInfo = param;
            SubName.Text = SubjectInfo.ElementAt(1).ToString();
            _subjectlist = subjectlist;
            widget = new Widget();

            //sec = 7200;
            //Clock();
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

        private void WidgetBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            widget.Show();
            MainFrame.Frame.Hide();
        }

        private void CloseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            MainFrame.Frame.Close();
        }
        #endregion
    }
}
