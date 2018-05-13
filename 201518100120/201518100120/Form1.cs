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
        //����ȫ�ֱ���
        private measureresult frmMeasureresult;//����������
        private bool bMeasureArea;//�Ƿ�ʼ������㹦��
        private bool bMeasureLength;//�Ƿ�ʼ��������
        private INewLineFeedback pNewLineFeedback;//׷���߶���
        private INewPolygonFeedback pNewPolygonFeedback;//׷�������
        private IPoint pPointPt;//�������
        private IPoint pMovePt;//����ƶ���ǰ�� 
        private double dToltaLength;//�����ܳ���
        private double dSegmentLength;//Ƭ�ξ���
        private IPointCollection pAreaPointCollection =new MultipointClass();//�������ʱ���ĵ���д洢
        string mousedownname = "";//��갴�²�����ȫ�ֱ���
        private object missing = Type.Missing;

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



    }
}