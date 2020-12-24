using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;

using System.Collections.Generic;

namespace QC_Processing
{
    /// <summary>
    /// Summary description for cmdQC20.
    /// </summary>
    [Guid("69329e67-fbf7-44fb-b90b-2e974f090266")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("QC_Processing.cmdQC20")]
    public sealed class cmdQC20 : BaseCommand
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
        public cmdQC20()
        {
            //
            // TODO: Define values for the public properties
            //
            //base.m_category = ""; //localizable text
            base.m_caption = "20% QC";  //localizable text
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
            // TODO: Add cmdQC.OnClick implementation
            if (Data.featurePntLyrCS == null)
            {
                MessageBox.Show("Please reset the bridge target point layer");
                return;
            }

            if (Data.intersectionPntLyrCS == null)
            {
                MessageBox.Show("Please reset the intersection point layer");
                return;
            }

            if (Data.railroadLineLyrCS == null)
            {
                MessageBox.Show("Please reset the railroad layer");
                return;
            }
            //if (Data.roadLineLyrCS == null)
            //{
            //    MessageBox.Show("Please reset the road/waterway layer");
            //    return;
            //}

            if (Data.QC20LyrCS == null)
            {
                MessageBox.Show("Please reset the QC report data");
                return;
            }

            try
            {

                Forms.QC_Form.Application = m_application;
                IMxDocument pMxDoc = m_application.Document as IMxDocument;
                Forms.QC_Form.Map = pMxDoc.FocusMap;
                Forms.QC_Form.clearQCView();
                Forms.QC_Form.getUniqueIDList();
                Forms.QC_Form.Show(NativeWindow.FromHandle(new IntPtr(m_application.hWnd)));
                //List<string> random20PctList = getRandomSamplingList(uniqueIDList,0.2);
                Forms.QC_Form.setQCMode("20%");
                Forms.QC_Form.Text="TeamLead QC (20%)";
                //Forms.QC_Form.setSampling20PctList(random20PctList);
            }
            catch
            {
                MessageBox.Show("Something wrong ");
            }
        }

        

        #endregion
    }
}
