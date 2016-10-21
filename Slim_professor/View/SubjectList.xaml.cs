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

namespace Slim_professor.View
{
	public partial class SubjectList
	{
        //public static bool isLoaded = false;
       
		public SubjectList()
		{
            InitializeComponent();

            ViewModelSubjectList viewModelSubjectList = new ViewModelSubjectList(this);
            viewModelSubjectList.makeList();
            DataContext = viewModelSubjectList;
            //isLoaded = true;
		}

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            MainFrame.Frame.Close();
        }
    }
}