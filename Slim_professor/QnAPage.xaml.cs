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
	public partial class QnAPage
	{
		public QnAPage()
		{
			this.InitializeComponent();

			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
		}

        private void StudentStateBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("StudentStatePage.xaml", UriKind.Relative));
        }

        private void HiddenTalkBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("HiddenTalkPage.xaml", UriKind.Relative));
        }

        private void SubjectStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("SubjectStatsPage.xaml", UriKind.Relative));
        }

        private void NoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("NoticePage.xaml", UriKind.Relative));
        }
	}
}