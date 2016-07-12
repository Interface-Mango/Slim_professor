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

namespace Slim_professor
{
	public partial class SubjectList
	{
       
		public SubjectList()
		{
			this.InitializeComponent();
            
		}

		private void StartBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
            NavigationService.Navigate(new Uri("NoticePage.xaml", UriKind.Relative));
		}


        private void AddSubBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("AddSubject.xaml", UriKind.Relative));
        }


	}
}