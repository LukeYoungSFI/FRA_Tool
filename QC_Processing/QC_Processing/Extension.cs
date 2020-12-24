using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.OleDb;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;


using ESRI.ArcGIS.DataSourcesGDB;


namespace QC_Processing
{
    [Guid("420d048f-ea33-4019-a7f3-0bd3896e5d67")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("QC_Processing.Extension")]
    public class Extension : IExtension
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
            MxExtension.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Unregister(regKey);

        }

        #endregion
        #endregion
        private IApplication m_application;

        #region IExtension Members

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name
        {
            get
            {
                //TODO: Modify string to uniquely identify extension
                return "Extension";
            }
        }

        public void Shutdown()
        {
            //TODO: Clean up resources

            m_application = null;
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
            if (m_application == null)
                return;

            //TODO: Add code to initialize the extension
        }

        #endregion
    }
    public class Forms
    {
        public static frmConfig config_Form = new frmConfig();
        public static frmIdentifyBridges identifyBridge_Form = new frmIdentifyBridges();
        public static frmCheckSpatialAccuracy checkSpatialAccuracy_Form = new frmCheckSpatialAccuracy();
        public static frmAssignBridgeAttributes assignAttributes_Form = new frmAssignBridgeAttributes();
        public static frmPopulateOtherAttributes populateOtherAttributes_Form = new frmPopulateOtherAttributes();
        public static frmQC QC_Form = new frmQC();
    }

    class Data
    {
        public static bool IsDataLoaded = false;
        public static string TargetFeatureLyrNM = "";
        public static IFeatureClass featurePntLyrCS;

        public static string IntersectionFeatureLyrNM = "";
        public static IFeatureClass intersectionPntLyrCS;

        public static string waterIntersectionFeatureLyrNM = "";
        public static IFeatureClass waterIntersectionPntLyrCS;

        public static string groupedTargetFeatureLyrNM = "";
        public static IFeatureClass groupedTargetPntLyrCS;

        public static string waterGroupedTargetFeatureLyrNM = "";
        public static IFeatureClass waterGroupedTargetPntLyrCS;

        public static string RailroadLyrNM = "";
        public static IFeatureClass railroadLineLyrCS;

        public static string RoadLyrNM = "";
        public static IFeatureClass roadLineLyrCS;

        public static string WaterLyrNM = "";
        public static IFeatureClass waterwayLineLyrCS;

        public static string QCLyrNM = "";
        public static IFeatureClass QCLyrCS;

        public static string QC20LyrNM = "";
        public static IFeatureClass QC20LyrCS;

        public static string QC5LyrNM = "";
        public static IFeatureClass QC5LyrCS;

        public static string SpatialAccuracyLyrNM = "";
        public static IFeatureClass SpatialAccuracyLyrCS;

        public static string RailMileLyrNM = "";
        public static IFeatureClass RailMileLyrCS;

        public static string WaterMileLyrNM = "";
        public static IFeatureClass WaterMileLyrCS;

        public static string USCountyLyrNM = "";
        public static IFeatureClass USCountyLyrCS;

        public static string USStateLyrNM = "";
        public static IFeatureClass USStateLyrCS;

        public static string UrbanLyrNM = "";
        public static IFeatureClass UrbanLyrCS;

        public static string RR_PotentialBridgeLyrNM = "";
        public static IFeatureClass RR_PotentialBridgeLyrCS;

        public static int CurrentErrorIndex = 0;
        public static long ErrorCount = 0;
        public static IWorkspaceFactory pWSF = new AccessWorkspaceFactory();
        public static IWorkspace pWS;
        public static IFeatureWorkspace pFWS;

        public static string samples20Pct="";
        public static string samples5Pct="";

    }

    public class functions
    {
       
        public static long getFeaturesCount(IQueryFilter pQF)
        {
            long count = 0;

            try
            {
                IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);

                IFeature pFeature = pCursor.NextFeature();

                while (pFeature != null)
                {
                    count = count + 1;
                    pFeature = pCursor.NextFeature();
                }

                Marshal.ReleaseComObject(pCursor);
            }

            catch
            {
                //MessageBox.Show("This file is not correct. Please reset ");
            }

            return count;
        }

        public static long getAllFeaturesCount(IQueryFilter pQF)
        {
            long count = 0;
            //IQueryFilter pQF = new QueryFilter();
            //pQF.WhereClause = "[Type] IN( 'Auto-Conflate' , 'Y' ) AND [Checked] IS NULL";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);

            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null )
            {
                count = count + 1;
                pFeature = pCursor.NextFeature();
            }
            return count;
        }

        public static void loadAllBridges(IQueryFilter pQF)
        {
            // Get feature layer
            try
            {
                Forms.assignAttributes_Form.clearListView();

                ITableSort tableSort = new TableSort();
                tableSort.Table = (ITable) Data.featurePntLyrCS;
                tableSort.QueryFilter = pQF;
                tableSort.Fields = "UniqueID";
                tableSort.set_Ascending("UniqueID", true);
                tableSort.Sort(null);
                ICursor sortedCursor = tableSort.Rows;
                IRow pFeature = sortedCursor.NextRow();
                int num = 0;

                while (pFeature != null && num < 1000)
                {

                    String bridgeType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString().Trim();
                    String featureID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                    String rrowner = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString().Trim();
                    String streetname = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Name")).ToString();
                    String designType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                    String type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Type")).ToString();
                    String crossType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();

                    string[] row = { featureID, rrowner, crossType, bridgeType, designType, type };
                    Forms.assignAttributes_Form.AddFileIntoList(new ListViewItem(row));
                    num = num + 1;
                    pFeature = sortedCursor.NextRow();
                }

                Marshal.ReleaseComObject(sortedCursor);
                Data.IsDataLoaded = true;
                Forms.identifyBridge_Form.setErrorCount(Data.ErrorCount);
            }
            catch
            {

                MessageBox.Show("FRA Bridge Point layer is not correct. Please reset ");
            }

            

        }
        public static void loadFeatures(IQueryFilter pQF)
        {
            // Get feature layer
            try
            {
                Forms.identifyBridge_Form.clearListView();
                
                ITableSort tableSort = new TableSort();
                tableSort.Table = (ITable)Data.featurePntLyrCS;
                tableSort.QueryFilter = pQF;
                tableSort.Fields = "UniqueID";
                tableSort.set_Ascending("UniqueID", true);
                tableSort.Sort(null);
                ICursor sortedCursor = tableSort.Rows;
                IRow pFeature = sortedCursor.NextRow();

                int num = 0;
                while (pFeature != null && num<1000 )
                {

                    String featureType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Type")).ToString().Trim();
                    String featureID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                    String rrowner = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString().Trim();
                    String streetname = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Name")).ToString();


                    string[] row = { featureID, featureType, rrowner, streetname };
                    Forms.identifyBridge_Form.AddFileIntoList(new ListViewItem(row));

                    num = num + 1;
                    pFeature = sortedCursor.NextRow();
                }

                Marshal.ReleaseComObject(sortedCursor);
                Data.IsDataLoaded = true;
                Forms.identifyBridge_Form.setErrorCount(Data.ErrorCount);
            }
            catch
            {
               MessageBox.Show("FRA Bridge Point layer is not correct. Please reset ");
            }
                       

        }
        public static void ZoomToCurrentError(IMxDocument pMxDoc, double zoomingAdjust,IPoint pPoint)
        {
            IMap pMap = pMxDoc.FocusMap as IMap;
            IGraphicsContainer pGraphicsContainer = pMap as IGraphicsContainer;
            //ILayer pLayer = null;
            //pLayer.Visible = false;
            ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbol();
            IRgbColor pLightGreen = (IRgbColor)new RgbColor();
            pLightGreen.Red = 181;
            pLightGreen.Green = 230;
            pLightGreen.Blue = 96;
            pSimpleMarkerSymbol.Color = pLightGreen;
            pSimpleMarkerSymbol.Outline = true;
            pSimpleMarkerSymbol.OutlineColor = pLightGreen;
            pSimpleMarkerSymbol.OutlineSize = 2;
            pSimpleMarkerSymbol.Size = 25;
            pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCross;
            IMarkerElement pMarkerElement = (IMarkerElement)new MarkerElement();
            pMarkerElement.Symbol = pSimpleMarkerSymbol;
            IElement pElement = pMarkerElement as IElement;
            pElement.Geometry = pPoint as IGeometry;
            pGraphicsContainer.DeleteAllElements();
            pGraphicsContainer.AddElement(pElement, 0);

            // IEnvelope pExtent = Data.ErrorTable[Data.CurrentErrorIndex].Penvelope;
            // pExtent.Expand(zoomingAdjust, zoomingAdjust, true);
            IEnvelope pExtent = (IEnvelope)new Envelope();
            pExtent.PutCoords(pPoint.X - zoomingAdjust, pPoint.Y - zoomingAdjust, pPoint.X + zoomingAdjust, pPoint.Y + zoomingAdjust);
            pMxDoc.ActiveView.Extent = pExtent;
            pMxDoc.ActiveView.Refresh();

        }
    }

}
