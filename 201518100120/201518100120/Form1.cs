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
using ESRI.ArcGIS.esriSystem;

namespace _201518100120
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //量测全局变量
        private measureresult frmMeasureresult;//量算结果窗体
        private bool bMeasureArea;//是否开始面积量算功能
        private bool bMeasureLength;//是否开始长度量算
        private INewLineFeedback pNewLineFeedback;//追踪线对象
        private INewPolygonFeedback pNewPolygonFeedback;//追踪面对象
        private IPoint pPointPt=new PointClass();//鼠标点击点
        private IPoint pMovePt=new PointClass();//鼠标移动当前点 
        private double dToltaLength;//量测总长度
        private double dSegmentLength;//片段距离
        private IPointCollection pAreaPointCollection =new MultipointClass();//面积量算时画的点进行存储
        string mousedownname = "";//鼠标按下操作的全局变量


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

        private void 上一个视图ToolStripMenuItem_Click(object sender, EventArgs e)//到上一个视图
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

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)//放大
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
       
        

        private void 拉框放大ToolStripMenuItem_Click(object sender, EventArgs e)//拉框放大
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
                case "MeasureLength":
                    pPointPt.PutCoords(e.mapX, e.mapY);
                    if (pNewLineFeedback == null)
                    {
                        pNewLineFeedback = new NewLineFeedback { Display = pActiveview.ScreenDisplay };//实例化追踪线对象
                        pNewLineFeedback.Start(pPointPt);//设置起点，开始动态绘制
                        dToltaLength = 0;
                    }
                    else//如果追踪线对象不为空，则添加当前鼠标点
                    {
                        pNewLineFeedback.AddPoint(pPointPt);
                    }
                    if (dSegmentLength != 0)
                        dToltaLength = dToltaLength + dSegmentLength;
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

        private string GetMapUnit(esriUnits esriUnit)
        {
            string MapUnit = "";
            switch(esriUnit)
            {
                case esriUnits.esriCentimeters:
                    MapUnit="厘米";
                    break;
                case esriUnits.esriDecimalDegrees:
                    MapUnit = "十进制";
                    break;
                case esriUnits.esriDecimeters:
                    MapUnit = "分米";
                    break;
                case esriUnits.esriFeet:
                    MapUnit = "尺";
                    break;
                case esriUnits.esriInches:
                    MapUnit = "英尺";
                    break;
                case esriUnits.esriKilometers:
                    MapUnit = "千米";
                    break;
                case esriUnits.esriMeters:
                    MapUnit = "米";
                    break;
                case esriUnits.esriMiles:
                    MapUnit = "英里";
                    break;
                case esriUnits.esriMillimeters:
                    MapUnit = "毫米";
                    break;
                case esriUnits.esriNauticalMiles:
                    MapUnit = "海里";
                    break;
                case esriUnits.esriPoints:
                    MapUnit = "点";
                    break;
                case esriUnits.esriYards:
                    MapUnit = "码";
                    break;
                case esriUnits.esriUnknownUnits:
                    MapUnit = "未知单位";
                    break;
              }
            return MapUnit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)//左下角鼠标坐标显示
        {
            sMapUnit = GetMapUnit(axMapControl1.Map.MapUnits);
            barCoorTxt.Text = string.Format("当前坐标：X{0:#.###}  Y={1:#.###}{2}",e.mapX,e.mapY,sMapUnit);
            pMovePt.PutCoords(e.mapX, e.mapY);//获取移动点
            switch (mousedownname)
            {
                case "MeasureLength":
                    if (pNewLineFeedback != null)
                        pNewLineFeedback.MoveTo(pMovePt);//移动至当前
                    if (pPointPt != null && pNewLineFeedback != null)
                    {
                        double deltaX = pMovePt.X - pPointPt.X;//两点间X差
                        double deltaY = pMovePt.Y - pPointPt.Y;//两点间Y 差
                        dSegmentLength = Math.Round(Math.Sqrt(deltaX * deltaX + deltaY * deltaY), 3);
                        dToltaLength = dToltaLength + dSegmentLength;
                        if (frmMeasureresult != null)
                        {
                            frmMeasureresult.label2.Text=String.Format("当前长度线段为：{0:.###}{1};\r\n总长度为：{2:.###}{1}",dSegmentLength,sMapUnit,dToltaLength);
                            dToltaLength = dToltaLength - dSegmentLength;//鼠标移动到新点重新开始计算
                        }
                    }

                    break;
            }
        }

        private void frmMeasureResult_frmClosed()//委托函数所调用的事件，主要用于结束量算和清空线和面对象
        {
            if (pNewLineFeedback != null)//判断面对象是否为空不为空则清空
            {
                pNewLineFeedback.Stop();
                pNewLineFeedback = null;
            }
            if (pNewPolygonFeedback != null)//清空面对象
            {
                pNewPolygonFeedback.Stop();
                pNewPolygonFeedback = null;
                pAreaPointCollection.RemovePoints(0,pAreaPointCollection.PointCount);//清空点集中所有点
            }
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground,null,null);//清空量算画的线、面对象
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;//结束量算功能
 
        }

        private void 距离量测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            mousedownname = "MeasureLength";//给后续鼠标参数传递参数
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//改变鼠标形状
            if (frmMeasureresult == null || frmMeasureresult.IsDisposed)//窗口不存在则新建一个窗口
            {
                frmMeasureresult = new measureresult();
                frmMeasureresult.frmClose += frmMeasureResult_frmClosed;//委托事件就是后面的函数
                frmMeasureresult.Show();
            }
            else
            {
                frmMeasureresult.Activate();//激活存在的窗口
            }

        }

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            switch(mousedownname)
            {
                case "MeasureLength":
                    if(frmMeasureresult!=null)
                    {
                        frmMeasureresult.label2.Text = "线段总长度为："+dToltaLength+sMapUnit;
                    }
                    if (pNewLineFeedback != null)
                    {
                        pNewLineFeedback.Stop();
                        pNewLineFeedback = null;
                        //清空线对象
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground,null,null);
                    }
                    dToltaLength = 0;
                    dSegmentLength = 0;
                    break;
            }
        }



    }
}