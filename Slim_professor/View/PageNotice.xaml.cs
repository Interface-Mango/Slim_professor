﻿using System;
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

namespace Slim_professor.View
{
	public partial class PageNotice : Page
	{
		public PageNotice()
		{
			this.InitializeComponent();

			
		}

		
		private void TitleClick(object sender, System.Windows.RoutedEventArgs e)
		{
			
            DialogNotice dialognotice = new DialogNotice();
			dialognotice.ShowDialog();
		}

		private void RegBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			EditNotice en = new EditNotice();
			en.ShowDialog();
		}


	}
}