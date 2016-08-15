using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Slim_professor.ViewModel;

namespace Slim_professor.View
{
    /// <summary>
    /// Interaction logic for PageMainSubject.xaml
    /// </summary>
    public partial class PageMainSubject : Page
    {
        public PageMainSubject(object[] param)
        {
            InitializeComponent();
            ViewModelMainSubject viewModelMainSubject = new ViewModelMainSubject();
            DataContext = viewModelMainSubject;
            viewModelMainSubject.FrameSource = new Uri("PageStudentState.xaml", UriKind.Relative);

            SubName.Text = param.ElementAt(1).ToString();
        }

        #region 메뉴 버튼들

        private void CloseBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainFrame.Frame.Close();
        }

        private void canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	FramePanel.Source = new Uri("PageStudentState.xaml", UriKind.Relative);
        }

        private void canvas1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	 FramePanel.Source = new Uri("PageHiddenTalk.xaml", UriKind.Relative);
        }

        private void canvas2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FramePanel.Source = new Uri("PageSubjectStatistic.xaml", UriKind.Relative);
        }

        private void canvas3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FramePanel.Source = new Uri("PageNotice.xaml", UriKind.Relative);
        }

        private void WidgetBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Widget widget = new Widget();
            widget.Show();
            MainFrame.Frame.Hide();
        }

        #endregion

        private void HomeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // To do 나중에
        }


    }
}
