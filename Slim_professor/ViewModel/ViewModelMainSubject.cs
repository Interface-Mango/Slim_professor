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

namespace Slim_professor.ViewModel
{
    class ViewModelMainSubject : ViewModelBase
    {
        private DBManager dbManager;
        public static ViewModelMainSubject MainSubjectObject;
        private DB_OnetimeProgram dbOneTime;
        private DB_AllProgram dbAllProgram;
        private SubjectList _subjectlist;

        public ViewModelMainSubject(SubjectList subjectlist)
        {
            dbManager = new DBManager();
            dbOneTime = new DB_OnetimeProgram(dbManager);
            dbAllProgram = new DB_AllProgram(dbManager);
            //makeRedGreenList();
            _ItemRedList = new List<object[]>();
            _ItemGreenList = new List<object[]>();
            _subjectlist = subjectlist;
            MainSubjectObject = this;
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