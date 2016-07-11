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

namespace Slim_professor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		// 로그인 버튼 누를시에 호출되는 함수
		// 기존의 로그인창을 '일단' 숨겨놓고 메인프레임 호출
		// 메인프레임에서 로그인창을 닫아준다
        private void loginBtnClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	MainFrame mf = new MainFrame();
			Hide();
			mf.ShowDialog();
        }
    }
}
