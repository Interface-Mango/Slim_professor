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
        Grid MainMenu;
		public SubjectList(Grid _MainMenu)
		{
			this.InitializeComponent();

            MainMenu = _MainMenu;
		}

		private void StartBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			//MainFrame mf = new MainFrame();
			//mf.MainMenu.Visibility = System.Windows.Visibility.Hidden;
			//MainMenu.Visibility = System.Windows.Visibility.Collapsed;
			
			MainMenu.Visibility = Visibility.Hidden;
			//SubjectList
			//SubjectMainFrame smf = new SubjectMainFrame();
			//smf.ShowDialog();
			
			
			
		}
	}
}