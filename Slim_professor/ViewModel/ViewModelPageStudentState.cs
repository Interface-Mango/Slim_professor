﻿using MVVMBase.Command;
using MVVMBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Slim_professor.Model;
using Slim_professor.View;

namespace Slim_professor.ViewModel
{
    /* 알고리즘
     *
     * 1. 선택 과목 정보 가져오기(sub_id 가져오기) 
     * 2. User 테이블에서 해당 과목을 듣는 학생 아이디(std_id)와 이름 (user_name)뽑아오기 
     * 3. 다시 attendance 테이블에서 user_id 이용해서 std_id와 조인해서 date와 check 얻어오기
     * 4. check 는 1은 출석, 2는 지각, 3은 결석으로 만들어놓기 
     * 5. 나중에 led 추가
     * 
     **/
    class ViewModelPageStudentState : ViewModelBase
    {
        private DBManager dbManager;
        private DB_Attendance dbAttendnace;
        private PageStudentState parentWindow;

        //생성자
        public ViewModelPageStudentState(PageStudentState pWindow)
        {
            dbManager = new DBManager();
            dbAttendnace = new DB_Attendance(dbManager);
            parentWindow = pWindow;
            _ItemList = new List<object[]>();
        }

        #region ShowCalendar
        private ICommand _ShowCalendar;
        public ICommand ShowCalendar1
        {
            get { return _ShowCalendar ?? (_ShowCalendar = new AppCommand(ShowCalendarFunc)); }
        }

        public void ShowCalendarFunc(Object o)
        {
            if (parentWindow.CalendarControl.Visibility == System.Windows.Visibility.Visible)
                parentWindow.CalendarControl.Visibility = System.Windows.Visibility.Hidden;
            else
                parentWindow.CalendarControl.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region StudentStateRefresh
        private ICommand _StudentStateRefresh;
        public ICommand StudentStateRefresh
        {
            get { return _StudentStateRefresh ?? (_StudentStateRefresh = new AppCommand(StudentStateRefreshFunc)); }
        }

        public void StudentStateRefreshFunc(Object o)
        {
            PageMainSubject.MainFrameObject.Refresh();
        }
        #endregion

        #region StudentStateItemList
        private List<AttendanceInfo> _AttendanceItemList;
        public List<AttendanceInfo> AttendanceItemList
        {
            get { return _AttendanceItemList; }
            set
            {
                _AttendanceItemList = value;
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




        /* makeList(void) 메소드
* 기능:     
*/
        public string BindingDate
        {
            get;
            set;
        }

        #region makeList
        public void makeList(string date)
        //public void makeList()
        {
            ItemList = null;
            AttendanceItemList = null;

            //1.선택 과목 정보 가져오기(sub_id 가져오기)
            DB_User dbUser = new DB_User(new DBManager());


            List<object[]> items = new List<object[]>();
            items = dbAttendnace.SelectAttendanceList(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), date);
            //items = dbAttendnace.SelectAttendanceList(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
            if (items == null) return;
            for(int i=0;i<items.Count;i++)
            {
                string name = Convert.ToString(dbUser.SelectUser(Convert.ToString(items[i].ElementAt((int)DB_Attendance.FIELD.std_id)))[(int)DB_User.FIELD.user_name]);
                items[i].SetValue(name, 5);
            }

            ItemList = items;

            AttendanceItemList = AttendanceInfo.Data(ItemList);
        }
        #endregion


        // attendance table 관하여

        internal class AttendanceInfo
        {
            private static List<AttendanceInfo> data;
            //private static DB_Attendance dbAttendnace;

            public int Id { get; private set; }
            public int AttendId { get; private set; }
            public string AttendStudentName { get; private set; } //user 에서 가져옴
            public int AttendSubjectId { get; private set; }
            public string AttendStudentId { get; private set; }
            public string AttendTime { get; private set; }
            public string AttendCheck { get; private set; }

            public Brush AttendCheckColor { get; private set; }



            public static List<AttendanceInfo> Data(List<object[]> items)
            {
                AttendanceInfo attendTemp;
                data = new List<AttendanceInfo>();

                for (int i = 0; i < items.Count; i++)
                {
                    string checkTemp = GetCheck(Convert.ToInt32(items[i].ElementAt((int)DB_Attendance.FIELD.check)));
                    SolidColorBrush b = GetFontColor(Convert.ToInt32(items[i].ElementAt((int)DB_Attendance.FIELD.check)));
                    string date = "";
                    if(Convert.ToInt32(items[i].ElementAt((int)DB_Attendance.FIELD.check)) == 1)
                        date = Convert.ToString(items[i].ElementAt((int)DB_Attendance.FIELD.date));

                    attendTemp = new AttendanceInfo
                    {
                        Id = items.Count - i,
                        AttendId = Convert.ToInt32(items[i].ElementAt((int)DB_Attendance.FIELD.id)),
                        AttendStudentName = Convert.ToString(items[i].ElementAt(5)),
                        AttendSubjectId = Convert.ToInt32(items[i].ElementAt((int)DB_Attendance.FIELD.sub_id)),
                        AttendStudentId = Convert.ToString(items[i].ElementAt((int)DB_Attendance.FIELD.std_id)),
                        AttendTime = date,
                        AttendCheck = checkTemp,
                        AttendCheckColor = b
                    };
                    data.Add(attendTemp);

                }
                return data;
            }

            private static SolidColorBrush GetFontColor(int attCheck)
            {
                if (attCheck == 0)
                    return Brushes.Red;
                return Brushes.Blue;
            }

            //출석여부 받아오기 
            private static string GetCheck(int check) 
            {
                string checkTemp="미출석";
                if (check == 1)
                    checkTemp = "출석";
                else if (check == 2)
                    checkTemp = "지각";
                else if (check == 0)
                    checkTemp = "결석";

                return checkTemp;
            }

            internal static object Data()
            {
                throw new NotImplementedException();
            }
        }
    }
        
}
