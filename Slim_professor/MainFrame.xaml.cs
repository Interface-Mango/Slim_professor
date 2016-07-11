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
	/// MainFrame.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainFrame : Window
	{
		public MainFrame()
		{
			this.InitializeComponent();
			
			SubjectList sl = new SubjectList(MainMenu);
			ChangePageFrame.NavigationService.Navigate(sl);
			

		}
		
		// Git 연습 주석
		// 로그인 창과 호환되기 위한 함수
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e); 
			Application.Current.Shutdown();
		}

		// 수업리스트버튼 클릭시
		private void SubListBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			//SubjectList sl = new SubjectList();
			ChangePageFrame.NavigationService.Navigate( new Uri("SubjectList.xaml",UriKind.Relative));
		}

		// 수업추가버튼 클릭시
		private void AddSubBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			AddSubject addsub = new AddSubject();
			ChangePageFrame.NavigationService.Navigate(addsub);
		}
	}
}