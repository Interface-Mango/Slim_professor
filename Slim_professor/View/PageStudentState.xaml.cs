﻿using System;
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


        public PageStudentState()
		{
			InitializeComponent();
            ViewModelPageStudentState ViewModelPageStudentState = new ViewModelPageStudentState(this);
            ViewModelPageStudentState.makeList();
            DataContext = ViewModelPageStudentState;



            // 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
        }

     
    }
}