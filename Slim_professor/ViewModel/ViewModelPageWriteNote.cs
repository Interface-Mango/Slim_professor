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
    class ViewModelPageWriteNote : ViewModelBase
    {
        private DBManager dbManager;
        private DB_Notice dbNotice;
        private PageWriteNote parentWindow;

        private int id;
        private bool isInsertUpdate;    // 0: update, 1: insert

        // 생성자
        public ViewModelPageWriteNote(PageWriteNote pWindow)
        {
            dbManager = new DBManager();
            dbNotice = new DB_Notice(dbManager);
            parentWindow = pWindow;
            isInsertUpdate = true;
        }
        public ViewModelPageWriteNote(PageWriteNote pWindow, int _id, string title, string content)
        {
            dbManager = new DBManager();
            dbNotice = new DB_Notice(dbManager);
            parentWindow = pWindow;
            id = _id;
            NoticeTitleTextBox = title;
            NoticeContentTextBox = content;
            isInsertUpdate = false;
        }

        #region NoticeTitleTextBox
        private string _NoticeTitleTextBox;
        public string NoticeTitleTextBox
        {
            get
            {
                return _NoticeTitleTextBox;
            }
            set
            {
                _NoticeTitleTextBox = value;
            }
        }
        #endregion

        #region NoticeContentTextBox
        private string _NoticeContentTextBox;
        public string NoticeContentTextBox
        {
            get
            {
                return _NoticeContentTextBox;
            }
            set
            {
                _NoticeContentTextBox = value;
            }
        }
        #endregion

        #region SaveNoticeCommand;
        private ICommand _SaveNoticeCommand;
        public ICommand SaveNoticeCommand
        {

            get { return _SaveNoticeCommand ?? (_SaveNoticeCommand = new AppCommand(SaveNoticeCommandFunc)); }
        }

        public void SaveNoticeCommandFunc(Object o)
        {
            if (NoticeTitleTextBox == null || NoticeContentTextBox == null) return;
            if (isInsertUpdate)
            {
                if (dbNotice.InsertNotice(NoticeTitleTextBox, NoticeContentTextBox, Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id))))
                {
                    MessageBox.Show("저장되었습니다.");
                }
                else
                {
                    MessageBox.Show("실패하였습니다.");
                }
            }
            else
            {
                if (dbNotice.UpdateNotice(id, NoticeTitleTextBox, NoticeContentTextBox))
                {
                    MessageBox.Show("수정되었습니다.");
                }
                else
                {
                    MessageBox.Show("실패하였습니다.");
                }
            }

            ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNotice;
        }
        #endregion

        #region BackCommand
        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get { return _BackCommand ?? (_BackCommand = new AppCommand(BackCommandFunc)); }
        }
        public void BackCommandFunc(Object o)
        {
            ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNotice;
        }
        #endregion
    }
}
