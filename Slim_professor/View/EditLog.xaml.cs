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
	/// <summary>
	/// EditLog.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class EditLog : Window
	{
		public EditLog()
		{
			this.InitializeComponent();
		}
		
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
		}

		private void RegBtnClick(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}

		
	}
}