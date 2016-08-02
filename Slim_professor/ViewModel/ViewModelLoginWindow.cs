using MVVMBase.Command;
using MVVMBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Slim_professor.Model;
using Slim_professor.View;

namespace Slim_professor.ViewModel
{
    class ViewModelLoginWindow : ViewModelBase

    {
        private DBManager dbManager;
        private DB_User dbUser;
        private LoginWindow parentWindow;

        public ViewModelLoginWindow(LoginWindow pWindow)
        {
            dbManager = new DBManager();
            dbUser = new DB_User(dbManager);
            parentWindow = pWindow;
        }

        #region IDTextBox
        private string _IDTextBox;
        public string IDTextBox
        {
            get { return _IDTextBox; }
            set
            {
                if(_IDTextBox != value)
                {
                    _IDTextBox = value;
                    OnPropertyChanged("IDTextBox");
                }
            }
        }
        #endregion
        
        #region LoginCommand
        private ICommand _LoginCommand;
        public ICommand LoginCommand
        {
            get { return _LoginCommand ?? (_LoginCommand = new AppCommand(LoginCommandFunc)); }
        }
        /* LoginCommandFunc
         * 기능 : 로그인 버튼 눌렀을 때 회원검사하는 함수
         */
        private void LoginCommandFunc(Object o)
        {
            if (IDTextBox == string.Empty || parentWindow.PWBox.Password == string.Empty)
                return;
            object[] obj = dbUser.SelectUser(IDTextBox, parentWindow.PWBox.Password);
           
            //string name = (string)obj[(int)DB_User.FIELD.user_name];
            
            if (obj == null)
            {
                MessageBox.Show("로그인 에러!");
            }                
            else
            {
                // 기존의 로그인창을 '일단' 숨겨놓고 메인프레임 호출
                // 메인프레임에서 로그인창을 닫아준다
                obj[(int)DB_User.FIELD.pw] = string.Empty;
                MainFrame mf = new MainFrame(obj);

                parentWindow.Hide();
                mf.ShowDialog(); 
            }
        }
        #endregion
    }
}
