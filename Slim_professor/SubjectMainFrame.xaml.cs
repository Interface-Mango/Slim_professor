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
	/// <summary>
	/// SubjectMainFrame.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SubjectMainFrame : Window
	{
		public SubjectMainFrame()
		{
			this.InitializeComponent();
			
			NoticePage np = new NoticePage();
			ChangePageFrame.NavigationService.Navigate(np);
		}

		private void NoticeBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			NoticePage np = new NoticePage();
			ChangePageFrame.NavigationService.Navigate(np);
		}

		private void StudentStateBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			StudentStatePage ssp = new StudentStatePage();
			ChangePageFrame.NavigationService.Navigate(ssp);
		}

		private void SubjectStatsBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			SubjectStatsPage subsp = new SubjectStatsPage();
			ChangePageFrame.NavigationService.Navigate(subsp);
		}

		private void HiddenTalkBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			HiddenTalkPage hiddentalkpage = new HiddenTalkPage();
			ChangePageFrame.NavigationService.Navigate(hiddentalkpage);
		}

		private void QnABtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			QnAPage qnapage = new QnAPage();
			ChangePageFrame.NavigationService.Navigate(qnapage);
		}
	}
}