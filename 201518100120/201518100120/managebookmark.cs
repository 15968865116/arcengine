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

namespace _201518100120
{
    public partial class managebookmark : Form
    {
        private IMap _currentmap = null;
        Dictionary<string, ISpatialBookmark> pdictionary = new Dictionary<string, ISpatialBookmark>();
        IMapBookmarks mapbookmarks = null;

        public managebookmark(IMap pmap)
        {
            InitializeComponent();
            _currentmap = pmap;//获取当前地图
            InitControl();
        }//获取空间书签，并对视图初始化
        private void InitControl() 
        {
            mapbookmarks = _currentmap as IMapBookmarks;//获取当前地图的书签集
            IEnumSpatialBookmark enumspatialbookmarks = mapbookmarks.Bookmarks;//赋给变量
            enumspatialbookmarks.Reset();
            ISpatialBookmark pspatialbookmark = enumspatialbookmarks.Next();//选择第一个书签
            string sbookmarksname = string.Empty;
            while (pspatialbookmark != null)//循环，判断是否为空
            {
                sbookmarksname = pspatialbookmark.Name;//书签名字获取
                treeView1.Nodes.Add(sbookmarksname);//将其加入到目录树视图
                pdictionary.Add(sbookmarksname,pspatialbookmark);//加到索引字典内
                pspatialbookmark = enumspatialbookmarks.Next();//进行下一个书签操作
                
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void managebookmark_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//书签视图定位
        {
            TreeNode pselectnode = treeView1.SelectedNode;//获得选中的书签
            ISpatialBookmark pspatialbm = pdictionary[pselectnode.Text];//获得选中的书签对象
            pspatialbm.ZoomTo(_currentmap);//缩放到选中的视图范围
            IActiveView pactiveview = _currentmap as IActiveView;
            pactiveview.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);



        }

        private void button2_Click(object sender, EventArgs e)//删除书签
        {
            TreeNode pselectnode = treeView1.SelectedNode;//获得选中的书签
            ISpatialBookmark pspatialbm = pdictionary[pselectnode.Text];//获得选中的书签对象
            mapbookmarks.RemoveBookmark(pspatialbm);//删除选中的书签对象
            pdictionary.Remove(pselectnode.Text);//删除字典中的数据
            treeView1.Nodes.Remove(pselectnode);//删除窗口中的树节点
            treeView1.Refresh();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
