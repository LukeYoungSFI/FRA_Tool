using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace QC_Processing
{
    /// <summary>
    /// Summary description for MainToolBar.
    /// </summary>
    [Guid("6a553781-4d0a-4a0e-a34e-1fe9b18c3791")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("QC_Processing.MainToolBar")]
    public sealed class MainToolBar : BaseToolbar
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
            MxCommandBars.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Unregister(regKey);
        }

        #endregion
        #endregion

        public MainToolBar()
        {
            
            AddItem("QC_Processing.cmdConfig");
            AddItem("QC_Processing.cmdIdentifyBridges");
            AddItem("QC_Processing.cmdCheckSpatialAccuracy");
            AddItem("QC_Processing.cmdAssignBridgeAttributes");
           AddItem("QC_Processing.cmdAutoPopulateOtherAttributes");
            AddItem("QC_Processing.cmdQC");
            AddItem("QC_Processing.cmdQC20");
            AddItem("QC_Processing.cmdQC5");
           // AddItem("QC_Processing.cmdHelp");

            base.m_barCaption = "FRA QC Toolbar";
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "FRA QC Toolbar";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "MainToolBar";
            }
        }
    }
}