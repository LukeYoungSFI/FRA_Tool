namespace QC_Processing
{
    partial class frmQC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.QCReportListView = new System.Windows.Forms.ListView();
            this.colBridgeID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOperator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDesignType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.NextBtn = new System.Windows.Forms.Button();
            this.ErrorNoTxt = new System.Windows.Forms.Label();
            this.fixedRdo = new System.Windows.Forms.RadioButton();
            this.BackwardBtn = new System.Windows.Forms.Button();
            this.legalRdo = new System.Windows.Forms.RadioButton();
            this.Subcategory = new System.Windows.Forms.Label();
            this.subcategoryCbx = new System.Windows.Forms.ComboBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.hideBtn = new System.Windows.Forms.Button();
            this.PerformQCBtn = new System.Windows.Forms.Button();
            this.processingStatusLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.loadQCBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.latLonTxt = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.crossTypeCbx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.designTypeCbx = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bridgeTypeCbx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBridgeUniqueID = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbxStreetViewAvailability = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // QCReportListView
            // 
            this.QCReportListView.AllowColumnReorder = true;
            this.QCReportListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBridgeID,
            this.colCategory,
            this.colStatus,
            this.colOperator,
            this.colDesignType});
            this.QCReportListView.Enabled = false;
            this.QCReportListView.FullRowSelect = true;
            this.QCReportListView.HideSelection = false;
            this.QCReportListView.Location = new System.Drawing.Point(201, 12);
            this.QCReportListView.MultiSelect = false;
            this.QCReportListView.Name = "QCReportListView";
            this.QCReportListView.Size = new System.Drawing.Size(562, 295);
            this.QCReportListView.TabIndex = 0;
            this.QCReportListView.UseCompatibleStateImageBehavior = false;
            this.QCReportListView.View = System.Windows.Forms.View.Details;
            this.QCReportListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.QCReportListView_MouseDoubleClick);
            // 
            // colBridgeID
            // 
            this.colBridgeID.Text = "TargetFeatureID";
            this.colBridgeID.Width = 110;
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 140;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 90;
            // 
            // colOperator
            // 
            this.colOperator.Text = "Operator";
            this.colOperator.Width = 90;
            // 
            // colDesignType
            // 
            this.colDesignType.Text = "Design_Type";
            this.colDesignType.Width = 110;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.NextBtn);
            this.groupBox1.Controls.Add(this.ErrorNoTxt);
            this.groupBox1.Controls.Add(this.fixedRdo);
            this.groupBox1.Controls.Add(this.BackwardBtn);
            this.groupBox1.Controls.Add(this.legalRdo);
            this.groupBox1.Controls.Add(this.Subcategory);
            this.groupBox1.Controls.Add(this.subcategoryCbx);
            this.groupBox1.Location = new System.Drawing.Point(20, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 232);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flagging Error";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(19, 123);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(78, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Non-Bridge";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // NextBtn
            // 
            this.NextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.Location = new System.Drawing.Point(66, 156);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(40, 28);
            this.NextBtn.TabIndex = 33;
            this.NextBtn.Text = ">";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // ErrorNoTxt
            // 
            this.ErrorNoTxt.AutoSize = true;
            this.ErrorNoTxt.Location = new System.Drawing.Point(7, 204);
            this.ErrorNoTxt.Name = "ErrorNoTxt";
            this.ErrorNoTxt.Size = new System.Drawing.Size(88, 13);
            this.ErrorNoTxt.TabIndex = 38;
            this.ErrorNoTxt.Text = "0 features in total";
            // 
            // fixedRdo
            // 
            this.fixedRdo.AutoSize = true;
            this.fixedRdo.Enabled = false;
            this.fixedRdo.Location = new System.Drawing.Point(19, 71);
            this.fixedRdo.Name = "fixedRdo";
            this.fixedRdo.Size = new System.Drawing.Size(50, 17);
            this.fixedRdo.TabIndex = 35;
            this.fixedRdo.TabStop = true;
            this.fixedRdo.Text = "Fixed";
            this.fixedRdo.UseVisualStyleBackColor = true;
            this.fixedRdo.CheckedChanged += new System.EventHandler(this.fixedRdo_CheckedChanged);
            // 
            // BackwardBtn
            // 
            this.BackwardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackwardBtn.Location = new System.Drawing.Point(10, 156);
            this.BackwardBtn.Name = "BackwardBtn";
            this.BackwardBtn.Size = new System.Drawing.Size(38, 28);
            this.BackwardBtn.TabIndex = 32;
            this.BackwardBtn.Text = "<";
            this.BackwardBtn.UseVisualStyleBackColor = true;
            this.BackwardBtn.Click += new System.EventHandler(this.BackwardBtn_Click);
            // 
            // legalRdo
            // 
            this.legalRdo.AutoSize = true;
            this.legalRdo.Enabled = false;
            this.legalRdo.Location = new System.Drawing.Point(19, 97);
            this.legalRdo.Name = "legalRdo";
            this.legalRdo.Size = new System.Drawing.Size(51, 17);
            this.legalRdo.TabIndex = 36;
            this.legalRdo.TabStop = true;
            this.legalRdo.Text = "Legal";
            this.legalRdo.UseVisualStyleBackColor = true;
            this.legalRdo.CheckedChanged += new System.EventHandler(this.legalRdo_CheckedChanged);
            // 
            // Subcategory
            // 
            this.Subcategory.AutoSize = true;
            this.Subcategory.Location = new System.Drawing.Point(7, 23);
            this.Subcategory.Name = "Subcategory";
            this.Subcategory.Size = new System.Drawing.Size(67, 13);
            this.Subcategory.TabIndex = 1;
            this.Subcategory.Text = "Subcategory";
            // 
            // subcategoryCbx
            // 
            this.subcategoryCbx.Enabled = false;
            this.subcategoryCbx.FormattingEnabled = true;
            this.subcategoryCbx.Items.AddRange(new object[] {
            "Incorrect Bridge Type",
            "Incorrect Crossing Type",
            "Incorrect Design Type",
            "Spatially Inaccurate"});
            this.subcategoryCbx.Location = new System.Drawing.Point(6, 42);
            this.subcategoryCbx.Name = "subcategoryCbx";
            this.subcategoryCbx.Size = new System.Drawing.Size(153, 21);
            this.subcategoryCbx.TabIndex = 0;
            this.subcategoryCbx.SelectedIndexChanged += new System.EventHandler(this.subcategoryCbx_SelectedIndexChanged);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Location = new System.Drawing.Point(114, 383);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(72, 23);
            this.refreshBtn.TabIndex = 34;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // hideBtn
            // 
            this.hideBtn.Location = new System.Drawing.Point(708, 452);
            this.hideBtn.Name = "hideBtn";
            this.hideBtn.Size = new System.Drawing.Size(54, 31);
            this.hideBtn.TabIndex = 2;
            this.hideBtn.Text = "Hide";
            this.hideBtn.UseVisualStyleBackColor = true;
            this.hideBtn.Click += new System.EventHandler(this.hideBtn_Click);
            // 
            // PerformQCBtn
            // 
            this.PerformQCBtn.Location = new System.Drawing.Point(26, 12);
            this.PerformQCBtn.Name = "PerformQCBtn";
            this.PerformQCBtn.Size = new System.Drawing.Size(75, 23);
            this.PerformQCBtn.TabIndex = 3;
            this.PerformQCBtn.Text = "Perform QC";
            this.PerformQCBtn.UseVisualStyleBackColor = true;
            this.PerformQCBtn.Click += new System.EventHandler(this.PerformQCBtn_Click);
            // 
            // processingStatusLbl
            // 
            this.processingStatusLbl.AutoSize = true;
            this.processingStatusLbl.Location = new System.Drawing.Point(27, 42);
            this.processingStatusLbl.Name = "processingStatusLbl";
            this.processingStatusLbl.Size = new System.Drawing.Size(0, 13);
            this.processingStatusLbl.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 469);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 31;
            // 
            // loadQCBtn
            // 
            this.loadQCBtn.Location = new System.Drawing.Point(114, 12);
            this.loadQCBtn.Name = "loadQCBtn";
            this.loadQCBtn.Size = new System.Drawing.Size(75, 23);
            this.loadQCBtn.TabIndex = 35;
            this.loadQCBtn.Text = "Load QC";
            this.loadQCBtn.UseVisualStyleBackColor = true;
            this.loadQCBtn.Click += new System.EventHandler(this.loadQCBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "LatLon:";
            // 
            // latLonTxt
            // 
            this.latLonTxt.Location = new System.Drawing.Point(20, 432);
            this.latLonTxt.Name = "latLonTxt";
            this.latLonTxt.Size = new System.Drawing.Size(166, 20);
            this.latLonTxt.TabIndex = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxStreetViewAvailability);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.crossTypeCbx);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.designTypeCbx);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.bridgeTypeCbx);
            this.groupBox2.Location = new System.Drawing.Point(202, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 155);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bridge Attributes";
            // 
            // crossTypeCbx
            // 
            this.crossTypeCbx.Enabled = false;
            this.crossTypeCbx.FormattingEnabled = true;
            this.crossTypeCbx.Items.AddRange(new object[] {
            "Above Road (underpass)",
            "Above Water",
            "Under Road (overpass)"});
            this.crossTypeCbx.Location = new System.Drawing.Point(133, 30);
            this.crossTypeCbx.Name = "crossTypeCbx";
            this.crossTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.crossTypeCbx.TabIndex = 32;
            this.crossTypeCbx.SelectedIndexChanged += new System.EventHandler(this.crossTypeCbx_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Cross Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Bridge Design Type:";
            // 
            // designTypeCbx
            // 
            this.designTypeCbx.Enabled = false;
            this.designTypeCbx.FormattingEnabled = true;
            this.designTypeCbx.Items.AddRange(new object[] {
            "Concrete",
            "Concrete/Steel",
            "Other",
            "Steel Deck Girder",
            "Steel Deck Truss",
            "Steel Other",
            "Steel Through Girder",
            "Steel Through Truss",
            "Timber",
            "Not Sure",
            "Unknown"});
            this.designTypeCbx.Location = new System.Drawing.Point(133, 95);
            this.designTypeCbx.Name = "designTypeCbx";
            this.designTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.designTypeCbx.TabIndex = 2;
            this.designTypeCbx.SelectedIndexChanged += new System.EventHandler(this.designTypeCbx_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Bridge Type:";
            // 
            // bridgeTypeCbx
            // 
            this.bridgeTypeCbx.Enabled = false;
            this.bridgeTypeCbx.FormattingEnabled = true;
            this.bridgeTypeCbx.Items.AddRange(new object[] {
            "Fixed - Non moveable",
            "Moveable-Bascule",
            "Moveable-Vertical Lift",
            "Moveable-Swing"});
            this.bridgeTypeCbx.Location = new System.Drawing.Point(133, 63);
            this.bridgeTypeCbx.Name = "bridgeTypeCbx";
            this.bridgeTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.bridgeTypeCbx.TabIndex = 0;
            this.bridgeTypeCbx.SelectedIndexChanged += new System.EventHandler(this.bridgeTypeCbx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Search a bridge by UniqueID";
            // 
            // txtBridgeUniqueID
            // 
            this.txtBridgeUniqueID.Enabled = false;
            this.txtBridgeUniqueID.Location = new System.Drawing.Point(26, 85);
            this.txtBridgeUniqueID.Name = "txtBridgeUniqueID";
            this.txtBridgeUniqueID.Size = new System.Drawing.Size(90, 20);
            this.txtBridgeUniqueID.TabIndex = 41;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(127, 82);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 23);
            this.btnSearch.TabIndex = 42;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbxStreetViewAvailability
            // 
            this.cbxStreetViewAvailability.Enabled = false;
            this.cbxStreetViewAvailability.FormattingEnabled = true;
            this.cbxStreetViewAvailability.Items.AddRange(new object[] {
            "Available",
            "Not-Available"});
            this.cbxStreetViewAvailability.Location = new System.Drawing.Point(133, 125);
            this.cbxStreetViewAvailability.Name = "cbxStreetViewAvailability";
            this.cbxStreetViewAvailability.Size = new System.Drawing.Size(250, 21);
            this.cbxStreetViewAvailability.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "StreetView";
            // 
            // frmQC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 529);
            this.ControlBox = false;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtBridgeUniqueID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.latLonTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.loadQCBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processingStatusLbl);
            this.Controls.Add(this.PerformQCBtn);
            this.Controls.Add(this.hideBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.QCReportListView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQC";
            this.Text = "QC";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView QCReportListView;
        private System.Windows.Forms.ColumnHeader colBridgeID;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Subcategory;
        private System.Windows.Forms.ComboBox subcategoryCbx;
        private System.Windows.Forms.Button hideBtn;
        private System.Windows.Forms.Button PerformQCBtn;
        private System.Windows.Forms.Label processingStatusLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BackwardBtn;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.ColumnHeader colOperator;
        private System.Windows.Forms.RadioButton fixedRdo;
        private System.Windows.Forms.RadioButton legalRdo;
        private System.Windows.Forms.Button loadQCBtn;
        private System.Windows.Forms.Label ErrorNoTxt;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox latLonTxt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox crossTypeCbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox designTypeCbx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox bridgeTypeCbx;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBridgeUniqueID;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ColumnHeader colDesignType;
        private System.Windows.Forms.ComboBox cbxStreetViewAvailability;
        private System.Windows.Forms.Label label5;
    }
}