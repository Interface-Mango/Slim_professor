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

namespace Slim_professor
{
	/// <summary>
	/// EditNotice.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class EditNotice : Window
	{
		public EditNotice()
		{
			this.InitializeComponent();
			
			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
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