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

        #region StudentStateCommand(나중에 필요하면 추가)
        private ICommand _WriteStudentStateCommand;
        // 나중에
        #endregion


        /* makeList(void) 메소드
* 기능:     
*/
        public void makeList()
        {
            //1.선택 과목 정보 가져오기(sub_id 가져오기)
            DBManager dbm = new DBManager();
            DB_User dbUser = new DB_User(dbm);

            ItemList = dbAttendnace.SelectAttendanceList(dbUser.SelectStudent(0));

            AttendanceItemList = AttendanceInfo.Data(ItemList);
            
        }
        //위의 데이터에서 sub_ids에서 _를 기준으로 데이터 구분하기



        // attendance table 관하여

        internal class AttendanceInfo
        {
            private static List<AttendanceInfo> data;
            private static DB_Attendance dbAttendnace;


            public int AttendId { get; private set; }
            public string AttendStudentName { get; private set; } //user 에서 가져옴
            public int AttendSubjectId { get; private set; }
            public string AttendStudentId { get; private set; }
            public DateTime AttendTime { get; private set; }
            public int AttendCheck { get; private set; }



            public static List<AttendanceInfo> Data(List<object[]> items)
            {
                AttendanceInfo attendTemp;
                data = new List<AttendanceInfo>();

                for (int i = 0; i < items.Count; i++)
                {
                    string studentNameTemp = GetStudentName(Convert.ToString(items[i].ElementAt((int)DB_Subject.FIELD.sub_id)));
                    string studentIdTemp = GetStudentId(Convert.ToString(items[i].ElementAt((int)DB_Subject.FIELD.sub_id)));

                }
                return data;
            }

            //User 테이블에서 해당 과목을 듣는 학생 이름(user_name)뽑아오기
            private static string GetStudentName(string sub_id)
            {
                DBManager dbm = new DBManager();
                DB_User dbUser = new DB_User(dbm);
                return Convert.ToString(dbUser.SelectUser(sub_id)[(int)DB_User.FIELD.user_name]);
            }

            //User 테이블에서 해당 과목을 듣는 학생 아이디(std_id)뽑아오기
            private static string GetStudentId(string sub_id)
            {
                DBManager dbm = new DBManager();
                DB_User dbUser = new DB_User(dbm);
                return Convert.ToString(dbUser.SelectUser(sub_id)[(int)DB_User.FIELD.user_id]);
            }

            internal static object Data()
            {
                throw new NotImplementedException();
            }
        }
    }
        
}
