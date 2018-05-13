using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _201518100120
{
    public partial class measureresult : Form
    {
        public delegate void FormClosedEventHandler();
        public event FormClosedEventHandler frmClosed=null;//定义一个空的委托事件
        
        //窗口关闭时引发委托事件。
        public measureresult()
        {
            InitializeComponent();
            label2.Text = "";
            
        }//构造窗口

        private void measureresult_Load(object sender, EventArgs e)
        {

        }

        private void measureresult_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmClosed != null)//判断委托是否为空
                frmClosed();

        }
    }
}
