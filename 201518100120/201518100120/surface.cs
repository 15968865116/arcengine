using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRasterUI;
using ESRI.ArcGIS.GeoAnalyst;


namespace _201518100120
{
    public partial class surface : Form
    {
        private AxMapControl _mapControl;
        private IMap _map;
        private object Missing = Type.Missing;

        //表面分析对象
        private ISurfaceOp surfaceOp = new RasterSurfaceOpClass();
        private IGeoDataset inGeodataset;//输入数据
        private IGeoDataset outGeodataset;//输出数据
        private double interValue;//Contour等值线间距
        private IGeoDataset beforeGeo;//填挖前数据
        private IGeoDataset afterGeo;//填挖后数据

        //Slopes输出测量单位
        private esriGeoAnalysisSlopeEnum slopeEnum;
        private double altitude;//光源高度角
        private double azimuth;//光源方位角
        private IGeoDataset observeLayer;//观察点数据

        //可见性分析类型
        private esriGeoAnalysisVisibilityEnum visibilityEnum;

        public surface(ref AxMapControl mapcontrol)
        {
            InitializeComponent();
            _mapControl = mapcontrol;
            _map = mapcontrol.Map;//获取地图

            for (var i = 0; i < _map.LayerCount; i++)
            {
                comboBox1.Items.Add(_map.Layer[i].Name);//下拉框添加栅格数据名字
                comboBox3.Items.Add(_map.Layer[i].Name);//添加填挖前栅格数据名字
                comboBox4.Items.Add(_map.Layer[i].Name);//添加填挖后的栅格数据名字
                comboBox6.Items.Add(_map.Layer[i].Name);//添加观察点栅格名字
            }
        }
        

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //填挖计算
        private void button5_Click(object sender, EventArgs e)
        {
            outGeodataset = surfaceOp.CutFill(beforeGeo, afterGeo, ref Missing);
            ShowRasterResult(outGeodataset, "CutFill");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                var layer = getLayerFromName(comboBox1.SelectedItem.ToString());//获取图层
                var rasterlayer = layer as IRasterLayer;//转化为栅格图层
                var raster = rasterlayer.Raster;
                inGeodataset = raster as IGeoDataset;
            }
            catch(Exception ex)
            {
                MessageBox.Show("所选择的图层不是栅格图层");
            }
        }
        private ILayer getLayerFromName(string layername)//根据名字获取图层
        {
            for (int i = 0; i < _map.LayerCount; i++)
            {
                if (layername == _map.Layer[i].Name)
                    return _map.Layer[i];
            }
            return null;
        }
        //显示栅格结果
        private void ShowRasterResult(IGeoDataset geoDataset, string interType)
        {
            IRasterLayer rasterLayer = new RasterLayerClass();
            IRaster raster = (IRaster)geoDataset;
            rasterLayer.CreateFromRaster(raster);//由栅格数据c产生栅格图层
            rasterLayer.Name = interType;
            _mapControl.AddLayer(rasterLayer, 0);
            _mapControl.ActiveView.Refresh();
        }
        //显示矢量结果
        private void ShowVectorResult(IGeoDataset geoDataset, string interType)
        {
            IFeatureClass featureClass = geoDataset as IFeatureClass;
            IFeatureLayer featureLayer=new FeatureLayerClass();
            featureLayer.FeatureClass=featureClass;
            featureLayer.Name=interType;
            _mapControl.AddLayer(featureLayer, 0);
            _mapControl.ActiveView.Refresh();
        }

        //坡向计算
        private void button2_Click(object sender, EventArgs e)
        {
            outGeodataset = surfaceOp.Aspect(inGeodataset);
            ShowRasterResult(outGeodataset, "Aspect");
        }

        //等值线计算
        private void button4_Click(object sender, EventArgs e)
        {
            outGeodataset = surfaceOp.Contour(inGeodataset, interValue,ref Missing);//等值线计算方法
            ShowVectorResult(outGeodataset, "Counter");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)//设置等值线间距的值
        {
            interValue = Convert.ToDouble(textBox1.Text);
        }

        //曲率计算
        private void button3_Click(object sender, EventArgs e)
        {
            outGeodataset = surfaceOp.Curvature(inGeodataset, true, true);
            ShowRasterResult(outGeodataset,"Curvature");

        }

        //设置填挖之前的数据集
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var layer = getLayerFromName(comboBox3.SelectedItem.ToString());
            var rasterLayer = layer as IRasterLayer;
            var raster = rasterLayer.Raster;
            beforeGeo = raster as IGeoDataset;

        }

        //设置填挖之后的数据集
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var layer = getLayerFromName(comboBox4.SelectedItem.ToString());
            var rasterLayer = layer as IRasterLayer;
            var raster = rasterLayer.Raster;
            afterGeo = raster as IGeoDataset;
        }

        //坡度计算
        private void button1_Click(object sender, EventArgs e)
        {
            if(outGeodataset!=null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(outGeodataset);
            
            outGeodataset = surfaceOp.Slope(inGeodataset, slopeEnum, ref Missing);
            ShowRasterResult(outGeodataset, "Slope");
            


        }
        //设置Slope输出测量单位
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox2.SelectedIndex;
            switch (index)
            {
                case 0://degree(度)
                    slopeEnum = esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopeDegrees;
                    break;
                case 1://高程增量百分比
                    slopeEnum = esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopePercentrise;
                    break;
            }

        }

        //山体阴影计算
        private void button6_Click(object sender, EventArgs e)
        {
            outGeodataset = surfaceOp.HillShade(inGeodataset, azimuth, altitude, true, ref Missing);
            ShowRasterResult(outGeodataset, "HillShade");

        }

        //光源方位角
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            azimuth = Convert.ToDouble(textBox2.Text);
        }

        //光源高度角
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            altitude = Convert.ToDouble(textBox3.Text);
        }


        //可见性分析方法
        private void button7_Click(object sender, EventArgs e)
        {
            var layer = getLayerFromName(comboBox6.SelectedItem.ToString());
            var fl = layer as IFeatureLayer;
            var fc = fl.FeatureClass;
            observeLayer = fc as IGeoDataset;
        }

        //设置可见性分析类型
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

            var index = comboBox5.SelectedIndex;
            switch (index) 
            {
                case 0://视点分析
                    visibilityEnum = esriGeoAnalysisVisibilityEnum.esriGeoAnalysisVisibilityFrequency;
                    break;
                case 1://视域分析
                    visibilityEnum = esriGeoAnalysisVisibilityEnum.esriGeoAnalysisVisibilityObservers;
                    break;
            }
        }
    }
}
