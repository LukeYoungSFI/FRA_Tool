using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using System.Linq;
namespace QC_Processing
{
    public partial class frmQC : Form
    {
        private IApplication m_application;
        public IWorkspaceEdit2 m_WorkspaceEdit = null;
        private IMap m_map;
        private IMxDocument m_mxDoc;


        private int indexSelected = -1;
        private string selectedTargetID = "";
        private IQueryFilter pQF = new QueryFilter();
        public List<string> designTypeDomain = new List<string>();
        public List<string> bridgeTypeDomain = new List<string>();
        public List<string> crossTypeDomain = new List<string>();
        static double bufferRange = 0.02;  // 1mile
        static double bufferRange1 = 0.00045;  //50m buffer

        private List<string> uniqueIDList;
        private List<string> sampling20PctList;
        private List<string> sampling5PctList;

        private string bridgeType = "";
        private string designType = "";
        private string crossType = "";


        private string QCMode="";

        private IFeatureClass activeQCReport;

        private void setActiveQCReport(IFeatureClass qcReport)
        {
            activeQCReport = qcReport;
        }

        private List<string> getRandomSamplingList(List<string> uniqueIDList, double percent)
        {
            List<string> randomList = new List<string>();
            int count = uniqueIDList.Count;
            Random rnd = new Random();
            
            int number = Convert.ToInt32(count * percent);
            randomList = uniqueIDList.OrderBy(x => rnd.Next()).Take(number).ToList();
            
            return randomList;

        }
        public void setSampling20PctList(List<string> sample20List)
        {
            sampling20PctList = sample20List;
        }
        public List<string> getSampling20PctList()
        {
            return sampling20PctList;
        }

        public void setSampling5PctList(List<string> sample5List)
        {
            sampling5PctList = sample5List;
        }

        public void getUniqueIDList()
        {
            uniqueIDList = new List<string>();
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[Type] NOT IN( 'N' , 'undefined' , 'non-bridge') ";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            int indexUniqueID = Data.featurePntLyrCS.Fields.FindField("UniqueID");
            if (indexUniqueID == -1)
            {

                MessageBox.Show("Field UniqueID does not exist");
                return ;

            }

            string uniqueId = "";
            while (pFeature != null)
            {
                uniqueId= pFeature.get_Value(indexUniqueID).ToString();
                uniqueIDList.Add(uniqueId);
                pFeature = pCursor.NextFeature();
            }
        }

        public void setQCMode(string mode)
        {
            QCMode = mode;
        }


        public frmQC()
        {
            InitializeComponent();
        }

        private void hideBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public IApplication Application
        {
            set { m_application = value; }
        }

        public IMap Map
        {
            get { return m_map; }
            set { m_map = value; }
        }

        public IMxDocument MxDoc
        {
            set { m_mxDoc = value; }
        }

        public void clearQCView()
        {
            QCReportListView.Enabled = false;
            QCReportListView.Items.Clear();
           
            fixedRdo.Checked = false;
            fixedRdo.Enabled = false;
            legalRdo.Checked = false;
            legalRdo.Enabled = false;
            subcategoryCbx.SelectedItem = null;
            subcategoryCbx.Enabled = false;
            NextBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            refreshBtn.Enabled = false;
            bridgeTypeCbx.Enabled = false;
            crossTypeCbx.Enabled = false;
            designTypeCbx.Enabled = false;
            //bridgeTypeCbx.SelectedItem = null;
            //crossTypeCbx.SelectedItem = null;
            //designTypeCbx.SelectedItem = null;
            radioButton2.Enabled = false;
            radioButton2.Checked = false;
            latLonTxt.Text= "";
            latLonTxt.Enabled = false;
            txtBridgeUniqueID.Enabled = false;
            btnSearch.Enabled = false;
            
            ErrorNoTxt.Text = "0 features in total";



        }
        //private void checkRRownerAlongSingleRailroad(string RailroadID)
        //{
        //    processingStatusLbl.Text = "Currently Checking RROwner";
        //    IQueryFilter streetQF = new QueryFilter();
        //    streetQF.WhereClause = "OBJECTID=" + Convert.ToInt32(RailroadID);
        //    IFeatureCursor pCursor = Data.railroadLineLyrCS.Search(streetQF, false);
        //    IFeature pFeature = pCursor.NextFeature();

        //    if (pFeature != null)
        //    {
        //        string rrowner = pFeature.get_Value(Data.railroadLineLyrCS.Fields.FindField("RROwner1")).ToString();

        //        ISpatialFilter pSF = null;
        //        pSF = new SpatialFilter();
        //        pSF.WhereClause= "[Type] IN( 'Auto-Conflate' , 'Y' ) AND [Checked] IS NULL";
        //        pSF.Geometry = pFeature.Shape;
        //        pSF.GeometryField = Data.railroadLineLyrCS.ShapeFieldName;
        //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //        IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pSF, false);
        //        IFeature pFeature1 = pCursor1.NextFeature();
        //        while (pFeature1 != null)
        //        {
        //            IPoint nodeOfInterest = (IPoint)pFeature1.Shape;
        //            string tempRROwner = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();
        //            string sourceOID = pFeature1.OID.ToString();
        //            string sourceFeatureID = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("TargetID")).ToString();
        //            if (tempRROwner != rrowner)
        //            {
        //                //write2QCReport(nodeOfInterest, "RROwner Not-Matched", nodeOfInterest.X, nodeOfInterest.Y, sourceOID, sourceFeatureID, "Unresolved");
        //            }

        //            pFeature1 = pCursor1.NextFeature();
        //        }
        //        Marshal.ReleaseComObject(pCursor1);
        //    }
        //    Marshal.ReleaseComObject(pCursor);

        //}
        //private void checkAlongRailroads()
        //{
        //    //m_WorkspaceEdit = Data.pWS as IWorkspaceEdit2;
        //    //m_WorkspaceEdit.StartEditing(true);
        //    //m_WorkspaceEdit.StartEditOperation();
        //    //m_WorkspaceEdit.EnableUndoRedo();


        //    List<string> railroadCheckList = new List<string>();

        //    IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
        //    IFeature pFeature = pCursor1.NextFeature();
        //    while (pFeature != null)
        //    {
        //        IPoint nodeOfInterest = pFeature.Shape as IPoint;
        //        ISpatialFilter pSF = new SpatialFilter();
        //        pSF.Geometry = nodeOfInterest;
        //        pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
        //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //        IFeatureCursor pCursor2 = Data.railroadLineLyrCS.Search(pSF, false);
        //        IFeature pFeature2 = pCursor2.NextFeature();

        //        while (pFeature2 != null)
        //        {
        //            string railroadID = pFeature2.OID.ToString();
        //            if (!railroadCheckList.Contains(railroadID))
        //            {
        //                checkRRownerAlongSingleRailroad(railroadID);
        //                railroadCheckList.Add(railroadID);
        //            }
        //            pFeature2 = pCursor2.NextFeature();
        //        }

        //        pFeature = pCursor2.NextFeature();
        //    }


        //    Marshal.ReleaseComObject(pCursor1);

        //    //m_WorkspaceEdit.StopEditOperation();
        //    //m_WorkspaceEdit.StopEditing(true);

        //    //MessageBox.Show("QC Completed");
        //}


        //private void checkAlongWaterway()
        //{

        //}


        //private void checkStreetNamesAlongSingleRoad(string roadID)
        //{
        //    processingStatusLbl.Text = "Currently Checking Street Name";
        //    IQueryFilter streetQF = new QueryFilter();
        //    streetQF.WhereClause = "OBJECTID=" + Convert.ToInt32(roadID);
        //    IFeatureCursor pCursor = Data.roadLineLyrCS.Search(streetQF, false);
        //    IFeature pFeature = pCursor.NextFeature();

        //    if (pFeature != null)
        //    {
        //        string streetname = pFeature.get_Value(Data.roadLineLyrCS.Fields.FindField("FULLNAME")).ToString();

        //        ISpatialFilter pSF = null;
        //        pSF = new SpatialFilter();
        //        pSF.WhereClause = "[Type] IN( 'Auto-Conflate' , 'Y' ) AND [Checked] IS NULL";
        //        pSF.Geometry = pFeature.Shape;
        //        pSF.GeometryField = Data.roadLineLyrCS.ShapeFieldName;
        //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //        IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pSF, false);
        //        IFeature pFeature1 = pCursor1.NextFeature();
        //        while (pFeature1 != null)
        //        {
        //            IPoint nodeOfInterest = (IPoint)pFeature1.Shape;
        //            string tempStreetname = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("StreetNames")).ToString();
        //            string[] streetNames = tempStreetname.Split(';');
        //            string sourceOID = pFeature1.OID.ToString();
        //            string sourceFeatureID = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("TargetID")).ToString();

        //            bool matched = false;
        //            for (int i=0;i< streetNames.Length;i++)
        //            {
        //                if (streetNames[i] == streetname)
        //                {
        //                    matched = true;
        //                    break;
        //                }

        //            }
        //            if (!matched)
        //            {
        //                //write2QCReport(nodeOfInterest, "Streetname Not-Matched", nodeOfInterest.X, nodeOfInterest.Y, sourceOID, sourceFeatureID, "Unresolved");
        //            }

        //            pFeature1 = pCursor1.NextFeature();
        //        }
        //        Marshal.ReleaseComObject(pCursor1);
        //    }
        //    Marshal.ReleaseComObject(pCursor);
        //}


        //private void checkAlongRoads()
        //{
        //    //m_WorkspaceEdit = Data.pWS as IWorkspaceEdit2;
        //    //m_WorkspaceEdit.StartEditing(true);
        //    //m_WorkspaceEdit.StartEditOperation();
        //    //m_WorkspaceEdit.EnableUndoRedo();


        //    List<string> roadCheckList = new List<string>();

        //    IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
        //    IFeature pFeature = pCursor1.NextFeature();
        //    while (pFeature != null)
        //    {
        //        IPoint nodeOfInterest = pFeature.Shape as IPoint;
        //        ISpatialFilter pSF = new SpatialFilter();
        //        pSF.Geometry = nodeOfInterest;
        //        pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
        //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //        IFeatureCursor pCursor2 = Data.roadLineLyrCS.Search(pSF, false);
        //        IFeature pFeature2 = pCursor2.NextFeature();

        //        while (pFeature2 != null)
        //        {
        //            string roadID = pFeature2.OID.ToString();
        //            if (!roadCheckList.Contains(roadID))
        //            {
        //                checkStreetNamesAlongSingleRoad(roadID);
        //                roadCheckList.Add(roadID);
        //            }
        //            pFeature2 = pCursor2.NextFeature();
        //        }

        //        pFeature = pCursor2.NextFeature();
        //    }


        //    Marshal.ReleaseComObject(pCursor1);

        //    //m_WorkspaceEdit.StopEditOperation();
        //    //m_WorkspaceEdit.StopEditing(true);

        //}

        private void checkCrossingType(IQueryFilter pQF,  bool exportNormal)
        {
            string subCategory = "Incorrect Crossing Type";

            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                string cross_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();
                string Operator = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Operator")).ToString();
                IPoint nodeOfInterest = (IPoint)pFeature.Shape;
                
                if ((targetID.Contains("W") && cross_Type.Contains("Road"))||((targetID.Contains("R") && cross_Type.Contains("Water"))) || cross_Type=="")
                {
                    write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, "Unresolved");
                                                            
                }

                pFeature = pCursor1.NextFeature();
            }
        }
        

        private void checkBridgeType(IQueryFilter pQF, bool exportNormal)
        {

            string subCategory = "Incorrect Bridge Type";
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                string cross_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();
                string design_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                string bridge_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString();
                string Operator = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Operator")).ToString();
                string num_tracks = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Num_Tracks")).ToString();


                //-Bridges in metropolitan areas: Bridge_Type = Unknown
                IPoint nodeOfInterest = pFeature.Shape as IPoint;

                bool found = false;

                if (cross_Type.Contains("Road"))
                {

                    if (bridge_Type.Contains("Moveable"))
                    {
                        found = true;
                    }
                }

                if (bridge_Type=="")
                {
                    found = true;
                }

                if (found)
                {
                    write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID,  "Unresolved");
                }

                pFeature = pCursor1.NextFeature();

            }

            Marshal.ReleaseComObject(pCursor1);
        }

       

        private void checkDesignType(IQueryFilter pQF, bool exportNormal)
        {
            string subCategory = "Incorrect Design Type";
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                string cross_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();
                string design_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                string bridge_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString();
                string Operator = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Operator")).ToString();
                string num_tracks = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Num_Tracks")).ToString();
                string streetview = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("StreetView")).ToString();


                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                bool found = false;

                if (design_Type== "Steel Deck Truss" || design_Type == "Steel Through TrusS" || design_Type == "Steel Deck Girder" || design_Type == "Steel Through Girder")
                {
                    found = true;
                }
                if (design_Type == "Not Sure" || design_Type == "" )
                {
                    found = true;
                }
                else if (( design_Type == "Other"  || design_Type == "Unknown") && streetview == "Available")
                {
                    found = true;
                }
                else if(design_Type != "Unknown" && streetview == "Not-Available")
                {
                    found = true;
                }
                else if (cross_Type == "Above Water")
                {
                    if (bridge_Type.Contains("Moveable"))
                    {
                        if (!design_Type.Contains("Steel") && design_Type != "Unknown")
                        {
                            found = true;
                        }
                    }

                }
                else if (cross_Type.Contains("Road"))
                {

                    if (design_Type == "Timber" || design_Type.Contains("Truss"))
                    {
                        found = true;
                    }

                }



                if (num_tracks == "Multiple")
                {
                    if (design_Type == "Timber")
                    {
                        found = true;
                    }
                }

                //if (!found)
                //{
                //    if (Data.UrbanLyrCS != null)
                //    {
                        
                //        ISpatialFilter pSF = new SpatialFilter();
                //        pSF.Geometry = nodeOfInterest;
                //        pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                //        IFeatureCursor pCursor2 = Data.UrbanLyrCS.Search(pSF, false);
                //        IFeature pFeature2 = pCursor2.NextFeature();
                //        if (pFeature2 != null && design_Type == "Unknown" && targetID.Contains("R"))
                //        {
                //            found = true;
                //        }
                //        Marshal.ReleaseComObject(pCursor2);
                //    }
                //}


                if (found)
                {
                    write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, "Unresolved");
                }

                pFeature = pCursor1.NextFeature();

            }

            Marshal.ReleaseComObject(pCursor1);
        }

        private void checkDesignTypeDomain()
        {
            processingStatusLbl.Text = "Currently Checking Design Type";
                     
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                string designType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                bool found = designTypeDomain.Contains(designType);
                if (!found)
                {
                    //write2QCReport(nodeOfInterest, "Incorrect Design Type", nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, "Unresolved");
                }
                Marshal.ReleaseComObject(pCursor1);

                pFeature = pCursor1.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor1);

            //m_WorkspaceEdit.StopEditOperation();
            //m_WorkspaceEdit.StopEditing(true);
        }

        private void checkCrossTypeDomain()
        {
            processingStatusLbl.Text = "Currently Checking Cross Type";
            //m_WorkspaceEdit = Data.pWS as IWorkspaceEdit2;
            //m_WorkspaceEdit.StartEditing(true);
            //m_WorkspaceEdit.StartEditOperation();
            //m_WorkspaceEdit.EnableUndoRedo();

            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();

                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                string designType = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();

                bool found = crossTypeDomain.Contains(designType);

                if (!found)
                {
                    //write2QCReport(nodeOfInterest, "Incorrect Cross Type", nodeOfInterest.X, nodeOfInterest.Y,sourceOID, targetID, "Unresolved");
                }
                Marshal.ReleaseComObject(pCursor1);

                pFeature = pCursor1.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor1);

            //m_WorkspaceEdit.StopEditOperation();
            //m_WorkspaceEdit.StopEditing(true);
        }

       

        private void write2QCReport( IPoint point, string flag_code, double location_x, double location_y, string sourceOID, string targetId,  string status)
        {

            DateTime now = DateTime.Now;
            IFeature feature = activeQCReport.CreateFeature();
            
            feature.Shape = point;
            
            feature.set_Value(feature.Fields.FindField("Flag_Code"), flag_code);
            feature.set_Value(feature.Fields.FindField("Location_X"), location_x);
            feature.set_Value(feature.Fields.FindField("Location_Y"), location_y);
            feature.set_Value(feature.Fields.FindField("TargetFeatureID"), targetId);
            feature.set_Value(feature.Fields.FindField("SourceFeatureID"), sourceOID);
            feature.set_Value(feature.Fields.FindField("Status"), status);
            feature.set_Value(feature.Fields.FindField("Create_Date"), now);
            feature.Store();
            Marshal.ReleaseComObject(feature);
        }

        private void PerformQCBtn_Click(object sender, EventArgs e)
        {
            

            IQueryFilter pQF = new QueryFilter();
            int count = -1;
            loadQCBtn.Enabled = false;
            ErrorNoTxt.Text = "0 features in total";
            if (QCMode == "100%")
            {
                if (Data.QCLyrCS == null)
                {
                    MessageBox.Show("QC_Report does not exist or is not properly set");
                    return;
                }
                else
                {
                    subcategoryCbx.Enabled = true;
                    setActiveQCReport(Data.QCLyrCS);
                    count = activeQCReport.FeatureCount(null);
                    if (count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("QC report table is not empty. Do you want to rewrite that table?", "QC", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            emptyQCTable(activeQCReport);
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }
                    pQF.WhereClause = "[Type] NOT IN( 'N' , 'undefined' , 'non-bridge') ";
                    //checkSpatialAccuracy(pQF, operatorName, false);
                    checkCrossingType(pQF,  false);
                    checkBridgeType(pQF,  false);
                    checkDesignType(pQF,  false);
                    clearQCView();
                }
                
            }
            else if (QCMode == "20%")
            {
                if (Data.QC20LyrCS == null)
                {
                    MessageBox.Show("TeamLead_QC_Report does not exist or is not properly set");
                    return;
                }
                else
                {
                    subcategoryCbx.SelectedItem = null;
                    subcategoryCbx.Enabled = false;
                    sampling20PctList = new List<string>();
                    setActiveQCReport(Data.QC20LyrCS);
                    count = activeQCReport.FeatureCount(null);
                    if (count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Team Lead QC report table is not empty. Do you want to rewrite that table?", "QC", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            emptyQCTable(activeQCReport);
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            //m_WorkspaceEdit.StopEditing(false);
                            return;


                        }
                    }
                    sampling20PctList = getRandomSamplingList(uniqueIDList, 0.2);

                    string IDs = "(";
                    for (int i = 0; i < sampling20PctList.Count; i++)
                    {
                        IDs = IDs + "'" + sampling20PctList[i] + "', ";
                    }
                    IDs = IDs.Substring(0, IDs.Length - 2) + ")";
                    Data.samples20Pct = IDs;
                    pQF.WhereClause = "UniqueID in " + IDs;
                    outputRandomQCTable(pQF);
                    clearQCView();
                }
                

            }
            else if (QCMode == "5%")
            {
                if (Data.QC5LyrCS == null)
                {
                    MessageBox.Show("PM_QC_Report does not exist or is not properly set");
                    return;
                }
                else
                {
                    subcategoryCbx.SelectedItem = null;
                    subcategoryCbx.Enabled = false;

                    setActiveQCReport(Data.QC5LyrCS);
                    count = activeQCReport.FeatureCount(null);
                    if (count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("PM QC report table is not empty. Do you want to rewrite that table?", "QC", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            emptyQCTable(activeQCReport);
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            //m_WorkspaceEdit.StopEditing(false);
                            return;


                        }
                    }
                    sampling5PctList = getRandomSamplingList(uniqueIDList, 0.05);

                    string IDs = "(";
                    for (int i = 0; i < sampling5PctList.Count; i++)
                    {
                        IDs = IDs + "'" + sampling5PctList[i] + "', ";
                    }
                    IDs = IDs.Substring(0, IDs.Length - 2) + ")";
                    Data.samples5Pct = IDs;
                    pQF.WhereClause = "UniqueID in " + IDs;
                    outputRandomQCTable(pQF);
                    clearQCView();
                }
                
            }
            
            MessageBox.Show("QC Completed");
            loadQCBtn.Enabled = true;
        }
        private void outputRandomQCTable(IQueryFilter pQF)
        {

            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                DateTime now = DateTime.Now;
                IFeature feature = activeQCReport.CreateFeature();
                feature.Shape = pFeature.Shape;
                double location_x = ((IPoint)pFeature.Shape).X;
                double location_y = ((IPoint)pFeature.Shape).Y;
                feature.set_Value(feature.Fields.FindField("Flag_Code"), "Random");
                feature.set_Value(feature.Fields.FindField("Location_X"), location_x);
                feature.set_Value(feature.Fields.FindField("Location_Y"), location_y);
                feature.set_Value(feature.Fields.FindField("TargetFeatureID"), targetID);
                feature.set_Value(feature.Fields.FindField("SourceFeatureID"), sourceOID);
                feature.set_Value(feature.Fields.FindField("Create_Date"), now);
                feature.set_Value(feature.Fields.FindField("Status"), "Unresolved");
                feature.Store();

                pFeature = pCursor1.NextFeature();

            }

            Marshal.ReleaseComObject(pCursor1);


        }
        private void emptyQCTable(IFeatureClass qcTable)
        {
            //m_WorkspaceEdit = Data.pWS as IWorkspaceEdit2;
            //m_WorkspaceEdit.StartEditing(true);
            //m_WorkspaceEdit.StartEditOperation();
            //m_WorkspaceEdit.EnableUndoRedo();

          
            IFeatureCursor pCursor = qcTable.Update(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                pCursor.DeleteFeature();
                pFeature = pCursor.NextFeature();
            }


            //m_WorkspaceEdit.StopEditOperation();
            //m_WorkspaceEdit.StopEditing(true);

            Marshal.ReleaseComObject(pCursor);
        }

       


        
        private void subcategoryCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            QCReportListView.Items.Clear();
            QCReportListView.Enabled = true;
            fixedRdo.Enabled = false;
            legalRdo.Enabled = false;

            refreshBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;
            string subCategory = "";
            IQueryFilter pQF = new QueryFilter();
            ITableSort tableSort = new TableSort();
            tableSort.Table = (ITable)activeQCReport;

            if (subcategoryCbx.SelectedItem != null)
            {
                subCategory = subcategoryCbx.SelectedItem.ToString();
                pQF.WhereClause = "[Flag_Code] = '" + subCategory + "' And [Status] = 'Unresolved'";
            }
            else
            {
                return;
            }
           
            tableSort.QueryFilter = pQF;
            tableSort.Fields = "TargetFeatureID";
            tableSort.set_Ascending("TargetFeatureID", true);

            tableSort.Sort(null);
            ICursor sortedCursor = tableSort.Rows;
            IRow pFeature = sortedCursor.NextRow();

            int num = 0;

            //while (pFeature != null && num < 1000)
           while (pFeature != null )

            {
                String targetFeatureID = pFeature.get_Value(activeQCReport.Fields.FindField("TargetFeatureID")).ToString();
                String status = pFeature.get_Value(activeQCReport.Fields.FindField("Status")).ToString().Trim();
                String flagcode = pFeature.get_Value(activeQCReport.Fields.FindField("Flag_Code")).ToString();
                String Operator = pFeature.get_Value(activeQCReport.Fields.FindField("Operator")).ToString();

                string design_Type = getDesignType(targetFeatureID);

                string[] row = { targetFeatureID, flagcode, status, Operator, design_Type };

                //    ListBridgeTargetPnts.Items.Add(new ListViewItem(row));
                QCReportListView.Items.Add(new ListViewItem(row));

                num = num + 1;
                pFeature = sortedCursor.NextRow();
            }
            ErrorNoTxt.Text = num.ToString() + " features in total";
            Marshal.ReleaseComObject(sortedCursor);

            bridgeTypeCbx.Enabled = false;
            designTypeCbx.Enabled = false;
            crossTypeCbx.Enabled = false;
            radioButton2.Enabled = false;


        }

        private void QCReportListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (QCReportListView.SelectedItems.Count > 0)
            {
                //subcategoryCbx.Enabled = true;
                ListViewItem selectedRow = QCReportListView.SelectedItems[0];
                indexSelected = QCReportListView.SelectedItems[0].Index;
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                QCReportListView.Focus();


                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    //ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();
                    //latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
                    getBridgeAttribute(selectedTargetID);

                    if (indexSelected == 0)
                    {
                        BackwardBtn.Enabled = false;
                        NextBtn.Enabled = true;
                    }
                    else if (indexSelected == Data.ErrorCount - 1)
                    {
                        NextBtn.Enabled = false;
                        BackwardBtn.Enabled = true;
                    }
                    else
                    {
                        NextBtn.Enabled = true;
                        BackwardBtn.Enabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("TargetFeatureID " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Marshal.ReleaseComObject(pCursor);
                fixedRdo.Enabled = true;
                legalRdo.Enabled = true;

                cbxStreetViewAvailability.Enabled = true;

                fixedRdo.Checked = false;
                legalRdo.Checked = false;
                refreshBtn.Enabled = true;
                radioButton2.Checked = false;
                radioButton2.Enabled = true;


            }
        }

        private void getBridgeAttribute(string selectedTargetID)
        {
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            if (pFeature != null)
            {
                string bridge_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString().Trim();
                string design_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString().Trim();
                string cross_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString().Trim();
                string streetview = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("StreetView")).ToString().Trim();
                if (streetview != "")
                {
                    cbxStreetViewAvailability.SelectedItem = streetview;
                }
                else
                {
                    cbxStreetViewAvailability.SelectedItem = null;
                }


                if (bridge_type != "")
                {
                    bridgeTypeCbx.SelectedItem = bridge_type;
                }
                else
                {
                    bridgeTypeCbx.SelectedItem = null;
                }

                if (design_type != "")
                {
                    designTypeCbx.SelectedItem = design_type;
                }
                else
                {
                    designTypeCbx.SelectedItem = null;
                }

                if (cross_type != "")
                {
                    crossTypeCbx.SelectedItem = cross_type;
                }
                else
                {
                    crossTypeCbx.SelectedItem = null;
                }
            }
            bridgeTypeCbx.Enabled = true;
            designTypeCbx.Enabled = true;
            crossTypeCbx.Enabled = true;

            
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            int count = QCReportListView.Items.Count;
            if (indexSelected < QCReportListView.Items.Count - 1)
            {
                int nextIndex = indexSelected + 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow = QCReportListView.Items[(nextIndex)];
                //ListBridgeTargetPnts.FocusedItem[nextIndex];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                QCReportListView.Items[(nextIndex)].Selected = true;
                QCReportListView.EnsureVisible(nextIndex);
                QCReportListView.Focus();
                //ListBridgeTargetPnts.FocusedItem = selectedRow;


                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                    getBridgeAttribute(selectedTargetID);
                    if (indexSelected == 0)
                    {
                        BackwardBtn.Enabled = false;
                    }
                    else if (indexSelected == count - 1)
                    {
                        NextBtn.Enabled = false;
                    }
                    else
                    {
                        NextBtn.Enabled = true;
                        BackwardBtn.Enabled = true;
                    }

                }

                else
                {
                    MessageBox.Show("TargetFeatureID " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                Marshal.ReleaseComObject(pCursor);
                fixedRdo.Enabled = true;
                legalRdo.Enabled = true;
                

                fixedRdo.Checked = false;
                legalRdo.Checked = false;
                radioButton2.Checked = false;
                radioButton2.Enabled = true;
            }
        }

        private void BackwardBtn_Click(object sender, EventArgs e)
        {
            int count = QCReportListView.Items.Count;
            if (indexSelected > 0)
            {
                int nextIndex = indexSelected - 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow = QCReportListView.Items[(nextIndex)];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                QCReportListView.Items[(nextIndex)].Selected = true;
                QCReportListView.EnsureVisible(nextIndex);
                QCReportListView.Focus();

                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
                    getBridgeAttribute(selectedTargetID);
                    //ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();
                    //latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                    if (indexSelected == 0)
                    {
                        BackwardBtn.Enabled = false;
                    }
                    else if (indexSelected == count - 1)
                    {
                        NextBtn.Enabled = false;
                    }
                    else
                    {
                        NextBtn.Enabled = true;
                        BackwardBtn.Enabled = true;
                    }



                }
                else
                {
                    MessageBox.Show("Target " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Marshal.ReleaseComObject(pCursor);
                fixedRdo.Enabled = true;
                legalRdo.Enabled = true;

                fixedRdo.Checked = false;
                legalRdo.Checked = false;
                radioButton2.Checked = false;
                radioButton2.Enabled = true;
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            QCReportListView.Items.Clear();
            fixedRdo.Enabled = false;
            legalRdo.Enabled = false;
            
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;//ErrorIDLbl.Text = "Editing Feature ID: ";
            //BackwardBtn.Enabled = false;
            latLonTxt.Text = "";

            QCReportListView.Enabled = true;
            //subcategoryCbx.Enabled = true;

            string subCategory = "";
            IQueryFilter pQF = new QueryFilter();
            if (subcategoryCbx.SelectedItem != null)
            {
                subCategory = subcategoryCbx.SelectedItem.ToString();
                pQF.WhereClause = "[Flag_Code] = '" + subCategory + "' And [Status] = 'Unresolved'";
            }
            else
            {
                pQF.WhereClause =  "[Status] = 'Unresolved'";
            }
            

            ITableSort tableSort = new TableSort();
            tableSort.Table = (ITable)activeQCReport;
            tableSort.QueryFilter = pQF;
            tableSort.Fields = "TargetFeatureID";
            tableSort.set_Ascending("TargetFeatureID", true);

            tableSort.Sort(null);
            ICursor sortedCursor = tableSort.Rows;
            IRow pFeature = sortedCursor.NextRow();

            int num = 0;
            //while (pFeature != null && num < 1000)
            while (pFeature != null )

            {
                String targetFeatureID = pFeature.get_Value(activeQCReport.Fields.FindField("TargetFeatureID")).ToString();
                String status = pFeature.get_Value(activeQCReport.Fields.FindField("Status")).ToString().Trim();
                String flagcode = pFeature.get_Value(activeQCReport.Fields.FindField("Flag_Code")).ToString();
                String Operator = pFeature.get_Value(activeQCReport.Fields.FindField("Operator")).ToString();
                string design_Type = getDesignType(targetFeatureID);

                string[] row = { targetFeatureID, flagcode, status, Operator, design_Type };

                //    ListBridgeTargetPnts.Items.Add(new ListViewItem(row));
                QCReportListView.Items.Add(new ListViewItem(row));

                num = num + 1;
                pFeature = sortedCursor.NextRow();
            }
            ErrorNoTxt.Text = num.ToString() + " features in total";
            Marshal.ReleaseComObject(sortedCursor);

            fixedRdo.Enabled = false;
            legalRdo.Enabled = false;

            refreshBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;
            radioButton2.Checked = false;
            radioButton2.Enabled = false;
            //latLonTxt.Text = "";
        }

        private void fixedRdo_CheckedChanged(object sender, EventArgs e)
        {
            
            if (fixedRdo.Checked)
            {
                
                string Status = "Fixed";
                string comments = "";
                if (QCMode == "20%" || QCMode == "5%")
                {
                    comments = Status;
                    Status = "Failed";
                }
               
                QCReportListView.Items[(indexSelected)].SubItems[2].Text = Status;
                string category = QCReportListView.Items[(indexSelected)].SubItems[1].Text;
                saveFlagStatus(selectedTargetID, Status, category, comments);
            }
        }

        private void saveFlagStatus(string selectedTargetID, string status, string category, string comments)
        {
            string operatorName = Environment.UserName.ToString();
            pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "' And [Flag_Code] = '" + category + "'";

            IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

            int indexBridgeType = activeQCReport.Fields.FindField("Status");
            int indexResolveDate = activeQCReport.Fields.FindField("Resolved_Date");
            int indexComments = activeQCReport.Fields.FindField("Comments");
            int indexOperator= activeQCReport.Fields.FindField("Operator");

            if (pFeature != null)
            {

                pFeature.set_Value(indexBridgeType, status);
                pFeature.set_Value(indexResolveDate, DateTime.Now);
                pFeature.set_Value(indexOperator, operatorName);
                pFeature.set_Value(indexComments,comments);
                pFeature.Store();

            }

            Marshal.ReleaseComObject(pCursor);
        }

        private void legalRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (legalRdo.Checked)
            {
               
                string Status = "Legal";
                string comments = "";
                if (QCMode == "20%" || QCMode == "5%")
                {
                    Status = "Pass";
                }
                QCReportListView.Items[(indexSelected)].SubItems[2].Text = Status;
                string category = QCReportListView.Items[(indexSelected)].SubItems[1].Text;
                saveFlagStatus(selectedTargetID, Status, category, comments);
            }
        }

        private void loadQCBtn_Click(object sender, EventArgs e)
        {
            BackwardBtn.Enabled = false;

            QCReportListView.Items.Clear();
            QCReportListView.Enabled = false;
            subcategoryCbx.Enabled = false;
            subcategoryCbx.SelectedItem = null;
            fixedRdo.Enabled = false;
            legalRdo.Enabled = false;

            refreshBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;

           

            ErrorNoTxt.Text =  "0 features in total";
            //string subCategory = subcategoryCbx.SelectedItem.ToString();

            int count = -1;
            if (QCMode == "100%")

            {
                subcategoryCbx.Enabled = true;
                setActiveQCReport(Data.QCLyrCS);
                count = activeQCReport.FeatureCount(null);
                //selectedQCTable = Data.QCLyrCS;
                if (count == 0)
                {
                    MessageBox.Show("QC Report table is empty.");
                    return;
                }

                
            }
            else if (QCMode == "20%")
            {
                subcategoryCbx.Enabled = false;
                setActiveQCReport(Data.QC20LyrCS);
                count = activeQCReport.FeatureCount(null);
                //selectedQCTable = Data.QC20LyrCS;
                if (count == 0)
                {
                    MessageBox.Show("TeamLead QC Report table is empty.");
                    return;
                }
            }
            else if (QCMode == "5%")
            {
                subcategoryCbx.Enabled = false;

                setActiveQCReport(Data.QC5LyrCS);
                count = activeQCReport.FeatureCount(null);
                //selectedQCTable = Data.QC5LyrCS;
                if (count == 0)
                {
                    MessageBox.Show("PM QC Report table is empty.");
                    return;
                }
            }

            QCReportListView.Enabled = true;
            
            btnSearch.Enabled = true;
            txtBridgeUniqueID.Enabled = true;

            IQueryFilter pQF = new QueryFilter();
            //pQF.WhereClause = "[Status] = 'Unresolved'";

            ITableSort tableSort = new TableSort();
            tableSort.Table = (ITable)activeQCReport;
            //tableSort.QueryFilter = pQF;
            tableSort.Fields = "TargetFeatureID";
            tableSort.set_Ascending("TargetFeatureID", true);

            tableSort.Sort(null);
            ICursor sortedCursor = tableSort.Rows;
            IRow pFeature = sortedCursor.NextRow();

            int num = 0;

            //while (pFeature != null && num < 1000)
            while (pFeature != null)

            {
                String targetFeatureID = pFeature.get_Value(activeQCReport.Fields.FindField("TargetFeatureID")).ToString();
                String status = pFeature.get_Value(activeQCReport.Fields.FindField("Status")).ToString().Trim();
                String flagcode = pFeature.get_Value(activeQCReport.Fields.FindField("Flag_Code")).ToString();
                String Operator = pFeature.get_Value(activeQCReport.Fields.FindField("Operator")).ToString();
                string design_Type = getDesignType(targetFeatureID);

                string[] row = { targetFeatureID, flagcode, status, Operator, design_Type };

                //    ListBridgeTargetPnts.Items.Add(new ListViewItem(row));
                QCReportListView.Items.Add(new ListViewItem(row));

                num = num + 1;
                pFeature = sortedCursor.NextRow();
            }

            Marshal.ReleaseComObject(sortedCursor);

            fixedRdo.Checked = false;
            legalRdo.Checked = false;
            ErrorNoTxt.Text = num.ToString() + " features in total";
        }

        private string getDesignType(string targetFeatureID)
        {
            string design_Type = "";
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + targetFeatureID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            if (pFeature != null)
            {
                design_Type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString().Trim();
                
            }
            Marshal.ReleaseComObject(pCursor);
            return design_Type;

        }

       
        private void updateBridgeAttribute(int index, string value)
        {
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();


            if (pFeature != null)
            {
                pFeature.set_Value(index, value);
                pFeature.Store();

            }
        }
        private void crossTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (crossTypeCbx.SelectedItem != null)
            {
                crossType = crossTypeCbx.SelectedItem.ToString();
                int indexCrossType = Data.featurePntLyrCS.Fields.FindField("Cross_Type");
                updateBridgeAttribute(indexCrossType, crossType);
            }
            
        }

        private void bridgeTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bridgeTypeCbx.SelectedItem != null)
            {
                bridgeType = bridgeTypeCbx.SelectedItem.ToString();
                int indexBridgeType = Data.featurePntLyrCS.Fields.FindField("Bridge_Type");
                updateBridgeAttribute(indexBridgeType, bridgeType);
            }
            
        }

        private void designTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (designTypeCbx.SelectedItem != null)
            {
                designType = designTypeCbx.SelectedItem.ToString();
                int indexDesignType = Data.featurePntLyrCS.Fields.FindField("Design_Type");
                updateBridgeAttribute(indexDesignType, designType);
                QCReportListView.Items[(indexSelected)].SubItems[4].Text = designType;
            }
            
        }

        private void saveBridgeType(string selectedTargetID, string featureType)
        {
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

            int indexBridgeType = Data.featurePntLyrCS.Fields.FindField("Type");

            if (pFeature != null)
            {
                pFeature.set_Value(indexBridgeType, featureType);
                pFeature.Store();

            }

            Marshal.ReleaseComObject(pCursor);
        }

       
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                string featureType = "N";
                saveBridgeType(selectedTargetID, featureType);
                string comments = "";
                string Status = "Non-Bridge";
                if (QCMode == "20%" || QCMode == "5%")
                {
                    comments = Status;
                    Status = "Failed";
                    
                }
                QCReportListView.Items[(indexSelected)].SubItems[2].Text = Status;
                string category = QCReportListView.Items[(indexSelected)].SubItems[1].Text;
                saveFlagStatus(selectedTargetID, Status, category, comments);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int count = QCReportListView.Items.Count;
            if (txtBridgeUniqueID.Text.Trim() == "")
            {
                MessageBox.Show("Please enter UniqueID");
                return;
            }
            else
            {
                string uniqueID = txtBridgeUniqueID.Text.Trim();
                int indexBridgeSearched = searchBridgeuniqueID(uniqueID);
                if (indexBridgeSearched > -1)
                {
                    selectedTargetID = uniqueID;
                    indexSelected = indexBridgeSearched;
                    ListViewItem selectedRow = QCReportListView.Items[(indexBridgeSearched)];
                    QCReportListView.Items[(indexBridgeSearched)].Selected = true;
                    QCReportListView.EnsureVisible(indexBridgeSearched);
                    QCReportListView.Focus();

                    IQueryFilter pQF = new QueryFilter();
                    pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "'";
                    IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
                    IFeature pFeature = pCursor.NextFeature();

                    IMxDocument pMxDoc = m_application.Document as IMxDocument;

                    if (pFeature != null)
                    {
                        IPoint point = (IPoint)pFeature.Shape;
                        functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                        latLonTxt.Enabled = true;
                        latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                        getBridgeAttribute(selectedTargetID);
                        if (indexSelected == 0)
                        {
                            BackwardBtn.Enabled = false;
                        }
                        else if (indexSelected == count - 1)
                        {
                            NextBtn.Enabled = false;
                        }
                        else
                        {
                            NextBtn.Enabled = true;
                            BackwardBtn.Enabled = true;
                        }

                    }

                    else
                    {
                        MessageBox.Show("TargetFeatureID " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    Marshal.ReleaseComObject(pCursor);
                    fixedRdo.Enabled = true;
                    legalRdo.Enabled = true;
                   


                    fixedRdo.Checked = false;
                    legalRdo.Checked = false;
                    
                    
                    radioButton2.Checked = false;
                    radioButton2.Enabled = true;
                }
                else
                {
                    IQueryFilter pQF = new QueryFilter();
                    pQF.WhereClause = "[TargetFeatureID] = '" + uniqueID + "'";
                    IFeatureCursor pCursor = activeQCReport.Search(pQF, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null)
                    {
                        MessageBox.Show("The bridge is already checked. Please reload QC_Report again to search it.");

                    }
                    else
                    {
                        MessageBox.Show("Not Found.");
                    }

                    Marshal.ReleaseComObject(pCursor);
                    
                   
                }
                return;
            }
        }

        private int searchBridgeuniqueID(string uniqueID)
        {
            int indexBridgeSearched = -1;

            int i = 0;
            while (i < QCReportListView.Items.Count)
            {
                
                ListViewItem selectedRow = QCReportListView.Items[i];
                //ListBridgeTargetPnts.FocusedItem[nextIndex];
                string targetID = selectedRow.SubItems[0].Text.ToString();
                if (targetID == uniqueID)
                {
                    indexBridgeSearched = i;
                    break;
                }
                i++;
            }
            return indexBridgeSearched;
        }

       
    }
}
