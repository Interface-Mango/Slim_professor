﻿using System;
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

namespace Slim_professor.View
{
    /// <summary>
    /// ProgressRIng.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProgressRIng : Window
    {
        private object[] obj;
        public ProgressRIng(object[] _obj)
        {
            InitializeComponent();
            obj = _obj;
        }

        public void AutoClose(object sender, EventArgs e)
        {
            MainFrame mf = new MainFrame(obj);
            mf.Show();
            this.Close();
        }
    }
}