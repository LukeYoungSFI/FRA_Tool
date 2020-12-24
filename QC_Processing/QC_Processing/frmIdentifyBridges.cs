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
    public partial class frmIdentifyBridges : Form
    {
        private IApplication m_application;
        public IWorkspaceEdit2 m_WorkspaceEdit = null;
        private IMap m_map;

        private int indexSelected = -1;
        private string selectedTargetID = "";
        private string bridgeType = "";

        private Dictionary<string,string> editObjectsList = new Dictionary<string, string>();

        public frmIdentifyBridges()
        {
            InitializeComponent();
        }

        //~frmIdentifyBridges()
        //{
        //    if (m_WorkspaceEdit.IsBeingEdited())
        //    {
        //        m_WorkspaceEdit.StopEditing(false);
        //    }
        //}

        public IMap Map
        {
            get { return m_map; }
            set { m_map = value; }
        }

        public IApplication Application
        {
            set { m_application = value; }
        }

        public void AddFileIntoList(ListViewItem itms)
        {
            ListBridgeTargetPnts.Items.Add(itms);
        }

        public void clearListView()
        {
            ListBridgeTargetPnts.Items.Clear();
        }

        public void setErrorCount(long count)
        {
            ErrorNoTxt.Text = count.ToString() + " features in total";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            //m_WorkspaceEdit.StopEditing(true);
        }

        private void LstErrors_doubleClick(object sender, MouseEventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            if (ListBridgeTargetPnts.SelectedItems.Count > 0)
            {
                ListViewItem selectedRow = ListBridgeTargetPnts.SelectedItems[0];
                indexSelected = ListBridgeTargetPnts.SelectedItems[0].Index;
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                ListBridgeTargetPnts.Focus();


                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature !=null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, (double)NumZoomingAdjust.Value, point);
                    ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();

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
                    MessageBox.Show("Target "+ selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }

            refreshBtn.Enabled = true;
            NumZoomingAdjust.Enabled = true;             
               


        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            
            if (indexSelected <Data.ErrorCount-1)
            {
                int nextIndex = indexSelected + 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow = ListBridgeTargetPnts.Items[(nextIndex)];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                ListBridgeTargetPnts.Items[(nextIndex)].Selected = true;
                ListBridgeTargetPnts.EnsureVisible(nextIndex);
                ListBridgeTargetPnts.Focus();

                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, (double)NumZoomingAdjust.Value, point);
                    ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();

                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                    if (indexSelected == 0)
                    {
                        BackwardBtn.Enabled = false;
                    }
                    else if (indexSelected == Data.ErrorCount - 1)
                    {
                        NextBtn.Enabled = false;
                    }
                    else
                    {
                        NextBtn.Enabled = true;
                        BackwardBtn.Enabled = true;
                    }

                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                }

                else
                {
                    MessageBox.Show("Target " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Marshal.ReleaseComObject(pCursor);
            }
        }

        private void BackwardBtn_Click(object sender, EventArgs e)
        {
            if (indexSelected > 0)
            {
                int nextIndex = indexSelected - 1;
                indexSelected = nextIndex;
                ListViewItem selectedRow= ListBridgeTargetPnts.Items[(nextIndex)];
                selectedTargetID = selectedRow.SubItems[0].Text.ToString();
                ListBridgeTargetPnts.Items[(nextIndex)].Selected = true;
                ListBridgeTargetPnts.EnsureVisible(nextIndex);
                ListBridgeTargetPnts.Focus();

                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
                IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();

                IMxDocument pMxDoc = m_application.Document as IMxDocument;

                if (pFeature != null)
                {
                    IPoint point = (IPoint)pFeature.Shape;
                    functions.ZoomToCurrentError(pMxDoc, (double)NumZoomingAdjust.Value, point);
                    ErrorIDLbl.Text = "Editing Feature ID: " + selectedTargetID.ToString();

                    latLonTxt.Text = point.Y.ToString() + ", " + point.X.ToString();

                    if (indexSelected == 0)
                    {
                        BackwardBtn.Enabled = false;
                    }
                    else if (indexSelected == Data.ErrorCount - 1)
                    {
                        NextBtn.Enabled = false;
                    }
                    else
                    {
                        NextBtn.Enabled = true;
                        BackwardBtn.Enabled = true;
                    }

                    radioButton1.Checked = false;
                    radioButton2.Checked = false;

                }
                else
                {
                    MessageBox.Show("Target " + selectedTargetID + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                Marshal.ReleaseComObject(pCursor);


            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                bridgeType = "Y";
                if (editObjectsList.Count > 0)
                {
                    if (editObjectsList.ContainsKey(selectedTargetID))
                    {
                        editObjectsList[selectedTargetID] = bridgeType;
                        
                    }
                    else
                    {
                        editObjectsList.Add(selectedTargetID, bridgeType);
                    }


                }
                else
                {
                    editObjectsList.Add(selectedTargetID, bridgeType);
                }
                ListBridgeTargetPnts.Items[(indexSelected)].SubItems[1].Text = bridgeType;

                saveBridgeType(selectedTargetID, bridgeType);
            }
            
           
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                bridgeType = "N";
                if (editObjectsList.Count > 0)
                {
                    if (editObjectsList.ContainsKey(selectedTargetID))
                    {

                        //int index = editObjectsList.ContainsKey
                        //   editObjectsList.Add(selectedTargetID, bridgeType);
                        editObjectsList[selectedTargetID] = bridgeType;

                    }
                    else
                    {
                        editObjectsList.Add(selectedTargetID, bridgeType);
                    }


                }
                else
                {
                    editObjectsList.Add(selectedTargetID, bridgeType);
                }

                ListBridgeTargetPnts.Items[(indexSelected)].SubItems[1].Text = bridgeType;
                saveBridgeType(selectedTargetID, bridgeType);
                
            }
        }

       
       

        private void saveBridgeType(string selectedTargetID, string bridgeType)
        {
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = "[UniqueID] = '" + selectedTargetID + "'";
            IFeatureCursor pCursor = Data.featurePntLyrCS.Search(pQF, false);
            IFeature pFeature = pCursor.NextFeature();

            int indexBridgeType = Data.featurePntLyrCS.Fields.FindField("Type");
            //int indexBridgeCheck = Data.featurePntLyrCS.Fields.FindField("CheckBridge");
            if (pFeature != null)
            {
                pFeature.set_Value(indexBridgeType, bridgeType);
                //pFeature.set_Value(indexBridgeCheck, "Y");
                pFeature.Store();
                
            }

            Marshal.ReleaseComObject(pCursor);
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            ErrorIDLbl.Text = "Editing Feature ID: ";
            BackwardBtn.Enabled = false;
            NextBtn.Enabled = false;
            NumZoomingAdjust.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            IQueryFilter pQF = new QueryFilter();

            string subCategory = "";
            if (featureCategoryCbx.SelectedItem != null)
            {
                subCategory = featureCategoryCbx.SelectedItem.ToString();
                pQF.WhereClause = "[Type]= '" + subCategory ;
            }
            else
            {
                pQF.WhereClause = "[Type]= 'undefined'";
            }

            functions.loadFeatures(pQF);
            Data.ErrorCount = functions.getFeaturesCount(pQF);
            Forms.identifyBridge_Form.setErrorCount(Data.ErrorCount);
            latLonTxt.Text = "";

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void featureCategoryCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            IQueryFilter pQF = new QueryFilter();
            string subCategory = "";
            if (featureCategoryCbx.SelectedItem != null)
            {
                subCategory = featureCategoryCbx.SelectedItem.ToString();
                pQF.WhereClause = "[Type]= '" + subCategory + "'";
                             
            }

            functions.loadFeatures(pQF);
            Data.ErrorCount = functions.getFeaturesCount(pQF);
            Forms.identifyBridge_Form.setErrorCount(Data.ErrorCount);
        }
    }
}
