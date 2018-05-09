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
    public partial class bookmark : Form
    {
        public string Bookmark;
        public bookmark()
        {
            InitializeComponent();
            Bookmark = textBox1.Text = "";//书签名字先为空
            button1.Enabled = false;//确定按钮为不可按
        }

        private void bookmark_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bookmark = textBox1.Text;
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.Text != "";
        }
    }
}
