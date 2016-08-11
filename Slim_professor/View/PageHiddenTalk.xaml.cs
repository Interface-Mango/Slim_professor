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
	public partial class PageHiddenTalk : Page
	{
		public PageHiddenTalk()
		{
			this.InitializeComponent();
            DataContext = new ViewModelPageHiddenTalk
                (this, msgTextBox2, PortBox, IDText, ServerConnectingBtn, ServerConnectBtn);
            msgTextBox.IsReadOnly = true;
            msgTextBox1.IsReadOnly = true;
            PortBox.MaxLength = 4;
		}

        private delegate void SetTextCallback(String nMessage);
      
        // 서버 Log 기록
        public void DisplayLog(String nMessage)
        {
            //TODO : 선생이름 가져오기
            if (msgTextBox.Dispatcher.CheckAccess())
            {
                msgTextBox.AppendText(" " + nMessage + "\n");
                msgTextBox.ScrollToEnd();
                msgTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                SetTextCallback d = new SetTextCallback(DisplayLog);
                msgTextBox.Dispatcher.BeginInvoke(d, new Object[] { nMessage });
            }
        }

        //Client 내용 기록
        public void DisplayMsg(String nMessage)
        {
            if (msgTextBox1.Dispatcher.CheckAccess())
            {
                msgTextBox1.AppendText(" " + nMessage + "\n");
                msgTextBox1.ScrollToEnd();
                msgTextBox1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                SetTextCallback d = new SetTextCallback(DisplayMsg);
                msgTextBox1.Dispatcher.BeginInvoke(d, new Object[] { nMessage });
            }
        }

        #region portBox를 숫자만 입력하게
        private string prevText;
        private void PortBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            double value;
            
            if (double.TryParse(textBox.Text, out value))
            {
                this.prevText = textBox.Text;
            }
            else
            {
                textBox.Text = this.prevText;
                textBox.SelectionLength = this.prevSelectionLength;
                textBox.SelectionStart = this.prevSelectionStart;
            }

        }

        int prevSelectionStart;
        int prevSelectionLength;
        private void PortBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            this.prevSelectionStart = textbox.SelectionStart;
            this.prevSelectionLength = textbox.SelectionLength;
        }

        #endregion

    }
}