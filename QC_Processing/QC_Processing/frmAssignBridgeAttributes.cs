using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;

namespace QC_Processing
{
    public partial class frmAssignBridgeAttributes : Form
    {
        private IApplication m_application;
        public IWorkspaceEdit2 m_WorkspaceEdit = null;
        private IMap m_map;        
        private int indexSelected = -1;
        private string selectedTargetID = "";
        private bool check20 = false;
        private bool check5 = false;

        public frmAssignBridgeAttributes()
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

        public void clearListView()
        {
            bridgeListView.Items.Clear();
        }

        public void AddFileIntoList(ListViewItem itms)
        {
            bridgeListView.Items.Add(itms);
        }


        public void setErrorCount(long count)
        {
            ErrorNoTxt.Text = count.ToString() + " features in total";
        }


        public List<string> getBridgeTypeDomain()
        {
            List<string> bridgeTypeDomain = new List<string>();
            for (int i = 0; i < bridgeTypeCbx.Items.Count; i++)
            {
                bridgeTypeDomain.Add(bridgeTypeCbx.Items[i].ToString());
                
            }
            return bridgeTypeDomain;
        }

        public List<string> getDesignTypeDomain()
        {
            List<string> designTypeDomain = new List<string>();
            for (int i = 0; i < designTypeCbx.Items.Count; i++)
            {
                designTypeDomain.Add(designTypeCbx.Items[i].ToString());

            }
            return designTypeDomain;
        }

        public List<string> getCrossTypeDomain()
        {
            List<string> crossTypeDomain = new List<string>();
            for (int i = 0; i < crossTypeCbx.Items.Count; i++)
            {
                crossTypeDomain.Add(crossTypeCbx.Items[i].ToString());

            }
            return crossTypeDomain;
        }
        private void BackwardBtn_Click(object sender, EventArgs e)
        {
            if (indexSelected > 0)
            {
                int nextIndex = indexSelected - 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow = bridgeListView.Items[(nextIndex)];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                getSelectedItem();

            }
        }

        private void getSelectedItem()
        {
            bridgeListView.Items[(indexSelected)].Selected = true;
            bridgeListView.EnsureVisible(indexSelected);
            bridgeListView.Focus();
            int streetViewIndex = Data.featurePntLyrCS.Fields.FindField("StreetView");
            if (streetViewIndex == -1)
            {
                MessageBox.Show("StreetView Column does not exist in Bridge table");
                return;
            }

            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();
            IMxDocument pMxDoc = m_application.Document as IMxDocument;

            if (pFeature != null)
            {
                IPoint point = (IPoint)pFeature.Shape;
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

                if (selectedTargetID.Contains("W"))
                {
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
                else if (selectedTargetID.Contains("R"))
                {
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



                functions.ZoomToCurrentError(pMxDoc, 0.001, point);
                ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();
                latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                if (indexSelected == 0)
                {
                    BackwardBtn.Enabled = false;
                }
                else if ((indexSelected == Data.ErrorCount - 1 ) || (indexSelected == 999))
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
            //saveBridgeAttrBtn.Enabled = false;
            rdoNonBridge.Checked = false;
            rdoNonBridge.Enabled = true;
        }
        private void NextBtn_Click(object sender, EventArgs e)
        {

            if ((indexSelected < Data.ErrorCount - 1))
            {
                int nextIndex = indexSelected + 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow = bridgeListView.Items[(nextIndex)];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();

                //deleteBridgeBtn.Enabled = true;
                getSelectedItem();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bridgeListView_DoubleClick(object sender, EventArgs e)
        {
            if (bridgeListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedRow = bridgeListView.SelectedItems[0];
                indexSelected = bridgeListView.SelectedItems[0].Index;
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                bridgeListView.Focus();


                getSelectedItem();
                //deleteBridgeBtn.Enabled = true;
                crossTypeCbx.Enabled = true;
                bridgeTypeCbx.Enabled = true;
                designTypeCbx.Enabled = true;
                cbxStreetViewAvailability.Enabled = true;
                rdoNonBridge.Enabled = true;

                if (indexSelected == 0)
                {
                    BackwardBtn.Enabled = false;
                    NextBtn.Enabled = true;
                }
                else if ((indexSelected == Data.ErrorCount - 1) || (indexSelected == 999))
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
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            IQueryFilter pQF = new QueryFilter();
            if (check20)
            {
                string IDs = selectedSampleList(Data.QC20LyrCS);
                pQF.WhereClause = "UniqueID in " + IDs + " AND [Check20] IS NULL";
            }
            else if (check5)
            {
                string IDs = selectedSampleList(Data.QC5LyrCS);
                pQF.WhereClause = "UniqueID in " + IDs + " AND [Check20] IS NULL";
            }
            else
            {
                pQF.WhereClause = "[Type] NOT IN( 'N' , 'undefined' , 'non-bridge' ) AND [Checked] IS NULL";
            }
           


            ErrorIDLbl.Text = "Editing Feature ID: ";
            BackwardBtn.Enabled = false;
            
            functions.loadAllBridges(pQF);
            Data.ErrorCount = functions.getAllFeaturesCount(pQF);
            Forms.assignAttributes_Form.setErrorCount(Data.ErrorCount);
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;
            crossTypeCbx.Enabled = false;
            bridgeTypeCbx.Enabled = false;
            designTypeCbx.Enabled = false;
            rdoNonBridge.Enabled = false;
            saveBridgeAttrBtn.Enabled = false;
            latLonTxt.Text = "";
        }

        private void saveBridgeAttrBtn_Click(object sender, EventArgs e)
        {
            string bridgeType = bridgeTypeCbx.SelectedItem.ToString();
            string desginType = designTypeCbx.SelectedItem.ToString();
            string crossType = crossTypeCbx.SelectedItem.ToString();


            int indexBridgeType= Data.featurePntLyrCS.Fields.FindField("Bridge_Type");
            int indexDesignType= Data.featurePntLyrCS.Fields.FindField("Design_Type");
            int indexBridgeLength= Data.featurePntLyrCS.Fields.FindField("Length");
            //int indexCrossingID= Data.groupedTargetPntLyrCS.Fields.FindField("BridgeGradeCrossingID");
            int indexChecked = Data.featurePntLyrCS.Fields.FindField("Checked");
            int indexChecked20 = Data.featurePntLyrCS.Fields.FindField("Check20");
            int indexChecked5 = Data.featurePntLyrCS.Fields.FindField("Check5");
            int indexCrossType = Data.featurePntLyrCS.Fields.FindField("Cross_Type");

            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

           
            if (pFeature != null)
            {

                if (crossType != "")
                {
                    pFeature.set_Value(indexCrossType, crossType);
                    bridgeListView.Items[(indexSelected)].SubItems[2].Text = crossType;
                }
                else
                {
                    MessageBox.Show("Please select Crossing Type, then save it.");
                    return;
                }

                if (bridgeType != "")
                {
                    pFeature.set_Value(indexBridgeType, bridgeType);
                    bridgeListView.Items[(indexSelected)].SubItems[3].Text = bridgeType;
                }
                else
                {
                    MessageBox.Show("Please select Bridge Type, then save it.");
                    return;
                }

                if (desginType != "")
                {
                    pFeature.set_Value(indexDesignType, desginType);
                    bridgeListView.Items[(indexSelected)].SubItems[4].Text = desginType;
                }
                else
                {
                    MessageBox.Show("Please select Design Type, then save it");
                    return;
                }




                if (check20)
                {
                    pFeature.set_Value(indexChecked20, "Y");
                }
                else if (check5)
                {
                    pFeature.set_Value(indexChecked5, "Y");
                }
                else
                {
                    pFeature.set_Value(indexChecked, "Y");
                }
               

                pFeature.Store();

            }
            Marshal.ReleaseComObject(pCursor);
            

            refreshBtn.Enabled = true;
        }

        

        private void bridgeTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveBridgeAttrBtn.Enabled = true;
        }

        private void designTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveBridgeAttrBtn.Enabled = true;
        }

        private void bridgeLengthTxt_TextChanged(object sender, EventArgs e)
        {
            saveBridgeAttrBtn.Enabled = true;
        }

        private void CrossingIDTxt_TextChanged(object sender, EventArgs e)
        {
            saveBridgeAttrBtn.Enabled = true;
        }

        private void crossTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveBridgeAttrBtn.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            check5 = false;
            check20 = false;
            functions.loadAllBridges(null);
            Data.ErrorCount = functions.getAllFeaturesCount(null);
            setErrorCount(Data.ErrorCount);
        }

        private string selectedSampleList(IFeatureClass activeQCLyr)
        {
            List<string> UniqueIDList = new List<string>();
            IFeatureCursor pCursor = activeQCLyr.Search(null, false);
            IFeature pFeature = pCursor.NextFeature();

            while (pFeature != null)
            {
                string uniqueID = pFeature.get_Value(activeQCLyr.Fields.FindField("TargetFeatureID")).ToString();
                UniqueIDList.Add(uniqueID);
                pFeature = pCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pCursor);
            string IDs = "(";
            for (int i = 0; i < UniqueIDList.Count; i++)
            {
                IDs = IDs + "'" + UniqueIDList[i] + "', ";
            }
            IDs = IDs.Substring(0, IDs.Length - 2) + ")";
            return IDs;
        }

        private void loadQC20Btn_Click(object sender, EventArgs e)
        {
            
           
            if ( Data.samples20Pct == "")
            {
                MessageBox.Show("Please perform 20% QC first");
                return;
            }
            else
            {
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "UniqueID in " + Data.samples20Pct + " AND [Check20] IS NULL";
               
                check20 = true;
                check5 = false;
                ErrorIDLbl.Text = "Editing Feature ID: ";
                BackwardBtn.Enabled = false;
               
               
                functions.loadAllBridges(pQF);
                Data.ErrorCount = functions.getAllFeaturesCount(pQF);
                Forms.assignAttributes_Form.setErrorCount(Data.ErrorCount);
                BackwardBtn.Enabled = false;
                NextBtn.Enabled = false;
                crossTypeCbx.Enabled = false;
                bridgeTypeCbx.Enabled = false;
                designTypeCbx.Enabled = false;
                latLonTxt.Text = "";
            }
        }

        private void loadQC5Btn_Click(object sender, EventArgs e)
        {
           

            if (Data.samples5Pct == "")
            {
                MessageBox.Show("Please perform 5% QC first");
                return;
            }
            else
            {
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "UniqueID in " + Data.samples5Pct + " AND [Check5] IS NULL";
                

                check5 = true;
                check20 = false;
                ErrorIDLbl.Text = "Editing Feature ID: ";
                BackwardBtn.Enabled = false;


                functions.loadAllBridges(pQF);
                Data.ErrorCount = functions.getAllFeaturesCount(pQF);
                Forms.assignAttributes_Form.setErrorCount(Data.ErrorCount);
                BackwardBtn.Enabled = false;
                NextBtn.Enabled = false;
                crossTypeCbx.Enabled = false;
                bridgeTypeCbx.Enabled = false;
                designTypeCbx.Enabled = false;
                latLonTxt.Text = "";
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNonBridge.Checked)
            {
                string featureType = "N";
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
                bridgeListView.Items[(indexSelected)].SubItems[5].Text = featureType;

            }
        }

       
    }
}
