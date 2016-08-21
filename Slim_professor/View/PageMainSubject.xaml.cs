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
        public static object[] SubjectInfo;
        public static Frame MainFrameObject;

        private SubjectList _subjectlist;

        public PageMainSubject(object[] param, SubjectList subjectlist)
        {
            InitializeComponent();
            DataContext = new ViewModelMainSubject(subjectlist);

            MainFrameObject = FramePanel;
            ViewModelMainSubject.MainSubjectObject.FrameSource = new Uri("PageStudentState.xaml", UriKind.Relative);
            SubjectInfo = param;
            SubName.Text = SubjectInfo.ElementAt(1).ToString();
            _subjectlist = subjectlist;
        }
    }
}
