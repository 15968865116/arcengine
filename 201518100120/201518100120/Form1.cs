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
        //����ȫ�ֱ���
        private measureresult frmMeasureresult;//����������       
        private INewLineFeedback pNewLineFeedback;//׷���߶���
        private INewPolygonFeedback pNewPolygonFeedback;//׷�������
        private IPoint pPointPt;//�������
        private IPoint pMovePt;//����ƶ���ǰ�� 
        private double dToltaLength;//�����ܳ���
        private double dSegmentLength;//Ƭ�ξ���
        private IPointCollection pAreaPointCollection =new MultipointClass();//�������ʱ���ĵ���д洢
        string mousedownname = "";//��갴�²�����ȫ�ֱ���
        private object missing = Type.Missing;
       //TOCControl�Ҽ��˵����õ��ı���
        IFeatureLayer pTocFeatureLayer = null;//�����Ҫ��ͼ��
        public attribute frmAttribute = null;//�½�һ�����Ա�
        private mapexport frmExpMap=null;//�½�һ���������
        //ӥ��ȫ�ֱ���
        private bool bCanDrag;//ӥ�۵�ͼ�ϵľ��ο��ƶ��ı�־
        private IPoint pMoveRectPoint;//��¼���ƶ�ӥ�۵�ͼ�ϵľ��ο�ʱ������λ��
        private IEnvelope pEnv;//��¼������ͼ��extent




        private void frmMeasureResult_frmClosed()//ί�к��������õ��¼�����Ҫ���ڽ������������ߺ������
        {
            if (pNewLineFeedback != null)//�ж�������Ƿ�Ϊ�ղ�Ϊ�������
            {
                pNewLineFeedback.Stop();
                pNewLineFeedback = null;
            }
            if (pNewPolygonFeedback != null)//��������
            {
                pNewPolygonFeedback.Stop();
                pNewPolygonFeedback = null;
                pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//��յ㼯�����е�
            }
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);//������㻭���ߡ������
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;//�������㹦��
            mousedownname = "";

        }


        private void �򿪵�ͼ�ĵ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.CheckFileExists = true;
            openfiledialog.Title = @"�򿪵�ͼ�ĵ�";
            openfiledialog.Filter = @"ArcMap�ĵ�(*.mxd)|*.mxd;|Arcmapģ��(*.mxt)|*.mxt";
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
                        MessageBox.Show(fileName + "����Ч�ĵ�ͼ�ĵ�", "��ʾ");
                    }

                }
            }
        }

        private void ��shapefile�ļ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.CheckFileExists = true;
            openfiledialog.Title = @"��shapefile";
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

        private void ��һ����ͼToolStripMenuItem_Click(object sender, EventArgs e)//����һ����ͼ
        {
            
            IExtentStack pExtentStack = axMapControl1.ActiveView.ExtentStack;
            

            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();//ת����һ����ͼ
                ��һ����ͼToolStripMenuItem.Enabled = true;//��ʱ��һ����ͼ���ڣ���һ����ͼ��ť��Ϊ�ɰ�
                if (!pExtentStack.CanUndo())
                {
                    ��һ����ͼToolStripMenuItem.Enabled = false;//�����һ����ͼ�����ڣ�����һ����ͼ�����ť��Ϊ�����á�
                }
                
               
            }
            axMapControl1.ActiveView.Refresh();
        }

        private void �Ŵ�ToolStripMenuItem_Click(object sender, EventArgs e)//�Ŵ�
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(0.5,0.5,true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();
        }

        private void ��СToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(2, 2, true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();
        }
       
        

        private void ����Ŵ�ToolStripMenuItem_Click(object sender, EventArgs e)//����Ŵ�
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
                case "zoomin"://����Ŵ�
                    envelope = axMapControl1.TrackRectangle();//��ȡ������Ϣ
                    if (envelope == null || envelope.IsEmpty || envelope.Height == 0 || envelope.Width == 0)//�ж��Ƿ�Ϊ�տ�
                        return;
                    pActiveview.Extent = envelope;
                    pActiveview.Refresh();
                    break;
                case "zoomout"://������С
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
                case "manyou"://����
                    axMapControl1.Pan();
                    break;
                case "MeasureLength"://��������
                    
                    if (pNewLineFeedback == null)
                    {
                        pNewLineFeedback = new NewLineFeedback { Display = pActiveview.ScreenDisplay };//ʵ����׷���߶���
                        pNewLineFeedback.Start(pPointPt);//������㣬��ʼ��̬����
                        dToltaLength = 0;
                    }
                    else//���׷���߶���Ϊ�գ�����ӵ�ǰ����
                    {
                        pNewLineFeedback.AddPoint(pPointPt);
                        
                    }
                    if (dSegmentLength != 0)
                        dToltaLength = dToltaLength + dSegmentLength;
                    break;
                case "MeasureArea"://�������
                    
                    if (pNewPolygonFeedback == null)
                    {
                        pNewPolygonFeedback = new NewPolygonFeedback { Display = pActiveview.ScreenDisplay };//ʵ���������
                        pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//��յ㼯
                        pNewPolygonFeedback.Start(pPointPt);//��ʼ���ƶ����
                        pAreaPointCollection.AddPoint(pPointPt, ref missing, ref missing);
                    }
                    else
                    {
                        pNewPolygonFeedback.AddPoint(pPointPt);
                        pAreaPointCollection.AddPoint(pPointPt, ref missing, ref missing);
                    }
                
                    break;
                case "selectfeature"://Ҫ��ѡ��
                    IEnvelope pEnv = axMapControl1.TrackRectangle();
                    IGeometry pGeo = pEnv;
                    if (pEnv.IsEmpty)//��Ϊ��������굱ǰ�����ѡ�񲿷�������Ϊ��
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

        private void ������СToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "zoomout";//����mousedown����¼�һ���������Թ�ѡ����Ҫ�����Ĳ�����
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "manyou";//����mousedown����¼�һ���������Թ�ѡ����Ҫ�����Ĳ�����
        }

        private void ȫͼ��ʾToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        private void ��һ����ͼToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IExtentStack pExtentStack = axMapControl1.ActiveView.ExtentStack;


            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();//ת����һ����ͼ
                ��һ����ͼToolStripMenuItem.Enabled = true;//ת����һ����ͼ��˵����ǰ����һ����ͼ����ʱ��һ����ͼ�����ť��ɿɰ�
                if (!pExtentStack.CanRedo())
                {
                    ��һ����ͼToolStripMenuItem.Enabled = false;//���֮��û�к�һ����ͼ�ˣ���һ����ͼ��ť���
                }


            }
            axMapControl1.ActiveView.Refresh();
        }

        private void ֱ�ӱ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mxdFileName = axMapControl1.DocumentFilename;
            IMapDocument mapDocument = new MapDocument();
            if (mxdFileName != null && axMapControl1.CheckMxFile(mxdFileName))
            {
                if (mapDocument.get_IsReadOnly(mxdFileName))
                {
                    MessageBox.Show("��ͼ�ĵ�ֻ�����޷�����");
                    mapDocument.Close();
                    return;
                }
                
                mapDocument.New(mxdFileName);
                mapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                mapDocument.Save(mapDocument.UsesRelativePaths, true);
                mapDocument.Close();
                MessageBox.Show("��ͼ�ĵ�����ɹ�");

            }
            else
            {
                MessageBox.Show("��ͼ�ĵ�������");
            }
        }

        private void ���ΪToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefiledialog = new SaveFileDialog
            {
                Title = "���Ϊ",
                OverwritePrompt = true,
                Filter = "Arcmap�ĵ�(*.mxd��|*.mxd|ArcMapģ��(*.mxt)|*.mxt",
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

        

        private void �����ǩToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookmark frmbookmark = new bookmark();
            frmbookmark.ShowDialog();//��ʼ��һ����ǩ������
            string pName = frmbookmark.Bookmark;//����ǩ���ָ���pName
            if (pName == "")
                return;

            IMapBookmarks mapbookmarks = axMapControl1.Map as IMapBookmarks;//���ʵ�ǰ��ǩ��
            IEnumSpatialBookmark enumspatialbookmarks = mapbookmarks.Bookmarks;//����ǰ����ǩ������enumspatialbookmarks
            enumspatialbookmarks.Reset();
            ISpatialBookmark pspatialbookmarks;//����һ���յĿռ���ǩ
            while((pspatialbookmarks=enumspatialbookmarks.Next())!=null)//ѭ���ж������Ƿ��Ѿ�����
            {
                if (pName == pspatialbookmarks.Name)
                {
                    DialogResult dr = MessageBox.Show("����ǩ���Ѵ��ڣ��Ƿ��滻��","��ʾ",MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                        mapbookmarks.RemoveBookmark(pspatialbookmarks);
                    else if (dr == DialogResult.No)
                        frmbookmark.ShowDialog();
                    else
                        return;

                }
            }
            IActiveView pactiveview = axMapControl1.Map as IActiveView;//��ȡ��ǰ��ͼ����
            IAOIBookmark pbookmark = new AOIBookmark();
            pbookmark.Location = pactiveview.Extent;//����һ����ǩ��������λ��Ϊ��ǰ��ͼ��Χ
            pbookmark.Name = pName;//�����ǩ��
            IMapBookmarks pmapbookmarks = axMapControl1.Map as IMapBookmarks;//���ʵ�ǰ��ǩ��
            pmapbookmarks.AddBookmark(pbookmark);//�������ǩ����ͼ����ǩ����



        }

        private void ��ǩ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            managebookmark frm = new managebookmark(axMapControl1.Map);
            frm.ShowDialog();
        }
        private string sMapUnit = "δ֪��λ";//��ͼ��λ����

        private string GetMapUnit(esriUnits esriUnit)
        {
            string MapUnit = "";
            switch(esriUnit)
            {
                case esriUnits.esriCentimeters:
                    MapUnit="����";
                    break;
                case esriUnits.esriDecimalDegrees:
                    MapUnit = "ʮ����";
                    break;
                case esriUnits.esriDecimeters:
                    MapUnit = "����";
                    break;
                case esriUnits.esriFeet:
                    MapUnit = "��";
                    break;
                case esriUnits.esriInches:
                    MapUnit = "Ӣ��";
                    break;
                case esriUnits.esriKilometers:
                    MapUnit = "ǧ��";
                    break;
                case esriUnits.esriMeters:
                    MapUnit = "��";
                    break;
                case esriUnits.esriMiles:
                    MapUnit = "Ӣ��";
                    break;
                case esriUnits.esriMillimeters:
                    MapUnit = "����";
                    break;
                case esriUnits.esriNauticalMiles:
                    MapUnit = "����";
                    break;
                case esriUnits.esriPoints:
                    MapUnit = "��";
                    break;
                case esriUnits.esriYards:
                    MapUnit = "��";
                    break;
                case esriUnits.esriUnknownUnits:
                    MapUnit = "δ֪��λ";
                    break;
              }
            return MapUnit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)//���½����������ʾ
        {
            sMapUnit = GetMapUnit(axMapControl1.Map.MapUnits);
            barCoorTxt.Text = string.Format("��ǰ���꣺X{0:#.###}  Y={1:#.###}{2}",e.mapX,e.mapY,sMapUnit);
            pMovePt = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);  //��ȡ�ƶ���
            switch (mousedownname)
            {
                case "MeasureLength"://��������
                    if (pNewLineFeedback != null)
                        pNewLineFeedback.MoveTo(pMovePt);//�ƶ�����ǰ
                    if (pPointPt != null && pNewLineFeedback != null)
                    {
                        double deltaX = pMovePt.X - pPointPt.X;//�����X��
                        double deltaY = pMovePt.Y - pPointPt.Y;//�����Y ��
                        dSegmentLength = Math.Round(Math.Sqrt(deltaX * deltaX + deltaY * deltaY), 3);
                        dToltaLength = dToltaLength + dSegmentLength;
                        if (frmMeasureresult != null)
                        {
                            frmMeasureresult.label2.Text=String.Format("��ǰ�����߶�Ϊ��{0:.###}{1};\r\n�ܳ���Ϊ��{2:.###}{1}",dSegmentLength,sMapUnit,dToltaLength);
                            dToltaLength = dToltaLength - dSegmentLength;//����ƶ����µ����¿�ʼ����
                        }
                        
                    }

                    break;
                case "MeasureArea"://�������
                    if (pNewPolygonFeedback != null)
                        pNewPolygonFeedback.MoveTo(pMovePt);
                    IPointCollection pPointCol=new Polygon();//ʵ����һ���µĵ㼯����
                    IPolygon pPolygon = new PolygonClass();//ʵ����һ���漸�ζ���
                    
                    
                    for (int i = 0; i <= pAreaPointCollection.PointCount - 1; i++)
                    {
                        
                        pPointCol.AddPoint(pAreaPointCollection.get_Point(i), ref missing, ref missing);//��ȫ�ֱ������Ѿ�ȷ���ĵ㼯����pPointCol����㼯����
                    }
                        
                    pPointCol.AddPoint(pMovePt, ref missing, ref missing);//����ǰ���ƶ��㸳��pPointCol����㼯����

                    if (pPointCol.PointCount < 3)
                    {
                        
                        return;//С���������޷��γ��棬��ִ���������
                    }
                    pPolygon = pPointCol as IPolygon;//���㼯���γ�һ�������
                    if ((pPolygon != null))
                    {
                        pPolygon.Close();
                        IGeometry pGeo = pPolygon;
                        ITopologicalOperator pTopo = pGeo as ITopologicalOperator;
                        pTopo.Simplify();
                        pGeo.Project(axMapControl1.Map.SpatialReference);
                        IArea pArea = pGeo as IArea;
                        frmMeasureresult.label2.Text = String.Format("�����Ϊ��{0:.####}ƽ��{1};\r\n�ܳ���Ϊ��{2:.#####}{1}",pArea.Area,sMapUnit,pPolygon.Length);
                        
                        
                        
                        
                    }
                    
                    break;
            }
        }

        

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            mousedownname = "MeasureLength";//���������������ݲ���
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//�ı������״
            if (frmMeasureresult == null || frmMeasureresult.IsDisposed)//���ڲ��������½�һ������
            {
                frmMeasureresult = new measureresult();
                frmMeasureresult.frmClosed+=frmMeasureResult_frmClosed;//ί���¼����Ǻ���ĺ���
                frmMeasureresult.Show();
            }
            else
            {
                frmMeasureresult.Activate();//������ڵĴ���
            }

        }

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            switch(mousedownname)
            {
                case "MeasureLength":
                    if(frmMeasureresult!=null)
                    {
                        frmMeasureresult.label2.Text = "�߶��ܳ���Ϊ��"+dToltaLength+sMapUnit;
                    }
                    if (pNewLineFeedback != null)
                    {
                        pNewLineFeedback.Stop();
                        pNewLineFeedback = null;
                        //����߶���
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
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);//��������

                    }
                    pAreaPointCollection.RemovePoints(0, pAreaPointCollection.PointCount);//��յ㼯�����е�
                    

                    break;
            }
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)//��ʼ����ر������������ⴰ��
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

        private void ѡ��Ҫ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mousedownname = "selectfeature";
        }

        private void ������ѡ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int nSelection = axMapControl1.Map.SelectionCount;//ͳ��ѡ���Ҫ�صĸ���
            if (nSelection == 0)
            {
                MessageBox.Show("����ѡ��Ҫ�أ�", "��ʾ");
            }
            else
            {
                ISelection selection = axMapControl1.Map.FeatureSelection;//ѡ���Ҫ��
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

        private void ���ѡ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IActiveView pActiveview = axMapControl1.ActiveView;
            pActiveview.FocusMap.ClearSelection();
            pActiveview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,pActiveview.Extent);
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)//�Ҽ���1Ϊ�����
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
                    ͼ���ѡToolStripMenuItem.Enabled = !pTocFeatureLayer.Selectable;//���õ�ǰΪͼ��Ϊ��ѡ�����Դ˰�ť��ɫ
                    ͼ�㲻��ѡToolStripMenuItem.Enabled = pTocFeatureLayer.Selectable;//ͼ�㲻��ѡ��ť���ſ���ѡ
                    contextMenuStrip1.Show(MousePosition);
                }
            }
        }

        private void ���Ա�ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmAttribute != null && !frmAttribute.IsDisposed) return;
            frmAttribute = new attribute(pTocFeatureLayer);//pTocFeatureLayer��axTOCControl1_OnMouseDown�Ѿ�������Ϊ��ǰѡ���ͼ��
            frmAttribute.ShowDialog();
            frmAttribute = null;
        }

        private void ���ŵ�ͼ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null) return;
            axMapControl1.ActiveView.Extent = pTocFeatureLayer.AreaOfInterest;//��ͼ�Ŵ�ѡ�е�ͼ��
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,null,null);
        }

        private void �Ƴ�ͼ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pTocFeatureLayer == null) return;
                DialogResult result = MessageBox.Show("�Ƿ�ɾ��["+pTocFeatureLayer.Name+"]ͼ��","��ʾ",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
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

        private void ͼ���ѡToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pTocFeatureLayer.Selectable = true;
            ͼ���ѡToolStripMenuItem.Enabled = !ͼ���ѡToolStripMenuItem.Enabled;

        }

        private void ͼ�㲻��ѡToolStripMenuItem_Click(object sender, EventArgs e)//ʹ��ѡ��Ҫ�ع������޷�ѡ���ͼ��Ҫ��
        {
            pTocFeatureLayer.Selectable = false;
            ͼ�㲻��ѡToolStripMenuItem.Enabled =! ͼ�㲻��ѡToolStripMenuItem.Enabled;
        }

        private void ȫ�򵼳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frmExpMap==null||frmExpMap.IsDisposed)
            {
                frmExpMap = new mapexport(axMapControl1);
            }
            frmExpMap.isRegion = false;//��ʾΪȫ�򵼳�
            frmExpMap.geometry = axMapControl1.ActiveView.Extent;
            frmExpMap.Show();
            frmExpMap.Activate();
        }

        private void ���򵼳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            mousedownname = "ExportRegion";
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();//һ�����������涨��
        }
        private void SynchronizeEagleEye()//����ӥ����ͼ
        {
            if (EagleEyemapcontrol.LayerCount > 0)//���ԭ��ͼ��
            {
                EagleEyemapcontrol.ClearLayers();
            }
            EagleEyemapcontrol.SpatialReference = axMapControl1.SpatialReference;//ӥ����ͼ����ϵ������ͼ����ϵ��ͬ
            for (int i = axMapControl1.LayerCount - 1; i >= 0; i--)
            {
                ILayer player = axMapControl1.get_Layer(i);//��ȡ��i����ͼ
                if (player is IGroupLayer || player is ICompositeLayer)//�жϴ���ͼ�Ƿ����û���ͼ��
                {
                    ICompositeLayer pcompositelayer = (ICompositeLayer)player;//ǿ��ת��
                    for (int j = pcompositelayer.Count - 1; j >= 0; j--)
                    {
                        ILayer psublayer = pcompositelayer.get_Layer(j);//��ȡ����ͼ�е�i��ͼ��
                        IFeatureLayer pfeaturelayer = psublayer as IFeatureLayer;//��ô�ͼ����ΪҪ��ͼ��
                        if (pfeaturelayer == null) continue;//��ͼ��Ϊ���򷵻ؽ�����һ��ͼ�����
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
                DrawRectangle(pEnv);//����һ����������ӥ����ͼ�ϻ����ο�
                EagleEyemapcontrol.Refresh();

            }

        }
        private void DrawRectangle(IEnvelope penvelope)
        {
            //�����ӥ����֮ǰ�ľ��ο�
            IGraphicsContainer pGraphicscontainer = EagleEyemapcontrol.Map as IGraphicsContainer;
            if (pGraphicscontainer == null) return;
            IActiveView pActiveView = pGraphicscontainer as IActiveView;
            pGraphicscontainer.DeleteAllElements();
            //�õ���ǰ��ͼ��Χ
            IRectangleElement pReactangleElement = new RectangleElementClass();
            IElement pElement = pReactangleElement as IElement;
            pElement.Geometry = penvelope;
            //���ú�ɫ͸����
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
            //��Ӿ��ο�
            IFillShapeElement pfillshapeelement = pElement as IFillShapeElement;
            pfillshapeelement.Symbol = pFillSymbol;//��ʽ
            pGraphicscontainer.AddElement((IElement)pfillshapeelement,0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//ˢ��

        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
        }

        private void EagleEyemapcontrol_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (EagleEyemapcontrol.Map.LayerCount <= 0) return;
            if (e.button == 1) //��������ƶ����ο�
            {
                if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    bCanDrag = true;
                pMoveRectPoint = new PointClass();
                pMoveRectPoint.PutCoords(e.mapX, e.mapY);
            }
            else if (e.button == 2)//�����Ҽ����ƾ��ο�
            {
                IEnvelope penvelope = EagleEyemapcontrol.TrackRectangle();
                IPoint ptemppoint = new PointClass();
                ptemppoint.PutCoords(penvelope.XMin + penvelope.Width / 2, penvelope.YMin + penvelope.Height / 2);
                axMapControl1.Extent = penvelope;
                axMapControl1.CenterAt(ptemppoint);//��һ�����ĵ���
            }
        }

        private void EagleEyemapcontrol_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (pEnv==null)
                return;
            else if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerHand;//����ƶ������ڣ���껻��С�֣���ʾ�����ƶ�
                if (e.button == 2)//���ڲ���������һ����������ʾ����ΪĬ����ʽ
                    EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            else
                EagleEyemapcontrol.MousePointer = esriControlsMousePointer.esriPointerDefault;//������λ�ý������ΪĬ�ϵ���ʽ
            if (!bCanDrag) return;
            //��¼����ƶ��ľ���
            double dx = e.mapX - pMoveRectPoint.X;
            double dy = e.mapY - pMoveRectPoint.Y;
            pEnv.Offset(dx, dy);//����ƫ�����޸�pEnv��λ��
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
            CopyToPageLayout();//������������ͼ����
        }
        private void CopyToPageLayout()
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object copyFromMap = axMapControl1.Map;
            object copiedmap = pObjectCopy.Copy(copyFromMap);//���Ƶ�ͼ��copiedmap
            object copytomap = axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(copiedmap, ref copytomap);//���Ƶ�ͼ
            axPageLayoutControl1.ActiveView.Refresh();
        }

        private void ���Բ�ѯToolStripMenuItem_Click(object sender, EventArgs e)
        {
            querybyattribute frmquerybyattri = new querybyattribute(axMapControl1.Map);
            frmquerybyattri.ShowDialog();
        }

        private void Ψһֵ���Ż�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uniquevaluerender frmuniqueValueRen = new uniquevaluerender();
            frmuniqueValueRen.UniqueValueRender += frmUnique_ValueRender;//ί�к������ں��涨��
            frmuniqueValueRen.Map = axMapControl1.Map;
            frmuniqueValueRen.InitUI();//���ڳ�ʼ��
            frmuniqueValueRen.ShowDialog();

        }
        private void frmUnique_ValueRender(string sFeatClsName, string sFieldName)
        {
            
            IFeatureLayer pFeatLyr = GetFeatLyrByName(axMapControl1.Map, sFeatClsName);
            UniqueValueRenderer(pFeatLyr, sFieldName);
        }
        #region UniqueValueRenderer������ʵ����֪���ڸ���
        private void UniqueValueRenderer(IFeatureLayer pFeatLyr, string sFieldName)
        {
            IGeoFeatureLayer pGeoFeatureLayer = pFeatLyr as IGeoFeatureLayer;
            ITable pTable = pFeatLyr as ITable;
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();
            int intFieldNumber = pTable.FindField(sFieldName);
            pUniqueValueRender.FieldCount = 1;//����Ψһֵ���Ż��Ĺؼ��ֶ�Ϊһ��
            pUniqueValueRender.set_Field(0, sFieldName);//����Ψһֵ���Ż��ĵ�һ���ؼ��ֶ�
            IRandomColorRamp pRandomcolorramp = new RandomColorRampClass();//�����ɫ
            pRandomcolorramp.StartHue = 0;
            pRandomcolorramp.MinValue = 0;
            pRandomcolorramp.MinSaturation = 15;
            pRandomcolorramp.EndHue = 360;
            pRandomcolorramp.MaxValue = 100;
            pRandomcolorramp.MaxSaturation = 30;//�����Ϊֹ
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pRandomcolorramp.Size = pFeatLyr.FeatureClass.FeatureCount(pQueryFilter);//ͨ�������ֶε�ֵ�ĸ����������ɫ����
            bool bSuccess;
            pRandomcolorramp.CreateRamp(out bSuccess);
            IEnumColors pEnumRamp = pRandomcolorramp.Colors;
            //��ѯ�ֶε�ֵ
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(sFieldName);
            ICursor pCursor = pTable.Search(pQueryFilter,true);//��ȡ���Ա��ж�Ӧ�ֶε�ֵ���α�
            IRow pNextRow = pCursor.NextRow();//��ȡ��һ�У����Ӧ�ǵ�һ��
            while(pNextRow!=null)//����Ϊ�������ɫ����ȡ��һ��ѭ��������
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
        #region ͼ�����ƻ�ȡͼ��
        /// <summary>
        /// ��ͼ�����ƻ�ȡͼ��
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

        private void �ּ�ɫ�ʷ��Ż�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graduatecolors frmgra = new graduatecolors();
            frmgra.Graduatedcolors +=frmgra_Graduatedcolors;
            frmgra.Map = axMapControl1.Map;//��ȡ_map
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
            ITable pTable = pGeoFeatureL.FeatureClass as ITable;//�����ֶα��һ����
            ITableHistogram pTableHistogram = new BasicTableHistogramClass();
            IBasicHistogram pBasicHistogram = (IBasicHistogram)pTableHistogram;
            pTableHistogram.Field = sFieldName;
            pTableHistogram.Table = pTable;
            pBasicHistogram.GetHistogram(out dataValues, out dataFrequency);//��ȡ��Ⱦֵ������ֵ�Ƶ��
            IClassifyGEN pClassify = new EqualIntervalClass();
            pClassify.Classify(dataValues, dataFrequency, ref numclasses);//���еȼ�����
            double[] Classes = pClassify.ClassBreaks as double[];
            int classescount = Classes.GetUpperBound(0);
            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = sFieldName;//�ֶ��ֶ�
            pClassBreaksRenderer.BreakCount = classescount;//���÷ּ���Ŀ
            pClassBreaksRenderer.SortClassesAscending = true;//�ֻ����ͼ���Ƿ�����˳������

            //�ּ���ɫ���Ŀ�ʼ��ɫ�ͽ�����ɫ�����ּ���ɫ�ڴ˷�Χ�ڣ�
            IHsvColor pFromColor = new HsvColorClass();//��ʼ��ɫ
            pFromColor.Hue = 0;//��ɫ
            pFromColor.Saturation = 50;
            pFromColor.Value = 96;
            IHsvColor ptocolor = new HsvColorClass();//��ֹ��ɫ
            ptocolor.Hue = 80;//��֪��ʲô��ɫ
            ptocolor.Saturation = 100;
            ptocolor.Value = 96;
            

            //������ɫ������
            IAlgorithmicColorRamp pAlgorithmicColorRamp = new AlgorithmicColorRampClass();
            pAlgorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            pAlgorithmicColorRamp.FromColor = pFromColor;
            pAlgorithmicColorRamp.ToColor = ptocolor;
            pAlgorithmicColorRamp.Size = classescount;
            pAlgorithmicColorRamp.CreateRamp(out ok);

            //�����ɫ
            IEnumColors pEnumColors = pAlgorithmicColorRamp.Colors;
            //symbol��break�±��0��ʼ
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
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimpleFills);//����������
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//�趨ÿһ�ּ��ķּ��ϵ�
                        break;
                    }
                    case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol pSimplelines = new SimpleLineSymbolClass();
                        pSimplelines.Color = pColor;
                        
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimplelines);//����������
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//�趨ÿһ�ּ��ķּ��ϵ�
                        break;
                    }
                    case esriGeometryType.esriGeometryPoint:
                    {
                        ISimpleMarkerSymbol pSimplemaker = new SimpleMarkerSymbolClass();
                        pSimplemaker.Color = pColor;
                        
                        pClassBreaksRenderer.set_Symbol(breakIndex, (ISymbol)pSimplemaker);//����������
                        pClassBreaksRenderer.set_Break(breakIndex, Classes[breakIndex + 1]);//�趨ÿһ�ּ��ķּ��ϵ�
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