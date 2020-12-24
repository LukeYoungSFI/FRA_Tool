namespace QC_Processing
{
    partial class frmIdentifyBridges
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
            System.Windows.Forms.ColumnHeader colFeatureID;
            System.Windows.Forms.ColumnHeader colFeatureType;
            this.ErrorNoTxt = new System.Windows.Forms.Label();
            this.ListBridgeTargetPnts = new System.Windows.Forms.ListView();
            this.colRROwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStreetName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NextBtn = new System.Windows.Forms.Button();
            this.BackwardBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.featureCategoryCbx = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.NumZoomingAdjust = new System.Windows.Forms.NumericUpDown();
            this.LblZoomingAdjust = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.ErrorIDLbl = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.latLonTxt = new System.Windows.Forms.TextBox();
            colFeatureID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colFeatureType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumZoomingAdjust)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // colFeatureID
            // 
            colFeatureID.Text = "FeatureID";
            // 
            // colFeatureType
            // 
            colFeatureType.Text = "Feature Type";
            colFeatureType.Width = 100;
            // 
            // ErrorNoTxt
            // 
            this.ErrorNoTxt.AutoSize = true;
            this.ErrorNoTxt.Location = new System.Drawing.Point(14, 251);
            this.ErrorNoTxt.Name = "ErrorNoTxt";
            this.ErrorNoTxt.Size = new System.Drawing.Size(88, 13);
            this.ErrorNoTxt.TabIndex = 8;
            this.ErrorNoTxt.Text = "0 features in total";
            // 
            // ListBridgeTargetPnts
            // 
            this.ListBridgeTargetPnts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colFeatureID,
            colFeatureType,
            this.colRROwner,
            this.colStreetName});
            this.ListBridgeTargetPnts.FullRowSelect = true;
            this.ListBridgeTargetPnts.HideSelection = false;
            this.ListBridgeTargetPnts.Location = new System.Drawing.Point(32, 42);
            this.ListBridgeTargetPnts.MultiSelect = false;
            this.ListBridgeTargetPnts.Name = "ListBridgeTargetPnts";
            this.ListBridgeTargetPnts.Size = new System.Drawing.Size(406, 311);
            this.ListBridgeTargetPnts.TabIndex = 8;
            this.ListBridgeTargetPnts.UseCompatibleStateImageBehavior = false;
            this.ListBridgeTargetPnts.View = System.Windows.Forms.View.Details;
            this.ListBridgeTargetPnts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LstErrors_doubleClick);
            // 
            // colRROwner
            // 
            this.colRROwner.Text = "RROwner";
            // 
            // colStreetName
            // 
            this.colStreetName.Text = "StreetName";
            this.colStreetName.Width = 150;
            // 
            // NextBtn
            // 
            this.NextBtn.Enabled = false;
            this.NextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.Location = new System.Drawing.Point(52, 25);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(30, 23);
            this.NextBtn.TabIndex = 4;
            this.NextBtn.Text = ">";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // BackwardBtn
            // 
            this.BackwardBtn.Enabled = false;
            this.BackwardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackwardBtn.Location = new System.Drawing.Point(17, 25);
            this.BackwardBtn.Name = "BackwardBtn";
            this.BackwardBtn.Size = new System.Drawing.Size(29, 23);
            this.BackwardBtn.TabIndex = 3;
            this.BackwardBtn.Text = "<";
            this.BackwardBtn.UseVisualStyleBackColor = true;
            this.BackwardBtn.Click += new System.EventHandler(this.BackwardBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.featureCategoryCbx);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.refreshBtn);
            this.groupBox1.Controls.Add(this.NumZoomingAdjust);
            this.groupBox1.Controls.Add(this.LblZoomingAdjust);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.ErrorNoTxt);
            this.groupBox1.Controls.Add(this.ErrorIDLbl);
            this.groupBox1.Controls.Add(this.NextBtn);
            this.groupBox1.Controls.Add(this.BackwardBtn);
            this.groupBox1.Location = new System.Drawing.Point(447, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 311);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Editing Target Features";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // featureCategoryCbx
            // 
            this.featureCategoryCbx.FormattingEnabled = true;
            this.featureCategoryCbx.Items.AddRange(new object[] {
            "auto-conflated",
            "auto-conflated-hunter",
            "Y",
            "N",
            "non-bridge",
            "undefined"});
            this.featureCategoryCbx.Location = new System.Drawing.Point(12, 106);
            this.featureCategoryCbx.Name = "featureCategoryCbx";
            this.featureCategoryCbx.Size = new System.Drawing.Size(121, 21);
            this.featureCategoryCbx.TabIndex = 19;
            this.featureCategoryCbx.SelectedIndexChanged += new System.EventHandler(this.featureCategoryCbx_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Feature Category";
            // 
            // refreshBtn
            // 
            this.refreshBtn.Enabled = false;
            this.refreshBtn.Location = new System.Drawing.Point(19, 277);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 23);
            this.refreshBtn.TabIndex = 17;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // NumZoomingAdjust
            // 
            this.NumZoomingAdjust.DecimalPlaces = 4;
            this.NumZoomingAdjust.Enabled = false;
            this.NumZoomingAdjust.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.NumZoomingAdjust.Location = new System.Drawing.Point(97, 223);
            this.NumZoomingAdjust.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.NumZoomingAdjust.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.NumZoomingAdjust.Name = "NumZoomingAdjust";
            this.NumZoomingAdjust.Size = new System.Drawing.Size(70, 20);
            this.NumZoomingAdjust.TabIndex = 15;
            this.NumZoomingAdjust.Value = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            // 
            // LblZoomingAdjust
            // 
            this.LblZoomingAdjust.AutoSize = true;
            this.LblZoomingAdjust.Location = new System.Drawing.Point(14, 225);
            this.LblZoomingAdjust.Name = "LblZoomingAdjust";
            this.LblZoomingAdjust.Size = new System.Drawing.Size(83, 13);
            this.LblZoomingAdjust.TabIndex = 16;
            this.LblZoomingAdjust.Text = "Zooming Adjust:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Location = new System.Drawing.Point(6, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(161, 61);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flag Feature Type";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(13, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(55, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Bridge";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(13, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(78, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Non-Bridge";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // ErrorIDLbl
            // 
            this.ErrorIDLbl.AutoSize = true;
            this.ErrorIDLbl.Location = new System.Drawing.Point(14, 63);
            this.ErrorIDLbl.Name = "ErrorIDLbl";
            this.ErrorIDLbl.Size = new System.Drawing.Size(101, 13);
            this.ErrorIDLbl.TabIndex = 7;
            this.ErrorIDLbl.Text = "Editing FeatureID: 0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(566, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Hide";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 370);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Latitude, Longitude";
            // 
            // latLonTxt
            // 
            this.latLonTxt.Location = new System.Drawing.Point(136, 367);
            this.latLonTxt.Name = "latLonTxt";
            this.latLonTxt.Size = new System.Drawing.Size(302, 20);
            this.latLonTxt.TabIndex = 13;
            // 
            // frmIdentifyBridges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 395);
            this.ControlBox = false;
            this.Controls.Add(this.latLonTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ListBridgeTargetPnts);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIdentifyBridges";
            this.Text = "Identify Bridges";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumZoomingAdjust)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ErrorNoTxt;
        private System.Windows.Forms.ListView ListBridgeTargetPnts;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button BackwardBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label ErrorIDLbl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader colRROwner;
        private System.Windows.Forms.ColumnHeader colStreetName;
        private System.Windows.Forms.NumericUpDown NumZoomingAdjust;
        private System.Windows.Forms.Label LblZoomingAdjust;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox latLonTxt;
        private System.Windows.Forms.ComboBox featureCategoryCbx;
        private System.Windows.Forms.Label label2;
    }
}