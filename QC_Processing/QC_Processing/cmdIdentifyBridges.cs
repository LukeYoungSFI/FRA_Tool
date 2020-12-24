using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;

namespace QC_Processing
{
    /// <summary>
    /// Summary description for cmdIdentifyBridges.
    /// </summary>
    [Guid("6e970c40-fb18-480c-b765-2dcfb33a820f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("QC_Processing.cmdIdentifyBridges")]
    public sealed class cmdIdentifyBridges : BaseCommand
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
        public cmdIdentifyBridges()
        {
            //
            // TODO: Define values for the public properties
            //
            //base.m_category = "Identify Bridge/Nonbridge"; //localizable text
            base.m_caption = "Identify Bridge/Nonbridge";  //localizable text
            //base.m_message = "Identify Bridge/Nonbridge";  //localizable text 
            //base.m_toolTip = "Identify Bridge/Nonbridge";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

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
            // TODO: Add cmdIdentifyBridges.OnClick implementation

            if (Data.featurePntLyrCS == null)
            {
                MessageBox.Show("Please click the first button to set FRA Bridge point layer: FRA_Bridge_DB");
                return;
            }

            try
            {
                IQueryFilter pQF = new QueryFilter();
                pQF.WhereClause = "[Type] = 'undefined'";
                functions.loadFeatures(pQF);
                Data.ErrorCount = functions.getFeaturesCount(pQF);
                Forms.identifyBridge_Form.setErrorCount(Data.ErrorCount);
                Forms.identifyBridge_Form.Application = m_application;
                IMxDocument pMxDoc = m_application.Document as IMxDocument;
                Forms.identifyBridge_Form.Map = pMxDoc.FocusMap;
                Forms.identifyBridge_Form.Show(NativeWindow.FromHandle(new IntPtr(m_application.hWnd)));
            }
            catch
            {
                MessageBox.Show("Something wrong");
            }
            



        }


        #endregion
    }
}
