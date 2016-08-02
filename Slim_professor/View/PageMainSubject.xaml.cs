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

        private void StudentStateBtn_Click(object sender, RoutedEventArgs e)
        {
            FramePanel.Source = new Uri("PageStudentState.xaml", UriKind.Relative);
        }

        private void HiddenTalkBtn_Click(object sender, RoutedEventArgs e)
        {
            FramePanel.Source = new Uri("PageHiddenTalk.xaml", UriKind.Relative);
        }

        private void SubjectStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            FramePanel.Source = new Uri("PageSubjectStatistic.xaml", UriKind.Relative);
        }

        private void QnABtn_Click(object sender, RoutedEventArgs e)
        {
            FramePanel.Source = new Uri("PageQnA.xaml", UriKind.Relative);
        }

        private void NoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            FramePanel.Source = new Uri("PageNotice.xaml", UriKind.Relative);
        }

       


       
    }
}
