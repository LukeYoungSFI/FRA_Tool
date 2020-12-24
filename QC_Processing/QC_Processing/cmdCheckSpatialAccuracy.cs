using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;

namespace QC_Processing
{
    /// <summary>
    /// Summary description for cmdCheckSpatialAccuracy.
    /// </summary>
    [Guid("04f6965b-cefc-4435-b358-5178e366f28e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("QC_Processing.cmdCheckSpatialAccuracy")]
    public sealed class cmdCheckSpatialAccuracy : BaseCommand
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

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        public cmdCheckSpatialAccuracy()
        {
            //
            // TODO: Define values for the public properties
            //
            //base.m_category = ""; //localizable text
            base.m_caption = "Check Spatial Accuracy";  //localizable text
            //base.m_message = "";  //localizable text 
            //base.m_toolTip = "";  //localizable text 
            //base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add cmdCheckSpatialAccuracy.OnClick implementation
            // TODO: Add cmdAutoPopulateOtherAttributes.OnClick implementation
            if (Data.featurePntLyrCS == null)
            {
                MessageBox.Show("Please reset the bridge point layer");
                return;
            }
                      
            
            //if (Data.intersectionPntLyrCS == null)
            //{
            //    MessageBox.Show("Please reset the road intersection layer");
            //    return;
            //}

            if (Data.SpatialAccuracyLyrCS == null)
            {
                MessageBox.Show("Please reset the SpatialAccuracyQC_Report layer");
                return;
            }


            try
            {

                Forms.checkSpatialAccuracy_Form.Application = m_application;
                IMxDocument pMxDoc = m_application.Document as IMxDocument;
                Forms.checkSpatialAccuracy_Form.Map = pMxDoc.FocusMap;
                Forms.checkSpatialAccuracy_Form.MxDoc = pMxDoc;
                Forms.checkSpatialAccuracy_Form.clearQCView();
                Forms.checkSpatialAccuracy_Form.Show(NativeWindow.FromHandle(new IntPtr(m_application.hWnd)));
                
            }
            catch
            {
                MessageBox.Show("Something wrong ");
            }
            
        }

    }

    #endregion
}

