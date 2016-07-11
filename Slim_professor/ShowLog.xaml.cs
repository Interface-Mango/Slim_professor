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
	/// ShowLog.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ShowLog : Window
	{
		public ShowLog()
		{
			this.InitializeComponent();
			
			
		}

		private void EditBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			EditLog editlog = new EditLog();
			Hide();
			editlog.ShowDialog();
		}
	}
}