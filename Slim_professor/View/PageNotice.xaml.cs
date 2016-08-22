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
using Slim_professor.ViewModel;

namespace Slim_professor.View
{
	public partial class PageNotice : Page
	{
		public PageNotice()
		{
			this.InitializeComponent();
            ViewModelPageNotice viewModelPageNotice = new ViewModelPageNotice(this);
            viewModelPageNotice.makeList();
            DataContext = viewModelPageNotice;
		}


	}
}