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

        private void ��һ����ͼToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void �Ŵ�ToolStripMenuItem_Click(object sender, EventArgs e)
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
        string mousedownname = "";
        private void ����Ŵ�ToolStripMenuItem_Click(object sender, EventArgs e)
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
                    envelope = axMapControl1.TrackRectangle();//��ȡ������Ϣ
                    if (envelope == null || envelope.IsEmpty || envelope.Height == 0 || envelope.Width == 0)//�ж��Ƿ�Ϊ�տ�
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

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

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

    }
}