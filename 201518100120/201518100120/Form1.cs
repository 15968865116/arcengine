using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开地图文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.CheckFileExists = true;
            openfiledialog.Title = @"打开地图文档";
            openfiledialog.Filter = @"ArcMap文档(*.mxd)|*.mxd;|Arcmap模板(*.mxt)|*.mxt";
            openfiledialog.RestoreDirectory = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openfiledialog.FileName;
                if (fileName == "")
                {
                    return;
                }
                else
                {
                    if (axMapControl1.CheckMxFile(fileName))
                    {
                        axMapControl1.ClearLayers();
                        axMapControl1.LoadMxFile(fileName);

                    }
                    else
                    {
                        MessageBox.Show(fileName + "是无效的地图文档", "提示");
                    }

                }
            }
        }

        private void 打开shapefile文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.CheckFileExists = true;
            openfiledialog.Title = @"打开shapefile";
            openfiledialog.Filter = @"shapefile(*.shp)|*.shp";
            openfiledialog.RestoreDirectory = true;
            openfiledialog.ShowDialog();

            string pFullPath = openfiledialog.FileName;
            if (pFullPath == "") return;
            int pIndext = pFullPath.LastIndexOf("\\");
            string filePath = pFullPath.Substring(0, pIndext);
            string filename = pFullPath.Substring(pIndext + 1);


            axMapControl1.AddShapeFile(filePath, filename);

        }

        private void 上一个视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            IExtentStack pExtentStack = axMapControl1.ActiveView.ExtentStack;
            

            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();//转到上一个视图
                后一个视图ToolStripMenuItem.Enabled = true;//此时后一个视图存在，后一个视图按钮变为可按
                if (!pExtentStack.CanUndo())
                {
                    上一个视图ToolStripMenuItem.Enabled = false;//如果上一个视图不存在，则上一个视图这个按钮变为不可用。
                }
                
               
            }
            axMapControl1.ActiveView.Refresh();
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(0.5,0.5,true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(2, 2, true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();
        }
        string mousedownname = "";
        private void 拉框放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "zoomin";
            
           
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            IActiveView pActiveview = axMapControl1.ActiveView;
            IEnvelope envelope = new EnvelopeClass();

            switch(mousedownname)
            { 
                case "zoomin":
                    envelope = axMapControl1.TrackRectangle();//获取拉框信息
                    if (envelope == null || envelope.IsEmpty || envelope.Height == 0 || envelope.Width == 0)//判断是否为空框
                        return;
                    pActiveview.Extent = envelope;
                    pActiveview.Refresh();
                    break;
                case "zoomout":
                    envelope = axMapControl1.TrackRectangle();
                    if (envelope == null || envelope.IsEmpty || envelope.Height == 0 || envelope.Width == 0)
                        return;
                    double dwidth=pActiveview.Extent.Width*pActiveview.Extent.Width/envelope.Width;
                    double dheight=pActiveview.Extent.Height*pActiveview.Extent.Height/envelope.Height;
                    double dXmin=pActiveview.Extent.XMin-((envelope.XMin-pActiveview.Extent.XMin)*pActiveview.Extent.Width/envelope.Width);
                    double dYmin = pActiveview.Extent.YMin - ((envelope.YMin - pActiveview.Extent.YMin) * pActiveview.Extent.Height / envelope.Height);
                    double dxMAX = dXmin + dwidth;
                    double dyMAX = dYmin + dheight;
                    envelope.PutCoords(dXmin,dYmin,dxMAX,dyMAX);
                    pActiveview.Extent = envelope;
                    pActiveview.Refresh();
                    break;
                case "manyou":
                    axMapControl1.Pan();
                    break;

            }
        }

        private void 拉框缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "zoomout";//给予mousedown这个事件一个参数，以供选择需要发生的操作。
        }

        private void 漫游ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "manyou";//给予mousedown这个事件一个参数，以供选择需要发生的操作。
        }

        private void 全图显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        private void 后一个视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IExtentStack pExtentStack = axMapControl1.ActiveView.ExtentStack;


            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();//转到后一个视图
                上一个视图ToolStripMenuItem.Enabled = true;//转到后一个视图后，说明其前面有一个视图，此时上一个视图这个按钮变成可按
                if (!pExtentStack.CanRedo())
                {
                    后一个视图ToolStripMenuItem.Enabled = false;//如果之后没有后一个视图了，后一个视图按钮变灰
                }


            }
            axMapControl1.ActiveView.Refresh();
        }

        private void 直接保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mxdFileName = axMapControl1.DocumentFilename;
            IMapDocument mapDocument = new MapDocument();
            if (mxdFileName != null && axMapControl1.CheckMxFile(mxdFileName))
            {
                if (mapDocument.get_IsReadOnly(mxdFileName))
                {
                    MessageBox.Show("地图文档只读，无法保存");
                    mapDocument.Close();
                    return;
                }
                
                mapDocument.New(mxdFileName);
                mapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                mapDocument.Save(mapDocument.UsesRelativePaths, true);
                mapDocument.Close();
                MessageBox.Show("地图文档保存成功");

            }
            else
            {
                MessageBox.Show("地图文档不存在");
            }
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefiledialog = new SaveFileDialog
            {
                Title = "另存为",
                OverwritePrompt = true,
                Filter = "Arcmap文档(*.mxd）|*.mxd|ArcMap模板(*.mxt)|*.mxt",
                RestoreDirectory = true
            };
            if (savefiledialog.ShowDialog() != DialogResult.OK)
                return;
            string filePath = savefiledialog.FileName;
            IMapDocument mapDocument = new MapDocument();
            mapDocument.New(filePath);
            mapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
            mapDocument.Save(true,true);
            mapDocument.Close();
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

        }

        private void 添加书签ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookmark frmbookmark = new bookmark();
            frmbookmark.ShowDialog();//初始化一个书签管理窗口
            string pName = frmbookmark.Bookmark;//将书签名字赋给pName
            if (pName == "")
                return;

            IMapBookmarks mapbookmarks = axMapControl1.Map as IMapBookmarks;//访问当前书签集
            IEnumSpatialBookmark enumspatialbookmarks = mapbookmarks.Bookmarks;//将当前的书签集赋给enumspatialbookmarks
            enumspatialbookmarks.Reset();
            ISpatialBookmark pspatialbookmarks;//创建一个空的空间书签
            while((pspatialbookmarks=enumspatialbookmarks.Next())!=null)//循环判断名字是否已经存在
            {
                if (pName == pspatialbookmarks.Name)
                {
                    DialogResult dr = MessageBox.Show("此书签名已存在！是否替换？","提示",MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                        mapbookmarks.RemoveBookmark(pspatialbookmarks);
                    else if (dr == DialogResult.No)
                        frmbookmark.ShowDialog();
                    else
                        return;

                }
            }
            IActiveView pactiveview = axMapControl1.Map as IActiveView;//获取当前地图对象
            IAOIBookmark pbookmark = new AOIBookmark();
            pbookmark.Location = pactiveview.Extent;//创建一个书签并设置其位置为当前视图范围
            pbookmark.Name = pName;//获得书签名
            IMapBookmarks pmapbookmarks = axMapControl1.Map as IMapBookmarks;//访问当前书签集
            pmapbookmarks.AddBookmark(pbookmark);//添加新书签到地图的书签集中



        }

        private void 书签管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            managebookmark frm = new managebookmark(axMapControl1.Map);
            frm.ShowDialog();
        }
        private string sMapUnit = "未知单位";//地图单位变量

    }
}