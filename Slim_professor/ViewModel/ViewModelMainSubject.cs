using MVVMBase.Command;
using MVVMBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Slim_professor.Model;
using Slim_professor.View;

namespace Slim_professor.ViewModel
{
    class ViewModelMainSubject : ViewModelBase
    {
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
        private SubjectList _subjectlist;
 

        public ViewModelMainSubject(SubjectList subjectlist)
        {
            _subjectlist = subjectlist;
             
        }

        #region GoStudentStateCommand
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
        private ICommand _GoHome;
        private MainFrame mf;
        public ICommand GoHome
        {
            get { return _GoHome ?? (_GoHome = new AppCommand(GoHomeFunc)); }
        }
        private void GoHomeFunc(Object o)
        {
            mf = MainFrame.thisMainFrame();
            mf.NavigationService.Navigate(_subjectlist);
        }
        #endregion

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
            //Widget widget = new Widget();
            //widget.Show();
            //MainFrame.Frame.Hide();
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
