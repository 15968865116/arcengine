using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace _201518100120
{
    public partial class mapexport : Form
    {
        private IActiveView pActiveView;
        private string pSavePath = "";//导出路径
        public IGeometry geometry;//地图导出的空间图形
        public bool isRegion;//定义一个判断变量,判断是全域导出还是自由区域导出
        public mapexport(AxMapControl mainAxMapControl)
        {
            InitializeComponent();
            pActiveView = mainAxMapControl.ActiveView;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdExportMap = new SaveFileDialog
            {
                DefaultExt="jpg|bmp|gif|tif|pdf",
                Filter = "JPEG文件(*.jpg)|*.jpg;|BMP文件(*.bmp)|*.bmp;|GIF文件(*.gif)|*.gif;|TIF文件(*.tif)|*.tif;|PNG文件(*.png)|*.png;|PDF文件(*.pdf)|*.pdf",
                OverwritePrompt=true, 
            };
            textBox3.Text = "";//路径初始为空
            if (sfdExportMap.ShowDialog() == DialogResult.Cancel) return;
            pSavePath = sfdExportMap.FileName;
            textBox3.Text = sfdExportMap.FileName;
        }

        private void mapexport_Load(object sender, EventArgs e)
        {
            InitFormSize();
        }
        private void InitFormSize() 
        {
            //窗口初始化时读取当前数据视图的分辨率并添加到分辨率文本框中
            comboBox1.Text = pActiveView.ScreenDisplay.DisplayTransformation.Resolution.ToString();
            comboBox1.Items.Add(comboBox1.Text);
            if (isRegion)//若是区域导出，获取区域的宽和高
            {
                IEnvelope pEnvelope = geometry.Envelope;
                tagRECT pRECT = new tagRECT();
                pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref pRECT, 9);//获取所选择的区域
                if (comboBox1.Text == "") return;
                textBox1.Text = pRECT.right.ToString();//获取宽度
                textBox2.Text = pRECT.bottom.ToString();//获取高度

            }
            else//全域导出，获取当前视图全域宽高
            {
                if (comboBox1.Text == null) return;
                textBox1.Text = pActiveView.ExportFrame.right.ToString();//获取宽度
                textBox2.Text = pActiveView.ExportFrame.bottom.ToString();//获取高度
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//属性值发生改变时
        {
            double num = (int)Math.Round(pActiveView.ScreenDisplay.DisplayTransformation.Resolution);//获取当前视图显示的分辨率
            if (comboBox1.Text == "")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                return;
            }
            if (isRegion)
            {
                IEnvelope pEnvelope = geometry.Envelope;
                tagRECT pRECT = new tagRECT();
                pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref pRECT, 9);//获取区域
                if (comboBox1.Text == "") return;
                //输出图片的宽=当前视图的宽*输出图片的分辨率/当前数据视图显示的分辨率
                textBox1.Text = Math.Round(pRECT.right * (double.Parse(comboBox1.Text) / num)).ToString();
                textBox2.Text = Math.Round(pRECT.bottom * (double.Parse(comboBox1.Text) / num)).ToString();
            }
            else
            {
                textBox1.Text = Math.Round(pActiveView.ExportFrame.right*(double.Parse(comboBox1.Text)/num)).ToString();
                textBox2.Text = Math.Round(pActiveView.ExportFrame.bottom * (double.Parse(comboBox1.Text) / num)).ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
                MessageBox.Show("请先确定导出路径","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            else if (comboBox1.Text == "")
            {               
                MessageBox.Show("请输入分辨率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Convert.ToInt16(comboBox1.Text) == 0)
            {
                MessageBox.Show("请正确输入分辨率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try {
                    int resolution = int.Parse(comboBox1.Text);//输出图片的分辨率
                    int wideth = int.Parse(textBox1.Text);//输出图片的高度
                    int height = int.Parse(textBox2.Text);//输出图片的高度
                    ExportMap.ExportView(pActiveView,geometry,resolution,wideth,height,pSavePath,isRegion);
                    pActiveView.GraphicsContainer.DeleteAllElements();
                    pActiveView.Refresh();
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
 
                }
                catch(Exception)
                { MessageBox.Show("导出失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pActiveView.GraphicsContainer.DeleteAllElements();//清除视图上的所有其他东西
            pActiveView.Refresh();
            Dispose();
        }

        private void mapexport_FormClosed(object sender, FormClosedEventArgs e)
        {
            pActiveView.GraphicsContainer.DeleteAllElements();
            pActiveView.Refresh();
            Dispose();
        }
    }
}
