namespace QC_Processing
{
    partial class frmAssignBridgeAttributes
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
            this.bridgeListView = new System.Windows.Forms.ListView();
            this.colFeatureID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRROwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCrossType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBridgeType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDesignType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBridgeIdentifyType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ErrorIDLbl = new System.Windows.Forms.Label();
            this.NextBtn = new System.Windows.Forms.Button();
            this.BackwardBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.crossTypeCbx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.saveBridgeAttrBtn = new System.Windows.Forms.Button();
            this.ErrorNoTxt = new System.Windows.Forms.Label();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.designTypeCbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bridgeTypeCbx = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.latLonTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.loadQC20Btn = new System.Windows.Forms.Button();
            this.loadQC5Btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoNonBridge = new System.Windows.Forms.RadioButton();
            this.cbxStreetViewAvailability = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bridgeListView
            // 
            this.bridgeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFeatureID,
            this.colRROwner,
            this.colCrossType,
            this.colBridgeType,
            this.colDesignType,
            this.colBridgeIdentifyType});
            this.bridgeListView.FullRowSelect = true;
            this.bridgeListView.Location = new System.Drawing.Point(12, 12);
            this.bridgeListView.MultiSelect = false;
            this.bridgeListView.Name = "bridgeListView";
            this.bridgeListView.Size = new System.Drawing.Size(588, 343);
            this.bridgeListView.TabIndex = 1;
            this.bridgeListView.UseCompatibleStateImageBehavior = false;
            this.bridgeListView.View = System.Windows.Forms.View.Details;
            this.bridgeListView.DoubleClick += new System.EventHandler(this.bridgeListView_DoubleClick);
            // 
            // colFeatureID
            // 
            this.colFeatureID.Text = "Feature ID";
            this.colFeatureID.Width = 80;
            // 
            // colRROwner
            // 
            this.colRROwner.Text = "RROwner";
            this.colRROwner.Width = 100;
            // 
            // colCrossType
            // 
            this.colCrossType.Text = "Cross Type";
            this.colCrossType.Width = 100;
            // 
            // colBridgeType
            // 
            this.colBridgeType.Text = "Bridge Type";
            this.colBridgeType.Width = 100;
            // 
            // colDesignType
            // 
            this.colDesignType.Text = "Design Type";
            this.colDesignType.Width = 100;
            // 
            // colBridgeIdentifyType
            // 
            this.colBridgeIdentifyType.Text = "Type";
            this.colBridgeIdentifyType.Width = 80;
            // 
            // ErrorIDLbl
            // 
            this.ErrorIDLbl.AutoSize = true;
            this.ErrorIDLbl.Location = new System.Drawing.Point(8, 166);
            this.ErrorIDLbl.Name = "ErrorIDLbl";
            this.ErrorIDLbl.Size = new System.Drawing.Size(101, 13);
            this.ErrorIDLbl.TabIndex = 20;
            this.ErrorIDLbl.Text = "Editing FeatureID: 0";
            // 
            // NextBtn
            // 
            this.NextBtn.Enabled = false;
            this.NextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.Location = new System.Drawing.Point(233, 159);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(35, 22);
            this.NextBtn.TabIndex = 19;
            this.NextBtn.Text = ">";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // BackwardBtn
            // 
            this.BackwardBtn.Enabled = false;
            this.BackwardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackwardBtn.Location = new System.Drawing.Point(193, 160);
            this.BackwardBtn.Name = "BackwardBtn";
            this.BackwardBtn.Size = new System.Drawing.Size(34, 21);
            this.BackwardBtn.TabIndex = 18;
            this.BackwardBtn.Text = "<";
            this.BackwardBtn.UseVisualStyleBackColor = true;
            this.BackwardBtn.Click += new System.EventHandler(this.BackwardBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxStreetViewAvailability);
            this.groupBox2.Controls.Add(this.crossTypeCbx);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.ErrorIDLbl);
            this.groupBox2.Controls.Add(this.saveBridgeAttrBtn);
            this.groupBox2.Controls.Add(this.ErrorNoTxt);
            this.groupBox2.Controls.Add(this.NextBtn);
            this.groupBox2.Controls.Add(this.refreshBtn);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.designTypeCbx);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.BackwardBtn);
            this.groupBox2.Controls.Add(this.bridgeTypeCbx);
            this.groupBox2.Location = new System.Drawing.Point(12, 370);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 235);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Assign Bridge Attributes";
            // 
            // crossTypeCbx
            // 
            this.crossTypeCbx.Enabled = false;
            this.crossTypeCbx.FormattingEnabled = true;
            this.crossTypeCbx.Items.AddRange(new object[] {
            "Above Road (underpass)",
            "Above Water",
            "Under Road (overpass)"});
            this.crossTypeCbx.Location = new System.Drawing.Point(115, 30);
            this.crossTypeCbx.Name = "crossTypeCbx";
            this.crossTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.crossTypeCbx.TabIndex = 32;
            this.crossTypeCbx.SelectedIndexChanged += new System.EventHandler(this.crossTypeCbx_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Cross Type:";
            // 
            // saveBridgeAttrBtn
            // 
            this.saveBridgeAttrBtn.Enabled = false;
            this.saveBridgeAttrBtn.Location = new System.Drawing.Point(290, 160);
            this.saveBridgeAttrBtn.Name = "saveBridgeAttrBtn";
            this.saveBridgeAttrBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBridgeAttrBtn.TabIndex = 30;
            this.saveBridgeAttrBtn.Text = "Save";
            this.saveBridgeAttrBtn.UseVisualStyleBackColor = true;
            this.saveBridgeAttrBtn.Click += new System.EventHandler(this.saveBridgeAttrBtn_Click);
            // 
            // ErrorNoTxt
            // 
            this.ErrorNoTxt.AutoSize = true;
            this.ErrorNoTxt.Location = new System.Drawing.Point(52, 206);
            this.ErrorNoTxt.Name = "ErrorNoTxt";
            this.ErrorNoTxt.Size = new System.Drawing.Size(88, 13);
            this.ErrorNoTxt.TabIndex = 21;
            this.ErrorNoTxt.Text = "0 features in total";
            // 
            // refreshBtn
            // 
            this.refreshBtn.Enabled = false;
            this.refreshBtn.Location = new System.Drawing.Point(290, 201);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 23);
            this.refreshBtn.TabIndex = 24;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "StreetView";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bridge Design Type:";
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
            this.designTypeCbx.Location = new System.Drawing.Point(115, 95);
            this.designTypeCbx.Name = "designTypeCbx";
            this.designTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.designTypeCbx.TabIndex = 2;
            this.designTypeCbx.SelectedIndexChanged += new System.EventHandler(this.designTypeCbx_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Bridge Type:";
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
            this.bridgeTypeCbx.Location = new System.Drawing.Point(115, 63);
            this.bridgeTypeCbx.Name = "bridgeTypeCbx";
            this.bridgeTypeCbx.Size = new System.Drawing.Size(250, 21);
            this.bridgeTypeCbx.TabIndex = 0;
            this.bridgeTypeCbx.SelectedIndexChanged += new System.EventHandler(this.bridgeTypeCbx_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(546, 593);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "Hide";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // latLonTxt
            // 
            this.latLonTxt.Location = new System.Drawing.Point(413, 565);
            this.latLonTxt.Name = "latLonTxt";
            this.latLonTxt.Size = new System.Drawing.Size(187, 20);
            this.latLonTxt.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(410, 537);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Latitude, Longitude";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(461, 370);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 23);
            this.button3.TabIndex = 32;
            this.button3.Text = "Load All Bridges";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // loadQC20Btn
            // 
            this.loadQC20Btn.Enabled = false;
            this.loadQC20Btn.Location = new System.Drawing.Point(461, 402);
            this.loadQC20Btn.Name = "loadQC20Btn";
            this.loadQC20Btn.Size = new System.Drawing.Size(110, 23);
            this.loadQC20Btn.TabIndex = 33;
            this.loadQC20Btn.Text = "Load 20% QC";
            this.loadQC20Btn.UseVisualStyleBackColor = true;
            this.loadQC20Btn.Click += new System.EventHandler(this.loadQC20Btn_Click);
            // 
            // loadQC5Btn
            // 
            this.loadQC5Btn.Enabled = false;
            this.loadQC5Btn.Location = new System.Drawing.Point(461, 435);
            this.loadQC5Btn.Name = "loadQC5Btn";
            this.loadQC5Btn.Size = new System.Drawing.Size(110, 23);
            this.loadQC5Btn.TabIndex = 34;
            this.loadQC5Btn.Text = "Load 5% QC";
            this.loadQC5Btn.UseVisualStyleBackColor = true;
            this.loadQC5Btn.Click += new System.EventHandler(this.loadQC5Btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoNonBridge);
            this.groupBox3.Location = new System.Drawing.Point(407, 474);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(193, 49);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Feature Type";
            // 
            // rdoNonBridge
            // 
            this.rdoNonBridge.AutoSize = true;
            this.rdoNonBridge.Enabled = false;
            this.rdoNonBridge.Location = new System.Drawing.Point(6, 28);
            this.rdoNonBridge.Name = "rdoNonBridge";
            this.rdoNonBridge.Size = new System.Drawing.Size(78, 17);
            this.rdoNonBridge.TabIndex = 6;
            this.rdoNonBridge.TabStop = true;
            this.rdoNonBridge.Text = "Non-Bridge";
            this.rdoNonBridge.UseVisualStyleBackColor = true;
            this.rdoNonBridge.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // cbxStreetViewAvailability
            // 
            this.cbxStreetViewAvailability.Enabled = false;
            this.cbxStreetViewAvailability.FormattingEnabled = true;
            this.cbxStreetViewAvailability.Items.AddRange(new object[] {
            "Available",
            "Not-Available"});
            this.cbxStreetViewAvailability.Location = new System.Drawing.Point(115, 127);
            this.cbxStreetViewAvailability.Name = "cbxStreetViewAvailability";
            this.cbxStreetViewAvailability.Size = new System.Drawing.Size(250, 21);
            this.cbxStreetViewAvailability.TabIndex = 33;
            // 
            // frmAssignBridgeAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 617);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.loadQC5Btn);
            this.Controls.Add(this.loadQC20Btn);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.latLonTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bridgeListView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAssignBridgeAttributes";
            this.Text = "Assign/Check Bridge Attributes";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView bridgeListView;
        private System.Windows.Forms.Label ErrorIDLbl;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button BackwardBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox designTypeCbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox bridgeTypeCbx;
        private System.Windows.Forms.Label ErrorNoTxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.ColumnHeader colFeatureID;
        private System.Windows.Forms.ColumnHeader colBridgeType;
        private System.Windows.Forms.ColumnHeader colRROwner;
        private System.Windows.Forms.ColumnHeader colDesignType;
        private System.Windows.Forms.Button saveBridgeAttrBtn;
        private System.Windows.Forms.ColumnHeader colBridgeIdentifyType;
        private System.Windows.Forms.TextBox latLonTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox crossTypeCbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader colCrossType;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button loadQC20Btn;
        private System.Windows.Forms.Button loadQC5Btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoNonBridge;
        private System.Windows.Forms.ComboBox cbxStreetViewAvailability;
    }
}