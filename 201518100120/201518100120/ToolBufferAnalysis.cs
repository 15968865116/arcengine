using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
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
    /// <summary>
    /// Summary description for ToolBufferAnalysis.
    /// </summary>
    [Guid("0afffc34-df00-4c4b-8ded-1110460ddb7b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("_201518100120.ToolBufferAnalysis")]
    public sealed class ToolBufferAnalysis : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;

        public ToolBufferAnalysis()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }
        
        
        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ToolBufferAnalysis.OnClick implementation
            if (m_hookHelper.FocusMap.LayerCount <= 0)//�ж�����ͼ��
            {
                MessageBox.Show("���ȼ���ͼ������");
                return;
            }
            (m_hookHelper.Hook as MapControl).MousePointer = esriControlsMousePointer.esriPointerHand;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolBufferAnalysis.OnMouseDown implementation
            if (Button != 1 || m_hookHelper.FocusMap.LayerCount <= 0) return;
            var pActiveView = m_hookHelper.ActiveView;
            var pGraCont = (IGraphicsContainer)pActiveView;
            pGraCont.DeleteAllElements();//�����ͼ����ӵ�����Element
            var pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);//���λ��ת��Ϊ��ͼ��Ҫ��
            var pFeatureLayer = m_hookHelper.FocusMap.Layer[0] as IFeatureLayer;//��ȡ��ͼ�е�ͼ��
            if (pFeatureLayer == null) return;
            var pFeatureClass = pFeatureLayer.FeatureClass;

            //�������ѯͼ��Ҫ��
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pPoint;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            var featureCursor = pFeatureClass.Search(pSpatialFilter, false);

            //��ȡ�����ѯ��Ҫ��
            var pFeature = featureCursor.NextFeature();
            if (pFeature != null && pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)//��һ�������
            {
                var pGeometry = pFeature.Shape;//��ȡ��״

                //����μ򵥻�����
                var pTopoOpe = (ITopologicalOperator)pGeometry;
                pTopoOpe.Simplify();

                //ͨ���������������
                var pBufferGeo = pTopoOpe.Buffer(5000);

                //��������η�����ʽ����ӵ���ͼ��
                ISimpleFillSymbol pSympol = new SimpleFillSymbolClass();
                pSympol.Style = esriSimpleFillStyle.esriSFSCross;
                RgbColor pColor = new RgbColorClass();
                pColor.Blue = 200;
                pColor.Red = 211;
                pColor.Green = 100;
                pSympol.Color = pColor;

                //�����������ȾЧ����Element
                IFillShapeElement pFillShapeElm = new PolygonElementClass();
                var pElm = (IElement)pFillShapeElm;
                pElm.Geometry = pBufferGeo;
                pFillShapeElm.Symbol = pSympol;

                //����Ⱦ֮��Ķ������ӵ�IGraphicscontainer����
                pGraCont.AddElement((IElement)pFillShapeElm, 0);

            }
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolBufferAnalysis.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolBufferAnalysis.OnMouseUp implementation
        }
        #endregion
    }
}
