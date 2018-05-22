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
    public partial class querybyattribute : Form
    {
        private IMap _currentMap;
        private IFeatureLayer _currentFeatureLayer;//使用IFeaturelayer接口的当前图层对象
        private string _currentFieldName;//存储字段名称的临时变量
        public querybyattribute(IMap currentMap)
        {

            InitializeComponent();
            _currentMap = currentMap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text += "=";
        }

        private void querybyattribute_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();//当前下拉框中的图层对象被清空
            
            
                for (int i = 0; i < _currentMap.LayerCount; i++)//对每个图层进行判断并加载
                {
                    string layerName;//设置临时变量存储图层名称

                    if (_currentMap.Layer[i] is GroupLayer)//若为图层组类型，对包含的图层进行操作
                    {
                        ICompositeLayer compositeLayer = _currentMap.Layer[i] as ICompositeLayer;
                        for (int j = 0; j < compositeLayer.Count; j++)
                        {
                            layerName = compositeLayer.Layer[j].Name;
                            comboBox1.Items.Add(layerName);
                        }
                    }
                    else//不是图层组，直接添加
                    {
                        layerName = _currentMap.Layer[i].Name;
                        comboBox1.Items.Add(layerName);
                    }
                }
            
            
            comboBox1.SelectionStart=0;
            comboBox2.SelectionStart= 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            for (int i = 0; i < _currentMap.LayerCount; i++)
            {
                if (_currentMap.Layer[i] is GroupLayer)
                {
                    ICompositeLayer compositelayer = _currentMap.Layer[i] as ICompositeLayer;
                    for (int j = 0; j < compositelayer.Count; j++)
                    {
                        if (compositelayer.Layer[j].Name == comboBox1.SelectedItem.ToString())
                        {
                            _currentFeatureLayer = compositelayer.Layer[j] as IFeatureLayer;
                            break;
                        }
                    }
                }
                else 
                {
                    if (_currentMap.Layer[i].Name == comboBox1.SelectedItem.ToString())
                    {
                        _currentFeatureLayer = _currentMap.Layer[i] as IFeatureLayer;
                        break;
                    }

                }
            }
            for (int i = 0; i < _currentFeatureLayer.FeatureClass.Fields.FieldCount; i++)
            {
                IField field = _currentFeatureLayer.FeatureClass.Fields.Field[i];
                if (field.Name.ToUpper() != "SHAPE")
                    listBox1.Items.Add("\"" + field.Name + "\"");
            }
            label3.Text = "SELECT * FROM" + _currentFeatureLayer.Name + "WHERE:";
            textBox1.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();//右边清空
            if (button15.Enabled == false)
                button15.Enabled = true;
            string str = listBox1.SelectedItem.ToString();
            str = str.Substring(1);
            str = str.Substring(0, str.Length - 1);
            _currentFieldName = str;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            IDataset dataset = (IDataset)_currentFeatureLayer.FeatureClass;
            IQueryDef queryDef = ((IFeatureWorkspace)dataset.Workspace).CreateQueryDef();
            queryDef.Tables = dataset.Name;
            queryDef.SubFields = "DISTINCT("+_currentFieldName+")";
            ICursor cursor = queryDef.Evaluate();
            IFields fields = _currentFeatureLayer.FeatureClass.Fields;
            IField field = fields.Field[fields.FindField(_currentFieldName)];
            IRow row = cursor.NextRow();
            while (row != null)
            {
                if (field.Type == esriFieldType.esriFieldTypeString)
                    listBox2.Items.Add("\"" + row.Value[0] + "\"");
                else
                    listBox2.Items.Add(row.Value[0].ToString());
                row = cursor.NextRow();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text += listBox1.SelectedItem.ToString();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text += listBox2.SelectedItem.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += "<>";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Like";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += ">";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += ">=";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += "+";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += "<";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += "<=";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Or";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += "_";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += "%";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text += "()";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Not";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text += "Is";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            selectFeatureByAttribute();
            Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            selectFeatureByAttribute();
        }
        private void selectFeatureByAttribute()
        {
            IFeatureSelection featureSelection = _currentFeatureLayer as IFeatureSelection;
            IQueryFilter queryfilter = new QueryFilterClass();
            queryfilter.WhereClause = textBox1.Text;
            IActiveView activeview = _currentMap as IActiveView;
            switch (comboBox2.SelectedIndex)
            {
                case 0://新建选择集
                    _currentMap.ClearSelection();
                    featureSelection.SelectFeatures(queryfilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;
                case 1://添加到当前选择集
                    featureSelection.SelectFeatures(queryfilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                    break;
                case 2://从当前选择集中删除
                    featureSelection.SelectFeatures(queryfilter, esriSelectionResultEnum.esriSelectionResultXOR, false);
                    break;
                case 3://从当前选择集中选择
                    featureSelection.SelectFeatures(queryfilter, esriSelectionResultEnum.esriSelectionResultAnd, false);
                    break;
                default:
                    _currentMap.ClearSelection();
                    featureSelection.SelectFeatures(queryfilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;

            }
            activeview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,activeview.Extent);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)//判断是否选择图层可选，此处为不选择不可选图层
            {
                comboBox1.Items.Clear();//当前下拉框中的图层对象被清空


                for (int i = 0; i < _currentMap.LayerCount; i++)//对每个图层进行判断并加载
                {
                    string layerName;//设置临时变量存储图层名称

                    if (_currentMap.Layer[i] is GroupLayer)//若为图层组类型，对包含的图层进行操作
                    {
                        ICompositeLayer compositeLayer = _currentMap.Layer[i] as ICompositeLayer;
                        for (int j = 0; j < compositeLayer.Count; j++)
                        {
                            layerName = compositeLayer.Layer[j].Name;
                            comboBox1.Items.Add(layerName);
                        }
                    }
                    else//不是图层组，直接添加
                    {
                        layerName = _currentMap.Layer[i].Name;
                        comboBox1.Items.Add(layerName);
                    }
                }


                comboBox1.SelectionStart = 0;
                comboBox2.SelectionStart = 0;
            }
            else 
            {
                comboBox1.Items.Clear();//当前下拉框中的图层对象被清空


                for (int i = 0; i < _currentMap.LayerCount; i++)//对每个图层进行判断并加载
                {
                    string layerName;//设置临时变量存储图层名称
                    IFeatureLayer a = (IFeatureLayer)_currentMap.Layer[i];
                    if (a.Selectable == true)
                    {
                        if (_currentMap.Layer[i] is GroupLayer)//若为图层组类型，对包含的图层进行操作
                        {
                            ICompositeLayer compositeLayer = _currentMap.Layer[i] as ICompositeLayer;
                            for (int j = 0; j < compositeLayer.Count; j++)
                            {
                                layerName = compositeLayer.Layer[j].Name;
                                comboBox1.Items.Add(layerName);
                            }
                        }
                        else//不是图层组，直接添加
                        {
                            layerName = _currentMap.Layer[i].Name;
                            comboBox1.Items.Add(layerName);
                        }
                    }
                    else
                        continue;
                }


                comboBox1.SelectionStart = 0;
                comboBox2.SelectionStart = 0;
            }
        }
    }
}
