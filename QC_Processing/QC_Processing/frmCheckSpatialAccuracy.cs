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
    public partial class frmCheckSpatialAccuracy : Form
    {
        private int indexSelected = -1;
        private string selectedTargetID = "";
        private IApplication m_application;
        private IMap m_map;
        private IMxDocument m_mxDoc;

        static double bufferRange1 = 0.00045;  //50m buffer
        private string operatorName = "";
        private bool non_grouped = false;

        public frmCheckSpatialAccuracy()
        {
            InitializeComponent();
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

        private void hideBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void clearQCView()
        {
            QCReportListView.Enabled = false;
            QCReportListView.Items.Clear();

            legalRdo.Checked = false;
            legalRdo.Enabled = false;
            createBridgeBtn.Enabled = false;

            NextBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            refreshBtn.Enabled = false;

            nonBridgeRdo.Enabled = false;
            nonBridgeRdo.Checked = false;
            latLonTxt.Text = "";
            latLonTxt.Enabled = false;
            txtBridgeUniqueID.Enabled = false;
            btnSearch.Enabled = false;

            ErrorNoTxt.Text = "0 features in total";



        }

        private void CheckSpatialAccuracyBtn_Click(object sender, EventArgs e)
        {
            operatorName = Environment.UserName.ToString();
            IQueryFilter pQF = new QueryFilter();
            int count = -1;
            ErrorNoTxt.Text = "0 features in total";
            if (Data.SpatialAccuracyLyrCS == null)
            {
                MessageBox.Show("SpatialAccuracy_Report does not exist or is not properly set");
                return;
            }
            else
            {
                count = Data.SpatialAccuracyLyrCS.FeatureCount(null);
                if (count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("SpatialAccuracy_Report table is not empty. Do you want to rewrite that table?", "QC", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        emptyQCTable(Data.SpatialAccuracyLyrCS);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                pQF.WhereClause = "[Type] NOT IN( 'N' , 'undefined' , 'non-bridge')";
                checkSpatialAccuracy(pQF, operatorName);
                clearQCView();
            }

            MessageBox.Show("Done");
        }

        private void write2QCReport(IPoint point, string flag_code, double location_x, double location_y, string sourceOID, string targetId, string Operator, string status)
        {

            DateTime now = DateTime.Now;
            IFeature feature = Data.SpatialAccuracyLyrCS.CreateFeature();
            feature.Shape = point;
            feature.set_Value(feature.Fields.FindField("Flag_Code"), flag_code);
            feature.set_Value(feature.Fields.FindField("Location_X"), location_x);
            feature.set_Value(feature.Fields.FindField("Location_Y"), location_y);
            feature.set_Value(feature.Fields.FindField("TargetFeatureID"), targetId);
            feature.set_Value(feature.Fields.FindField("SourceFeatureID"), sourceOID);
            feature.set_Value(feature.Fields.FindField("Status"), status);
            feature.set_Value(feature.Fields.FindField("Operator"), Operator);
            feature.set_Value(feature.Fields.FindField("Create_Date"), now);
            feature.Store();
            Marshal.ReleaseComObject(feature);
        }


 private void checkSpatialAccuracy(IQueryFilter pQF, string operatorName)
        {
            string subCategory = "Spatially Inaccurate";


            //Check bridges above/below roads against road intersection points, bridges over waterway against water intersection points
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();


                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = nodeOfInterest;
                pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                if (targetID.Contains("W"))
                {
                    if (Data.waterIntersectionPntLyrCS == null)
                    {
                        MessageBox.Show("Water_IntersectionPoints does not exist or is not properly set");
                        return;
                    }
                    IFeatureCursor pCursor2 = Data.waterIntersectionPntLyrCS.Search(pSF, false);
                    IFeature pFeature2 = pCursor2.NextFeature();

                    //IEnvelope envelope = new EnvelopeClass();
                    //envelope.PutCoords();
                    bool found = false;
                    if (pFeature2 != null)
                    {
                        found = true;
                    }

                    if (!found)
                    {
                        write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, operatorName, "Unresolved");

                    }

                    Marshal.ReleaseComObject(pCursor2);

                }
                else
                {
                    IFeatureCursor pCursor2 = Data.intersectionPntLyrCS.Search(pSF, false);
                    IFeature pFeature2 = pCursor2.NextFeature();
                    bool found = false;
                    if (pFeature2 != null)
                    {
                        found = true;
                    }

                    if (!found)
                    {
                        write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, operatorName, "Unresolved");

                    }

                    Marshal.ReleaseComObject(pCursor2);

                }

                pFeature = pCursor1.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor1);


        }


        private void emptyQCTable(IFeatureClass qcTable)
        {
            IFeatureCursor pCursor = qcTable.Update(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                pCursor.DeleteFeature();
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
        }

        private void LoadSpatialAccuracyReportBtn_Click(object sender, EventArgs e)
        {
            QCReportListView.Items.Clear();
            QCReportListView.Enabled = false;
            legalRdo.Enabled = false;
            nonBridgeRdo.Enabled = false;
            legalRdo.Checked = false;
            nonBridgeRdo.Checked = false;

            refreshBtn.Enabled = false;
            createBridgeBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;

            ErrorNoTxt.Text = "0 features in total";

            int count = -1;
            count = Data.SpatialAccuracyLyrCS.FeatureCount(null);
            //selectedQCTable = Data.SpatialAccuracyLyrCS;
            if (count == 0)
            {
                MessageBox.Show("SpatialAccuracy_Report table is empty.");
                return;
            }

            QCReportListView.Enabled = true;
            btnSearch.Enabled = true;
            txtBridgeUniqueID.Enabled = true;
            IQueryFilter pQF = new QueryFilter();
            loadData(pQF);

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
                IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

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
                legalRdo.Enabled = true;
                legalRdo.Checked = false;
                createBridgeBtn.Enabled = true;
                nonBridgeRdo.Checked = false;
                nonBridgeRdo.Enabled = true;
            }
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
                IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
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
                legalRdo.Enabled = true;
                legalRdo.Checked = false;
                createBridgeBtn.Enabled = true;
                nonBridgeRdo.Checked = false;
                nonBridgeRdo.Enabled = true;
            }
        }
        private void loadData(IQueryFilter pQF)
        {
            ITableSort tableSort = new TableSort();
            tableSort.Table = (ITable)Data.SpatialAccuracyLyrCS;
            tableSort.QueryFilter = pQF;
            tableSort.Fields = "TargetFeatureID";
            tableSort.set_Ascending("TargetFeatureID", true);
            tableSort.Sort(null);

            ICursor sortedCursor = tableSort.Rows;
            IRow pFeature = sortedCursor.NextRow();

            int num = 0;

            int indexUniqueID = Data.SpatialAccuracyLyrCS.Fields.FindField("TargetFeatureID");
            int indexStatus = Data.SpatialAccuracyLyrCS.Fields.FindField("Status");
            int indexFlagcode = Data.SpatialAccuracyLyrCS.Fields.FindField("Flag_Code");
            int indexOperator = Data.SpatialAccuracyLyrCS.Fields.FindField("Operator");

            while (pFeature != null)

            {
                String targetFeatureID = pFeature.get_Value(indexUniqueID).ToString();
                String status = pFeature.get_Value(indexStatus).ToString().Trim();
                String Operator = pFeature.get_Value(indexOperator).ToString();

                string[] row = { targetFeatureID, status, Operator };
                QCReportListView.Items.Add(new ListViewItem(row));

                num = num + 1;
                pFeature = sortedCursor.NextRow();
            }

            Marshal.ReleaseComObject(sortedCursor);

            ErrorNoTxt.Text = num.ToString() + " features in total";
        }
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            QCReportListView.Items.Clear();
            legalRdo.Enabled = false;
            legalRdo.Checked = false;
            createBridgeBtn.Enabled = false;
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;
            latLonTxt.Text = "";

            QCReportListView.Enabled = true;
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[Status] = 'Unresolved'";

            loadData(pQF);


            refreshBtn.Enabled = false;
            nonBridgeRdo.Checked = false;
            nonBridgeRdo.Enabled = false;
        }


        private void createNewBridge(IPoint newPoint, string rrowner, string streetnames, string objectIds, int NoOfPoints, string type)
        {
            int layerIndex = -1;
            IFeatureClass selectedIntersectionPointLyr = Data.intersectionPntLyrCS;
            IFeatureClass selectedTargetPointLyr = Data.groupedTargetPntLyrCS;
            string[] bridgeTypeAttr = {};

            if (type == "RN")
            { 
                layerIndex = 0;
                bridgeTypeAttr = CopyFromRelevantBridge(layerIndex, selectedIntersectionPointLyr, selectedTargetPointLyr, type);
               }
            else if (type == "WN")
            {
                layerIndex = 1;
                selectedIntersectionPointLyr = Data.waterIntersectionPntLyrCS;
                selectedTargetPointLyr = Data.waterGroupedTargetPntLyrCS;
                bridgeTypeAttr = CopyFromRelevantBridge(layerIndex, selectedIntersectionPointLyr, selectedTargetPointLyr, type);
            }
            else if (type == "NRR")
            {
                layerIndex = 1;
                selectedIntersectionPointLyr = Data.featurePntLyrCS;
                bridgeTypeAttr = CopyFromRelevantRRBridge(layerIndex, selectedIntersectionPointLyr, type);
            }

            IFeature feature = Data.featurePntLyrCS.CreateFeature();
            feature.Shape = newPoint;
            feature.set_Value(feature.Fields.FindField("UniqueID"), type + feature.OID.ToString());

            feature.set_Value(feature.Fields.FindField("RROwner"), rrowner);
            feature.set_Value(feature.Fields.FindField("Name_Backup"), streetnames);
            feature.set_Value(feature.Fields.FindField("Bridge_Type"), bridgeTypeAttr[0]);
            feature.set_Value(feature.Fields.FindField("Design_Type"), bridgeTypeAttr[1]);
            feature.set_Value(feature.Fields.FindField("Cross_Type"), bridgeTypeAttr[2]);
            feature.set_Value(feature.Fields.FindField("GradeCross_ID"), bridgeTypeAttr[3]);
            feature.set_Value(feature.Fields.FindField("Secondary_Name"), bridgeTypeAttr[4]);
            feature.set_Value(feature.Fields.FindField("PointIDs"), objectIds);
            feature.set_Value(feature.Fields.FindField("NoOfPoints"), NoOfPoints);
            if (bridgeTypeAttr[5] == "auto-conflated")
                feature.set_Value(feature.Fields.FindField("Type"), bridgeTypeAttr[5]);
            else
                feature.set_Value(feature.Fields.FindField("Type"), "Y");
            if (!(bridgeTypeAttr[0] == null || bridgeTypeAttr[1] == null || bridgeTypeAttr[2] == null))
            {
                feature.set_Value(feature.Fields.FindField("Checked"), "Y");
            }
                
            feature.set_Value(feature.Fields.FindField("StreetView"), bridgeTypeAttr[6]);
            feature.set_Value(feature.Fields.FindField("CheckBridge"), bridgeTypeAttr[7]);

            feature.Store();
            Marshal.ReleaseComObject(feature);


        }

        private string[] CopyFromRelevantRRBridge(int layerIndex, IFeatureClass intersectionPointLyr, string bridgeType)
        {
            string[] attrArr = new string[8];
            IFeatureSelection pFeatSel = (IFeatureSelection)m_map.Layer[layerIndex];
            ISelectionSet pSelSet = pFeatSel.SelectionSet;
            ICursor pCursor;
            pSelSet.Search(null, false, out pCursor);
            IFeature pFeature = (IFeature)pCursor.NextRow();
            bool found = false;

            string bridge_type = null;
            string design_type = null;
            string cross_type = null;
            string secondary_Name = null;
            string gradeCrossing_Id = null;
            string type = null;
            string streetview = null;

            while (pFeature != null)
            {
                string pointId = pFeature.get_Value(intersectionPointLyr.Fields.FindField("UniqueID")).ToString();

                found = true;
                bridge_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString();
                design_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                cross_type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();
                gradeCrossing_Id = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("GradeCross_ID")).ToString();
                secondary_Name = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Secondary_Name")).ToString();
                type = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("Type")).ToString();
                streetview = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("StreetView")).ToString();
                string bridgeID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();

                if (bridge_type == "")
                {
                    attrArr[0] = null;
                }
                else
                {
                    attrArr[0] = bridge_type;
                }

                if (design_type == "")
                {
                    attrArr[1] = null;
                }
                else
                {
                    attrArr[1] = design_type;
                }

                if (cross_type == "")
                {
                    attrArr[2] = null;
                }
                else
                {
                    attrArr[2] = cross_type;
                }

                if (gradeCrossing_Id == "")
                {
                    attrArr[3] = null;
                }
                else
                {
                    attrArr[3] = gradeCrossing_Id;
                }

                if (secondary_Name == "")
                {
                    attrArr[4] = null;
                }
                else
                {
                    attrArr[4] = secondary_Name;
                }

                if (type == "")
                {
                    attrArr[5] = null;
                }
                else
                {
                    attrArr[5] = type;
                }

                if (streetview == "")
                {
                    attrArr[6] = null;
                }
                else
                {
                    attrArr[6] = streetview;
                }
                if (bridgeID == "")
                {
                    attrArr[7] = null;
                }
                else
                {
                    attrArr[7] = bridgeID;
                }

                pFeature = (IFeature)pCursor.NextRow();

            }

            Marshal.ReleaseComObject(pCursor);

            if (found == false)
            {
                MessageBox.Show("Cannot find the related bridge to copy attributes. Please assign bridge type manually later");
            }

            return attrArr;
        }

        //Copy bridge_type, design_type, cross_type, crossing_id, secondary_streetname from the grouped bridge 
        private string[] CopyFromRelevantBridge(int layerIndex, IFeatureClass intersectionPointLyr, IFeatureClass targetPointLyr, string bridgeType)
        {
            string[] attrArr = new string[8];
            IFeatureSelection pFeatSel = (IFeatureSelection)m_map.Layer[layerIndex];
            ISelectionSet pSelSet = pFeatSel.SelectionSet;
            ICursor pCursor;
            pSelSet.Search(null, false, out pCursor);
            IFeature pFeature = (IFeature)pCursor.NextRow();
            bool found = false;

            string bridge_type = null;
            string design_type = null;
            string cross_type = null;
            string secondary_Name = null;
            string gradeCrossing_Id = null;
            string type = null;
            string streetview = null;

            string where = "";
            if (bridgeType.Contains("R"))
            {
                where = "[UniqueID] LIKE 'R*'";
            }
            else
            {
                where = "[UniqueID] LIKE 'W*'";
            }
            while (pFeature != null)
            {
                string pointId = pFeature.get_Value(intersectionPointLyr.Fields.FindField("OBJECTID")).ToString();
                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                ITopologicalOperator pTopoOpt = null;
                pTopoOpt = (ITopologicalOperator)nodeOfInterest;
                IPolygon pPoly = pTopoOpt.Buffer(bufferRange1) as IPolygon;
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = pPoly;
                pSF.WhereClause = where;
                pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor2 = Data.featurePntLyrCS.Search(pSF, false);
                IFeature pFeature2 = pCursor2.NextFeature();

                while (pFeature2 != null)
                {
                    string pointIDs = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("PointIDs")).ToString();
                    if (pointIDs != "")
                    {
                        int numPoints = Convert.ToInt32(pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("NoOfPoints")).ToString());
                        string targetID = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();
                        IQueryFilter pQF = new QueryFilter();

                        string[] pointObjIds = pointIDs.Split(';');
                        if (pointObjIds.Contains(pointId))
                        {
                            found = true;
                            bridge_type = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("Bridge_Type")).ToString();
                            design_type = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("Design_Type")).ToString();
                            cross_type = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("Cross_Type")).ToString();
                            gradeCrossing_Id = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("GradeCross_ID")).ToString();
                            secondary_Name = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("Secondary_Name")).ToString();
                            type = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("Type")).ToString();
                            streetview = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("StreetView")).ToString();
                            string bridgeID = pFeature2.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();

                            if (bridge_type == "")
                            {
                                attrArr[0] = null;
                            }
                            else
                            {
                                attrArr[0] = bridge_type;
                            }

                            if (design_type == "")
                            {
                                attrArr[1] = null;
                            }
                            else
                            {
                                attrArr[1] = design_type;
                            }

                            if (cross_type == "")
                            {
                                attrArr[2] = null;
                            }
                            else
                            {
                                attrArr[2] = cross_type;
                            }

                            if (gradeCrossing_Id == "")
                            {
                                attrArr[3] = null;
                            }
                            else
                            {
                                attrArr[3] = gradeCrossing_Id;
                            }

                            if (secondary_Name == "")
                            {
                                attrArr[4] = null;
                            }
                            else
                            {
                                attrArr[4] = secondary_Name;
                            }

                            if (type == "")
                            {
                                attrArr[5] = null;
                            }
                            else
                            {
                                attrArr[5] = type;
                            }

                            if (streetview == "")
                            {
                                attrArr[6] = null;
                            }
                            else
                            {
                                attrArr[6] = streetview;
                            }
                            if (bridgeID == "")
                            {
                                attrArr[7] = null;
                            }
                            else
                            {
                                attrArr[7] = bridgeID;
                            }
                            break;
                        }

                    }
                    pFeature2 = pCursor2.NextFeature();

                }

                Marshal.ReleaseComObject(pCursor2);

                pFeature = (IFeature)pCursor.NextRow();

            }

            Marshal.ReleaseComObject(pCursor);

            if (found == false)
            {
                MessageBox.Show("Cannot find the related bridge to copy attributes. Please assign bridge type manually later");
            }

            return attrArr;
        }

        

        private Boolean checkPointExistence(IPointCollection points, IPoint point)
        {
            Boolean exist = false;
            IPoint oldPoint;
            for (int i = 0; i < points.PointCount; i++)
            {
                oldPoint = points.Point[i];
                if (oldPoint.X == point.X && oldPoint.Y == point.Y)
                {
                    exist = true;
                    return exist;
                }

            }


            return exist;
        }
        private void createBridgeBtn_Click(object sender, EventArgs e)
        {
            IMxDocument pMxDoc = m_application.Document as IMxDocument;

            //if (m_map.Layer[0].Name != "Road_IntersectionPoints" || m_map.Layer[1].Name!= "Water_IntersectionPoints")
            if (m_map.LayerCount<2)
            {
                MessageBox.Show("Please at least add Road_IntersectionPoints to the top first layer");
                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                m_application.RefreshWindow();
                return;
            }
            if (m_map.Layer[0].Name != "Road_IntersectionPoints")
            {
                MessageBox.Show("Please drag Road_IntersectionPoints to the top first layer");
                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                m_application.RefreshWindow();
                return;
            }
            IFeatureSelection pFeatSel_Road = (IFeatureSelection)m_map.Layer[0];
            ISelectionSet pSelSet_Road = pFeatSel_Road.SelectionSet;

            IFeatureSelection pFeatSel_Water = (IFeatureSelection)m_map.Layer[1];
            ISelectionSet pSelSet_Water = pFeatSel_Water.SelectionSet;


            IPoint tempPnt = new ESRI.ArcGIS.Geometry.PointClass();
            if (pSelSet_Road.Count == 0 && pSelSet_Water.Count == 0)
            {
                MessageBox.Show("Please select at least one intersection point to generate a new bridge.");

                return;
            }
            else if (pSelSet_Road.Count >= 1)
            {

                int noOfUniquePoints = 0;
                IPointCollection pointColl = new MultipointClass();
                List<string> rrownerList = new List<string>();
                List<string> streetnameList = new List<string>();

                double x_sum = 0.0;
                double y_sum = 0.0;
                IPoint point;
                ICursor pCursor;

                pSelSet_Road.Search(null, false, out pCursor);
                IFeature pFeature = (IFeature)pCursor.NextRow();

                string streetNames = "";
                string RROwner = "";
                string objectIds = "";
                int NoOfPoints = 0;
                while (pFeature != null)
                {
                    point = (IPoint)pFeature.Shape;
                    NoOfPoints = NoOfPoints + 1;
                    string rrowner = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("RROWNER1")).ToString();
                    string streetname = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("FULLNAME")).ToString();
                    string objectID= pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                    bool sameRROwner = true;
                    objectIds = objectIds + objectID + ";";
                    if (rrownerList.Count > 0)
                    {
                        sameRROwner = rrownerList.Contains(rrowner);
                    }
                    rrownerList.Add(rrowner);
                    if (!streetnameList.Contains(streetname))
                    {
                        streetnameList.Add(streetname);
                    }

                    if (!sameRROwner)
                    {
                        MessageBox.Show("A new bridge cannot be created due to different rrowners of the selected interesection points");
                        return;

                    }

                    bool exist = checkPointExistence(pointColl, point);
                    if (!exist)
                    {
                        pointColl.AddPoint(point);
                        noOfUniquePoints = noOfUniquePoints + 1;
                        x_sum = x_sum + point.X;
                        y_sum = y_sum + point.Y;

                    }
                    
                    
                    RROwner = rrowner;

                    pFeature = (IFeature)pCursor.NextRow();
                }

                for (int i = 0; i < streetnameList.Count(); i++)
                {
                    streetNames = streetNames + streetnameList[i] + ";";

                }

                IPoint newPoint = new PointClass();
                newPoint.PutCoords(x_sum / noOfUniquePoints, y_sum / noOfUniquePoints);

                functions.ZoomToCurrentError(pMxDoc, 0.001, newPoint);
                DialogResult dialogResult = MessageBox.Show("Do you want to create a new bridge over/below roads at the highlighted location?", "Create a new bridge", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    createNewBridge(newPoint, RROwner, streetNames, objectIds, NoOfPoints,"RN");
                }

                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                pFeatSel_Road.Clear();
                Marshal.ReleaseComObject(pCursor);
                m_application.RefreshWindow();


            }
            else if (pSelSet_Water.Count >= 1)
            {
                if (m_map.Layer[1].Name != "Water_IntersectionPoints")
                {
                    MessageBox.Show("A new bridge cannot be generated.");
                    pMxDoc.ActiveView.Refresh();
                    m_map.ClearSelection();
                    m_application.RefreshWindow();
                    return;
                }
                int noOfUniquePoints = 0;
                IPointCollection pointColl = new MultipointClass();
                List<string> rrownerList = new List<string>();
                List<string> streetnameList = new List<string>();

                double x_sum = 0.0;
                double y_sum = 0.0;
                IPoint point;
                ICursor pCursor;
                int NoOfPoints = 0;
                pSelSet_Water.Search(null, false, out pCursor);
                IFeature pFeature = (IFeature)pCursor.NextRow();

                string streetNames = "";
                string RROwner = "";
                string objectIds = "";

                while (pFeature != null)
                {
                    point = (IPoint)pFeature.Shape;
                    NoOfPoints = NoOfPoints + 1;
                    string rrowner = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("RROWNER1")).ToString();
                    string streetname = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("GNIS_NAME")).ToString();
                    string objectID = pFeature.get_Value(Data.waterIntersectionPntLyrCS.Fields.FindField("OBJECTID")).ToString();
                    objectIds = objectIds + objectID + ";";
                    bool sameRROwner = true;
                    if (rrownerList.Count > 0)
                    {
                        sameRROwner = rrownerList.Contains(rrowner);
                    }
                    rrownerList.Add(rrowner);
                    //bool sameStreetnames = streetnameList.Contains(streetname);
                    streetnameList.Add(streetname);

                    if (!sameRROwner)
                    {
                        MessageBox.Show("A new bridge cannot be created due to different rrowner of the selected interesection points");
                        return;

                    }

                    bool exist = checkPointExistence(pointColl, point);
                    if (!exist)
                    {
                        pointColl.AddPoint(point);
                        noOfUniquePoints = noOfUniquePoints + 1;
                        x_sum = x_sum + point.X;
                        y_sum = y_sum + point.Y;

                    }


                    for (int i = 0; i < streetnameList.Count(); i++)
                    {
                        streetNames = streetNames + streetnameList[i] + ";";

                    }
                    RROwner = rrowner;


                    pFeature = (IFeature)pCursor.NextRow();
                }

                IPoint newPoint = new PointClass();
                newPoint.PutCoords(x_sum / noOfUniquePoints, y_sum / noOfUniquePoints);

                functions.ZoomToCurrentError(pMxDoc, 0.001, newPoint);
                DialogResult dialogResult = MessageBox.Show("Do you want to create a new bridge over waterway at the highlighted location?", "Create a new bridge", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    createNewBridge(newPoint, RROwner, streetNames, objectIds, NoOfPoints,"WN");
                }
                m_map.ClearSelection();
                pFeatSel_Water.Clear();
                Marshal.ReleaseComObject(pCursor);
                m_application.RefreshWindow();
            }

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
                IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                    latLonTxt.Enabled = true;
                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
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
                legalRdo.Enabled = true;
                legalRdo.Checked = false;
                createBridgeBtn.Enabled = true;
                refreshBtn.Enabled = true;
                nonBridgeRdo.Checked = false;
                nonBridgeRdo.Enabled = true;


            }
        }

        private void legalRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (legalRdo.Checked)
            {
                nonBridgeRdo.Checked = false;
                string Status = "Legal";
                
                QCReportListView.Items[(indexSelected)].SubItems[1].Text = Status;
                saveFlagStatus(selectedTargetID, Status);
            }
        }
        private void saveFlagStatus(string selectedTargetID, string status)
        {
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID +  "'";

            IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

            int indexBridgeType = Data.SpatialAccuracyLyrCS.Fields.FindField("Status");
            int indexResolveDate = Data.SpatialAccuracyLyrCS.Fields.FindField("Resolved_Date");

            if (pFeature != null)
            {

                pFeature.set_Value(indexBridgeType, status);
                pFeature.set_Value(indexResolveDate, DateTime.Now);
                pFeature.Store();

            }

            Marshal.ReleaseComObject(pCursor);
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

        private void nonBridgeRdo_CheckedChanged(object sender, EventArgs e)
        {
            IMxDocument pMxDoc = m_application.Document as IMxDocument;
            if (nonBridgeRdo.Checked)
            {

                DialogResult dialogResult = MessageBox.Show("Do you really want to mark this as a non-bridge?", "Not a bridge", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                string Status = "Non-Bridge";
                if (!non_grouped)
                {
                    QCReportListView.Items[(indexSelected)].SubItems[1].Text = Status;
                    saveFlagStatus(selectedTargetID, Status);
                }
                else
                {
                    non_grouped = false;
                }
                
                

                string featureType = "N";
                saveBridgeType(selectedTargetID, featureType);

                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                m_application.RefreshWindow();
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

    

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IMxDocument pMxDoc = m_application.Document as IMxDocument;
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
                    indexSelected = indexBridgeSearched;
                    selectedTargetID = uniqueID;
                    ListViewItem selectedRow = QCReportListView.Items[(indexBridgeSearched)];
                    QCReportListView.Items[(indexBridgeSearched)].Selected = true;
                    QCReportListView.EnsureVisible(indexBridgeSearched);
                    QCReportListView.Focus();

                    IQueryFilter pQF = new QueryFilter();
                    pQF.WhereClause = "[TargetFeatureID] = '" + selectedTargetID + "'";
                    IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
                    IFeature pFeature = pCursor.NextFeature();


                    if (pFeature != null) //Available in current window
                    {
                        IPoint point = (IPoint)pFeature.Shape;
                        functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                        latLonTxt.Enabled = true;
                        latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();
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

                    Marshal.ReleaseComObject(pCursor);
                    legalRdo.Enabled = true;
                    legalRdo.Checked = false;
                    createBridgeBtn.Enabled = true;
                    nonBridgeRdo.Checked = false;
                    nonBridgeRdo.Enabled = true;
                }
                else
                {
                    IQueryFilter pQF = new QueryFilter();
                    pQF.WhereClause = "[TargetFeatureID] = '" + uniqueID + "'";
                    IFeatureCursor pCursor = Data.SpatialAccuracyLyrCS.Search(pQF, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null) //Available in QC_Report but is already checked.
                    {
                        MessageBox.Show("That bridge is already checked. Please reload QC_Report again to check it again.");
                        Marshal.ReleaseComObject(pCursor);
                        return;

                    }
                    else //Not available in QC_report, but may be in FRA_Bridge_DB Layer
                    {

                        pQF.WhereClause = "[UniqueID] = '" + uniqueID + "'";
                        IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
                        IFeature pFeature1 = pCursor1.NextFeature();
                        if (pFeature1 != null)
                        {
                            selectedTargetID = uniqueID;
                            non_grouped = true;
                            IPoint point = (IPoint)pFeature1.Shape;
                            functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                            latLonTxt.Enabled = true;
                            latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                            MessageBox.Show("Found. That bridge you searched is not a grouped bridge");
                        }
                        else
                        {
                            MessageBox.Show("That bridge you searched cannot be found");
                        }


                    }
                }
            }
            pMxDoc.ActiveView.Refresh();
            m_map.ClearSelection();
            m_application.RefreshWindow();
            return;
        }

        private void checkRRAccuracy_Click(object sender, EventArgs e)
        {
            operatorName = Environment.UserName.ToString();
            IQueryFilter pQF = new QueryFilter();
            int count = -1;
            ErrorNoTxt.Text = "0 features in total";
            if (Data.SpatialAccuracyLyrCS == null)
            {
                MessageBox.Show("SpatialAccuracy_Report does not exist or is not properly set");
                return;
            }
            else
            {
                count = Data.SpatialAccuracyLyrCS.FeatureCount(null);
                if (count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("SpatialAccuracy_Report table is not empty. Do you want to rewrite that table?", "QC", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        emptyQCTable(Data.SpatialAccuracyLyrCS);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                pQF.WhereClause = "[Type] = ( 'Y' )";
                checkRRSpatialAccuracy(pQF, operatorName);
                clearQCView();
            }

            MessageBox.Show("Done");
        }

        private void checkRRSpatialAccuracy(IQueryFilter pQF, string operatorName)
        {
            string subCategory = "Spatially Inaccurate";


            //Check bridges above/below roads against road intersection points, bridges over waterway against water intersection points
            IFeatureCursor pCursor1 = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor1.NextFeature();
            while (pFeature != null)
            {
                string sourceOID = pFeature.OID.ToString();
                string targetID = pFeature.get_Value(Data.featurePntLyrCS.Fields.FindField("UniqueID")).ToString();


                IPoint nodeOfInterest = pFeature.Shape as IPoint;
                //ISpatialFilter pSF = new SpatialFilter();
                //pSF.Geometry = nodeOfInterest;
                //pSF.GeometryField = Data.featurePntLyrCS.ShapeFieldName;
                //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;


                //IFeatureCursor pCursor2 = Data.waterIntersectionPntLyrCS.Search(pSF, false);
                //IFeature pFeature2 = pCursor2.NextFeature();


                //Marshal.ReleaseComObject(pCursor2);


                write2QCReport(nodeOfInterest, subCategory, nodeOfInterest.X, nodeOfInterest.Y, sourceOID, targetID, operatorName, "Unresolved");

                pFeature = pCursor1.NextFeature();
            }

            Marshal.ReleaseComObject(pCursor1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IMxDocument pMxDoc = m_application.Document as IMxDocument;

            //if (m_map.Layer[0].Name != "Road_IntersectionPoints" || m_map.Layer[1].Name!= "Water_IntersectionPoints")
            if (m_map.LayerCount < 2)
            {
                MessageBox.Show("Please at least add RR_PotentialBridgePoints_Single to the top first layer");
                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                m_application.RefreshWindow();
                return;
            }
            if (m_map.Layer[0].Name != "RR_PotentialBridgePoints_Single")
            {
                MessageBox.Show("Please drag RR_PotentialBridgePoints_Single to the top first layer");
                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                m_application.RefreshWindow();
                return;
            }
            IFeatureSelection pFeatSel_RR = (IFeatureSelection)m_map.Layer[0];
            ISelectionSet pSelSet_RR = pFeatSel_RR.SelectionSet;


            IPoint tempPnt = new ESRI.ArcGIS.Geometry.PointClass();
            if (pSelSet_RR.Count == 0)
            {
                MessageBox.Show("Please select at least one intersection point to generate a new bridge.");

                return;
            }
            else if (pSelSet_RR.Count >= 1)
            {

                int noOfUniquePoints = 0;
                IPointCollection pointColl = new MultipointClass();
                List<string> rrownerList = new List<string>();

                double x_sum = 0.0;
                double y_sum = 0.0;
                IPoint point;
                ICursor pCursor;

                pSelSet_RR.Search(null, false, out pCursor);
                IFeature pFeature = (IFeature)pCursor.NextRow();

                string streetNames = "";
                string RROwner = "";
                string objectIds = "";
                int NoOfPoints = 0;
                while (pFeature != null)
                {
                    point = (IPoint)pFeature.Shape;
                    NoOfPoints = NoOfPoints + 1;
                    string rrowner = pFeature.get_Value(Data.RR_PotentialBridgeLyrCS.Fields.FindField("RROWNER1")).ToString();
                    //string streetname = pFeature.get_Value(Data.intersectionPntLyrCS.Fields.FindField("FULLNAME")).ToString();
                    string objectID = pFeature.get_Value(Data.RR_PotentialBridgeLyrCS.Fields.FindField("OBJECTID")).ToString();
                    bool sameRROwner = true;
                    objectIds = objectIds + objectID + ";";
                    //if (rrownerList.Count > 0)
                    //{
                    //    sameRROwner = rrownerList.Contains(rrowner);
                    //}
                    //rrownerList.Add(rrowner);

                    //if (!sameRROwner)
                    //{
                    //    MessageBox.Show("A new bridge cannot be created due to different rrowners of the selected interesection points");
                    //    return;

                    //}

                    bool exist = checkPointExistence(pointColl, point);
                    if (!exist)
                    {
                        pointColl.AddPoint(point);
                        noOfUniquePoints = noOfUniquePoints + 1;
                        x_sum = x_sum + point.X;
                        y_sum = y_sum + point.Y;

                    }


                    RROwner = rrowner;

                    pFeature = (IFeature)pCursor.NextRow();
                }


                IPoint newPoint = new PointClass();
                newPoint.PutCoords(x_sum / noOfUniquePoints, y_sum / noOfUniquePoints);

                functions.ZoomToCurrentError(pMxDoc, 0.001, newPoint);
                DialogResult dialogResult = MessageBox.Show("Do you want to create a new bridge over/below RR at the highlighted location?", "Create a new bridge", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    createNewBridge(newPoint, RROwner, streetNames, objectIds, NoOfPoints, "NRR");
                }

                pMxDoc.ActiveView.Refresh();
                m_map.ClearSelection();
                Marshal.ReleaseComObject(pCursor);
                m_application.RefreshWindow();


            }

        }
    }
}
