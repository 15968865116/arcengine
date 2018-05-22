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
using MapOperation.ThematicMaps;

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
        private INewLineFeedback pNewLineFeedback;//追踪线对象
        private INewPolygonFeedback pNewPolygonFeedback;//追踪面对象
        private IPoint pPointPt;//鼠标点击点
        private IPoint pMovePt;//鼠标移动当前点 
        private double dToltaLength;//量测总长度
        private double dSegmentLength;//片段距离
        private IPointCollection pAreaPointCollection =new MultipointClass();//面积量算时画的点进行存储
        string mousedownname = "";//鼠标按下操作的全局变量
        private object missing = Type.Missing;
       //TOCControl右键菜单所用到的变量
        IFeatureLayer pTocFeatureLayer = null;//点击的要素图层
        public attribute frmAttribute = null;//新建一个属性表
        private mapexport frmExpMap=null;//新建一个输出窗口
        //鹰眼全局变量
        private bool bCanDrag;//鹰眼地图上的矩形可移动的标志
        private IPoint pMoveRectPoint;//记录在移动鹰眼地图上的矩形框时的鼠标的位置
        private IEnvelope pEnv;//记录数据视图的extent




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
                pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//清空点集中所有点
            }
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);//清空量算画的线、面对象
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;//结束量算功能
            mousedownname = "";

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
            pPointPt = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x,e.y);   

            switch(mousedownname)
            { 

                case "":
                    break;
                case "zoomin"://拉框放大
                    envelope = axMapControl1.TrackRectangle();//获取拉框信息
                    if (envelope == null || envelope.IsEmpty || envelope.Height == 0 || envelope.Width == 0)//判断是否为空框
                        return;
                    pActiveview.Extent = envelope;
                    pActiveview.Refresh();
                    break;
                case "zoomout"://拉框缩小
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
                case "manyou"://漫游
                    axMapControl1.Pan();
                    break;
                case "MeasureLength"://长度量测
                    
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
                case "MeasureArea"://面积量测
                    
                    if (pNewPolygonFeedback == null)
                    {
                        pNewPolygonFeedback = new NewPolygonFeedback { Display = pActiveview.ScreenDisplay };//实例化面对象
                        pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//清空点集
                        pNewPolygonFeedback.Start(pPointPt);//开始绘制多边形
                        pAreaPointCollection.AddPoint(pPointPt, ref missing, ref missing);
                    }
                    else
                    {
                        pNewPolygonFeedback.AddPoint(pPointPt);
                        pAreaPointCollection.AddPoint(pPointPt, ref missing, ref missing);
                    }
                
                    break;
                case "selectfeature"://要素选择
                    IEnvelope pEnv = axMapControl1.TrackRectangle();
                    IGeometry pGeo = pEnv;
                    if (pEnv.IsEmpty)//若为空则在鼠标当前点进行选择部分区域作为框
                    {
                        tagRECT r;
                        r.left = e.x - 5;
                        r.top = e.y - 5;
                        r.right = e.x + 5;
                        r.bottom = e.y + 5;
                        pActiveview.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);
                        pEnv.SpatialReference = pActiveview.FocusMap.SpatialReference;
                    }
                    pGeo = pEnv;
                    axMapControl1.Map.SelectByShape(pGeo, null, false);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection,null,null);
                    break;
                case "ExportRegion":
                    pActiveview.GraphicsContainer.DeleteAllElements();
                    pActiveview.Refresh();
                    IPolygon pPolygon = ExportMap.DrawPolygon(axMapControl1);
                    if (pPolygon == null) return;
                    ExportMap.AddElement(pPolygon, pActiveview);
                    if (frmExpMap == null || frmExpMap.IsDisposed)
                        frmExpMap = new mapexport(axMapControl1);
                    frmExpMap.isRegion = true;
                    frmExpMap.geometry = pPolygon;
                    frmExpMap.Show();
                    frmExpMap.Activate();
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
            pMovePt = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);  //获取移动点
            switch (mousedownname)
            {
                case "MeasureLength"://距离量测
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
                case "MeasureArea"://面积量测
                    if (pNewPolygonFeedback != null)
                        pNewPolygonFeedback.MoveTo(pMovePt);
                    IPointCollection pPointCol=new Polygon();//实例化一个新的点集对象
                    IPolygon pPolygon = new PolygonClass();//实例化一个面几何对象
                    
                    
                    for (int i = 0; i <= pAreaPointCollection.PointCount - 1; i++)
                    {
                        
                        pPointCol.AddPoint(pAreaPointCollection.get_Point(i), ref missing, ref missing);//将全局变量中已经确定的点集赋给pPointCol这个点集对象
                    }
                        
                    pPointCol.AddPoint(pMovePt, ref missing, ref missing);//将当前的移动点赋给pPointCol这个点集对象

                    if (pPointCol.PointCount < 3)
                    {
                        
                        return;//小于三个点无法形成面，不执行以下语句
                    }
                    pPolygon = pPointCol as IPolygon;//将点集合形成一个面对象
                    if ((pPolygon != null))
                    {
                        pPolygon.Close();
                        IGeometry pGeo = pPolygon;
                        ITopologicalOperator pTopo = pGeo as ITopologicalOperator;
                        pTopo.Simplify();
                        pGeo.Project(axMapControl1.Map.SpatialReference);
                        IArea pArea = pGeo as IArea;
                        frmMeasureresult.label2.Text = String.Format("总面积为：{0:.####}平方{1};\r\n总长度为：{2:.#####}{1}",pArea.Area,sMapUnit,pPolygon.Length);
                        
                        
                        
                        
                    }
                    
                    break;
            }
        }

        

        private void 距离量测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            mousedownname = "MeasureLength";//给后续鼠标参数传递参数
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//改变鼠标形状
            if (frmMeasureresult == null || frmMeasureresult.IsDisposed)//窗口不存在则新建一个窗口
            {
                frmMeasureresult = new measureresult();
                frmMeasureresult.frmClosed+=frmMeasureResult_frmClosed;//委托事件就是后面的函数
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
                case "MeasureArea":
                    if (pNewPolygonFeedback != null)
                    {
                        pNewPolygonFeedback.Stop();
                        pNewPolygonFeedback = null;
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);//清空面对象

                    }
                    pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//清空点集中所有点
                    

                    break;
            }
        }

        private void 面积量测ToolStripMenuItem_Click(object sender, EventArgs e)//初始化相关变量，弹出量测窗体
        {
            axMapControl1.CurrentTool = null;
            mousedownname = "MeasureArea";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (frmMeasureresult == null || frmMeasureresult.IsDisposed)
            {
                frmMeasureresult = new measureresult();
                frmMeasureresult.frmClosed += frmMeasureResult_frmClosed;
                frmMeasureresult.Show();
            }
            else
                frmMeasureresult.Activate();
        }

        private void 选择要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "selectfeature";
        }

        private void 缩放至选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int nSelection = axMapControl1.Map.SelectionCount;//统计选择的要素的个数
            if (nSelection == 0)
            {
                MessageBox.Show("请先选择要素！", "提示");
            }
            else
            {
                ISelection selection = axMapControl1.Map.FeatureSelection;//选择的要素
                IEnumFeature enumfeature = (IEnumFeature)selection;//
                enumfeature.Reset();
                IEnvelope pEnvelop = new EnvelopeClass();
                IFeature pFeature = enumfeature.Next();
                while (pFeature != null)
                {
                    pEnvelop.Union(pFeature.Extent);
                    pFeature = enumfeature.Next();
                }
                pEnvelop.Expand(1.1, 1.1, true);
                axMapControl1.ActiveView.Extent = pEnvelop;
                axMapControl1.ActiveView.Refresh();
            }

        }

        private void 清除选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IActiveView pActiveview = axMapControl1.ActiveView;
            pActiveview.FocusMap.ClearSelection();
            pActiveview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,pActiveview.Extent);
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)//右键，1为按左键
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pMap = null;
                object unk = null;
                object data = null;
                ILayer pLayer = null;
                axTOCControl1.HitTest(e.x,e.y,ref pItem,ref pMap,ref pLayer,ref unk,ref data);
                pTocFeatureLayer = pLayer as IFeatureLayer;
                if(pItem==esriTOCControlItem.esriTOCControlItemLayer&&pTocFeatureLayer!=null)
                {
                    图层可选ToolStripMenuItem.Enabled = !pTocFeatureLayer.Selectable;//设置当前为图层为可选，所以此按钮灰色
                    图层不可选ToolStripMenuItem.Enabled = pTocFeatureLayer.Selectable;//图层不可选按钮亮着可以选
                    contextMenuStrip1.Show(MousePosition);
                }
            }
        }

        private void 属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmAttribute != null && !frmAttribute.IsDisposed) return;
            frmAttribute = new attribute(pTocFeatureLayer);//pTocFeatureLayer在axTOCControl1_OnMouseDown已经被复制为当前选择的图层
            frmAttribute.ShowDialog();
            frmAttribute = null;
        }

        private void 缩放到图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null) return;
            axMapControl1.ActiveView.Extent = pTocFeatureLayer.AreaOfInterest;//视图放大到选中的图层
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,null,null);
        }

        private void 移除图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pTocFeatureLayer == null) return;
                DialogResult result = MessageBox.Show("是否删除["+pTocFeatureLayer.Name+"]图层","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    axMapControl1.Map.DeleteLayer(pTocFeatureLayer);
                }
                axMapControl1.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 图层可选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pTocFeatureLayer.Selectable = true;
            图层可选ToolStripMenuItem.Enabled = !图层可选ToolStripMenuItem.Enabled;

        }

        private void 图层不可选ToolStripMenuItem_Click(object sender, EventArgs e)//使得选择要素过程中无法选择此图层要素
        {
            pTocFeatureLayer.Selectable = false;
            图层不可选ToolStripMenuItem.Enabled =! 图层不可选ToolStripMenuItem.Enabled;
        }

        private void 全域导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frmExpMap==null||frmExpMap.IsDisposed)
            {
                frmExpMap = new mapexport(axMapControl1);
            }
            frmExpMap.isRegion = false;//表示为全域导出
            frmExpMap.geometry = axMapControl1.ActiveView.Extent;
            frmExpMap.Show();
            frmExpMap.Activate();
        }

        private void 区域导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            mousedownname = "ExportRegion";
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();//一个函数，下面定义
        }
        private void SynchronizeEagleEye()//加载鹰眼视图
        {
            if (EagleEyemapcontrol.LayerCount > 0)//清除原有图层
            {
                EagleEyemapcontrol.ClearLayers();
            }
            EagleEyemapcontrol.SpatialReference = axMapControl1.SpatialReference;//鹰眼视图坐标系和主地图坐标系相同
            for (int i = axMapControl1.LayerCount - 1; i >= 0; i--)
            {
                ILayer player = axMapControl1.get_Layer(i);//获取第i个视图
                if (player is IGroupLayer || player is ICompositeLayer)//判断此视图是否内置还有图层
                {
                    ICompositeLayer pcompositelayer = (ICompositeLayer)player;//强制转换
                    for (int j = pcompositelayer.Count - 1; j >= 0; j--)
                    {
                        ILayer psublayer = pcompositelayer.get_Layer(j);//获取此视图中第i个图层
                        IFeatureLayer pfeaturelayer = psublayer as IFeatureLayer;//获得此图层作为要素图层
                        if (pfeaturelayer == null) continue;//此图层为空则返回进行下一个图层操作
                        if (pfeaturelayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint && pfeaturelayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                            EagleEyemapcontrol.AddLayer(player);
                    }
                }
                else
                {
                    IFeatureLayer pfeaturelayer1 = new FeatureLayer();
                    pfeaturelayer1 = player as IFeatureLayer;
                    if (pfeaturelayer1 == null) continue;
                    if (pfeaturelayer1 != null)
                    {
                        if (pfeaturelayer1.FeatureClass == null)
                            return;
                        else
                        {
                            if (pfeaturelayer1.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint && pfeaturelayer1.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                                EagleEyemapcontrol.AddLayer(player);
                        }
                            
                    }
                }
                EagleEyemapcontrol.Extent = axMapControl1.FullExtent;
                pEnv = axMapControl1.Extent;
                DrawRectangle(pEnv);//定义一个函数，在鹰眼视图上画矩形框
                EagleEyemapcontrol.Refresh();

            }

        }
        private void DrawRectangle(IEnvelope penvelope)
        {
            //先清除鹰眼中之前的矩形框
            IGraphicsContainer pGraphicscontainer = EagleEyemapcontrol.Map as IGraphicsContainer;
            if (pGraphicscontainer == null) return;
            IActiveView pActiveView = pGraphicscontainer as IActiveView;
            pGraphicscontainer.DeleteAllElements();
            //得到当前视图范围
            IRectangleElement pReactangleElement = new RectangleElementClass();
            IElement pElement = pReactangleElement as IElement;
            pElement.Geometry = penvelope;
            //设置红色透明框
            ILineSymbol poutline = new SimpleLineSymbolClass
            {
                Width = 2,
                Color = new RgbColorClass { Transparency = 255, Red = 255 }

            };
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass
            {
                Color = new RgbColorClass { Transparency = 0 },
                Outline = poutline
            };
            //添加矩形框
            IFillShapeElement pfillshapeelement = pElement as IFillShapeElement;
            pfillshapeelement.Symbol = pFillSymbol;//样式
            pGraphicscontainer.AddElement((IElement)pfillshapeelement,0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//刷新

        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
        }

        private void EagleEyemapcontrol_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (EagleEyemapcontrol.Map.LayerCount <= 0) return;
            if (e.button == 1) //单击左键移动矩形框
            {
                if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    bCanDrag = true;
                pMoveRectPoint = new PointClass();
                pMoveRectPoint.PutCoords(e.mapX, e.mapY);
            }
            else if (e.button == 2)//单击右键绘制矩形框
            {
                IEnvelope penvelope = EagleEyemapcontrol.TrackRectangle();
                IPoint ptemppoint = new PointClass();
                ptemppoint.PutCoords(penvelope.XMin + penvelope.Width / 2, penvelope.YMin + penvelope.Height / 2);
                axMapControl1.Extent = penvelope;
                axMapControl1.CenterAt(ptemppoint);//做一个中心调整
            }
        }

        private void EagleEyemapcontrol_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (pEnv==null)
                return;
            else if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerHand;//鼠标移动到框内，鼠标换成小手，表示可以移动
                if (e.button == 2)//在内部按下鼠标右击，将鼠标演示设置为默认样式
                    EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            else
                EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerDefault;//在其他位置将鼠标设为默认的样式
            if (!bCanDrag) return;
            //记录鼠标移动的距离
            double dx = e.mapX - pMoveRectPoint.X;
            double dy = e.mapY - pMoveRectPoint.Y;
            pEnv.Offset(dx, dy);//根据偏移量修改pEnv的位置
            pMoveRectPoint.PutCoords(e.mapX,e.mapX);
            DrawRectangle(pEnv);
            axMapControl1.Extent = pEnv;
            
        }

        private void EagleEyemapcontrol_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button != 1 || pMoveRectPoint == null) return;
            if (e.mapX == pMoveRectPoint.X && e.mapY == pMoveRectPoint.Y)
                axMapControl1.CenterAt(pMoveRectPoint);
            bCanDrag = false;
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView pActiveview = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = pActiveview.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            CopyToPageLayout();//拷贝到布局视图方法
        }
        private void CopyToPageLayout()
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object copyFromMap = axMapControl1.Map;
            object copiedmap = pObjectCopy.Copy(copyFromMap);//复制地图到copiedmap
            object copytomap = axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(copiedmap, ref copytomap);//复制地图
            axPageLayoutControl1.ActiveView.Refresh();
        }

        private void 属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            querybyattribute frmquerybyattri = new querybyattribute(axMapControl1.Map);
            frmquerybyattri.ShowDialog();
        }

        private void 唯一值符号化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uniquevaluerender frmuniqueValueRen = new uniquevaluerender();
            frmuniqueValueRen.UniqueValueRender += frmUnique_ValueRender;//委托函数，在后面定义
            frmuniqueValueRen.Map = axMapControl1.Map;
            frmuniqueValueRen.InitUI();//窗口初始化
            frmuniqueValueRen.ShowDialog();

        }
        private void frmUnique_ValueRender(string sFeatClsName, string sFieldName)
        {
            
            IFeatureLayer pFeatLyr = GetFeatLyrByName(axMapControl1.Map, sFeatClsName);
            UniqueValueRenderer(pFeatLyr, sFieldName);
        }
        #region UniqueValueRenderer函数其实并不知道在干嘛
        private void UniqueValueRenderer(IFeatureLayer pFeatLyr, string sFieldName)
        {
            IGeoFeatureLayer pGeoFeatureLayer = pFeatLyr as IGeoFeatureLayer;
            ITable pTable = pFeatLyr as ITable;
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();
            int intFieldNumber = pTable.FindField(sFieldName);
            pUniqueValueRender.FieldCount = 1;//设置唯一值符号化的关键字段为一个
            pUniqueValueRender.set_Field(0, sFieldName);//设置唯一值符号化的第一个关键字段
            IRandomColorRamp pRandomcolorramp = new RandomColorRampClass();//随机颜色
            pRandomcolorramp.StartHue = 0;
            pRandomcolorramp.MinValue = 0;
            pRandomcolorramp.MinSaturation = 15;
            pRandomcolorramp.EndHue = 360;
            pRandomcolorramp.MaxValue = 100;
            pRandomcolorramp.MaxSaturation = 30;//到这儿为止
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pRandomcolorramp.Size = pFeatLyr.FeatureClass.FeatureCount(pQueryFilter);//通过属性字段的值的个数来添加颜色个数
            bool bSuccess;
            pRandomcolorramp.CreateRamp(out bSuccess);
            IEnumColors pEnumRamp = pRandomcolorramp.Colors;
            //查询字段的值
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(sFieldName);
            ICursor pCursor = pTable.Search(pQueryFilter,true);//获取属性表中对应字段的值的游标
            IRow pNextRow = pCursor.NextRow();//获取下一行，这边应是第一行
            while(pNextRow!=null)//若不为空添加颜色并获取下一行循环到结束
            {
                IRowBuffer pnextrowBuffer = pNextRow;
                object codeValue=pnextrowBuffer.Value[intFieldNumber];
                IColor pnextuniquecolor = pEnumRamp.Next();
                if (pnextuniquecolor==null){
                    pEnumRamp.Reset();
                    pnextuniquecolor = pEnumRamp.Next();

                }
                switch (pGeoFeatureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        {
                            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                            pFillSymbol.Color = pnextuniquecolor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pFillSymbol as ISymbol);
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPolyline:
                        {
                            ILineSymbol plineSymbol = new SimpleLineSymbolClass();
                            plineSymbol.Color = pnextuniquecolor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", plineSymbol as ISymbol);
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                    case esriGeometryType.esriGeometryPoint:
                        {
                            IMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                            pMarkerSymbol.Color = pnextuniquecolor;
                            pUniqueValueRender.AddValue(codeValue.ToString(), "", pMarkerSymbol as ISymbol);
                            pNextRow = pCursor.NextRow();
                            break;
                        }
                }
            }
            pGeoFeatureLayer.Renderer = pUniqueValueRender as IFeatureRenderer;
            axMapControl1.Refresh();
            axTOCControl1.Update();
        }
        #endregion
        #region 图层名称获取图层
        /// <summary>
        /// 由图层名称获取图层
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="sFeatLyrName"></param>
        /// <returns></returns>
        public IFeatureLayer GetFeatLyrByName(IMap pLayer, string sFeatLyrName)
        {
            IMap pLyr = null;
            IFeatureLayer pFeatureLyr = null;
            IFeatureLayer pFeatLyr = null;
            for (int i=0; i < pLayer.LayerCount; i++)
            {
                pFeatLyr=pLayer.get_Layer(i) as IFeatureLayer;
                if (pFeatLyr.FeatureClass.AliasName == sFeatLyrName)
                {
                    pFeatureLyr = pFeatLyr;
                    return pFeatureLyr;
                }
                else
                    continue;
            }
            return pFeatureLyr;
        }
        #endregion

        private void 分级色彩符号化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graduatecolors frmgra = new graduatecolors();
            frmgra.Graduatedcolors +=frmgra_Graduatedcolors;
            frmgra.Map = axMapControl1.Map;//获取_map
            frmgra.InitUI();
            frmgra.ShowDialog();
        }

        void frmgra_Graduatedcolors(string sFeatClsName, string sFieldName, int intnumclasses)
        {
            IFeatureLayer pfeature = GetFeatLyrByName(axMapControl1.Map,sFeatClsName);
            GraduatedColors(pfeature, sFieldName, intnumclasses);
            
        }
        public void GraduatedColors(IFeatureLayer pfeatLyr, string sFieldName, int numclasses)
        {
            IGeoFeatureLayer pGeoFeatureL = pfeatLyr as IGeoFeatureLayer;
            object dataFrequency;
            object dataValues;
            bool ok;
            int breakIndex;
            ITable pTable = pGeoFeatureL.FeatureClass as ITable;//各个字段变成一个表
            ITableHistogram pTableHistogram = new BasicTableHistogramClass();
            IBasicHistogram pBasicHistogram = (IBasicHistogram)pTableHistogram;
            pTableHistogram.Field = sFieldName;
            pTableHistogram.Table = pTable;
            pBasicHistogram.GetHistogram(out dataValues, out dataFrequency);//获取渲染值及其出现的频率
            IClassifyGEN pClassify = new EqualIntervalClass();
            pClassify.Classify(dataValues, dataFrequency, ref numclasses);//进行等级划分
            double[] Classes = pClassify.ClassBreaks as double[];
            int classescount = Classes.GetUpperBound(0);
            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = sFieldName;//分段字段
            pClassBreaksRenderer.BreakCount = classescount;//设置分级数目
            pClassBreaksRenderer.SortClassesAscending = true;//分机后的图例是否按升级顺序排列

            //分级颜色带的开始颜色和结束颜色（即分级颜色在此范围内）
            IHsvColor pFromColor = new HsvColorClass();//起始颜色
            pFromColor.Hue = 0;//黄色
            pFromColor.Saturation = 50;
            pFromColor.Value = 96;
            IHsvColor ptocolor = new HsvColorClass();//终止颜色
            ptocolor.Hue = 80;//不知道什么颜色
            ptocolor.Saturation = 100;
            ptocolor.Value = 96;
            

            //产生颜色带对象
            IAlgorithmicColorRamp pAlgorithmicColorRamp = new AlgorithmicColorRampClass();
            pAlgorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            pAlgorithmicColorRamp.FromColor = pFromColor;
            pAlgorithmicColorRamp.ToColor = ptocolor;
            pAlgorithmicColorRamp.Size = classescount;
            pAlgorithmicColorRamp.CreateRamp(out ok);

            //获得颜色
            IEnumColors pEnumColors = pAlgorithmicColorRamp.Colors;
            //symbol和break下标从0开始
            for (breakIndex = 0; breakIndex <= classescount - 1; breakIndex++)
            {
                IColor pColor = pEnumColors.Next();
                switch (pGeoFeatureL.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon: 
                    {
                        ISimpleFillSymbol pSimpleFills = new SimpleFillSymbolClass();
                        pSimpleFills.Color = pColor;
                        pSimpleFills.Style = esriSimpleFillStyle.esriSFSSolid;
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimpleFills);//设置填充符号
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//设定每一分级的分级断点
                        break;
                    }
                    case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol pSimplelines = new SimpleLineSymbolClass();
                        pSimplelines.Color = pColor;
                        
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimplelines);//设置填充符号
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//设定每一分级的分级断点
                        break;
                    }
                    case esriGeometryType.esriGeometryPoint:
                    {
                        ISimpleMarkerSymbol pSimplemaker = new SimpleMarkerSymbolClass();
                        pSimplemaker.Color = pColor;
                        
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimplemaker);//设置填充符号
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//设定每一分级的分级断点
                        break;
                    }
                }
            }
            pGeoFeatureL.Renderer = (IFeatureRenderer)pClassBreaksRenderer;
            axMapControl1.Refresh();
            axTOCControl1.Update();

        }

    }
}