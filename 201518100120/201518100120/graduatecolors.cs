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
using MapOperation.ThematicMaps;

namespace _201518100120
{
    public partial class graduatecolors : Form
    {
        List<IFeatureClass> _lstFeatCls;
        public delegate void GraduatedcolorsEventHandler(string sFeatClsName, string sFieldName,int intnumclasses);
        public event GraduatedcolorsEventHandler Graduatedcolors;//委托
        public graduatecolors()
        {
            InitializeComponent();
        }
        public IMap _map;
        public IMap Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public void InitUI() //初始化
        {
            string sClsName = string.Empty;
            comboBox1.Items.Clear();
            OperateMap _operateMap = new OperateMap();
            _lstFeatCls = _operateMap.GetLstFeatCls(_map);//获得图层信息
            foreach (IFeatureClass pFeatCls in _lstFeatCls)//循环
            {
                sClsName = pFeatCls.AliasName;//获得图层名字
                if (!comboBox1.Items.Contains(sClsName))//若原选择框中不包含此名字，将此图层名添加至选择框值
                    comboBox1.Items.Add(sClsName);
            }
        }
        private bool Check()//判断是否已经选择了图层和字段
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择符号化字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("请选择颜色分级数目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        private IFeatureClass GetFeatClsByName(string sFeatClsName)//通过名字找要素
        {
            IFeatureClass pFeatCls = null;
            foreach (IFeatureClass featureClass in _lstFeatCls)
            {
                pFeatCls = featureClass;
                if (pFeatCls.AliasName == sFeatClsName)
                    break;
            }
            return pFeatCls;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Check()) return;
            Graduatedcolors(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(),Convert.ToInt32(comboBox3.SelectedItem.ToString()));
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.SelectedIndex = -1;
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();//字段框清空
            comboBox2.Text = "";
            IFeatureClass pFeatCls = GetFeatClsByName(comboBox1.SelectedItem.ToString());//通过图层选择字段赋值给pFeatCls
            for (int i = 0; i < pFeatCls.Fields.FieldCount; i++)//对字段循环并加入下拉框
            {
                IField pField = pFeatCls.Fields.Field[i];//第i个字段
                if (pField.Type == esriFieldType.esriFieldTypeDouble || pField.Type == esriFieldType.esriFieldTypeSmallInteger || pField.Type == esriFieldType.esriFieldTypeInteger || pField.Type == esriFieldType.esriFieldTypeSingle)//判断字段类型只能渲染这四个类型
                {
                    if (!comboBox2.Items.Contains(pField.Name))
                        comboBox2.Items.Add(pField.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
