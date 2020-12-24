using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using System.Linq;
using StreetNameHandler;
using System.Text.RegularExpressions;

namespace QC_Processing
{
    public partial class frmPopulateOtherAttributes : Form
    {

        static double bufferRange = 0.02;  // 1mile
        static double bufferRange1 = 0.0009;
        //static double bufferRange3 = 0.001;
        static double bufferRange3 = 0.00045;  //50m buffer

        Dictionary<String, String> suffixList = new Dictionary<String, String>();
        List<String> directSuffixList = new List<String>() {
            "E","S","W","N","EAST","SOUTH","WEST","NORTH","SE","SW","NE","NW","SOUTHEAST","SOUTHWEST","NORTHEAST","NORTHWEST"
        };
        List<String> prefixList = new List<String>(){
            "E","S","W","N","EAST","SOUTH","WEST","NORTH","SE","SW","NE","NW","SOUTHEAST","SOUTHWEST","NORTHEAST","NORTHWEST"
        };
        List<String> matchLogList = new List<String>();
        string suffixListFileName = "";
        public frmPopulateOtherAttributes()
        {
            InitializeComponent();
        }

        private void populateOtherAttrBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to populate other attribtes for bridges now?", "Populate Other Attributes ", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (suffixListFileName == "" && cbxStreetNM.Checked)
                {
                    MessageBox.Show("Suffix file is not specified.");
                    return;
                }
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[Type] NOT IN ( 'N' , 'non-bridge')";
                if (cbxStreetNM.Checked)
                {
                    processStreetName(suffixListFileName);
                }
                if (cbxState.Checked)
                {
                    populateState();
                }
                if (cbxSource.Checked)
                {
                    populateSource();
                }

                if (cbxNoTracks.Checked)
                {
                    populateNumTracks();
                }

                if (cbxSubDivision.Checked)
                {
                    populateSubdivision();
                }

                if (cbxLatLng.Checked)
                {
                    populateLatLon(pQF);
                }
                if (cbxRailPost.Checked)
                {
                    populateRailMileMarker();
                }
                if (cbxFIPS.Checked)
                {
                    populateFIPS();
                }

                if (cbxWaterPost.Checked)
                {
                    populateWaterMilePost();
                }
                if (cbxNone.Checked)
                {
                    populateColumnsWithNull();
                }

                

                MessageBox.Show("Done");
            }
            else if (dialogResult == DialogResult.No)
            {
                //m_WorkspaceEdit.StopEditing(false);
                return;


            }
            
        }
        private void populateColumnsWithNull()
        {
            int indexSecondaryName = Data.featurePntLyrCS.Fields.FindField("Secondary_Name");
            int indexSecondaryRROwner = Data.featurePntLyrCS.Fields.FindField("Secondary_RROwner");
            int indexGradeCrossID = Data.featurePntLyrCS.Fields.FindField("GradeCross_ID");
            int indexUniqueID = Data.featurePntLyrCS.Fields.FindField("UniqueID");
            int indexRROwner = Data.featurePntLyrCS.Fields.FindField("RROwner");

            if (indexSecondaryName == -1)
            {
                MessageBox.Show("Secondary_Name does not exist ");
                return;
            }
            if (indexUniqueID == -1)
            {
                MessageBox.Show("UniqueID does not exist ");
                return;
            }
            if (indexRROwner == -1)
            {
                MessageBox.Show("RROwner does not exist ");
                return;
            }

            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(indexUniqueID).ToString();
                string secondary_name = pFeature.get_Value(indexSecondaryName).ToString();
                string secondary_rrowner = pFeature.get_Value(indexSecondaryRROwner).ToString();
                string secondary_crossingid = pFeature.get_Value(indexGradeCrossID).ToString();
                string rrowner= pFeature.get_Value(indexRROwner).ToString();
                if (secondary_name.Trim()=="")
                {
                    pFeature.set_Value(indexSecondaryName, "None");
                    
                }
                if (secondary_rrowner.Trim() == "")
                {
                    pFeature.set_Value(indexSecondaryRROwner, "None");

                }
                if (secondary_crossingid.Trim() == "")
                {
                    pFeature.set_Value(indexGradeCrossID, "None");

                }
                if (rrowner.Trim() == "")
                {
                    pFeature.set_Value(indexRROwner, "Unknown");

                }

                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

        }
        private void populateSource()
        {
            int indexSource = Data.featurePntLyrCS.Fields.FindField("Source");
            int indexUniqueID = Data.featurePntLyrCS.Fields.FindField("UniqueID");

            if (indexSource == -1)
            {
                MessageBox.Show("Source does not exist ");
                return;
            }
            if (indexUniqueID == -1)
            {
                MessageBox.Show("UniqueID does not exist ");
                return;
            }

            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                string source = "";
                if (uniqueID.Contains("R"))
                {
                    source = "Tiger Roads";
                }
                else if (uniqueID.Contains("W"))
                {
                    source = "NHD_Waterway";
                }

                pFeature.set_Value(indexSource, source);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

        }

        private void populateState()
        {
            int indexState = Data.featurePntLyrCS.Fields.FindField("State");
            if (Data.USStateLyrCS == null)
            {
                MessageBox.Show("US State Layer does not exist ");
                return;
            }

            if (indexState == -1)
            {
                MessageBox.Show("State does not exist ");
                return;
            }

           
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {

                IPoint nodeOfInterest = (IPoint)pFeature.Shape;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = "Shape";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor2 = Data.USStateLyrCS.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();
                string state = "";
                if (pFeature2 != null)
                {
                    state = pFeature2.get_Value(Data.USStateLyrCS.Fields.FindField("STUSPS")).ToString();

                }
                Marshal.ReleaseComObject(pCursor2);
                
                pFeature.set_Value(indexState, state);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

        }

        private void getStreetName(string suffixListFileName)
        {
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();
           

            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();

                if (uniqueID.Contains("R"))
                {

                }
                else if (uniqueID.Contains("W"))
                {
                }
                pFeature = pCursor.NextFeature();

            }

            Marshal.ReleaseComObject(pCursor);
        }
        private void processStreetName(string suffixListFileName)
        {

            //int indexNameBackup = Data.featurePntLyrCS.Fields.FindField("Name_Backup");
            //if (indexNameBackup==-1)
            //{
            //    MessageBox.Show("Please add Name_Backup column to Bridge layer and assign Secondary_StreetNames to it ");
            //    return;
            //}
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();
            StreetnameSplitter strSplitter = new StreetnameSplitter(suffixListFileName);
            RuleOverlappingStreet overlappingStreetHandler = new RuleOverlappingStreet(strSplitter);
            
            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();

              
                if (uniqueID.Contains("R"))
                {
                    //overlappingStreetHandler.overlappingStreetHandler4Cook(pFeature, Data.roadLineLyrCS);
                    overlappingStreetHandler.overlappingStreetHandler(pFeature, Data.intersectionPntLyrCS,Data.roadLineLyrCS);
                }
                else if (uniqueID.Contains("W"))
                {
                    int indexName = (Data.featurePntLyrCS.Fields.FindField("Name"));
                    int indexSecondaryStreetnames = (Data.featurePntLyrCS.Fields.FindField("Secondary_StreetNames"));
                    List<string> wwnames = populateWaterwayNM(pFeature);
                    if (wwnames.Count == 0)
                    {
                        pFeature.set_Value(indexName, "None");
                        pFeature.set_Value(indexSecondaryStreetnames, "None");
                    }
                    else if (wwnames.Count == 1)
                    {
                        pFeature.set_Value(indexName, wwnames[0]);
                        pFeature.set_Value(indexSecondaryStreetnames, "None");
                    }
                    else
                    {
                        pFeature.set_Value(indexName, wwnames[0]);
                        string wwname = "";
                        for (int i = 1; i < wwnames.Count; i++)
                        {
                            wwname = wwname + wwnames[i]+ ";";
                        }
                        pFeature.set_Value(indexSecondaryStreetnames, wwname.Substring(0, wwname.Length - 1));
                    }
                    pFeature.Store();

                }
                pFeature = pCursor.NextFeature();

            }

            Marshal.ReleaseComObject(pCursor);
        }
        private List<string> populateWaterwayNM(IFeature feature)
        {
            List< string > wwNames = new List<string>();
            string name = feature.get_Value(feature.Fields.FindField("Name")).ToString();
            // string[] secondaryStNames = secondary_streetnames.Split(';');
            int numPoints = Convert.ToInt32(feature.get_Value(feature.Fields.FindField("NoOfPoints")).ToString());
            string pointIDs = feature.get_Value(feature.Fields.FindField("PointIDs")).ToString();

            if (numPoints == 1) // at intersection point
            {
                string[] pointObjIds = pointIDs.Split(';');
                IQueryFilter pQF1 = new QueryFilter();
                pQF1.WhereClause = "OBJECTID =" + Convert.ToInt32(pointObjIds[0]);
                IFeatureCursor pCursor2 = Data.waterIntersectionPntLyrCS.Search(pQF1, false);
                IFeature pFeature2 = pCursor2.NextFeature();
                if (pFeature2 != null)
                {
                    IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                    ISpatialFilter pSF = new SpatialFilter();
                    pSF.Geometry = nodeOfInterest;
                    pSF.GeometryField = Data.waterIntersectionPntLyrCS.ShapeFieldName;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pFeaCur = Data.waterwayLineLyrCS.Search(pSF, false);
                    IFeature pFea = null;

                    while ((pFea = pFeaCur.NextFeature()) != null)
                    {
                        string wwName = pFea.get_Value(pFea.Fields.FindField("GNIS_NAME")).ToString();
                       
                        if (wwName != null & wwName != "" & wwName.Trim().Length > 0)
                        {
                            if (!wwNames.Contains(wwName))
                            {
                                wwNames.Add(wwName);
                            }
                            // int cfccListPlacement = cfccPriorities.IndexOf(cfcc);

                        }

                    }
                    Marshal.ReleaseComObject(pFeaCur);

                }
                Marshal.ReleaseComObject(pCursor2);
            }

            else //grouped location
            {
                string[] pointObjIds = pointIDs.Split(';');

                string pointObjs = "OBJECTID = ";
                for (int i = 0; i < pointObjIds.Length - 2; i++)
                {
                    pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR " + " OBJECTID = ";
                }

                pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                IQueryFilter pQF2 = new QueryFilter();
                pQF2.WhereClause = pointObjs;
                IFeatureCursor pCursor2 = Data.waterIntersectionPntLyrCS.Search(pQF2, false);
                IFeature pFeature2 = pCursor2.NextFeature();
                while (pFeature2 != null)
                {
                    IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                    ISpatialFilter pSF = new SpatialFilter();
                    pSF.Geometry = nodeOfInterest;
                    pSF.GeometryField = Data.waterIntersectionPntLyrCS.ShapeFieldName;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pFeaCur = Data.waterwayLineLyrCS.Search(pSF, false);
                    IFeature pFea = null;

                    while ((pFea = pFeaCur.NextFeature()) != null)
                    {
                        string wwName = pFea.get_Value(pFea.Fields.FindField("GNIS_NAME")).ToString();

                        if (wwName != null & wwName != "" & wwName.Trim().Length > 0)
                        {
                            if (!wwNames.Contains(wwName))
                            {
                                wwNames.Add(wwName);
                            }
                            // int cfccListPlacement = cfccPriorities.IndexOf(cfcc);

                        }

                    }
                    Marshal.ReleaseComObject(pFeaCur);
                    pFeature2 = pCursor2.NextFeature();
                }
                Marshal.ReleaseComObject(pCursor2);

            }

            
            return wwNames;
        }
        private string populateSubDiv(IQueryFilter pQF,  IFeatureClass intersectionPointsCS,string objectID_Col)
        {
            string subdiv = "Unknown";

            int indexRROWNER = Data.railroadLineLyrCS.Fields.FindField("RROWNER1");
            int indexNET = Data.railroadLineLyrCS.Fields.FindField("NET");
            int indexSubdiv = Data.railroadLineLyrCS.Fields.FindField("SUBDIV");
            List<string> subdivs = new List<string>();
            List<string> subdivNETs = new List<string>();

            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {

                string rrowner = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();
                int numPoints = Convert.ToInt32(pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints")).ToString());
                string pointIDs = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs")).ToString();

                string[] pointObjIds = pointIDs.Split(';');

                IQueryFilter pQF1 = new QueryFilter();

                if (numPoints == 1)
                {
                    
                    pQF1.WhereClause = objectID_Col+"=" + Convert.ToInt32(pointObjIds[0]);
                }
                else
                {
                    string pointObjs = objectID_Col+ "=";
                    for (int i = 0; i < pointObjIds.Length - 2; i++)
                    {
                        pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR "+ objectID_Col +" = ";
                    }
                    pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                    pQF1.WhereClause = pointObjs;
                }

                IFeatureCursor pCursor1 = intersectionPointsCS.Search(pQF1, false);
                IFeature pFeature1 = pCursor1.NextFeature();

                while (pFeature1 != null)
                {
                    IPoint nodeOfInterest = (IPoint)pFeature1.Shape;

                    ISpatialFilter pSF = new SpatialFilter();
                    pSF.Geometry = nodeOfInterest;
                    pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor2 = Data.railroadLineLyrCS.Search(pSF, false);
                    IFeature pFeature2 = pCursor2.NextFeature();

                    while (pFeature2 != null)
                    {
                        string rrowner1 = pFeature2.get_Value(indexRROWNER).ToString();
                        string subdiv1 = pFeature2.get_Value(indexSubdiv).ToString();
                        string net = pFeature2.get_Value(indexNET).ToString();

                        if (subdiv1.Trim() != "" && rrowner1==rrowner)
                        {
                            subdivs.Add(subdiv1);
                            subdivNETs.Add(net);
                        }
                        pFeature2 = pCursor2.NextFeature();
                    }
                    Marshal.ReleaseComObject(pCursor2);
                    pFeature1 = pCursor1.NextFeature();
                }

                Marshal.ReleaseComObject(pCursor1);
                pFeature = pCursor.NextFeature();
            }
       
            Marshal.ReleaseComObject(pCursor);

            if (subdivs.Count > 0)
            {
                if (subdivNETs.Contains("M"))
                {
                    subdiv = subdivs[subdivNETs.IndexOf("M")];                    
                }
                
            }


            return subdiv;
        }


        private string populateSubDiv4NewBridges(IQueryFilter pQF, IFeatureClass targetPointsCS, IFeatureClass intersectionPointsCS)
        {
            string subdiv = "Unknown";
            
            int indexRROWNER = Data.railroadLineLyrCS.Fields.FindField("RROWNER1");
            int indexNET = Data.railroadLineLyrCS.Fields.FindField("NET");
            int indexSubdiv = Data.railroadLineLyrCS.Fields.FindField("SUBDIV");
            List<string> subdivs = new List<string>();
            List<string> subdivNETs = new List<string>();

            //New bridges added manually during spatial accuracy check; does not exist in target point layer
            IFeatureCursor pCursor2 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature2 = pCursor2.NextFeature();
            if (pFeature2 != null)
            {
                string rrowner = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();
                IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor3 = Data.railroadLineLyrCS.Search(pSF, false);
                IFeature pFeature3 = pCursor3.NextFeature();

                if (pFeature3 != null)
                {
                    while (pFeature3 != null)
                    {
                        string rrowner1 = pFeature3.get_Value(indexRROWNER).ToString();
                        string subdiv1 = pFeature3.get_Value(indexSubdiv).ToString();
                        string net = pFeature3.get_Value(indexNET).ToString();

                        if (subdiv1.Trim() != "" && rrowner1 == rrowner)
                        {
                            subdivs.Add(subdiv1);
                            subdivNETs.Add(net);
                        }
                        pFeature3 = pCursor3.NextFeature();
                    }
                    Marshal.ReleaseComObject(pCursor3);
                }
                else
                {
                    List<string> raillinesIDs = new List<string>();
                    ITopologicalOperator pTopoOpt = (ITopologicalOperator)nodeOfInterest;
                    IPolygon pPoly = pTopoOpt.Buffer(0.00045) as IPolygon;
                    ISpatialFilter pSF1 = new SpatialFilter();
                    pSF1.Geometry = pPoly;
                    pSF1.GeometryField = "Shape";
                    pSF1.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor4 = Data.railroadLineLyrCS.Search(pSF1, false);
                    IFeature pFeature4 = pCursor4.NextFeature();
                    while (pFeature4 != null)
                    {
                        string rrowner1 = pFeature4.get_Value(indexRROWNER).ToString();
                        string subdiv1 = pFeature4.get_Value(indexSubdiv).ToString();
                        string net = pFeature4.get_Value(indexNET).ToString();

                        if (subdiv1.Trim() != "" && rrowner1 == rrowner)
                        {
                            subdivs.Add(subdiv1);
                            subdivNETs.Add(net);
                        }
                        pFeature4 = pCursor4.NextFeature();

                    }
                   
                    Marshal.ReleaseComObject(pCursor4);
                }
                Marshal.ReleaseComObject(pCursor2);
            }
            if (subdivs.Count > 0)
            {
                if (subdivNETs.Contains("M"))
                {
                    subdiv = subdivs[subdivNETs.IndexOf("M")];
                }

            }
            return subdiv;
        }
        private void populateSubdivision()
        {
            int indexSubDivision = Data.featurePntLyrCS.Fields.FindField("Subdivision");

            string subdiv= "";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "UniqueID = '" + uniqueID + "'";

                //if (uniqueID.Contains("RN"))
                //{
                //    pQF.WhereClause = "UniqueID = '" + uniqueID + "'";
                //    subdiv = populateSubDiv4NewBridges(pQF, Data.groupedTargetPntLyrCS, Data.intersectionPntLyrCS);
                //}
                //else if (uniqueID.Contains("WN"))
                //{
                //    pQF.WhereClause = "UniqueID = '" + uniqueID + "'";
                //    subdiv = populateSubDiv4NewBridges(pQF, Data.waterGroupedTargetPntLyrCS, Data.waterIntersectionPntLyrCS);
                //}
                //else if (uniqueID.Contains("R"))
                //{
                //    subdiv = populateSubDiv(pQF, Data.groupedTargetPntLyrCS, Data.intersectionPntLyrCS);
                //}
                //else if (uniqueID.Contains("W"))
                //{
                //    subdiv = populateSubDiv(pQF, Data.waterGroupedTargetPntLyrCS, Data.waterIntersectionPntLyrCS);
                //}

                if (uniqueID.Contains("R"))
                {
                    subdiv = populateSubDiv(pQF, Data.intersectionPntLyrCS, "OBJECTID");
                }
                else if (uniqueID.Contains("W"))
                {
                    subdiv = populateSubDiv(pQF,  Data.waterIntersectionPntLyrCS, "OBJECTID");
                }

                pFeature.set_Value(indexSubDivision, subdiv);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);


        }
        private void populateLatLon(IQueryFilter pQF)
        {
            int indexLat = Data.featurePntLyrCS.Fields.FindField("Latitute");
            int indexLon = Data.featurePntLyrCS.Fields.FindField("Longitude");
            if (indexLat == -1)
            {
                MessageBox.Show("Latitute does not exist " );
                return;
            }
            if (indexLon == -1)
            {
                MessageBox.Show("Longitude does not exist ");
                return;
            }

           
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();


            while (pFeature != null)
            {
                IPoint point = (IPoint)pFeature.Shape;
                pFeature.set_Value(indexLat, point.Y);
                pFeature.set_Value(indexLon, point.X);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
                
            }

            Marshal.ReleaseComObject(pCursor);
        }
        private void populateFIPS()
        {
            int indexFIPS = Data.featurePntLyrCS.Fields.FindField("FIPS");
            int indexState = Data.featurePntLyrCS.Fields.FindField("State");
            int indexCounty = Data.featurePntLyrCS.Fields.FindField("County");

            if (indexFIPS == -1)
            {
                MessageBox.Show("FIPS does not exist ");
                return;
            }
            if (indexState == -1)
            {
                MessageBox.Show("State does not exist ");
                return;
            }

            if (indexCounty == -1)
            {
                MessageBox.Show("County does not exist ");
                return;
            }

            if (Data.USCountyLyrCS == null)
            {
                MessageBox.Show("US County Layer does not exist ");
                return;
            }
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {

                IPoint nodeOfInterest = (IPoint)pFeature.Shape;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = "Shape";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor2 = Data.USCountyLyrCS.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();
                string fips = "";
                string county = "";
                if (pFeature2 != null)
                {
                    fips= pFeature2.get_Value(Data.USCountyLyrCS.Fields.FindField("STATEFP")).ToString()+ pFeature2.get_Value(Data.USCountyLyrCS.Fields.FindField("COUNTYFP")).ToString();
                    county = pFeature2.get_Value(Data.USCountyLyrCS.Fields.FindField("NAME")).ToString();

                }
                Marshal.ReleaseComObject(pCursor2);

                pFeature.set_Value(indexFIPS, fips);
                pFeature.set_Value(indexCounty, county);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

            
        }

        private string calculateNumTracks(IQueryFilter pQF, IFeatureClass intersectionPointsCS,  string objectID_Col)
        {
            string num_tracks = "Unknown";
            int indexFRANumTracks = Data.railroadLineLyrCS.Fields.FindField("TRACKS");
            int indexRROWNER = Data.railroadLineLyrCS.Fields.FindField("RROWNER1");
            
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature1 = pCursor1.NextFeature();
            
            if (pFeature1 != null) // exist in target point layer
            {
                num_tracks = "One";
                string rrowner = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();
                int numPoints = Convert.ToInt32(pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints")).ToString());
                string pointIDs = pFeature1.get_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs")).ToString();
                if (numPoints == 1) // at intersection point
                {
                    string[] pointObjIds = pointIDs.Split(';');
                    IQueryFilter pQF1 = new QueryFilter();
                    pQF1.WhereClause = objectID_Col + " = " + Convert.ToInt32(pointObjIds[0]);
                    IFeatureCursor pCursor2 = intersectionPointsCS.Search(pQF1, false);
                    IFeature pFeature2 = pCursor2.NextFeature();
                    if (pFeature2 != null)
                    {
                        IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                        ISpatialFilter pSF = new SpatialFilter();
                        pSF.Geometry = nodeOfInterest;
                        pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCursor3 = Data.railroadLineLyrCS.Search(pSF, false);
                        IFeature pFeature3 = pCursor3.NextFeature();

                        while (pFeature3 != null)
                        {
                            string rrowner1 = pFeature3.get_Value(indexRROWNER).ToString();
                            string tracks = pFeature3.get_Value(indexFRANumTracks).ToString();
                            if (tracks != "")
                            {
                                int tracks_num = Convert.ToInt32(tracks);
                                if (tracks_num > 1 && rrowner == rrowner1)
                                {
                                    num_tracks = "Multiple";
                                    break;
                                }
                            }
                            pFeature3 = pCursor3.NextFeature();

                        }
                        Marshal.ReleaseComObject(pCursor3);

                    }
                    Marshal.ReleaseComObject(pCursor2);
                }
                else //grouped location
                {
                    string[] pointObjIds = pointIDs.Split(';');

                    string pointObjs = objectID_Col + "=";
                    for (int i = 0; i < pointObjIds.Length - 2; i++)
                    {
                        pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR " + objectID_Col + " = ";
                    }
                   
                    pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                    IQueryFilter pQF2 = new QueryFilter();
                    pQF2.WhereClause = pointObjs;
                    IFeatureCursor pCursor2 = intersectionPointsCS.Search(pQF2, false);
                    IFeature pFeature2 = pCursor2.NextFeature();
                    List<string> raillinesIDs = new List<string>();
                    while (pFeature2 != null)
                    {
                        IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                        ISpatialFilter pSF = new SpatialFilter();
                        pSF.Geometry = nodeOfInterest;
                        pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCursor3 = Data.railroadLineLyrCS.Search(pSF, false);
                        IFeature pFeature3 = pCursor3.NextFeature();


                        while (pFeature3 != null)
                        {
                            string id = pFeature3.OID.ToString();
                            string rrowner1 = pFeature3.get_Value(indexRROWNER).ToString();
                            string tracks = pFeature3.get_Value(indexFRANumTracks).ToString();
                            if (!raillinesIDs.Contains(id))
                                raillinesIDs.Add(id);

                            if (tracks != "")
                            {
                                int tracks_num = Convert.ToInt32(tracks);
                                if (tracks_num > 1 && rrowner == rrowner1)
                                {
                                    num_tracks = "Multiple";
                                    break;
                                }
                            }
                            pFeature3 = pCursor3.NextFeature();

                        }
                        Marshal.ReleaseComObject(pCursor3);
                        pFeature2 = pCursor2.NextFeature();
                    }
                    Marshal.ReleaseComObject(pCursor2);

                    if (raillinesIDs.Count > 1)
                    {
                        num_tracks = "Multiple";
                    }
                }               

            }
          
            Marshal.ReleaseComObject(pCursor1);
            return num_tracks;

        }


        private string calculateNumTracks4NewBridges(IQueryFilter pQF, IFeatureClass targetPointsCS, IFeatureClass intersectionPointsCS)
        {
            string num_tracks = "Unknown";
            int indexRailroadId = intersectionPointsCS.Fields.FindField("FID_RailRoads_Unsplit");
            int indexFRANumTracks = Data.railroadLineLyrCS.Fields.FindField("TRACKS");
            int indexRROWNER = Data.railroadLineLyrCS.Fields.FindField("RROWNER1");

            //New bridges added manually during spatial accuracy check; does not exist in target point layer
            IFeatureCursor pCursor2 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature2 = pCursor2.NextFeature();
            if (pFeature2 != null)
            {
                num_tracks = "One";
                string rrowner = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();
                IPoint nodeOfInterest = (IPoint)pFeature2.Shape;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor3 = Data.railroadLineLyrCS.Search(pSF, false);
                IFeature pFeature3 = pCursor3.NextFeature();

                if (pFeature3 != null)
                {
                    while (pFeature3 != null)
                    {
                        string rrowner1 = pFeature3.get_Value(indexRROWNER).ToString();
                        string tracks = pFeature3.get_Value(indexFRANumTracks).ToString();
                        if (tracks != "")
                        {
                            int tracks_num = Convert.ToInt32(tracks);
                            if (tracks_num > 1 && rrowner == rrowner1)
                            {
                                num_tracks = "Multiple";
                                break;
                            }
                        }
                        pFeature3 = pCursor3.NextFeature();

                    }
                    Marshal.ReleaseComObject(pCursor3);
                }
                else
                {
                    List<string> raillinesIDs = new List<string>();
                    ITopologicalOperator pTopoOpt = (ITopologicalOperator)nodeOfInterest;
                    IPolygon pPoly = pTopoOpt.Buffer(0.00045) as IPolygon;
                    ISpatialFilter pSF1 = new SpatialFilter();
                    pSF1.Geometry = pPoly;
                    pSF1.GeometryField = "Shape";
                    pSF1.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor4 = Data.railroadLineLyrCS.Search(pSF1, false);
                    IFeature pFeature4 = pCursor4.NextFeature();
                    while (pFeature4 != null)
                    {
                        string id = pFeature4.OID.ToString();
                        string rrowner1 = pFeature4.get_Value(indexRROWNER).ToString();
                        string tracks = pFeature4.get_Value(indexFRANumTracks).ToString();
                        if (!raillinesIDs.Contains(id))
                        {
                            raillinesIDs.Add(id);
                        }
                        if (tracks != "")
                        {
                            int tracks_num = Convert.ToInt32(tracks);
                            if (tracks_num > 1 && rrowner == rrowner1)
                            {
                                num_tracks = "Multiple";
                                break;
                            }
                        }
                        pFeature4 = pCursor4.NextFeature();

                    }
                    if (raillinesIDs.Count > 1)
                        num_tracks = "Multiple";
                    Marshal.ReleaseComObject(pCursor4);
                }
                Marshal.ReleaseComObject(pCursor2);
            }
            return num_tracks;

        }
        private void populateNumTracks()
        {
            int indexNumTracks = Data.featurePntLyrCS.Fields.FindField("Num_Tracks");

            if (indexNumTracks == -1)
            {
                MessageBox.Show("Num_Tracks does not exist");
                return;
            }

            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();
            
            while (pFeature != null)
            {
                string uniqueID= pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "UniqueID = '" + uniqueID+"'";

                string num_tracks = "";
                if (uniqueID.Contains("R"))
                {
                    num_tracks = calculateNumTracks(pQF, Data.intersectionPntLyrCS,  "OBJECTID");
                }
                else
                if (uniqueID.Contains("W"))
                {
                    num_tracks = calculateNumTracks(pQF,  Data.waterIntersectionPntLyrCS,  "OBJECTID");
                }

                //if (num_tracks == "Unknown")
                //{
                //    pQF.WhereClause = "UniqueID = '" + uniqueID + "'";
                //    if (uniqueID.Contains("R"))
                //    {
                //        num_tracks = calculateNumTracks4NewBridges(pQF, Data.groupedTargetPntLyrCS, Data.intersectionPntLyrCS);
                //    }
                //    else if (uniqueID.Contains("W"))
                //    {
                //        num_tracks = calculateNumTracks4NewBridges(pQF, Data.waterGroupedTargetPntLyrCS, Data.waterIntersectionPntLyrCS);
                //    }
                    
                //}
              
                //IPoint point = (IPoint)pFeature.Shape;
                pFeature.set_Value(indexNumTracks, num_tracks);
                pFeature.Store();
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

        }
        private void populateWaterMilePost()
        {

            int indexWaterMile = Data.featurePntLyrCS.Fields.FindField("Mile_Post");
            int indexUniqueID = Data.featurePntLyrCS.Fields.FindField("UniqueID");
            


            if (indexWaterMile == -1)
            {
                MessageBox.Show("MilePost does not exist.");
                return;
            }
            
            
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            if (Data.WaterMileLyrCS == null)
            {
                MessageBox.Show("Waterway Mile Post does not exist. Therefore, Mile_Post will not be populated");

                while (pFeature != null)
                {
                    pFeature.set_Value(indexWaterMile, "Unknown");
                    pFeature.Store();
                    pFeature = pCursor.NextFeature();
                }
            }
            else
            {
                int indexMile = Data.WaterMileLyrCS.Fields.FindField("MILE");
                while (pFeature != null)
                {
                    string uniqueID = pFeature.get_Value(indexUniqueID).ToString();

                    string mile = "Unknown";
                    if (uniqueID.Contains("W"))
                    {

                        IPoint nodeOfInterest = pFeature.Shape as IPoint;
                        ITopologicalOperator pTopoOpt = null;
                        pTopoOpt = (ITopologicalOperator)nodeOfInterest;
                        IPolygon pPoly = pTopoOpt.Buffer(bufferRange) as IPolygon;
                        ISpatialFilter pSF = null;
                        pSF = new SpatialFilter();
                        pSF.Geometry = pPoly;
                        pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCursor2 = Data.WaterMileLyrCS.Search(pSF, false);
                        IFeature pFeature2 = pCursor2.NextFeature();

                        string rivername = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Name")).ToString();

                        //IProximityOperator proximityOp = (IProximityOperator)nodeOfInterest;

                        //List<string> pointsIds = new List<string>();
                        List<double> pointsDist = new List<double>();
                        List<int> pointsdRMile = new List<int>();

                        while (pFeature2 != null)
                        {
                            string rivername1 = pFeature2.get_Value(Data.WaterMileLyrCS.Fields.FindField("RIVER_NAME")).ToString();
                            double dist = calculateDist((IPoint)pFeature2.Shape, nodeOfInterest);
                            int milePost = Convert.ToInt32(pFeature2.get_Value(indexMile).ToString());

                            if (rivername.ToUpper().Trim().Contains(rivername1.ToUpper().Trim()))
                            {
                                pointsDist.Add(dist);
                                pointsdRMile.Add(milePost);

                            }
                            //pointsIds.Add(oid);

                            pFeature2 = pCursor2.NextFeature();
                        }
                        Marshal.ReleaseComObject(pCursor2);


                        if (pointsDist.Count > 0)
                        {
                            int index = pointsDist.IndexOf(pointsDist.Min());
                            int milePost = pointsdRMile[index];
                            mile = milePost.ToString();
                        }


                    }

                    pFeature.set_Value(indexWaterMile, mile);
                    pFeature.Store();

                    pFeature = pCursor.NextFeature();

                }

            }


           

            Marshal.ReleaseComObject(pCursor);
        }

        private double calculateDist(IPoint fromPnt, IPoint toPnt)
        {
            double dist = 0.0;

            dist = Math.Sqrt((fromPnt.X - toPnt.X) * (fromPnt.X - toPnt.X) + (fromPnt.Y - toPnt.Y) * (fromPnt.Y - toPnt.Y));

            return dist;
        }
        private void populateAttrFromRailroads()
        {
            

            int indexNumTracks = Data.groupedTargetPntLyrCS.Fields.FindField("Num_Tracks");
            int indexSubDivision = Data.groupedTargetPntLyrCS.Fields.FindField("Subdivision");
            int indexRROwner = Data.groupedTargetPntLyrCS.Fields.FindField("RROwner");
            int indexLat = Data.groupedTargetPntLyrCS.Fields.FindField("Latitute");
            int indexLon = Data.groupedTargetPntLyrCS.Fields.FindField("Longitude");
            int indexCountyFIPS = Data.groupedTargetPntLyrCS.Fields.FindField("County_FIPS");
            int indexStateFIPS = Data.groupedTargetPntLyrCS.Fields.FindField("State_FIPS");

            int indexTracks = Data.railroadLineLyrCS.Fields.FindField("TRACKS");
            int indexSubdiv= Data.railroadLineLyrCS.Fields.FindField("SUBDIV");
            int indexSTFIPS = Data.railroadLineLyrCS.Fields.FindField("STFIPS");
            int indexCNTYFIPS = Data.railroadLineLyrCS.Fields.FindField("CNTYFIPS");

            if (indexNumTracks == -1)
            {
                MessageBox.Show("Num_Tracks does not exist in "+Data.groupedTargetFeatureLyrNM);
                return;
            }

            if (indexSubDivision == -1)
            {
                MessageBox.Show("Subdivision does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }
            if (indexRROwner == -1)
            {
                MessageBox.Show("RROwner does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }
            if (indexLat == -1)
            {
                MessageBox.Show("Latitute does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }
            if (indexLon == -1)
            {
                MessageBox.Show("Longitude does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }

            if (indexCountyFIPS == -1)
            {
                MessageBox.Show("County_FIPS does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }
            if (indexStateFIPS == -1)
            {
                MessageBox.Show("State_FIPS does not exist " + Data.groupedTargetFeatureLyrNM);
                return;
            }


            if (indexTracks == -1)
            {
                MessageBox.Show("TRACKS does not exist " + Data.RailroadLyrNM);
                return;
            }
            if (indexSubdiv == -1)
            {
                MessageBox.Show("SUBDIV does not exist " + Data.RailroadLyrNM);
                return;
            }

            if (indexSTFIPS == -1)
            {
                MessageBox.Show("STFIPS does not exist " + Data.RailroadLyrNM);
                return;
            }
            if (indexCNTYFIPS == -1)
            {
                MessageBox.Show("CNTYFIPS does not exist " + Data.RailroadLyrNM);
                return;
            }


            IFeatureCursor pCursor = Data.groupedTargetPntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            //string rrowner = "";
            string subdiv = "";
            int num_tracks = -1;
            string stfips = "";
            string countyfips = "";

            while (pFeature != null)
            {
                string rrowner= pFeature.get_Value(Data.groupedTargetPntLyrCS.Fields.FindField("RROwner")).ToString();
                int numPoints = Convert.ToInt32(pFeature.get_Value(Data.groupedTargetPntLyrCS.Fields.FindField("NoOfPoints")).ToString());
                string pointIDs = pFeature.get_Value(Data.groupedTargetPntLyrCS.Fields.FindField("PointIDs")).ToString();
                //IPoint nodeOfInterest = (IPoint)pFeature.Shape;

                IQueryFilter pQF = new QueryFilter();
                
                if (numPoints == 1)
                {
                    pQF.WhereClause = "OBJECTID=" +Convert.ToInt32(pointIDs);
                }
                else
                {
                    string[] pointObjIds = pointIDs.Split(';');
                    if (numPoints != pointObjIds.Length - 1)
                    {
                        MessageBox.Show("Bad data");
                        return;
                    }
                    string pointObjs = "OBJECTID=";
                    for (int i = 0; i < pointObjIds.Length - 2; i++)
                    {
                        pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR OBJECTID= ";
                    }
                    pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                    pQF.WhereClause = pointObjs;
                }

                IFeatureCursor pCursor1 = Data.intersectionPntLyrCS.Search(pQF, false);
                IFeature pFeature1 = pCursor1.NextFeature();

                if (pFeature1 == null)
                {
                    MessageBox.Show("No intersection point found");
                    return;

                }

                while (pFeature1 !=null)
                {
                    IPoint nodeOfInterest = (IPoint)pFeature1.Shape;

                    ISpatialFilter pSF = new SpatialFilter();
                    pSF.Geometry = nodeOfInterest;
                    pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor2 = Data.railroadLineLyrCS.Search(pSF, false);
                    IFeature pFeature2 = pCursor2.NextFeature();

                    if (pFeature2 != null)
                    {
                        subdiv = pFeature2.get_Value(indexSubdiv).ToString();
                        //num_tracks = Convert.ToInt32(pFeature2.get_Value(indexTracks).ToString());
                        stfips = pFeature2.get_Value(indexSTFIPS).ToString();
                        countyfips = pFeature2.get_Value(indexCNTYFIPS).ToString();
                        Marshal.ReleaseComObject(pCursor2);
                        break;
                    }

                    Marshal.ReleaseComObject(pCursor2);
                    pFeature1 = pCursor1.NextFeature();
                }

                Marshal.ReleaseComObject(pCursor1);

                IPoint point = (IPoint)pFeature.Shape;
                pFeature.set_Value(indexLat, point.Y);
                pFeature.set_Value(indexLon, point.X);
               
                pFeature.set_Value(indexStateFIPS, stfips);
                pFeature.set_Value(indexCountyFIPS, countyfips);

                pFeature.set_Value(indexNumTracks, num_tracks);
                pFeature.set_Value(indexSubDivision, subdiv);
                pFeature.Store();
                pFeature = pCursor.NextFeature();

                

            }

            Marshal.ReleaseComObject(pCursor);


        }

       
        private void populateRailMileMarker()
        {
            

            int indexRailMile = Data.featurePntLyrCS.Fields.FindField("Rail_MilePost");
            int indexRROwner = Data.featurePntLyrCS.Fields.FindField("RROwner");

            int indexRROwner1 = Data.RailMileLyrCS.Fields.FindField("RAILROAD");
            int indexMile = Data.RailMileLyrCS.Fields.FindField("MILEPOST");

            if (Data.RailMileLyrCS == null)
            {
                MessageBox.Show("Rail Mile Post does not exist. Therefore, Rail_MilePost will not be populated");
                return;
            }

            if (indexRailMile == -1)
            {
                MessageBox.Show("Rail_Mile does not exist.");
                return;
            }
            if (indexRROwner == -1)
            {
                MessageBox.Show("RROwner does not exist.");
                return;
            }
            if (indexRROwner1 == -1)
            {
                MessageBox.Show("RAILROAD does not exist in " + Data.RailMileLyrCS);
                return;
            }
            if (indexMile == -1)
            {
                MessageBox.Show("MILEPOST does not exist in " + Data.RailMileLyrCS);
                return;
            }

            


            //long milePost = -1;
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                string rrowner = pFeature.get_Value(indexRROwner).ToString();
                

                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                ITopologicalOperator pTopoOpt = null;
                pTopoOpt = (ITopologicalOperator)nodeOfInterest;
                IPolygon pPoly = pTopoOpt.Buffer(bufferRange) as IPolygon;
                ISpatialFilter pSF = null;
                pSF = new SpatialFilter();
                pSF.Geometry = pPoly;
                pSF.GeometryField =Data.featurePntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSF.WhereClause = "RAILROAD ='" + rrowner + "' " ;
                IFeatureCursor pCursor2 =Data.RailMileLyrCS.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();

                IProximityOperator proximityOp = (IProximityOperator)nodeOfInterest;
               
                //List<string> pointsIds = new List<string>();
                List<double> pointsDist = new List<double>();
                List<int> pointsdRMile = new List<int>();

                while (pFeature2 != null)
                {
                    double dist = proximityOp.ReturnDistance(pFeature2.Shape);
                    int milePost = Convert.ToInt32( pFeature2.get_Value(indexMile).ToString());

                    //pointsIds.Add(oid);
                    pointsDist.Add(dist);
                    pointsdRMile.Add(milePost);
                  
                    pFeature2 = pCursor2.NextFeature();
                }
                Marshal.ReleaseComObject(pCursor2);

                string mile = "Unknown";
                if (pointsDist.Count > 0)
                {
                    int index = pointsDist.IndexOf(pointsDist.Min());
                    int milePost = pointsdRMile[index];
                    mile = milePost.ToString();
                }
                pFeature.set_Value(indexRailMile, mile);
                pFeature.Store();

                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

        }

        private void hideBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Original Connection Files|*.txt";
            openFileDialog1.Title = "Select a Original Connection File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.
                try
                {
                    //open txt file and read to an array
                    suffixListFileName = openFileDialog1.FileName;
                    suffixlistNM.Text = suffixListFileName;
                   
                }
                catch
                {
                    MessageBox.Show("This file is not correct. ");
                }
                //MessageBox.Show("Done loading txt.");
            }
        }

        private void cbxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAll.Checked)
            {
                cbxLatLng.Checked = true;
                cbxFIPS.Checked = true;
                cbxNone.Checked = true;
                cbxNoTracks.Checked = true;
                cbxRailPost.Checked = true;
                cbxState.Checked = true;
                cbxStreetNM.Checked = true;
                cbxWaterPost.Checked = true;
                cbxSubDivision.Checked = true;
                cbxSource.Checked = true;
                cbxNothing.Checked = false;
            }
        }

        private void cbxNothing_CheckedChanged(object sender, EventArgs e)
        {

            if (cbxNothing.Checked)
            {
                cbxAll.Checked = false;
                cbxLatLng.Checked = false;
                cbxFIPS.Checked = false;
                cbxNone.Checked = false;
                cbxNoTracks.Checked = false;
                cbxRailPost.Checked = false;
                cbxState.Checked = false;
                cbxStreetNM.Checked = false;
                cbxWaterPost.Checked = false;
                cbxSubDivision.Checked = false;
                cbxSource.Checked = false;
            }
        }

        private void cbxLatLng_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxLatLng.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked=false;
            }
        }

        private void cbxStreetNM_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxStreetNM.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxSubDivision_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxSubDivision.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxNoTracks_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxNoTracks.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxRailPost_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxRailPost.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxWaterPost_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxWaterPost.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxSource_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxSource.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxState_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxState.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxFIPS_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxFIPS.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void cbxNone_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxNone.Checked)
            {
                cbxAll.Checked = false;
            }
            else
            {
                cbxNothing.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to fix locations of highway bridges now?", "Fix Highway Bridges ", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                 return;                
            }
            IFeatureClass tiger_old= Data.pFWS.OpenFeatureClass("Tiger_Roads_Old");
            IFeatureClass railroad_old = Data.pFWS.OpenFeatureClass("FRA_Railroads_Old");
            IFeatureClass roadIntersectionPoints_old = Data.pFWS.OpenFeatureClass("Road_IntersectionPoints_Old");
            IFeatureClass roadTargetPoints_old = Data.pFWS.OpenFeatureClass("Road_TargetPoints_Old");
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "UniqueID Like 'R*'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                string rrowner = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("RROwner")).ToString();

                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor2 = roadIntersectionPoints_old.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();
                bool found = false;
                if (pFeature2 != null)
                {
                    IPoint nodeOfInterest1 = pFeature2.Shape as IPoint;
                    List<string> oldTigerIDList = getTigerID(nodeOfInterest1, tiger_old);

                    string oldRailroadID = getRailroadID(nodeOfInterest1, railroad_old);
                    int i = 0;

                    while (i < oldTigerIDList.Count)
                    {
                        string objectID = findPoint(nodeOfInterest1, oldTigerIDList[i], oldRailroadID);

                        if (objectID != "")
                        {

                            IQueryFilter pQF1 = new QueryFilter();
                            pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID);
                            IFeatureCursor pCursor6 = Data.intersectionPntLyrCS.Search(pQF1, false);
                            IFeature pFeature6 = pCursor6.NextFeature();

                            if (pFeature6 != null)
                            {
                                //string streetNames = getIntersectStreetName(pFeature6);
                                pFeature.Shape = pFeature6.Shape;
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 1);
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), objectID);
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), 1);
                                //pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), streetNames);
                                pFeature.Store();
                                Marshal.ReleaseComObject(pFeature6);
                                Marshal.ReleaseComObject(pCursor6);
                                found = true;
                                break;
                            }
                            //Marshal.ReleaseComObject(pCursor6);

                        }
                        i = i + 1;
                    }



                    if (!found)
                    {
                        i = 0;
                        while (i < oldTigerIDList.Count)
                        {
                            string objectID = findNearestPoint(nodeOfInterest1, oldTigerIDList[i], oldRailroadID, rrowner);

                            if (objectID != "")
                            {
                                IQueryFilter pQF1 = new QueryFilter();
                                pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID);
                                IFeatureCursor pCursor6 = Data.intersectionPntLyrCS.Search(pQF1, false);
                                IFeature pFeature6 = pCursor6.NextFeature();

                                if (pFeature6 != null)
                                {
                                   // string streetNames = getIntersectStreetName(pFeature6);
                                    pFeature.Shape = pFeature6.Shape;
                                    pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 0);
                                    pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), objectID);
                                    pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), 1);
                                    //pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), streetNames);
                                    pFeature.Store();
                                    Marshal.ReleaseComObject(pCursor6);
                                    found = true;
                                    break;
                                }
                                Marshal.ReleaseComObject(pCursor6);
                            }
                            i = i + 1;
                        }
                        if (!found)
                        {
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), -1);
                            pFeature.Store();
                        }
                    }

                }
                else
                {
                    IFeatureCursor pCursor3 = roadTargetPoints_old.Search(pSF, false);
                    IFeature pFeature3 = pCursor3.NextFeature();
                    if (pFeature3 != null)
                    {
                        IPointCollection pointColl = new MultipointClass();
                        int numPoints = Convert.ToInt32(pFeature3.get_Value(roadTargetPoints_old.Fields.FindField("NoOfPoints")).ToString());
                        string pointIDs = pFeature3.get_Value(roadTargetPoints_old.Fields.FindField("PointIDs")).ToString();

                        string[] pointObjIds = pointIDs.Split(';');

                        IQueryFilter pQF1 = new QueryFilter();

                        if (numPoints == 1)
                        {

                            pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(pointObjIds[0]);
                        }
                        else
                        {
                            string pointObjs = "OBJECTID=";
                            for (int i = 0; i < pointObjIds.Length - 2; i++)
                            {
                                pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR OBJECTID= ";
                            }
                            pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                            pQF1.WhereClause = pointObjs;
                        }

                        IFeatureCursor pCursor4 = roadIntersectionPoints_old.Search(pQF1, false);
                        IFeature pFeature4 = pCursor4.NextFeature();

                        double x_sum = 0;
                        double y_sum = 0;
                        string newPointIDs = "";
                        double noOfPoints = 0;
                        string streetnames = "";
                        while (pFeature4 != null)
                        {
                            IPoint nodeOfInterest2 = (IPoint)pFeature4.Shape;

                            List<string> oldTigerIDList = getTigerID(nodeOfInterest2, tiger_old);

                            string oldRailroadID = getRailroadID(nodeOfInterest2, railroad_old);

                            int i = 0;
                            while (i < oldTigerIDList.Count)
                            {

                                string objectID = findPoint(nodeOfInterest2, oldTigerIDList[i], oldRailroadID);


                                if (objectID != "")
                                {
                                    IQueryFilter pQF2 = new QueryFilter();
                                    pQF2.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID);
                                    IFeatureCursor pCursor6 = Data.intersectionPntLyrCS.Search(pQF2, false);
                                    IFeature pFeature6 = pCursor6.NextFeature();

                                    if (pFeature6 != null)
                                    {
                                        //string streetname = getIntersectStreetName(pFeature6);
                                        //streetnames = streetnames + streetname + ";";
                                        IPoint newPoint = (IPoint)pFeature6.Shape;
                                        pointColl.AddPoint(newPoint);
                                        x_sum = x_sum + newPoint.X;
                                        y_sum = y_sum + newPoint.Y;
                                        newPointIDs = newPointIDs + objectID + ";";
                                        noOfPoints = noOfPoints + 1;
                                        Marshal.ReleaseComObject(pQF2);
                                        Marshal.ReleaseComObject(pFeature6);
                                        Marshal.ReleaseComObject(pCursor6);
                                        break;
                                    }
                                    Marshal.ReleaseComObject(pCursor6);

                                }

                                i = i + 1;
                            }
                            pFeature4 = pCursor4.NextFeature();

                        }
                        Marshal.ReleaseComObject(pCursor4);

                        if (numPoints == noOfPoints)
                        {

                            IPoint newPoint1 = new PointClass();
                            newPoint1.PutCoords(x_sum / noOfPoints, y_sum / noOfPoints);
                            pFeature.Shape = newPoint1;
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), newPointIDs);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), noOfPoints);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 3);

                        }
                        else if (noOfPoints > 0)
                        {

                            IPoint newPoint1 = new PointClass();
                            newPoint1.PutCoords(x_sum / noOfPoints, y_sum / noOfPoints);
                            pFeature.Shape = newPoint1;
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), newPointIDs);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), noOfPoints);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 4);

                        }
                        else
                        {
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 5);
                        }

                        pFeature.Store();
                    }
                    Marshal.ReleaseComObject(pCursor3);
                }

                Marshal.ReleaseComObject(pCursor2);


                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
            MessageBox.Show("Finished!");
        }

        private string getIntersectStreetName(IFeature pFeature1)
        {
            string streetNames = "";
            IPoint newPoint = (IPoint)pFeature1.Shape;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = newPoint;
            pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = Data.roadLineLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            List<string> streetNMList = new List<string>();
            while (pFeature != null)
            {
                string streetname= pFeature.get_Value(Data.roadLineLyrCS.Fields.FindField("FULLNAME")).ToString().Trim().ToUpper();
                if (streetNMList.Count == 0 && streetname != "")
                {
                    streetNMList.Add(streetname);
                    streetNames = streetname + ";";
                }
                if (streetNMList.Count != 0 && streetNMList.Contains(streetname) && streetname != "")
                {
                    streetNMList.Add(streetname);
                    streetNames = streetNames+ streetname + ";";
                }
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
            return streetNames;
        }

        private string getRiverName(IFeature pFeature1)
        {
            string rivername = "";
            IPoint newPoint = (IPoint)pFeature1.Shape;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = newPoint;
            pSF.GeometryField = Data.waterIntersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = Data.roadLineLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            List<string> riverNMList = new List<string>();
            while (pFeature != null)
            {
                string streetname = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("GNIS_NAME")).ToString().Trim().ToUpper();
                if (riverNMList.Count == 0 && streetname != "")
                {
                    riverNMList.Add(streetname);
                    rivername = streetname + ";";
                }
                if (riverNMList.Count != 0 && riverNMList.Contains(streetname) && streetname != "")
                {
                    riverNMList.Add(streetname);
                    rivername = rivername + streetname + ";";
                }
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
            return rivername;
        }


        private string findPoint(IPoint nodeOfInterest, string oldTigerID, string oldRailroadID)
        {
            IPoint newPoint= nodeOfInterest;
            ITopologicalOperator pTopoOpt = null;
            pTopoOpt = (ITopologicalOperator)nodeOfInterest;
            IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = pPoly;
            pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor= Data.intersectionPntLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            string objectID = "";
            while (pFeature != null)
            {
                IPoint nodeOfInterest1 = (IPoint)pFeature.Shape;
                List<string> tigerIDList = getTigerID(nodeOfInterest1, Data.roadLineLyrCS);
                string railroadID = getRailroadID(nodeOfInterest1, Data.railroadLineLyrCS);
                
                if (tigerIDList.Contains(oldTigerID) && railroadID== oldRailroadID)
                {
                    string flag= pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("ProcessFlag")).ToString();
                    string groupID = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("GroupID")).ToString();

                    if (flag != "5")
                    {
                        newPoint = nodeOfInterest1;
                        if (groupID != "")
                        {
                            IQueryFilter pQF = new QueryFilter();
                            pQF.WhereClause = "GroupID = " + groupID;
                            IFeatureCursor pCursor1 = Data.intersectionPntLyrCS.Search(pQF, false);
                            IFeature pFeature1 = pCursor1.NextFeature();
                            while (pFeature1 != null)
                            {
                                pFeature1.set_Value(Data.intersectionPntLyrCS.Fields.FindField("ProcessFlag"), 5);
                                pFeature1.Store();
                                pFeature1 = pCursor1.NextFeature();
                            }
                            Marshal.ReleaseComObject(pCursor1);
                        }
                        else
                        {
                            pFeature.set_Value(Data.intersectionPntLyrCS.Fields.FindField("ProcessFlag"), 5);
                            pFeature.Store();
                        }

                        objectID = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                        Marshal.ReleaseComObject(pCursor);
                        break;
                    }
                    
                }
                                
                pFeature = pCursor.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor);

            return objectID;
        }
        private String findNearestPoint(IPoint nodeOfInterest, string oldTigerID, string oldRailroadID, string rrowner)
        {
            string nearObjectID = "";
            IPoint newPoint = nodeOfInterest;
            ITopologicalOperator pTopoOpt = null;
            pTopoOpt = (ITopologicalOperator)nodeOfInterest;
            IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = pPoly;
            pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = Data.intersectionPntLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            string groupID = "";
            double minDist = 9999;

            while (pFeature != null)
            {
                string rrowner1 = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("RROWNER1")).ToString();
                IPoint nodeOfInterest1 = (IPoint)pFeature.Shape;
                List<string> tigerIDList = getTigerID(nodeOfInterest1, Data.roadLineLyrCS);
                string railroadID = getRailroadID(nodeOfInterest1, Data.railroadLineLyrCS);
                string objectID = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                string flag = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("ProcessFlag")).ToString();
                string groupID1= pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("GroupID")).ToString();
                if ((tigerIDList.Contains(oldTigerID) || railroadID == oldRailroadID) && flag !="5" && rrowner==rrowner1)
                {
                    double dist = calculateDist(nodeOfInterest1, nodeOfInterest);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        newPoint = nodeOfInterest1;
                        nearObjectID = objectID;
                        groupID = groupID1;
                    }
                    
                }

                pFeature = pCursor.NextFeature();
            }

            if (nearObjectID !="" && groupID!="")
            {
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "GroupID = " + groupID;
                IFeatureCursor pCursor1 = Data.intersectionPntLyrCS.Search(pQF, false);
                IFeature pFeature1 = pCursor1.NextFeature();
                while (pFeature1 != null)
                {
                    pFeature1.set_Value(Data.intersectionPntLyrCS.Fields.FindField("ProcessFlag"), 5);
                    pFeature1.Store();
                    pFeature1 = pCursor1.NextFeature();
                }
                Marshal.ReleaseComObject(pCursor1);
            }
            
            Marshal.ReleaseComObject(pCursor);

            return nearObjectID;
            
           
        }

        private string findWaterwayPoint(IPoint nodeOfInterest, string oldWaterwayID, string oldRailroadID)
        {
            IPoint newPoint = nodeOfInterest;
            ITopologicalOperator pTopoOpt = null;
            pTopoOpt = (ITopologicalOperator)nodeOfInterest;
            IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = pPoly;
            pSF.GeometryField = Data.waterIntersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            string objectID = "";
            IFeatureCursor pCursor = Data.waterIntersectionPntLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            while (pFeature != null)
            {
                IPoint nodeOfInterest1 = (IPoint)pFeature.Shape;
                string waterwayID = getWaterwayID(nodeOfInterest1, Data.waterwayLineLyrCS);
                string railroadID = getRailroadID(nodeOfInterest1, Data.railroadLineLyrCS);

                if (waterwayID == oldWaterwayID && railroadID == oldRailroadID)
                {
                    string flag = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("ProcessFlag")).ToString();
                    if (flag != "5")
                    {
                        newPoint = nodeOfInterest1;
                        pFeature.set_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("ProcessFlag"), 5);
                        pFeature.Store();
                        objectID = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                        Marshal.ReleaseComObject(pFeature);
                        Marshal.ReleaseComObject(pCursor);
                        break;
                    }
                   
                }
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);

            return objectID;
        }

        private string findNearestWaterwayPoint(IPoint nodeOfInterest, string oldWaterwayID, string oldRailroadID)
        {
            string objectID = "";
            IPoint newPoint = nodeOfInterest;
            ITopologicalOperator pTopoOpt = null;
            pTopoOpt = (ITopologicalOperator)nodeOfInterest;
            IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
            ISpatialFilter pSF = null;
            pSF = new SpatialFilter();
            pSF.Geometry = pPoly;
            pSF.GeometryField = Data.intersectionPntLyrCS.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = Data.waterIntersectionPntLyrCS.Search(pSF, false);
            IFeature pFeature = pCursor.NextFeature();
            double minDist = 9999;
            while (pFeature != null)
            {
                IPoint nodeOfInterest1 = (IPoint)pFeature.Shape;
                string waterwayID = getWaterwayID(nodeOfInterest1, Data.waterwayLineLyrCS);
                string railroadID = getRailroadID(nodeOfInterest1, Data.railroadLineLyrCS);

                if (waterwayID == oldWaterwayID || railroadID == oldRailroadID)
                {
                    string flag = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("ProcessFlag")).ToString();
                    if (flag != "5")
                    {
                        newPoint = nodeOfInterest1;
                        pFeature.set_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("ProcessFlag"), 5);
                        pFeature.Store();
                        objectID = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                        Marshal.ReleaseComObject(pFeature);
                        Marshal.ReleaseComObject(pCursor);
                        break;
                    }
                }
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);

            return objectID;
        }
        private List<string> getTigerID(IPoint nodeOfInterest, IFeatureClass tigerData)
        {
            ISpatialFilter pSF1 = new SpatialFilter();
            pSF1.Geometry = nodeOfInterest;
            pSF1.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
            pSF1.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = tigerData.Search(pSF1, false);
            IFeature pFeature = pCursor.NextFeature();
            List<string> oldTigerIDList= new List<string>();
            while (pFeature != null)
            {
                string oldTigerID = pFeature.get_Value(tigerData.Fields.FindField("LINEARID")).ToString();
                oldTigerIDList.Add(oldTigerID);
                pFeature = pCursor.NextFeature();
            }
            
            Marshal.ReleaseComObject(pCursor);

            return oldTigerIDList;
        }


        private string getRailroadID(IPoint nodeOfInterest, IFeatureClass railroadData)
        {
            ISpatialFilter pSF1 = new SpatialFilter();
            pSF1.Geometry = nodeOfInterest;
            pSF1.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
            pSF1.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = railroadData.Search(pSF1, false);
            IFeature pFeature = pCursor.NextFeature();
            string oldRailroadID = "";
            if (pFeature != null)
            { 

                oldRailroadID = pFeature.get_Value(railroadData.Fields.FindField("OBJECTID")).ToString();
            }
            Marshal.ReleaseComObject(pCursor);

            return oldRailroadID;
        }

        private string getWaterwayID(IPoint nodeOfInterest, IFeatureClass waterwayData)
        {
            ISpatialFilter pSF1 = new SpatialFilter();
            pSF1.Geometry = nodeOfInterest;
            pSF1.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
            pSF1.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = waterwayData.Search(pSF1, false);
            IFeature pFeature = pCursor.NextFeature();
            string oldWaterwayID = "";
            if (pFeature != null)
            {

                oldWaterwayID = pFeature.get_Value(waterwayData.Fields.FindField("OBJECTID")).ToString();
            }
            Marshal.ReleaseComObject(pCursor);

            return oldWaterwayID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to fix locations of waterway bridges now?", "Fix Waterway Bridges ", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            IFeatureClass railroad_old = Data.pFWS.OpenFeatureClass("FRA_Railroads_Old");
            IFeatureClass waterIntersectionPoints_old = Data.pFWS.OpenFeatureClass("Water_IntersectionPoints_Old");
            IFeatureClass waterTargetPoints_old = Data.pFWS.OpenFeatureClass("Water_TargetPoints_Old");
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "UniqueID Like 'W*'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor2 = waterIntersectionPoints_old.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();

                if (pFeature2 != null)
                {
                    IPoint nodeOfInterest1 = pFeature2.Shape as IPoint;
                    string oldWaterwayID = getWaterwayID(nodeOfInterest1, Data.waterwayLineLyrCS);

                    string oldRailroadID = getRailroadID(nodeOfInterest1, railroad_old);

                    string objectID = findWaterwayPoint(nodeOfInterest1, oldWaterwayID, oldRailroadID);

                    if (objectID != "")
                    {
                        IQueryFilter pQF1 = new QueryFilter();
                        pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID);
                        IFeatureCursor pCursor6 = Data.waterIntersectionPntLyrCS.Search(pQF1, false);
                        IFeature pFeature6 = pCursor6.NextFeature();

                        if (pFeature6 != null)
                        {
                            string rivername = getRiverName(pFeature6);
                            pFeature.Shape = pFeature6.Shape;
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 1);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), objectID);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), 1);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), rivername);
                            pFeature.Store();
                        }
                        Marshal.ReleaseComObject(pCursor6);

                    }
                    else
                    {
                        string objectID1 = findNearestWaterwayPoint(nodeOfInterest1, oldWaterwayID, oldRailroadID);
                        if (objectID1 != "")
                        {
                            IQueryFilter pQF1 = new QueryFilter();
                            pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID1);
                            IFeatureCursor pCursor6 = Data.waterIntersectionPntLyrCS.Search(pQF1, false);
                            IFeature pFeature6 = pCursor6.NextFeature();

                            if (pFeature6 != null)
                            {
                                string rivername = getRiverName(pFeature6);
                                pFeature.Shape = pFeature6.Shape;
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 0);
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), objectID1);
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), 1);
                                pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), rivername);
                                pFeature.Store();
                            }
                            Marshal.ReleaseComObject(pCursor6);

                        }
                        else
                        {
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), -1);
                            pFeature.Store();
                        }

                    }


                }
                else
                {
                    IFeatureCursor pCursor3 = waterTargetPoints_old.Search(pSF, false);
                    IFeature pFeature3 = pCursor3.NextFeature();
                    if (pFeature3 != null)
                    {
                        IPointCollection pointColl = new MultipointClass();
                        int numPoints = Convert.ToInt32(pFeature3.get_Value(waterTargetPoints_old.Fields.FindField("NoOfPoints")).ToString());
                        string pointIDs = pFeature3.get_Value(waterTargetPoints_old.Fields.FindField("PointIDs")).ToString();

                        string[] pointObjIds = pointIDs.Split(';');

                        IQueryFilter pQF1 = new QueryFilter();

                        if (numPoints == 1)
                        {

                            pQF1.WhereClause = "OBJECTID=" + Convert.ToInt32(pointObjIds[0]);
                        }
                        else
                        {
                            string pointObjs = "OBJECTID=";
                            for (int i = 0; i < pointObjIds.Length - 2; i++)
                            {
                                pointObjs = pointObjs + Convert.ToInt32(pointObjIds[i]) + " OR OBJECTID= ";
                            }
                            pointObjs = pointObjs + Convert.ToInt32(pointObjIds[(pointObjIds.Length - 2)]);
                            pQF1.WhereClause = pointObjs;
                        }

                        IFeatureCursor pCursor4 = waterIntersectionPoints_old.Search(pQF1, false);
                        IFeature pFeature4 = pCursor4.NextFeature();
                        bool flag = false;
                        double x_sum = 0;
                        double y_sum = 0;
                        string newPointIDs = "";
                        double noOfPoints = 0;
                        string rivernames = "";
                        while (pFeature4 != null)
                        {
                            IPoint nodeOfInterest2 = (IPoint)pFeature4.Shape;

                            string oldWaterwayID = getWaterwayID(nodeOfInterest2, Data.waterwayLineLyrCS);

                            string oldRailroadID = getRailroadID(nodeOfInterest2, railroad_old);

                            string objectID = findWaterwayPoint(nodeOfInterest2, oldWaterwayID, oldRailroadID);
                            if (objectID != "")
                            {
                                IQueryFilter pQF2 = new QueryFilter();
                                pQF2.WhereClause = "OBJECTID=" + Convert.ToInt32(objectID);
                                IFeatureCursor pCursor6 = Data.waterIntersectionPntLyrCS.Search(pQF2, false);
                                IFeature pFeature6 = pCursor6.NextFeature();

                                if (pFeature6 != null)
                                {
                                    string rivername = getRiverName(pFeature6);
                                    rivernames = rivernames + rivername + ";";
                                    IPoint newPoint = (IPoint)pFeature6.Shape;
                                    pointColl.AddPoint(newPoint);
                                    x_sum = x_sum + newPoint.X;
                                    y_sum = y_sum + newPoint.Y;
                                    newPointIDs = newPointIDs + objectID + ";";
                                    noOfPoints = noOfPoints + 1;
                                    Marshal.ReleaseComObject(pQF2);
                                    Marshal.ReleaseComObject(pFeature6);
                                    Marshal.ReleaseComObject(pCursor6);
                                    break;
                                }
                                Marshal.ReleaseComObject(pCursor6);

                            }
                            pFeature4 = pCursor4.NextFeature();

                        }

                        Marshal.ReleaseComObject(pCursor4);

                        if (numPoints == noOfPoints)
                        {
                            IPoint newPoint = new PointClass();
                            newPoint.PutCoords(x_sum / noOfPoints, y_sum / noOfPoints);
                            pFeature.Shape = newPoint;
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), newPointIDs);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), noOfPoints);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 3);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), rivernames);
                            pFeature.Store();
                        }
                        else if (noOfPoints>0)
                        {
                            IPoint newPoint = new PointClass();
                            newPoint.PutCoords(x_sum / noOfPoints, y_sum / noOfPoints);
                            pFeature.Shape = newPoint;
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs"), newPointIDs);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints"), noOfPoints);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), 4);
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Name_Backup"), rivernames);
                            pFeature.Store();
                        }
                        else
                        {
                            pFeature.set_Value(Data.featurePntLyrCS.Fields.FindField("Processed"), -1);
                            pFeature.Store();
                        }




                    }
                    Marshal.ReleaseComObject(pCursor3);
                }

                Marshal.ReleaseComObject(pCursor2);


                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
            MessageBox.Show("Finished!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to rematch highway bridges now?", "Rematch Highway Bridges ", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            rematchWithGC();
            rematchWithATIP();
                
        }
        private void rematchWithATIP()
        {
            IFeatureClass atip = Data.pFWS.OpenFeatureClass("ATIP");
            int rrownerIndex = Data.featurePntLyrCS.Fields.FindField("RROwner");
            int uniqueIDIndex = Data.featurePntLyrCS.Fields.FindField("UniqueID");
            int statusCodeIndex = Data.featurePntLyrCS.Fields.FindField("Status");

            
            //Loop this point layer
            ITopologicalOperator pTopoOpt = null;
            ISpatialFilter pSF = null;
            int nodeCount = Data.featurePntLyrCS.FeatureCount(null);
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(null, false);
            IFeature pFeatureP = pCursor.NextFeature();
            if (pFeatureP == null)
            {
                MessageBox.Show("No point features found. ");
            }
            while (pFeatureP != null)
            {
                IPoint node = pFeatureP.Shape as IPoint;
                pTopoOpt = (ITopologicalOperator)node;
                double bufferRange = bufferRange3;
                IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
                pSF = new SpatialFilter();
                pSF.Geometry = pPoly;
                pSF.GeometryField = "Shape";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pFeaCur = atip.Search(pSF, false);

                
                //int ATIPIdIndex = pSourcePointLayerCS.Fields.FindField("ATIP_ID");

                IFeature pFeatureL = pFeaCur.NextFeature();

                Boolean foundATIP = false;
                Boolean foundMatchedATIP = false;
                string ATIP_Id = "";

                while (pFeatureL != null)
                {
                    String RwyNameSource = pFeatureP.get_Value(rrownerIndex).ToString().Trim();
                    String RwyNameTarget = pFeatureL.get_Value(atip.Fields.FindField("RROWNER")).ToString().Trim();
                    String TargetID = pFeatureL.get_Value(atip.Fields.FindField("ID")).ToString().Trim();
                    foundATIP = true;
                    ATIP_Id = TargetID;
                    if (RwyNameSource == RwyNameTarget)
                    {
                        foundMatchedATIP = true;
                        break;
                    }

                    pFeatureL = pFeaCur.NextFeature();
                }
                Marshal.ReleaseComObject(pFeaCur);

                if (!foundMatchedATIP)
                {
                    pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
                    pSF = new SpatialFilter();
                    pSF.Geometry = pPoly;
                    pSF.GeometryField = "Shape";
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pFeaCur2 = atip.Search(pSF, false);
                    IFeature pFeatureL2 = pFeaCur2.NextFeature();

                    while (pFeatureL2 != null)
                    {
                        String RwyNameSource = pFeatureP.get_Value(rrownerIndex).ToString().Trim();
                        String RwyNameTarget = pFeatureL2.get_Value(atip.Fields.FindField("RROWNER")).ToString().Trim();
                        String TargetID = pFeatureL2.get_Value(atip.Fields.FindField("ID")).ToString().Trim();
                        

                        foundATIP = true;
                        ATIP_Id = TargetID;
                        if (RwyNameSource == RwyNameTarget)
                        {
                            foundMatchedATIP = true;
                            break;
                        }

                        pFeatureL2 = pFeaCur2.NextFeature();
                    }
                    Marshal.ReleaseComObject(pFeaCur2);
                }

                if (foundMatchedATIP)
                {
                    pFeatureP.set_Value(statusCodeIndex, 5);
                   // pFeatureP.set_Value(ATIPIdIndex, ATIP_Id);
                    pFeatureP.Store();
                }
                else if (foundATIP)
                {
                    pFeatureP.set_Value(statusCodeIndex, 4);
                    //pFeatureP.set_Value(ATIPIdIndex, ATIP_Id);
                    pFeatureP.Store();
                }
                else
                {
                    pFeatureP.set_Value(statusCodeIndex, -1);
                    pFeatureP.Store();
                }


                //MessageBox.Show(roadName);

                pFeatureP = pCursor.NextFeature();
                this.Refresh();
            }

            MessageBox.Show("Matching Done! ");

        }
        private void rematchWithGC()
        {
            IFeatureClass gc_bridges = Data.pFWS.OpenFeatureClass("GC_Bridges");
                       
            int nameIndex = Data.featurePntLyrCS.Fields.FindField("Name");
            int secondarySTNamesIndex = Data.featurePntLyrCS.Fields.FindField("Secondary_StreetNames");
            int typeIndex = Data.featurePntLyrCS.Fields.FindField("Type");
            int rrownerIndex = Data.featurePntLyrCS.Fields.FindField("RROwner");
            int uniqueIDIndex = Data.featurePntLyrCS.Fields.FindField("UniqueID");
            int noofPointsIndex = Data.featurePntLyrCS.Fields.FindField("NoOfPoints");
            int statusCodeIndex = Data.featurePntLyrCS.Fields.FindField("Bridge_Type");
            int crossingIDIndex = Data.featurePntLyrCS.Fields.FindField("GradeCross_ID");
            int secondaryStreenameIndex = Data.featurePntLyrCS.Fields.FindField("Secondary_Name");

            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "UniqueID Like 'R*'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            string name = "";
            string secondarySTName = "";
            string RoadNameSource = "";
            while (pFeature != null)
            {
                name = pFeature.get_Value(nameIndex).ToString();
                secondarySTName = pFeature.get_Value(secondarySTNamesIndex).ToString();

                if (name != "None")
                {
                    RoadNameSource = name;
                }
                if (secondarySTName != "None")
                {
                    RoadNameSource = name + secondarySTName;
                }

                ITopologicalOperator pTopoOpt = null;
                ISpatialFilter pSF = null;
                IPoint node = pFeature.Shape as IPoint;
                pTopoOpt = (ITopologicalOperator)node;
                //double bufferRange = bufferRange3;
                IPolygon pPoly = pTopoOpt.Buffer(bufferRange3) as IPolygon;
                pSF = new SpatialFilter();
                pSF.Geometry = pPoly;
                pSF.GeometryField = "Shape";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pFeaCur = gc_bridges.Search(pSF, false);


                IFeature pFeatureL = pFeaCur.NextFeature();

                int matchcode = -1;
                string GC_ID = null;
                string GC_Streetnames = null;


                List<string> List_GC = new List<string>();
                List<int> List_MatchCode = new List<int>();
                List<string> List_GCStreetnames = new List<string>();

                String RwyNameSource = pFeature.get_Value(rrownerIndex).ToString().Trim();
                String SourceID = pFeature.get_Value(uniqueIDIndex).ToString();
                int noOfPoints = Convert.ToInt32(pFeature.get_Value(noofPointsIndex).ToString());

                while (pFeatureL != null)
                {
                    String RoadNameTarget = pFeatureL.get_Value(gc_bridges.Fields.FindField("Street")).ToString();
                    String RwyNameTarget = pFeatureL.get_Value(gc_bridges.Fields.FindField("Railroad")).ToString().Trim();
                    String TargetID = pFeatureL.get_Value(gc_bridges.Fields.FindField("CrossingID")).ToString().Trim();
                    
                    if (RwyNameSource == RwyNameTarget)
                    {
                        int match = roadNamesMatchfunction(RoadNameSource, RoadNameTarget, noOfPoints);
                        List_MatchCode.Add(match);
                        List_GC.Add(TargetID);
                        List_GCStreetnames.Add(RoadNameTarget);
                    }
                    pFeatureL = pFeaCur.NextFeature();
                }
                Marshal.ReleaseComObject(pFeaCur);

                if (List_MatchCode.Count == 0)
                {
                    pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
                    pSF = new SpatialFilter();
                    pSF.Geometry = pPoly;
                    pSF.GeometryField = "Shape";
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pFeaCur2 = gc_bridges.Search(pSF, false);
                    IFeature pFeatureL2 = pFeaCur2.NextFeature();

                    while (pFeatureL2 != null)
                    {

                        String RoadNameTarget = pFeatureL2.get_Value(gc_bridges.Fields.FindField("Street")).ToString();
                        String RwyNameTarget = pFeatureL2.get_Value(gc_bridges.Fields.FindField("Railroad")).ToString().Trim();
                        String TargetID = pFeatureL2.get_Value(gc_bridges.Fields.FindField("CrossingID")).ToString().Trim();
                        //IPoint targetP = pFeatureL.Shape as IPoint;

                        if (RwyNameSource == RwyNameTarget)
                        {
                            int match = roadNamesMatchfunction(RoadNameSource, RoadNameTarget, noOfPoints);
                            List_MatchCode.Add(match);
                            List_GC.Add(TargetID);
                            List_GCStreetnames.Add(RoadNameTarget);

                        }
                        pFeatureL2 = pFeaCur2.NextFeature();
                    }
                    Marshal.ReleaseComObject(pFeaCur2);
                }

               
                if (List_MatchCode.Count > 0)
                {
                    matchcode = List_MatchCode.Max();
                    GC_ID = List_GC.ElementAt(List_MatchCode.IndexOf(matchcode));
                    GC_Streetnames = List_GCStreetnames.ElementAt(List_MatchCode.IndexOf(matchcode));
                    pFeature.set_Value(statusCodeIndex, matchcode);
                    pFeature.set_Value(crossingIDIndex, GC_ID);
                    pFeature.set_Value(secondaryStreenameIndex, GC_Streetnames);
                    pFeature.Store();

                }
                else
                {
                    pFeature.set_Value(statusCodeIndex, -1);
                    pFeature.Store();

                }


                pFeature = pCursor.NextFeature();
                this.Refresh();
            }

            Marshal.ReleaseComObject(pCursor);
        }

        private int roadNamesMatchfunction(String sourceRoadNames, String targetRoadName, int noOfPoints)
        {
            int matchStatus = -1;
            string[] targetNames = targetRoadName.Split('/');
            if (targetRoadName.ToUpper().Contains("&"))
            {
                targetNames = targetRoadName.Split('&');

            }

            if (targetNames.Count() > 1)
            {

                int[] matchStatusArr = new Int32[noOfPoints * targetNames.Count() + 1]; // kevin added  + 1 since the array index is out of range
                for (int j = 0; j < targetNames.Count(); j++)
                {
                    if (noOfPoints > 1)
                    {
                        string[] sourceNames = sourceRoadNames.Split(';');

                        for (int i = 0; i < sourceNames.Count() - 1; i++)
                        {
                            matchStatusArr[j * noOfPoints + i] = roadNameMatchfunction(sourceNames[i], targetNames[j]);
                        }

                    }
                    else
                    {
                        matchStatusArr[j] = roadNameMatchfunction(sourceRoadNames, targetNames[j]);
                    }
                }
                matchStatus = matchStatusArr.Max();
            }
            else
            {
                if (noOfPoints > 1)
                {
                    string[] sourceNames = sourceRoadNames.Split(';');
                    int[] matchStatusArr = new Int32[sourceNames.Count()];
                    for (int i = 0; i < sourceNames.Count() - 1; i++)
                    {
                        matchStatusArr[i] = roadNameMatchfunction(sourceNames[i], targetRoadName);
                    }

                    matchStatus = matchStatusArr.Max();


                }
                else
                {
                    matchStatus = roadNameMatchfunction(sourceRoadNames, targetRoadName);
                }
            }


            return matchStatus;

        }
        public int roadNameMatchfunction(String sourceRoadName, String targetRoadName)
        {
            /*
             Match status:
             * 0 : do not match at all;
             * 1: either roadName is null
             * 2 : roadName contains
             * 3 : Only road Name matched
             * 4 : Road Name + one more part
             * 5 : Fully matched
             */


            int matchStatus = 0;
            bool matchFlag1 = false;
            bool matchFlag2 = false;
            bool matchFlag3 = false;



            if ((sourceRoadName.ToUpper() == targetRoadName.ToUpper()) || (sourceRoadName.ToUpper().Replace(" ", "") == targetRoadName.ToUpper().Replace(" ", "")))
            {
                matchStatus = 5;
                return matchStatus;
            }
            else
            {
                sourceRoadName = sourceRoadName.ToUpper().Replace(".", "");
                targetRoadName = targetRoadName.ToUpper().Replace(".", "");
                //sourceRoadName = sourceRoadName.ToUpper().Replace(" ", "");
                //targetRoadName = targetRoadName.ToUpper().Replace(" ", "");
                string SourceDirectStr = null;
                string targetDirectStr = null;

                if (sourceRoadName == null || sourceRoadName == "" || sourceRoadName == " " || targetRoadName == null || targetRoadName == "" || targetRoadName == " ")
                {
                    matchStatus = 1;
                    return matchStatus;
                }

                string[] sourceParts = sourceRoadName.Split(' ');
                string[] targetParts = targetRoadName.Split(' ');
                int sourcePartsLength = sourceParts.Length;
                int targetPartsLength = targetParts.Length;
                //1. get the last part of it
                string lastSourcePart = sourceParts[sourcePartsLength - 1];

                //if direction suffix appears, get rid of it;
                foreach (string directStrList in directSuffixList)
                {
                    if (lastSourcePart == directStrList)
                    {
                        if (sourcePartsLength >= 2)
                        {
                            SourceDirectStr = lastSourcePart;
                            lastSourcePart = sourceParts[sourcePartsLength - 2];
                            break;
                        }
                        else
                        {
                            lastSourcePart = null;
                        }

                    }
                }
                bool sourcesuffixmatchFlag = false;
                foreach (KeyValuePair<string, string> entry in suffixList)
                {
                    // do something with entry.Value or entry.Key
                    if (lastSourcePart == (entry.Key).ToUpper())
                    {
                        //this is suffix.
                        sourceRoadName = sourceRoadName.Replace(lastSourcePart, entry.Value);
                        lastSourcePart = entry.Value.ToUpper();
                        sourcesuffixmatchFlag = true;
                        break;
                    }
                    else if (lastSourcePart == (entry.Value).ToUpper())
                    {
                        //this is suffix.
                        //sourceRoadName = sourceRoadName.Replace(lastSourcePart, entry.Value);
                        //lastSourcePart = entry.Value;
                        sourcesuffixmatchFlag = true;
                        break;
                    }
                }
                if (sourcesuffixmatchFlag == false)
                {
                    lastSourcePart = null;
                }

                string lastTargetPart = targetParts[targetPartsLength - 1];
                //if direction suffix appears, get rid of it;
                foreach (string directStrList in directSuffixList)
                {
                    if (lastTargetPart == directStrList)
                    {
                        if (targetPartsLength >= 2)
                        {
                            targetDirectStr = lastTargetPart;
                            lastTargetPart = targetParts[targetPartsLength - 2];
                            break;
                        }
                        else
                        {
                            lastTargetPart = null;
                        }

                    }
                }
                bool targetsuffixmatchFlag = false;
                foreach (KeyValuePair<string, string> entry in suffixList)
                {
                    // do something with entry.Value or entry.Key
                    if (lastTargetPart == (entry.Key).ToUpper())
                    {
                        //this is suffix.
                        targetRoadName = targetRoadName.Replace(lastTargetPart, entry.Value);
                        lastTargetPart = entry.Value.ToUpper();
                        targetsuffixmatchFlag = true;
                        break;
                    }
                    else if (lastTargetPart == (entry.Value).ToUpper())
                    {
                        //this is suffix.
                        //sourceRoadName = sourceRoadName.Replace(lastSourcePart, entry.Value);
                        //lastSourcePart = entry.Value;
                        targetsuffixmatchFlag = true;
                        break;
                    }

                }
                if (targetsuffixmatchFlag == false)
                {
                    lastTargetPart = null;
                }
                /*
                foreach (string suffix in suffixList)
                {

                }
                 * */
                //2. Get prefix
                string firstSourcePart = sourceParts[0];
                bool sourcePrefixFlag = false;
                foreach (string prefix in prefixList)
                {
                    if (firstSourcePart == prefix)
                    {
                        sourcePrefixFlag = true;

                        break;
                        // this is prefix
                    }
                }
                if (sourcePrefixFlag == false)
                {
                    firstSourcePart = null;
                }
                else
                {
                    foreach (KeyValuePair<string, string> entry in suffixList)
                    {
                        // do something with entry.Value or entry.Key
                        if (firstSourcePart == (entry.Key).ToUpper())
                        {
                            //this is suffix.
                            sourceRoadName = sourceRoadName.Replace(firstSourcePart, entry.Value);
                            firstSourcePart = entry.Value.ToUpper();
                            break;
                        }

                    }
                }

                string firstTargetPart = targetParts[0];
                bool targetPrefixFlag = false;
                foreach (string prefix in prefixList)
                {
                    if (firstTargetPart == prefix)
                    {
                        targetPrefixFlag = true;
                        break;
                        // this is prefix
                    }
                }

                if (targetPrefixFlag == false)
                {
                    firstTargetPart = null;
                }
                else
                {
                    foreach (KeyValuePair<string, string> entry in suffixList)
                    {
                        // do something with entry.Value or entry.Key
                        if (firstTargetPart == (entry.Key).ToUpper())
                        {
                            //this is suffix.
                            targetRoadName = targetRoadName.Replace(firstTargetPart, entry.Value);
                            firstTargetPart = entry.Value.ToUpper();
                            break;
                        }

                    }
                }

                if (targetRoadName.ToUpper() == sourceRoadName.ToUpper())
                {
                    matchStatus = 5;
                    return matchStatus;
                }
                else
                {
                    //3. Get the road name.
                    //get source road name
                    // MessageBox.Show(firstSourcePart + ":" + lastSourcePart);
                    String finalSourceRoadName = null;
                    String finalTargetRoadName = null;
                    if (SourceDirectStr == null || SourceDirectStr == "")
                    {
                        // do nothing
                    }
                    else
                    {
                        lastSourcePart = lastSourcePart + " " + SourceDirectStr;
                    }
                    if ((firstSourcePart == null || firstSourcePart == "") && (lastSourcePart == null || lastSourcePart == ""))
                    {
                        finalSourceRoadName = sourceRoadName.Trim().Replace(" ", "");
                    }
                    else if (firstSourcePart == null || firstSourcePart == "")
                    {
                        finalSourceRoadName = sourceRoadName.Replace(lastSourcePart, "").Trim().Replace(" ", "");
                    }
                    else if (lastSourcePart == null || lastSourcePart == "")
                    {
                        if (sourcePartsLength == 1)
                        {
                            finalSourceRoadName = sourceRoadName.Trim().Replace(" ", "");
                        }
                        else
                        {
                            Regex rgx = new Regex(firstSourcePart);
                            finalSourceRoadName = rgx.Replace(sourceRoadName, "", 1).Trim().Replace(" ", "");
                        }

                    }
                    else
                    {
                        if (sourcePartsLength == 2)
                        {
                            finalSourceRoadName = sourceRoadName.Replace(lastSourcePart, "").Trim().Replace(" ", "");
                        }
                        else
                        {
                            Regex rgx = new Regex(firstSourcePart);
                            finalSourceRoadName = rgx.Replace(sourceRoadName, "", 1).Replace(lastSourcePart, "").Trim().Replace(" ", "");
                        }

                    }
                    //============================================================================================================================
                    //get target road name
                    if (targetDirectStr == null || targetDirectStr == "")
                    {
                        // do nothing
                    }
                    else
                    {
                        lastTargetPart = lastTargetPart + " " + targetDirectStr;
                    }
                    if ((firstTargetPart == null || firstTargetPart == "") && (lastTargetPart == null || lastTargetPart == ""))
                    {
                        finalTargetRoadName = targetRoadName.Trim().Replace(" ", "");
                    }
                    else if (firstTargetPart == null || firstTargetPart == "")
                    {
                        finalTargetRoadName = targetRoadName.Replace(lastTargetPart, "").Trim().Replace(" ", "");
                    }
                    else if (lastTargetPart == null || lastTargetPart == "")
                    {
                        if (targetPartsLength == 1)
                        {
                            finalTargetRoadName = targetRoadName.Trim().Replace(" ", "");
                        }
                        else
                        {
                            Regex rgx = new Regex(firstTargetPart);
                            finalTargetRoadName = rgx.Replace(targetRoadName, "", 1).Trim().Replace(" ", "");
                        }

                    }
                    else
                    {
                        if (targetPartsLength == 2)
                        {
                            finalTargetRoadName = targetRoadName.Replace(lastTargetPart, "").Trim().Replace(" ", "");
                        }
                        else
                        {
                            Regex rgx = new Regex(firstTargetPart);
                            finalTargetRoadName = rgx.Replace(targetRoadName, "", 1).Replace(lastTargetPart, "").Trim().Replace(" ", "");
                        }

                    }

                    bool contain = finalSourceRoadName.Contains(finalTargetRoadName);
                    bool contain2 = finalTargetRoadName.Contains(finalSourceRoadName);
                    //4. Match if they are same
                    if (finalSourceRoadName == finalTargetRoadName)
                    {
                        matchFlag1 = true;
                    }
                    if (firstSourcePart == firstTargetPart)
                    {
                        matchFlag2 = true;
                    }
                    if (lastSourcePart == lastTargetPart)
                    {
                        matchFlag3 = true;
                    }
                    if (matchFlag1 == true && matchFlag2 == true && matchFlag3 == true)
                    {
                        matchStatus = 5;
                    }
                    else if ((matchFlag1 == true && matchFlag2 == true) || (matchFlag1 == true && matchFlag3 == true))
                    {
                        matchStatus = 4;
                    }
                    else if ((contain == true || contain2 == true) && matchFlag3 == true)
                    {
                        matchStatus = 4;
                    }
                    else if (matchFlag1 == true)
                    {
                        matchStatus = 3;
                    }
                    else if (contain == true || contain2 == true)
                    {
                        matchStatus = 2;
                    }

                    return matchStatus;
                }
            }




        }
    }
}
