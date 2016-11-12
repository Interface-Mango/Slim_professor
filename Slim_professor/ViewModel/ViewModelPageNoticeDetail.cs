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
    class ViewModelPageNoticeDetail : ViewModelBase
    {
        private PageNoticeDetail pNoticeDetail;

        public ViewModelPageNoticeDetail(PageNoticeDetail pDetail, int id, string title, string content, int sub_id, DateTime date)
        {
            pNoticeDetail = pDetail;
            NoticeDetailTitle = title;
            NoticeDetailContent = content;
            NoticeDetailDate = date;
        }

        #region NoticeDetailTitle
        private string _NoticeDetailTitle;
        public string NoticeDetailTitle
        {
            get { return _NoticeDetailTitle; }
            set
            {
                if (_NoticeDetailTitle != value)
                {
                    _NoticeDetailTitle = value;
                    OnPropertyChanged("NoticeDetailTitle");
                }
            }
        }
        #endregion

        #region NoticeDetailContent
        private string _NoticeDetailContent;
        public string NoticeDetailContent
        {
            get { return _NoticeDetailContent; }
            set
            {
                if (_NoticeDetailContent != value)
                {
                    _NoticeDetailContent = value;
                    OnPropertyChanged("NoticeDetailContent");
                }
            }
        }
        #endregion

        #region NoticeDetailDate
        private DateTime _NoticeDetailDate;
        public DateTime NoticeDetailDate
        {
            get { return _NoticeDetailDate; }
            set
            {
                if (_NoticeDetailDate != value)
                {
                    _NoticeDetailDate = value;
                    OnPropertyChanged("NoticeDetailDate");
                }
            }
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
