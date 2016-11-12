using MVVMBase.Command;
using MVVMBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Slim_professor.Model;
using Slim_professor.View;

namespace Slim_professor.ViewModel
{
    class ViewModelPageManageProgramAll : ViewModelBase
    {

        private DBManager dbManager;
        private DB_AllProgram dbAllProgram;
        //private ViewModelPageManageProgramAll parentWindow;

        // 생성자
        public ViewModelPageManageProgramAll(/*ViewModelPageManageProgramAll pWindow*/)
        {
            dbManager = new DBManager();
            dbAllProgram = new DB_AllProgram(dbManager);
            //parentWindow = pWindow;
            _GreenItemList = new List<object[]>();
            _RedItemList = new List<object[]>();
        }

        #region AllGreenList
        private List<GreenInfo> _AllGreenList;
        public List<GreenInfo> AllGreenList
        {
            get { return _AllGreenList; }
            set
            {
                _AllGreenList = value;
            }
        }
        #endregion

        #region AllRedList
        private List<RedInfo> _AllRedList;
        public List<RedInfo> AllRedList
        {
            get { return _AllRedList; }
            set
            {
                _AllRedList = value;
            }
        }
        #endregion

        #region GreenItemList
        private List<object[]> _GreenItemList;
        public List<object[]> GreenItemList
        {
            get { return _GreenItemList; }
            set { _GreenItemList = value; }
        }
        #endregion

        #region RedItemList
        private List<object[]> _RedItemList;
        public List<object[]> RedItemList
        {
            get { return _RedItemList; }
            set { _RedItemList = value; }
        }
        #endregion

        #region RegistAllProgramCommand
        private ICommand _RegistAllProgramCommand;
        public ICommand RegistAllProgramCommand
        {
            get { return _RegistAllProgramCommand ?? (_RegistAllProgramCommand = new AppCommand(RegistAllProgramCommandFunc)); }
        }

        public void RegistAllProgramCommandFunc(Object o)
        {// 새로 추가할 프로그램이 있을 경우
            //ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.;
        }
        #endregion

        #region GoBack
        private ICommand _GoBack;
        public ICommand GoBack
        {
            get { return _GoBack ?? (_GoBack = new AppCommand(GoBackFunc)); }
        }

        public void GoBackFunc(Object o)
        {
            ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgram;
        }
        #endregion

        public void makeList()
        {
            int sub_id = Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id));

            GreenItemList = dbAllProgram.SelectAllProgramList(sub_id, 1);
            RedItemList = dbAllProgram.SelectAllProgramList(sub_id, 0);

            if (GreenItemList != null)
                AllGreenList = GreenInfo.Data(GreenItemList, dbAllProgram);
            if (RedItemList != null)
                AllRedList = RedInfo.Data(RedItemList, dbAllProgram);
        }

        internal class RedInfo
        {
            private static List<RedInfo> data;
            private static DB_AllProgram dbAllProgram;

            public int Id { get; private set; }
            public int AllRedId { get; private set; }
            public string AllRedTitle { get; private set; }
            public string AllRedProcessName { get; private set; }

            public static List<RedInfo> Data(List<object[]> items, DB_AllProgram _dbAllProgram)
            {
                RedInfo listItem;
                data = new List<RedInfo>();
                dbAllProgram = _dbAllProgram;

                for (int i = 0; i < items.Count; i++)
                {
                    string processName = Convert.ToString(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.process_name));
                    string title = "";
                    #region 하드코딩
                    if (processName == "KakaoTalk")
                        title = "카카오톡";
                    else if (processName == "explorer")
                        title = "탐색기";
                    else if (processName == "notepad")
                        title = "메모장";
                    else if (processName == "devenv")
                        title = "Visual Studio 2013";
                    else if (processName == "WDExpress")
                        title = "Visual Studio 2015";
                    else if (processName == "chrome")
                        title = "Google Chrome";
                    else if (processName == "Slim_Student.vshost")
                        title = "SLIM 학생 디버깅 프로그램";
                    else if (processName == "Slim_professor.vshost")
                        title = "SLIM 강사 디버깅 프로그램";
                    else if (processName == "Slim_Student")
                        title = "SLIM 학생 프로그램";
                    else if (processName == "Slim_professor")
                        title = "SLIM 강사 프로그램";
                    else if (processName == "Microsoft.MicrosoftEdge_8wekyb3d8bbwe")
                        title = "Microsoft Edge";
                    else if (processName == "iexplore")
                        title = "Microsoft Internet Explorer";
                    else
                        title = "기타 프로그램(" + processName + ")";
                    #endregion
                    listItem = new RedInfo
                    {
                        Id = items.Count - i,
                        AllRedId = Convert.ToInt32(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.id)),
                        AllRedTitle = title,
                        AllRedProcessName = processName
                    };
                    data.Add(listItem);
                }
                return data;
            }

            #region AllRedChangeCommand
            private ICommand _AllRedChangeCommand;
            public ICommand AllRedChangeCommand
            {
                get { return _AllRedChangeCommand ?? (_AllRedChangeCommand = new AppCommand(AllRedChangeCommandFunc)); }
            }
            public void AllRedChangeCommandFunc(Object o)
            {
                MessageBoxResult result = MessageBox.Show(AllRedTitle+"을(를) 수업과 관련된 프로그램으로 전환(예) 그렇지 않으면(아니오)", "정상/비정상 프로그램 선택", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbAllProgram.UpdateRedGreen(AllRedId, 1))
                    {
                        PageMainSubject.MainFrameObject.Refresh();
                        ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgramAll;
                    }
                }
            }
            #endregion

            #region AllRedDeleteCommand
            private ICommand _AllRedDeleteCommand;
            public ICommand AllRedDeleteCommand
            {
                get { return _AllRedDeleteCommand ?? (_AllRedDeleteCommand = new AppCommand(AllRedDeleteCommandFunc)); }
            }
            public void AllRedDeleteCommandFunc(Object o)
            {
                if (MessageBox.Show("삭제하시겠습니까?", "삭제 경고", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                dbAllProgram.DeleteById(AllRedId);
                PageMainSubject.MainFrameObject.Refresh();
                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgramAll;
            }
            #endregion
        }

        internal class GreenInfo
        {
            private static List<GreenInfo> data;
            private static DB_AllProgram dbAllProgram;

            public int Id { get; private set; }
            public int AllGreenId { get; private set; }
            public string AllGreenTitle { get; private set; }
            public string AllGreenProcessName { get; private set; }

            public static List<GreenInfo> Data(List<object[]> items, DB_AllProgram _dbAllProgram)
            {
                GreenInfo listItem;
                data = new List<GreenInfo>();
                dbAllProgram = _dbAllProgram;

                for (int i = 0; i < items.Count; i++)
                {
                    string processName = Convert.ToString(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.process_name));
                    string title = "";
                    #region 하드코딩
                    if (processName == "KakaoTalk")
                        title = "카카오톡";
                    else if (processName == "explorer")
                        title = "탐색기";
                    else if (processName == "notepad")
                        title = "메모장";
                    else if (processName == "devenv")
                        title = "Visual Studio 2013";
                    else if (processName == "WDExpress")
                        title = "Visual Studio 2015";
                    else if (processName == "chrome")
                        title = "Google Chrome";
                    else if (processName == "Slim_Student.vshost")
                        title = "SLIM 학생 디버깅 프로그램";
                    else if (processName == "Slim_professor.vshost")
                        title = "SLIM 강사 디버깅 프로그램";
                    else if (processName == "Slim_Student")
                        title = "SLIM 학생 프로그램";
                    else if (processName == "Slim_professor")
                        title = "SLIM 강사 프로그램";
                    else if (processName == "Microsoft.MicrosoftEdge_8wekyb3d8bbwe")
                        title = "Microsoft Edge";
                    else if (processName == "iexplore")
                        title = "Microsoft Internet Explorer";
                    else
                        title = "기타 프로그램(" + processName + ")";
                    #endregion
                    listItem = new GreenInfo
                    {
                        Id = items.Count - i,
                        AllGreenId = Convert.ToInt32(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.id)),
                        AllGreenTitle = title,
                        AllGreenProcessName = processName
                    };
                    data.Add(listItem);
                }
                return data;
            }

            #region AllGreenChangeCommand
            private ICommand _AllGreenChangeCommand;
            public ICommand AllGreenChangeCommand
            {
                get { return _AllGreenChangeCommand ?? (_AllGreenChangeCommand = new AppCommand(AllGreenChangeCommandFunc)); }
            }
            public void AllGreenChangeCommandFunc(Object o)
            {
                MessageBoxResult result = MessageBox.Show(AllGreenTitle+"을(를) 수업과 무관한 프로그램으로 전환(예) 그렇지 않으면(아니오)", "정상/비정상 프로그램 선택", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbAllProgram.UpdateRedGreen(AllGreenId, 0))
                    {
                        PageMainSubject.MainFrameObject.Refresh();
                        ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgramAll;
                    }
                }
            }
            #endregion

            #region AllGreenDeleteCommand
            private ICommand _AllGreenDeleteCommand;
            public ICommand AllGreenDeleteCommand
            {
                get { return _AllGreenDeleteCommand ?? (_AllGreenDeleteCommand = new AppCommand(AllGreenDeleteCommandFunc)); }
            }
            public void AllGreenDeleteCommandFunc(Object o)
            {
                if (MessageBox.Show("삭제하시겠습니까?", "삭제 경고", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                dbAllProgram.DeleteById(AllGreenId);
                PageMainSubject.MainFrameObject.Refresh();
                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgramAll;
            }
            #endregion
        }
    }
}
