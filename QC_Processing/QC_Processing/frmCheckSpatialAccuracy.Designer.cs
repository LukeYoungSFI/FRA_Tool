namespace QC_Processing
{
    partial class frmCheckSpatialAccuracy
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
            this.txtBridgeUniqueID = new System.Windows.Forms.TextBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.latLonTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.processingStatusLbl = new System.Windows.Forms.Label();
            this.CheckSpatialAccuracyBtn = new System.Windows.Forms.Button();
            this.nonBridgeRdo = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.hideBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.ErrorNoTxt = new System.Windows.Forms.Label();
            this.BackwardBtn = new System.Windows.Forms.Button();
            this.createBridgeBtn = new System.Windows.Forms.Button();
            this.legalRdo = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colOperator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBridgeID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QCReportListView = new System.Windows.Forms.ListView();
            this.LoadSpatialAccuracyReportBtn = new System.Windows.Forms.Button();
            this.checkRRAccuracy = new System.Windows.Forms.Button();
            this.R = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBridgeUniqueID
            // 
            this.txtBridgeUniqueID.Enabled = false;
            this.txtBridgeUniqueID.Location = new System.Drawing.Point(29, 152);
            this.txtBridgeUniqueID.Name = "txtBridgeUniqueID";
            this.txtBridgeUniqueID.Size = new System.Drawing.Size(90, 20);
            this.txtBridgeUniqueID.TabIndex = 56;
            // 
            // refreshBtn
            // 
            this.refreshBtn.Location = new System.Drawing.Point(117, 332);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(72, 23);
            this.refreshBtn.TabIndex = 49;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // latLonTxt
            // 
            this.latLonTxt.Location = new System.Drawing.Point(23, 364);
            this.latLonTxt.Name = "latLonTxt";
            this.latLonTxt.Size = new System.Drawing.Size(166, 20);
            this.latLonTxt.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "LatLon:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 48;
            // 
            // processingStatusLbl
            // 
            this.processingStatusLbl.AutoSize = true;
            this.processingStatusLbl.Location = new System.Drawing.Point(30, 109);
            this.processingStatusLbl.Name = "processingStatusLbl";
            this.processingStatusLbl.Size = new System.Drawing.Size(0, 13);
            this.processingStatusLbl.TabIndex = 47;
            // 
            // CheckSpatialAccuracyBtn
            // 
            this.CheckSpatialAccuracyBtn.Location = new System.Drawing.Point(29, 15);
            this.CheckSpatialAccuracyBtn.Name = "CheckSpatialAccuracyBtn";
            this.CheckSpatialAccuracyBtn.Size = new System.Drawing.Size(160, 23);
            this.CheckSpatialAccuracyBtn.TabIndex = 46;
            this.CheckSpatialAccuracyBtn.Text = "Check Spatial Accuracy";
            this.CheckSpatialAccuracyBtn.UseVisualStyleBackColor = true;
            this.CheckSpatialAccuracyBtn.Click += new System.EventHandler(this.CheckSpatialAccuracyBtn_Click);
            // 
            // nonBridgeRdo
            // 
            this.nonBridgeRdo.AutoSize = true;
            this.nonBridgeRdo.Enabled = false;
            this.nonBridgeRdo.Location = new System.Drawing.Point(12, 25);
            this.nonBridgeRdo.Name = "nonBridgeRdo";
            this.nonBridgeRdo.Size = new System.Drawing.Size(78, 17);
            this.nonBridgeRdo.TabIndex = 6;
            this.nonBridgeRdo.TabStop = true;
            this.nonBridgeRdo.Text = "Non-Bridge";
            this.nonBridgeRdo.UseVisualStyleBackColor = true;
            this.nonBridgeRdo.CheckedChanged += new System.EventHandler(this.nonBridgeRdo_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Search a bridge by UniqueID";
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(130, 152);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 23);
            this.btnSearch.TabIndex = 57;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // hideBtn
            // 
            this.hideBtn.Location = new System.Drawing.Point(477, 354);
            this.hideBtn.Name = "hideBtn";
            this.hideBtn.Size = new System.Drawing.Size(54, 31);
            this.hideBtn.TabIndex = 45;
            this.hideBtn.Text = "Hide";
            this.hideBtn.UseVisualStyleBackColor = true;
            this.hideBtn.Click += new System.EventHandler(this.hideBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.Location = new System.Drawing.Point(149, 225);
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
            this.ErrorNoTxt.Location = new System.Drawing.Point(20, 319);
            this.ErrorNoTxt.Name = "ErrorNoTxt";
            this.ErrorNoTxt.Size = new System.Drawing.Size(88, 13);
            this.ErrorNoTxt.TabIndex = 38;
            this.ErrorNoTxt.Text = "0 features in total";
            // 
            // BackwardBtn
            // 
            this.BackwardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackwardBtn.Location = new System.Drawing.Point(149, 187);
            this.BackwardBtn.Name = "BackwardBtn";
            this.BackwardBtn.Size = new System.Drawing.Size(40, 30);
            this.BackwardBtn.TabIndex = 32;
            this.BackwardBtn.Text = "<";
            this.BackwardBtn.UseVisualStyleBackColor = true;
            this.BackwardBtn.Click += new System.EventHandler(this.BackwardBtn_Click);
            // 
            // createBridgeBtn
            // 
            this.createBridgeBtn.Enabled = false;
            this.createBridgeBtn.Location = new System.Drawing.Point(23, 260);
            this.createBridgeBtn.Name = "createBridgeBtn";
            this.createBridgeBtn.Size = new System.Drawing.Size(166, 23);
            this.createBridgeBtn.TabIndex = 30;
            this.createBridgeBtn.Text = "Re-Generate a Bridge";
            this.createBridgeBtn.UseVisualStyleBackColor = true;
            this.createBridgeBtn.Click += new System.EventHandler(this.createBridgeBtn_Click);
            // 
            // legalRdo
            // 
            this.legalRdo.AutoSize = true;
            this.legalRdo.Enabled = false;
            this.legalRdo.Location = new System.Drawing.Point(12, 48);
            this.legalRdo.Name = "legalRdo";
            this.legalRdo.Size = new System.Drawing.Size(51, 17);
            this.legalRdo.TabIndex = 36;
            this.legalRdo.TabStop = true;
            this.legalRdo.Text = "Legal";
            this.legalRdo.UseVisualStyleBackColor = true;
            this.legalRdo.CheckedChanged += new System.EventHandler(this.legalRdo_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nonBridgeRdo);
            this.groupBox1.Controls.Add(this.legalRdo);
            this.groupBox1.Location = new System.Drawing.Point(23, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 73);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flagging Error";
            // 
            // colOperator
            // 
            this.colOperator.Text = "Operator";
            this.colOperator.Width = 90;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 110;
            // 
            // colBridgeID
            // 
            this.colBridgeID.Text = "TargetFeatureID";
            this.colBridgeID.Width = 110;
            // 
            // QCReportListView
            // 
            this.QCReportListView.AllowColumnReorder = true;
            this.QCReportListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBridgeID,
            this.colStatus,
            this.colOperator});
            this.QCReportListView.Enabled = false;
            this.QCReportListView.FullRowSelect = true;
            this.QCReportListView.HideSelection = false;
            this.QCReportListView.Location = new System.Drawing.Point(202, 15);
            this.QCReportListView.MultiSelect = false;
            this.QCReportListView.Name = "QCReportListView";
            this.QCReportListView.Size = new System.Drawing.Size(343, 333);
            this.QCReportListView.TabIndex = 43;
            this.QCReportListView.UseCompatibleStateImageBehavior = false;
            this.QCReportListView.View = System.Windows.Forms.View.Details;
            this.QCReportListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.QCReportListView_MouseDoubleClick);
            // 
            // LoadSpatialAccuracyReportBtn
            // 
            this.LoadSpatialAccuracyReportBtn.Location = new System.Drawing.Point(29, 50);
            this.LoadSpatialAccuracyReportBtn.Name = "LoadSpatialAccuracyReportBtn";
            this.LoadSpatialAccuracyReportBtn.Size = new System.Drawing.Size(160, 23);
            this.LoadSpatialAccuracyReportBtn.TabIndex = 58;
            this.LoadSpatialAccuracyReportBtn.Text = "Load Spatial Accuracy Report";
            this.LoadSpatialAccuracyReportBtn.UseVisualStyleBackColor = true;
            this.LoadSpatialAccuracyReportBtn.Click += new System.EventHandler(this.LoadSpatialAccuracyReportBtn_Click);
            // 
            // checkRRAccuracy
            // 
            this.checkRRAccuracy.Location = new System.Drawing.Point(29, 83);
            this.checkRRAccuracy.Name = "checkRRAccuracy";
            this.checkRRAccuracy.Size = new System.Drawing.Size(160, 23);
            this.checkRRAccuracy.TabIndex = 60;
            this.checkRRAccuracy.Text = "Check RR Accuracy";
            this.checkRRAccuracy.UseVisualStyleBackColor = true;
            this.checkRRAccuracy.Click += new System.EventHandler(this.checkRRAccuracy_Click);
            // 
            // R
            // 
            this.R.Location = new System.Drawing.Point(23, 289);
            this.R.Name = "R";
            this.R.Size = new System.Drawing.Size(166, 23);
            this.R.TabIndex = 61;
            this.R.Text = "Re-Generate a RR Bridge";
            this.R.UseVisualStyleBackColor = true;
            this.R.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCheckSpatialAccuracy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 396);
            this.Controls.Add(this.R);
            this.Controls.Add(this.checkRRAccuracy);
            this.Controls.Add(this.LoadSpatialAccuracyReportBtn);
            this.Controls.Add(this.txtBridgeUniqueID);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.ErrorNoTxt);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.latLonTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BackwardBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processingStatusLbl);
            this.Controls.Add(this.createBridgeBtn);
            this.Controls.Add(this.CheckSpatialAccuracyBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.hideBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.QCReportListView);
            this.Name = "frmCheckSpatialAccuracy";
            this.Text = "Check Spatial Accuracy";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBridgeUniqueID;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.TextBox latLonTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label processingStatusLbl;
        private System.Windows.Forms.Button CheckSpatialAccuracyBtn;
        private System.Windows.Forms.RadioButton nonBridgeRdo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button hideBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Label ErrorNoTxt;
        private System.Windows.Forms.Button BackwardBtn;
        private System.Windows.Forms.Button createBridgeBtn;
        private System.Windows.Forms.RadioButton legalRdo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader colOperator;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colBridgeID;
        private System.Windows.Forms.ListView QCReportListView;
        private System.Windows.Forms.Button LoadSpatialAccuracyReportBtn;
        private System.Windows.Forms.Button checkRRAccuracy;
        private System.Windows.Forms.Button R;
    }
}