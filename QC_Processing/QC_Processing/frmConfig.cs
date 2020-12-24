using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.OleDb;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace QC_Processing
{
    public partial class frmConfig : Form
    {

        //public string targetFeatureLyrNM = "";
        private IMap m_map;
        private bool m_isShowing = false;


        public frmConfig()
        {
            InitializeComponent();
        }

        public bool IsShowing
        {
            get { return m_isShowing; }
        }

        public IMap Map
        {
            get { return m_map; }
            set { m_map = value; }
        }

        public object RR_PotentialCbx { get; private set; }

        private void frmConfig_Activated(object sender, EventArgs e)
        {
            this.m_isShowing = true;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            clearAllItems();


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "GeoDatabase Files|*.mdb";
            openFileDialog1.Title = "Select a GeoDatabase File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.
                try
                {
                    Data.pFWS=(IFeatureWorkspace)Data.pWSF.OpenFromFile(openFileDialog1.FileName, 0);
                    Data.pWS = (IWorkspace)Data.pWSF.OpenFromFile(openFileDialog1.FileName, 0);
                }
                catch
                {
                    MessageBox.Show("This file is not correct. ");
                }

                DBNMTxt.Text = openFileDialog1.FileName;

                //getLayersFromWokrspace(pFWS);



                // i tried the both ways to get the datasets in the workspace (i tried the type "esriDatasetType.esriDTAny" too)
                IEnumDataset datasets = Data.pWS.get_Datasets(esriDatasetType.esriDTFeatureClass);
                //IEnumDataset datasets = workspace.Datasets[esriDatasetType.esriDTFeatureClass];


                IDataset dataset = null;
                dataset = datasets.Next();

                List<string> lyrNamesList = new List<string>();
                while (dataset  != null)
                {
                    if (dataset.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        var subFC = dataset.Subsets;
                        IDataset pSubSet = subFC.Next();

                        while (pSubSet != null)
                        {
                            if (esriDatasetType.esriDTFeatureClass == pSubSet.Type)
                            {
                                //layers.Add(pSubSet.Name);
                                featureLayersCbx.Items.Add(pSubSet.Name);
                                intersectionCbx.Items.Add(pSubSet.Name);
                                targetPntsLyrCbx.Items.Add(pSubSet.Name);
                                railroadLyrCbx.Items.Add(pSubSet.Name);
                                roadLyrCbx.Items.Add(pSubSet.Name);
                                railMileLyrCbx.Items.Add(pSubSet.Name);
                                waterLyrCbx.Items.Add(pSubSet.Name);
                                waterIntersectionCbx.Items.Add(pSubSet.Name); ;
                                waterTargetPntsLyrCbx.Items.Add(pSubSet.Name);
                                QCLyrNMCbx.Items.Add(pSubSet.Name);
                                QC5LyrNMCbx.Items.Add(pSubSet.Name);
                                QC20LyrNMCbx.Items.Add(pSubSet.Name);
                                SpatialAccuracyReportCbx.Items.Add(pSubSet.Name);
                                waterMilePostLyrCbx.Items.Add(pSubSet.Name);
                                urbanCbx.Items.Add(pSubSet.Name);
                                countyCbx.Items.Add(pSubSet.Name);
                                rrPotentialBridgeCbx.Items.Add(pSubSet.Name);
                                stateCbx.Items.Add(pSubSet.Name);
                                lyrNamesList.Add(pSubSet.Name);
                            }
                            pSubSet = subFC.Next();
                        }
                    }
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        featureLayersCbx.Items.Add(dataset.Name);
                        intersectionCbx.Items.Add(dataset.Name);
                        targetPntsLyrCbx.Items.Add(dataset.Name);
                        railroadLyrCbx.Items.Add(dataset.Name);
                        roadLyrCbx.Items.Add(dataset.Name);
                        railMileLyrCbx.Items.Add(dataset.Name);
                        waterLyrCbx.Items.Add(dataset.Name);
                        waterIntersectionCbx.Items.Add(dataset.Name); ;
                        waterTargetPntsLyrCbx.Items.Add(dataset.Name);
                        QCLyrNMCbx.Items.Add(dataset.Name);
                        QC5LyrNMCbx.Items.Add(dataset.Name);
                        QC20LyrNMCbx.Items.Add(dataset.Name);
                        SpatialAccuracyReportCbx.Items.Add(dataset.Name);
                        lyrNamesList.Add(dataset.Name);
                        waterMilePostLyrCbx.Items.Add(dataset.Name);
                        urbanCbx.Items.Add(dataset.Name);
                        stateCbx.Items.Add(dataset.Name);
                        rrPotentialBridgeCbx.Items.Add(dataset.Name);
                        countyCbx.Items.Add(dataset.Name);
                    }
                    dataset = datasets.Next();
                }

                
                for (int i = 0; i < lyrNamesList.Count(); i++)
                {
                    
                    string lyrName = lyrNamesList[i].ToUpper();
                    if (lyrName.Contains("FRA_BRIDGE"))
                    {
                        featureLayersCbx.Text = "";
                        featureLayersCbx.SelectedText = lyrNamesList[i];
                        Data.TargetFeatureLyrNM = lyrNamesList[i];
                        Data.featurePntLyrCS = Data.pFWS.OpenFeatureClass(Data.TargetFeatureLyrNM);
                    }
                    else if (lyrName.Contains("ROAD_INTERSECTION"))
                    {
                        intersectionCbx.Text = "";
                        intersectionCbx.SelectedText = lyrNamesList[i];
                        Data.IntersectionFeatureLyrNM = lyrNamesList[i];
                        Data.intersectionPntLyrCS = Data.pFWS.OpenFeatureClass(Data.IntersectionFeatureLyrNM);
                    }
                    else if (lyrName.Contains("ROAD_TARGET"))
                    {
                        targetPntsLyrCbx.Text = "";
                        targetPntsLyrCbx.SelectedText = lyrNamesList[i];
                        Data.groupedTargetFeatureLyrNM = lyrNamesList[i];
                        Data.groupedTargetPntLyrCS = Data.pFWS.OpenFeatureClass(Data.groupedTargetFeatureLyrNM);
                    }

                    else if (lyrName.Contains("FRA_RAIL"))
                    {
                        railroadLyrCbx.Text = "";
                        railroadLyrCbx.SelectedText = lyrNamesList[i];
                        Data.RailroadLyrNM = lyrNamesList[i];
                        Data.railroadLineLyrCS = Data.pFWS.OpenFeatureClass(Data.RailroadLyrNM);
                    }
                    else if (lyrName.Contains("TIGER"))
                    {
                        roadLyrCbx.Text = "";
                        roadLyrCbx.SelectedText = lyrNamesList[i];
                        Data.RoadLyrNM = lyrNamesList[i];
                        Data.roadLineLyrCS = Data.pFWS.OpenFeatureClass(Data.RoadLyrNM);
                    }
                    else if (lyrName.Contains("NHD"))
                    {
                        waterLyrCbx.Text = "";
                        waterLyrCbx.SelectedText = lyrNamesList[i];
                        Data.WaterLyrNM = lyrNamesList[i];
                        Data.waterwayLineLyrCS = Data.pFWS.OpenFeatureClass(Data.WaterLyrNM);
                    }
                    else if (lyrName.Contains("WATER_INTERSECTION"))
                    {
                        waterIntersectionCbx.Text = "";
                        waterIntersectionCbx.SelectedText = lyrNamesList[i];
                        Data.waterIntersectionFeatureLyrNM = lyrNamesList[i];
                        Data.waterIntersectionPntLyrCS = Data.pFWS.OpenFeatureClass(Data.waterIntersectionFeatureLyrNM);
                    }
                    else if (lyrName.Contains("WATER_TARGET"))
                    {
                        waterTargetPntsLyrCbx.Text = "";
                        waterTargetPntsLyrCbx.SelectedText = lyrNamesList[i];
                        Data.waterGroupedTargetFeatureLyrNM = lyrNamesList[i];
                        Data.waterGroupedTargetPntLyrCS = Data.pFWS.OpenFeatureClass(Data.waterGroupedTargetFeatureLyrNM);
                    }
                    else if (lyrName.Contains("RAIL_MILE"))
                    {
                        railMileLyrCbx.Text = "";
                        railMileLyrCbx.SelectedText = lyrNamesList[i];
                        Data.RailMileLyrNM = lyrNamesList[i];
                        Data.RailMileLyrCS = Data.pFWS.OpenFeatureClass(Data.RailMileLyrNM);
                    }
                    else if (lyrName.Contains("WATER_MILE"))
                    {
                        waterMilePostLyrCbx.Text = "";
                        waterMilePostLyrCbx.SelectedText = lyrNamesList[i];
                        Data.WaterMileLyrNM = lyrNamesList[i];
                        Data.WaterMileLyrCS = Data.pFWS.OpenFeatureClass(Data.WaterMileLyrNM);
                    }
                    else if (lyrName.Contains("SPATIAL"))
                    {
                        SpatialAccuracyReportCbx.Text = "";
                        SpatialAccuracyReportCbx.SelectedText = lyrNamesList[i];
                        Data.SpatialAccuracyLyrNM = lyrNamesList[i];
                        Data.SpatialAccuracyLyrCS = Data.pFWS.OpenFeatureClass(Data.SpatialAccuracyLyrNM);
                    }
                    else if (lyrName.Contains("LEAD"))
                    {
                        QC20LyrNMCbx.Text = "";
                        QC20LyrNMCbx.SelectedText = lyrNamesList[i];
                        Data.QC20LyrNM = lyrNamesList[i];
                        Data.QC20LyrCS = Data.pFWS.OpenFeatureClass(Data.QC20LyrNM);
                    }
                    else if (lyrName.Contains("PM"))
                    {
                        QC5LyrNMCbx.Text = "";
                        QC5LyrNMCbx.SelectedText = lyrNamesList[i];
                        Data.QC5LyrNM = lyrNamesList[i];
                        Data.QC5LyrCS = Data.pFWS.OpenFeatureClass(Data.QC5LyrNM);
                    }
                    else if (lyrName.Contains("QC"))
                    {
                        QCLyrNMCbx.Text = "";
                        QCLyrNMCbx.SelectedText = lyrNamesList[i];
                        Data.QCLyrNM = lyrNamesList[i];
                        Data.QCLyrCS = Data.pFWS.OpenFeatureClass(Data.QCLyrNM);
                    }
                    else if (lyrName.Contains("COUNTY"))
                    {
                        countyCbx.Text = "";
                        countyCbx.SelectedText = lyrNamesList[i];
                        Data.USCountyLyrNM= lyrNamesList[i];
                        Data.USCountyLyrCS = Data.pFWS.OpenFeatureClass(Data.USCountyLyrNM);
                    }
                    else if (lyrName.Contains("STATE"))
                    {
                        stateCbx.Text = "";
                        stateCbx.SelectedText = lyrNamesList[i];
                        Data.USStateLyrNM = lyrNamesList[i];
                        Data.USStateLyrCS = Data.pFWS.OpenFeatureClass(Data.USStateLyrNM);
                    }
                    else if (lyrName.Contains("URBAN"))
                    {
                        urbanCbx.Text = "";
                        urbanCbx.SelectedText = lyrNamesList[i];
                        Data.UrbanLyrNM = lyrNamesList[i];
                        Data.UrbanLyrCS = Data.pFWS.OpenFeatureClass(Data.UrbanLyrNM);
                    }
                    else if (lyrName.Equals("RR_POTENTIALBRIDGEPOINTS_SINGLE"))
                    {
                        rrPotentialBridgeCbx.Text = "";
                        rrPotentialBridgeCbx.SelectedText = lyrNamesList[i];
                        Data.RR_PotentialBridgeLyrNM = lyrNamesList[i];
                        Data.RR_PotentialBridgeLyrCS = Data.pFWS.OpenFeatureClass(Data.RR_PotentialBridgeLyrNM);
                    }
                }            
                                
            }
        }

        private void featureLayersCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.TargetFeatureLyrNM = featureLayersCbx.SelectedItem.ToString();
            Data.featurePntLyrCS = Data.pFWS.OpenFeatureClass(Data.TargetFeatureLyrNM);
            
            

        }

        private void intersectionCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.IntersectionFeatureLyrNM = intersectionCbx.SelectedItem.ToString();
            Data.intersectionPntLyrCS = Data.pFWS.OpenFeatureClass(Data.IntersectionFeatureLyrNM);
        }

        private void railroadLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.RailroadLyrNM = railroadLyrCbx.SelectedItem.ToString();
            Data.railroadLineLyrCS = Data.pFWS.OpenFeatureClass(Data.RailroadLyrNM);
        }

        private void roadLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.RoadLyrNM = roadLyrCbx.SelectedItem.ToString();
            Data.roadLineLyrCS = Data.pFWS.OpenFeatureClass(Data.RoadLyrNM);
        }

        private void QC5LyrNMCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.QC5LyrNM = QC5LyrNMCbx.SelectedItem.ToString();
            Data.QC5LyrCS = Data.pFWS.OpenFeatureClass(Data.QC5LyrNM);
        }

        private void QC20LyrNMCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.QC20LyrNM = QC20LyrNMCbx.SelectedItem.ToString();
            Data.QC20LyrCS = Data.pFWS.OpenFeatureClass(Data.QC20LyrNM);
        }

        private void QCLyrNMCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.QCLyrNM= QCLyrNMCbx.SelectedItem.ToString();
            Data.QCLyrCS = Data.pFWS.OpenFeatureClass(Data.QCLyrNM);
        }
        private void clearAllItems()
        {
            featureLayersCbx.Items.Clear();
            intersectionCbx.Items.Clear();
            targetPntsLyrCbx.Items.Clear();
            railroadLyrCbx.Items.Clear();
            roadLyrCbx.Items.Clear();
            railMileLyrCbx.Items.Clear();
            QCLyrNMCbx.Items.Clear();
            QC20LyrNMCbx.Items.Clear();
            QC5LyrNMCbx.Items.Clear();
            SpatialAccuracyReportCbx.Items.Clear();
            waterIntersectionCbx.Items.Clear();
            waterTargetPntsLyrCbx.Items.Clear();
            waterLyrCbx.Items.Clear();
            waterMilePostLyrCbx.Items.Clear();
            countyCbx.Items.Clear();
            urbanCbx.Items.Clear();
            stateCbx.Items.Clear();



            DBNMTxt.Text = "";

            featureLayersCbx.Text = "";
            intersectionCbx.Text = "";
            targetPntsLyrCbx.Text = "";
            railroadLyrCbx.Text = "";
            roadLyrCbx.Text = "";
            QCLyrNMCbx.Text = "";
            QC20LyrNMCbx.Text = "";
            QC5LyrNMCbx.Text = "";
            SpatialAccuracyReportCbx.Text = "";
            railMileLyrCbx.Text = "";
            DBNMTxt.Text = "";
            waterMilePostLyrCbx.Text = "";
            waterIntersectionCbx.Text = "";
            waterTargetPntsLyrCbx.Text = ""; 
            waterLyrCbx.Text = ""; 

            countyCbx.Text = "";
            urbanCbx.Text = "";
            stateCbx.Text = "";
            rrPotentialBridgeCbx.Text = "";

            Data.TargetFeatureLyrNM = "";
            Data.featurePntLyrCS = null;

            Data.IntersectionFeatureLyrNM = "";
            Data.intersectionPntLyrCS = null;

            Data.groupedTargetFeatureLyrNM = "";
            Data.groupedTargetPntLyrCS = null;

            Data.RailroadLyrNM = "";
            Data.railroadLineLyrCS = null;

            Data.RoadLyrNM = "";
            Data.roadLineLyrCS = null;

            Data.RailMileLyrNM = "";
            Data.RailMileLyrCS = null;

            Data.WaterMileLyrNM = "";
            Data.WaterMileLyrCS = null;

            Data.QCLyrNM = "";
            Data.QCLyrCS = null;

            Data.QC20LyrNM = "";
            Data.QC20LyrCS = null;


            Data.QC5LyrNM = "";
            Data.QC5LyrCS = null;

            Data.SpatialAccuracyLyrNM = "";
            Data.SpatialAccuracyLyrCS = null;

            Data.USCountyLyrNM = "";
            Data.USCountyLyrCS = null;

            Data.USStateLyrNM = "";
            Data.USStateLyrCS = null;

            Data.UrbanLyrNM = "";
            Data.UrbanLyrCS = null;

            Data.RR_PotentialBridgeLyrNM = "";
            Data.RR_PotentialBridgeLyrCS = null;
        }

        private void railMileLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.RailMileLyrNM = railMileLyrCbx.SelectedItem.ToString();
            Data.RailMileLyrCS = Data.pFWS.OpenFeatureClass(Data.RailMileLyrNM);
        }

        private void targetPntsLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.groupedTargetFeatureLyrNM = targetPntsLyrCbx.SelectedItem.ToString();
            Data.groupedTargetPntLyrCS= Data.pFWS.OpenFeatureClass(Data.groupedTargetFeatureLyrNM);
        }

        private void countyCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.USCountyLyrNM = countyCbx.SelectedItem.ToString();
            Data.USCountyLyrCS = Data.pFWS.OpenFeatureClass(Data.USCountyLyrNM);
        }

        private void waterMilePostLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.WaterMileLyrNM = waterMilePostLyrCbx.SelectedItem.ToString();
            Data.WaterMileLyrCS = Data.pFWS.OpenFeatureClass(Data.WaterMileLyrNM);
        }

        private void urbanCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.UrbanLyrNM = urbanCbx.SelectedItem.ToString();
            Data.UrbanLyrCS = Data.pFWS.OpenFeatureClass(Data.UrbanLyrNM);
        }

        private void stateCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.USStateLyrNM = stateCbx.SelectedItem.ToString();
            Data.USStateLyrCS = Data.pFWS.OpenFeatureClass(Data.USStateLyrNM);

        }

        private void SpatialAccuracyReportCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.SpatialAccuracyLyrNM = SpatialAccuracyReportCbx.SelectedItem.ToString();
            Data.SpatialAccuracyLyrCS = Data.pFWS.OpenFeatureClass(Data.SpatialAccuracyLyrNM);
        }

        private void waterIntersectionCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.waterIntersectionFeatureLyrNM = waterIntersectionCbx.SelectedItem.ToString();
            Data.waterIntersectionPntLyrCS = Data.pFWS.OpenFeatureClass(Data.waterIntersectionFeatureLyrNM);
        }

        private void waterTargetPntsLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.waterGroupedTargetFeatureLyrNM = waterTargetPntsLyrCbx.SelectedItem.ToString();
            Data.waterGroupedTargetPntLyrCS = Data.pFWS.OpenFeatureClass(Data.waterGroupedTargetFeatureLyrNM);
        }

        private void waterLyrCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.WaterLyrNM = waterLyrCbx.SelectedItem.ToString();
            Data.waterwayLineLyrCS = Data.pFWS.OpenFeatureClass(Data.WaterLyrNM);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.RR_PotentialBridgeLyrNM = rrPotentialBridgeCbx.SelectedItem.ToString();
            Data.RR_PotentialBridgeLyrCS = Data.pFWS.OpenFeatureClass(Data.RR_PotentialBridgeLyrNM);
        }
    }
}
