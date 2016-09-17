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
    class ViewModelPageSubjectStatistic : ViewModelBase
    {
        /*
         * TODO 수업통계
         * ==날짜별로==
         * 빨간불 횟수
         * 질문 횟수
         * 과제
         * 진도
         * 특이사항
         * ============
         * 콤보박스로 날짜선택하고 위의 내용들 뜨도록
         * 
         * 테이블하나 만들어서 저장해야할듯tj
         */

        PageSubjectStatistic window;
        public ViewModelPageSubjectStatistic(PageSubjectStatistic pWindow)
        {
            window = pWindow;
        }

        #region ShowCalendar
        private ICommand _ShowCalendar;
        public ICommand ShowCalendar
        {
            get { return _ShowCalendar ?? (_ShowCalendar = new AppCommand(ShowCalendarFunc)); }
        }

        public void ShowCalendarFunc(Object o)
        {
            if (window.CalendarControl.Visibility == System.Windows.Visibility.Visible)
                window.CalendarControl.Visibility = System.Windows.Visibility.Hidden;
            else 
                window.CalendarControl.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

    }
}
