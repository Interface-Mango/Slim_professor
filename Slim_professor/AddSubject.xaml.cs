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
	public partial class AddSubject
	{
		public AddSubject()
		{
			this.InitializeComponent();

			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
		}

        private void SubListBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("SubjectList.xaml", UriKind.Relative));
        }
	}
}