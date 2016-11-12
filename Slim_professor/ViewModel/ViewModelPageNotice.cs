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
    class ViewModelPageNotice : ViewModelBase
    {

        private DBManager dbManager;
        private DB_Notice dbNotice;
        private PageNotice parentWindow;

        // 생성자
        public ViewModelPageNotice(PageNotice pWindow)
        {
            dbManager = new DBManager();
            dbNotice = new DB_Notice(dbManager);
            parentWindow = pWindow;
            _ItemList = new List<object[]>();
        }

        #region NoticeItemList
        private List<NoticeInfo> _NoticeItemList;
        public List<NoticeInfo> NoticeItemList
        {
            get { return _NoticeItemList; }
            set
            {
                _NoticeItemList = value;
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

        #region WriteNoticeCommand
        private ICommand _WriteNoticeCommand;
        public ICommand WriteNoticeCommand
        {
            get { return _WriteNoticeCommand ?? (_WriteNoticeCommand = new AppCommand(WriteNoticeCommandFunc)); }
        }

        public void WriteNoticeCommandFunc(Object o)
        {
            PageWriteNote.isInsertUpdate = true;

            ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNoticeWrite;
        }
        #endregion

        public void makeList()
        {
            object[] userItems = MainFrame.UserInfo;    // 1. 유저(학생) 정보 가져오기
            object[] subItems = PageMainSubject.SubjectInfo;
            // _기준으로 배열에 string넣음
            string std_id = Convert.ToString(userItems[(int)DB_User.FIELD.user_id]);
            int sub_id = Convert.ToInt32(subItems[(int)DB_Subject.FIELD.sub_id]);

            ItemList = dbNotice.SelectNoticeList(Convert.ToInt32(sub_id));
            if (ItemList != null)
                NoticeItemList = NoticeInfo.Data(ItemList, dbNotice);
        }

        internal class NoticeInfo
        {
            private static List<NoticeInfo> data;
            private static DB_Notice dbNotice;

            public int Id { get; private set; }
            public int NoticeId { get; private set; }
            private string originTitle;
            private string _NoticeTitle;
            public string NoticeTitle
            {
                get
                {
                    return _NoticeTitle;
                }
                set
                {
                    originTitle = value;
                    if (value.Length > 100)
                    {
                        string temp = "";
                        for (int i = 0; i < 100; i++)
                        {
                            temp += value[i];
                        }
                        temp += "...";
                        _NoticeTitle = temp;
                    }
                    else
                    {
                        _NoticeTitle = value;
                    }
                }
            }
            public string NoticeContent { get; private set; }
            public int NoticeSubjectId { get; private set; }
            public DateTime NoticeDate { get; private set; }

            public static List<NoticeInfo> Data(List<object[]> items, DB_Notice _dbNotice)
            {
                NoticeInfo listItem;
                data = new List<NoticeInfo>();
                dbNotice = _dbNotice;

                for (int i = 0; i < items.Count; i++)
                {
                    listItem = new NoticeInfo
                    {
                        Id = items.Count - i,
                        NoticeId = Convert.ToInt32(items[i].ElementAt((int)DB_Notice.FIELD.id)),
                        NoticeTitle = Convert.ToString(items[i].ElementAt((int)DB_Notice.FIELD.title)),
                        NoticeContent = Convert.ToString(items[i].ElementAt((int)DB_Notice.FIELD.content)),
                        NoticeSubjectId = Convert.ToInt32(items[i].ElementAt((int)DB_Notice.FIELD.sub_id)),
                        NoticeDate = Convert.ToDateTime(items[i].ElementAt((int)DB_Notice.FIELD.date))
                    };
                    data.Add(listItem);
                }
                return data;
            }

            #region ListBoxItem_MouseDoubleClickCommand
            private ICommand _ListBoxItem_MouseDoubleClickCommand;
            public ICommand ListBoxItem_MouseDoubleClickCommand
            {
                get { return _ListBoxItem_MouseDoubleClickCommand ?? (_ListBoxItem_MouseDoubleClickCommand = new AppCommand(NoticeListBoxItem_MouseDoubleClickFunc)); }
            }
            public void NoticeListBoxItem_MouseDoubleClickFunc(Object o)
            {
                PageNoticeDetail.mId = NoticeId;
                PageNoticeDetail.mTitle = NoticeTitle;
                PageNoticeDetail.mContent = NoticeContent;
                PageNoticeDetail.mSubId = NoticeSubjectId;
                PageNoticeDetail.mDate = NoticeDate;

                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNoticeDetail;
            }
            #endregion

            #region NoticeModifyCommand
            private ICommand _NoticeModifyCommand;
            public ICommand NoticeModifyCommand
            {
                get { return _NoticeModifyCommand ?? (_NoticeModifyCommand = new AppCommand(NoticeModifyCommandFunc)); }
            }
            public void NoticeModifyCommandFunc(Object o)
            {
                PageWriteNote.mId = NoticeId;
                PageWriteNote.mTitle = NoticeTitle;
                PageWriteNote.mContent = NoticeContent;
                PageWriteNote.isInsertUpdate = false;

                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNoticeWrite;
            }
            #endregion

            #region NoticeDeleteCommand
            private ICommand _NoticeDeleteCommand;
            public ICommand NoticeDeleteCommand
            {
                get { return _NoticeDeleteCommand ?? (_NoticeDeleteCommand = new AppCommand(NoticeDeleteCommandFunc)); }
            }
            public void NoticeDeleteCommandFunc(Object o)
            {
                if (MessageBox.Show("삭제하시겠습니까?", "삭제 경고", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                dbNotice.DeleteNotice(NoticeId);
                PageMainSubject.MainFrameObject.Refresh();
                ViewModelMainSubject.MainSubjectObject.FrameSource = MainFrame.UriNotice;
            }
            #endregion
        }
    }
}
