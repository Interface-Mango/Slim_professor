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
    class ViewModelPageManageProgram : ViewModelBase
    {

        private DBManager dbManager;
        private DB_OnetimeProgram dbOnetimeProgram;
        private DB_AllProgram dbAllProgram;
        //private PageManageProgram parentWindow;

        // 생성자
        public ViewModelPageManageProgram(/*PageManageProgram pWindow*/)
        {
            dbManager = new DBManager();
            dbOnetimeProgram = new DB_OnetimeProgram(dbManager);
            dbAllProgram = new DB_AllProgram(dbManager);
            //parentWindow = pWindow;
            _ItemList = new List<object[]>();
        }

        #region OneTimeList
        private List<OnetimeInfo> _OneTimeList;
        public List<OnetimeInfo> OneTimeList
        {
            get { return _OneTimeList; }
            set
            {
                _OneTimeList = value;
            }
        }
        #endregion

        #region ItemList
        private List<object[]> _ItemList;
        public List<object[]> ItemList
        {
            get { return _ItemList; }
            set { _ItemList = value; }
        }
        #endregion

        #region AllProgramCommand
        private ICommand _AllProgramCommand;
        public ICommand AllProgramCommand
        {
            get { return _AllProgramCommand ?? (_AllProgramCommand = new AppCommand(AllProgramCommandFunc)); }
        }

        public void AllProgramCommandFunc(Object o)
        {
            PageWriteNote.isInsertUpdate = true;

            ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgramAll;
        }
        #endregion

        public void makeList()
        {
            object[] userItems = MainFrame.UserInfo;    // 1. 유저(학생) 정보 가져오기
            object[] subItems = PageMainSubject.SubjectInfo;
            // _기준으로 배열에 string넣음
            string std_id = Convert.ToString(userItems[(int)DB_User.FIELD.user_id]);
            int sub_id = Convert.ToInt32(subItems[(int)DB_Subject.FIELD.sub_id]);

            ItemList = dbOnetimeProgram.SelectOneTimeListAll(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            if (ItemList != null)
                OneTimeList = OnetimeInfo.Data(ItemList, dbOnetimeProgram, dbAllProgram);
        }

        internal class OnetimeInfo
        {
            private static List<OnetimeInfo> data;
            private static DB_OnetimeProgram dbOnetimeProgram;
            private static DB_AllProgram dbAllProgram;

            public int Id { get; private set; }
            public int OnetimeId { get; private set; }
            public string OneTitle { get; private set; }
            public string OneProcessName { get; private set; }

            public static List<OnetimeInfo> Data(List<object[]> items, DB_OnetimeProgram _dbOnetimeProgram, DB_AllProgram _dbAllProgram)
            {
                OnetimeInfo listItem;
                data = new List<OnetimeInfo>();
                dbOnetimeProgram = _dbOnetimeProgram;
                dbAllProgram = _dbAllProgram;

                for (int i = 0; i < items.Count; i++)
                {//하드코딩
                    string processName= Convert.ToString(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.process_name));
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

                    listItem = new OnetimeInfo
                    {
                        Id = items.Count - i,
                        OnetimeId = Convert.ToInt32(items[i].ElementAt((int)DB_OnetimeProgram.FIELD.id)),
                        OneTitle = title,
                        OneProcessName = processName
                    };
                    data.Add(listItem);
                }
                return data;
            }

            #region OneRegistToAllCommand
            private ICommand _OneRegistToAllCommand;
            public ICommand OneRegistToAllCommand
            {
                get { return _OneRegistToAllCommand ?? (_OneRegistToAllCommand = new AppCommand(OneRegistToAllCommandFunc)); }
            }
            public void OneRegistToAllCommandFunc(Object o)
            {
                //TODO: 
                MessageBoxResult result = MessageBox.Show("수업 관련 프로그램(예) 그렇지 않으면(아니오)", "정상/비정상 프로그램 선택", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) 
                {
                    if (dbAllProgram.InsertAllProgram(OneProcessName, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), Convert.ToString(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.lectureler_id)), 1, 0))
                    {
                        MessageBox.Show("등록 되었습니다.");
                    }
                    else
                    {
                        MessageBox.Show("이미 등록되어 있습니다.");
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    if (dbAllProgram.InsertAllProgram(OneProcessName, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), Convert.ToString(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.lectureler_id)), 0, 1))
                    {
                        MessageBox.Show("등록 되었습니다.");
                    }
                    else
                    {
                        MessageBox.Show("이미 등록되어 있습니다.");
                    }
                }
            }
            #endregion

            #region OneDeleteCommand
            private ICommand _OneDeleteCommand;
            public ICommand OneDeleteCommand
            {
                get { return _OneDeleteCommand ?? (_OneDeleteCommand = new AppCommand(OneDeleteCommandFunc)); }
            }
            public void OneDeleteCommandFunc(Object o)
            {
                if (MessageBox.Show("삭제하시겠습니까?", "삭제 경고", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                dbOnetimeProgram.DeleteOneTimeById(OnetimeId);
                PageMainSubject.MainFrameObject.Refresh();
                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriManageProgram;
            }
            #endregion
        }
    }
}
