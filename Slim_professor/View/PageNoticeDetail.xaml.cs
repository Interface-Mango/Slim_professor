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
    /// PageNoticeDetail.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageNoticeDetail : Page
    {
        public static int mId;
        public static string mTitle;
        public static string mContent;
        public static int mSubId;
        public static DateTime mDate;

        public PageNoticeDetail()
        {
            InitializeComponent();
            DataContext = new ViewModelPageNoticeDetail(this, mId, mTitle, mContent, mSubId, mDate);
        }
    }
}
