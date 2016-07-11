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
	public partial class SubjectStatsPage
	{
		public SubjectStatsPage()
		{
			this.InitializeComponent();

			
		}

		private void ShowBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			ShowLog showlog = new ShowLog();
			showlog.ShowDialog();
		}

		private void RegBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			EditLog editlog = new EditLog();
			editlog.ShowDialog();
		}
	}
}