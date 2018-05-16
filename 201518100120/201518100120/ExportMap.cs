using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;

namespace _201518100120 {
    public class ExportMap {

        public static void ExportView(IActiveView activeView, IGeometry pGeo, int OutputResolution, int Width,
    int Height, string ExpPath, bool bRegion)
        {
            IExport pExport;
            tagRECT exportRect = new tagRECT();
            IEnvelope pEnvelope = pGeo.Envelope;
            string sType = System.IO.Path.GetExtension(ExpPath);
            switch (sType)
            {
                case ".jpg":
                    pExport = new ExportJPEGClass();
                    break;
                case ".bmp":
                    pExport = new ExportBMPClass();
                    break;
                case ".gif":
                    pExport = new ExportGIFClass();
                    break;
                case ".tif":
                    pExport = new ExportTIFFClass();
                    break;
                case ".png":
                    pExport = new ExportPNGClass();
                    break;
                case ".pdf":
                    pExport = new ExportPDFClass();
                    break;
                default:
                    MessageBox.Show("没有输出格式，默认到JPEG格式");
                    pExport = new ExportJPEGClass();
                    break;
            }
            pExport.ExportFileName = ExpPath;

            exportRect.left = 0;
            exportRect.top = 0;
            exportRect.right = Width;
            exportRect.bottom = Height;
            if (bRegion)
            {
                //删除区域导出拖出的多边形框
                activeView.GraphicsContainer.DeleteAllElements();
                activeView.Refresh();
            }
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(exportRect.left, exportRect.top, exportRect.right, exportRect.bottom);
            pExport.PixelBounds = envelope;
            activeView.Output(pExport.StartExporting(), OutputResolution, ref exportRect, pEnvelope, null);
            pExport.FinishExporting();
            pExport.Cleanup();
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="mapCtrl"></param>
        /// <returns></returns>
        public static IPolygon DrawPolygon(AxMapControl mapCtrl)
        {
            if (mapCtrl == null) return null;
            IRubberBand rb = new RubberPolygonClass();
            IGeometry pGeometry = rb.TrackNew(mapCtrl.ActiveView.ScreenDisplay, null);
            return pGeometry as IPolygon;
        }

        /// <summary>
        /// 视图窗口绘制几何图形元素
        /// </summary>
        /// <param name="pGeometry">几何图形</param>
        /// <param name="activeView">视图</param>
        public static void AddElement(IGeometry pGeometry, IActiveView activeView)
        {
            IRgbColor fillColor = GetRgbColor(204, 175, 235);
            IRgbColor lineColor = GetRgbColor(0, 0, 0);
            IElement pEle = CreateElement(pGeometry, lineColor, fillColor);
            IGraphicsContainer pGC = activeView.GraphicsContainer;
            if (pGC == null) return;
            pGC.AddElement(pEle, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pEle, null);
        }

        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="intR">红</param>
        /// <param name="intG">绿</param>
        /// <param name="intB">蓝</param>
        /// <returns></returns>
        private static IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
                return pRgbColor;
            pRgbColor = new RgbColorClass
            {
                Red = intR,
                Green = intG,
                Blue = intB
            };
            return pRgbColor;
        }

        /// <summary>
        /// 创建图形元素
        /// </summary>
        /// <param name="pGeometry">几何图形</param>
        /// <param name="lineColor">边框颜色</param>
        /// <param name="fillColor">填充颜色</param>
        /// <returns></returns>
        private static IElement CreateElement(IGeometry pGeometry, IRgbColor lineColor, IRgbColor fillColor)
        {
            if (pGeometry == null || lineColor == null || fillColor == null)
            {
                return null;
            }
            IElement pElem = null;
            try
            {
                if (pGeometry is IEnvelope)
                    pElem = new RectangleElementClass();
                else if (pGeometry is IPolygon)
                    pElem = new PolygonElementClass();
                else if (pGeometry is ICircularArc)
                {
                    ISegment pSegCircle = pGeometry as ISegment;//QI
                    ISegmentCollection pSegColl = new PolygonClass();
                    object o = Type.Missing;
                    pSegColl.AddSegment(pSegCircle, ref o, ref o);
                    IPolygon pPolygon = pSegColl as IPolygon;
                    pGeometry = pPolygon;
                    pElem = new CircleElementClass();
                }
                else if (pGeometry is IPolyline)
                    pElem = new LineElementClass();

                if (pElem == null)
                    return null;
                pElem.Geometry = pGeometry;
                IFillShapeElement pFElem = pElem as IFillShapeElement;
                ISimpleFillSymbol pSymbol = new SimpleFillSymbolClass();
                pSymbol.Color = fillColor;
                pSymbol.Outline.Color = lineColor;
                pSymbol.Style = esriSimpleFillStyle.esriSFSCross;
                pFElem.Symbol = pSymbol;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return pElem;
        } 
    }
}