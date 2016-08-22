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

        private void CloseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Frame.Close();
        }


    }
}