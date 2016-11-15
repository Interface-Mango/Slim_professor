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
using System.Windows.Shapes;
using Slim_professor.Model;

namespace Slim_professor.View
{
    /// <summary>
    /// Widget.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Widget : Window
    {
        private const int FINISH_CLASS = 0;
        private DB_OnetimeProgram dbOnetimeProgram;
        private DB_Subject dbSubject;
        public Widget()
        {
            InitializeComponent();

            dbOnetimeProgram = new DB_OnetimeProgram(new DBManager());
            dbSubject = new DB_Subject(new DBManager());

            //위젯 창의 위치(왼쪽 위)
            this.Left = SystemParameters.WorkArea.Width - SystemParameters.WorkArea.Width;
            this.Top = SystemParameters.WorkArea.Height - (SystemParameters.WorkArea.Height);
            subTitle.Text = Convert.ToString(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_name));
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "수업 종료", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), string.Empty, 0);      // 수업 종료 DB 변경
                dbSubject.UpdateIsProcessing(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), FINISH_CLASS);   // 수업 종료 DB 변경
                dbOnetimeProgram.DeleteOneTime(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
                MainFrame.Frame.Close();
                this.Close();
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Frame.Show();
            this.Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
