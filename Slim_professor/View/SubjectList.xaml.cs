using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Slim_professor.ViewModel;
using Slim_professor.View;

namespace Slim_professor
{
	public partial class SubjectList
	{
       
		public SubjectList()
		{
            this.InitializeComponent();

            ViewModelSubjectList viewModelSubjectList = new ViewModelSubjectList(this);
            viewModelSubjectList.makeList();
            DataContext = viewModelSubjectList;
		}

        #region 위젯버튼 & 닫기버튼
        private void WidgetBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Widget widget = new Widget();
            widget.Show();
            MainFrame.Frame.Hide();
        }

        private void CloseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Frame.Close();
        }
        #endregion


    }
}