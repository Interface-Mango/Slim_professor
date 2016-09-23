using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Threading;
using Slim_professor.Model;

namespace Slim_professor.View
{
	/// <summary>
	/// MainFrame.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainFrame : NavigationWindow
    {
        public static object[] UserInfo;
        public static MainFrame Frame;

        private DB_Subject dbSubject;
        private const int FINISH_CLASS = 0;

        public MainFrame(object[] _userInfo)
		{
            this.InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            Frame = this;
            UserInfo = _userInfo;

            dbSubject = new DB_Subject(new DBManager());

            // 창 중앙 위치!!
           // this.Left = (SystemParameters.WorkArea.Width - Width) / 2.0 + SystemParameters.WorkArea.Left;
           // this.Height = (SystemParameters.WorkArea.Height - Height) / 2.0 + SystemParameters.WorkArea.Top;

           
		}

		// 로그인 창과 호환되기 위한 함수
		protected override void OnClosed(EventArgs e)
        {
            dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), string.Empty, 0);      // 수업 종료 DB 변경
            dbSubject.UpdateIsProcessing(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), FINISH_CLASS);   // 창 강제 종료시 수업 종료
			base.OnClosed(e); 
			Application.Current.Shutdown();
		}

        private void Main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ButtonState == MouseButtonState.Pressed)   // 닫기 버튼 때문에 해줘야 됨
                this.DragMove();
        }

        public static MainFrame thisMainFrame()
        {
            return Frame;
        }

	}
}