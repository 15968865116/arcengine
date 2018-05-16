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
    public partial class attribute : Form
    {
        private IFeatureLayer curFeatureLayer;

        public attribute(IFeatureLayer _CurFeatureLarer)
        {
            InitializeComponent();
            curFeatureLayer = _CurFeatureLarer;
            InitUI();//定义的函数
        }
        public void InitUI()
        {
            if (curFeatureLayer == null)
            {
                return;
            }
            DataTable pFeatDT = new DataTable();//创建数据表
            for (int i = 0; i < curFeatureLayer.FeatureClass.Fields.FieldCount; i++)//获取字段名，字段类型
            {
                DataColumn pDataCol = new DataColumn();//数据表列变量
                IField pField = curFeatureLayer.FeatureClass.Fields.get_Field(i);//取一列
                pDataCol.ColumnName = pField.AliasName;//获取字段名作为列标题
                pDataCol.DataType = Type.GetType("System.Object");//定义列字段类型
                pFeatDT.Columns.Add(pDataCol);//在数据表中添加字段信息
            }
            IFeatureCursor pFeatureCursor = curFeatureLayer.Search(null,true);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                DataRow pDataRow = pFeatDT.NewRow();//数据表行向量
                for (int k = 0; k < pFeatDT.Columns.Count;k++ )//获取一行数据
                {
                    pDataRow[k] = pFeature.get_Value(k);     
                }
                pFeatDT.Rows.Add(pDataRow);//行数据添加到数据表中
                pFeature = pFeatureCursor.NextFeature();//获取下一个特征行
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            dataGridView1.DataSource = pFeatDT;//将创建的数据表赋给数据视图窗口
        }

        private void attribute_Load(object sender, EventArgs e)
        {

        }

        private void attribute_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
