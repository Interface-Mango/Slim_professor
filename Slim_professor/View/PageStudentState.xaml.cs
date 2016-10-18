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

namespace Slim_professor.View
{
    /// <summary>
    /// PageStudentState.xaml에 대한 상호 작용 논리
    /// </summary>
	public partial class PageStudentState : Page
    {
        /*
         public static int mId; //attendance id
         public static int mSubId; //sub_id
         public static string mStdId; //std_id
         public static DateTime mDate; //date(0000-00-00 00:00:00)
         public static int mCheck; // 1(출석) 2(지각) 3(결석)
         */
        public static object[] SelectSubjectInfo;
        public static string DateValue = DateTime.Now.ToShortDateString();
        private ViewModelPageStudentState viewModelPageStudentState;

        public PageStudentState()
		{
			InitializeComponent();
            viewModelPageStudentState = new ViewModelPageStudentState(this);
            viewModelPageStudentState.makeList(DateValue);
            //viewModelPageStudentState.makeList("2016-09-23");
            DataContext = viewModelPageStudentState;



            // 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
        }

        private void CalendarControl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: 위에 선택 날짜 표기하고 직접 새로고침 버튼 하나 표시
            //새로운 날짜로 리스트 생성
            DateValue = CalendarControl.SelectedDate.Value.ToShortDateString();
            CalendarControl.Visibility = System.Windows.Visibility.Hidden;
            //새로고침
            PageMainSubject.MainFrameObject.Refresh();
        }

     
    }
}