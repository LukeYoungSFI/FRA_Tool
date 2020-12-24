namespace QC_Processing
{
    partial class frmPopulateOtherAttributes
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
            this.populateOtherAttrBtn = new System.Windows.Forms.Button();
            this.hideBtn = new System.Windows.Forms.Button();
            this.suffixlistNM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxNothing = new System.Windows.Forms.CheckBox();
            this.cbxAll = new System.Windows.Forms.CheckBox();
            this.cbxNone = new System.Windows.Forms.CheckBox();
            this.cbxFIPS = new System.Windows.Forms.CheckBox();
            this.cbxState = new System.Windows.Forms.CheckBox();
            this.cbxSource = new System.Windows.Forms.CheckBox();
            this.cbxWaterPost = new System.Windows.Forms.CheckBox();
            this.cbxRailPost = new System.Windows.Forms.CheckBox();
            this.cbxNoTracks = new System.Windows.Forms.CheckBox();
            this.cbxSubDivision = new System.Windows.Forms.CheckBox();
            this.cbxStreetNM = new System.Windows.Forms.CheckBox();
            this.cbxLatLng = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // populateOtherAttrBtn
            // 
            this.populateOtherAttrBtn.Location = new System.Drawing.Point(326, 100);
            this.populateOtherAttrBtn.Name = "populateOtherAttrBtn";
            this.populateOtherAttrBtn.Size = new System.Drawing.Size(82, 23);
            this.populateOtherAttrBtn.TabIndex = 0;
            this.populateOtherAttrBtn.Text = "Populate";
            this.populateOtherAttrBtn.UseVisualStyleBackColor = true;
            this.populateOtherAttrBtn.Click += new System.EventHandler(this.populateOtherAttrBtn_Click);
            // 
            // hideBtn
            // 
            this.hideBtn.Location = new System.Drawing.Point(438, 282);
            this.hideBtn.Name = "hideBtn";
            this.hideBtn.Size = new System.Drawing.Size(56, 23);
            this.hideBtn.TabIndex = 1;
            this.hideBtn.Text = "Hide";
            this.hideBtn.UseVisualStyleBackColor = true;
            this.hideBtn.Click += new System.EventHandler(this.hideBtn_Click);
            // 
            // suffixlistNM
            // 
            this.suffixlistNM.Location = new System.Drawing.Point(13, 41);
            this.suffixlistNM.Name = "suffixlistNM";
            this.suffixlistNM.Size = new System.Drawing.Size(364, 20);
            this.suffixlistNM.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "SuffixDictionaryFile";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(412, 38);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxNothing);
            this.groupBox1.Controls.Add(this.cbxAll);
            this.groupBox1.Controls.Add(this.cbxNone);
            this.groupBox1.Controls.Add(this.cbxFIPS);
            this.groupBox1.Controls.Add(this.cbxState);
            this.groupBox1.Controls.Add(this.cbxSource);
            this.groupBox1.Controls.Add(this.cbxWaterPost);
            this.groupBox1.Controls.Add(this.cbxRailPost);
            this.groupBox1.Controls.Add(this.cbxNoTracks);
            this.groupBox1.Controls.Add(this.cbxSubDivision);
            this.groupBox1.Controls.Add(this.cbxStreetNM);
            this.groupBox1.Controls.Add(this.cbxLatLng);
            this.groupBox1.Location = new System.Drawing.Point(13, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 190);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Populate following columns";
            // 
            // cbxNothing
            // 
            this.cbxNothing.AutoSize = true;
            this.cbxNothing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNothing.Location = new System.Drawing.Point(146, 20);
            this.cbxNothing.Name = "cbxNothing";
            this.cbxNothing.Size = new System.Drawing.Size(110, 17);
            this.cbxNothing.TabIndex = 11;
            this.cbxNothing.Text = "Select Nothing";
            this.cbxNothing.UseVisualStyleBackColor = true;
            this.cbxNothing.CheckedChanged += new System.EventHandler(this.cbxNothing_CheckedChanged);
            // 
            // cbxAll
            // 
            this.cbxAll.AutoSize = true;
            this.cbxAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAll.Location = new System.Drawing.Point(17, 20);
            this.cbxAll.Name = "cbxAll";
            this.cbxAll.Size = new System.Drawing.Size(80, 17);
            this.cbxAll.TabIndex = 10;
            this.cbxAll.Text = "Select All";
            this.cbxAll.UseVisualStyleBackColor = true;
            this.cbxAll.CheckedChanged += new System.EventHandler(this.cbxAll_CheckedChanged);
            // 
            // cbxNone
            // 
            this.cbxNone.AutoSize = true;
            this.cbxNone.Location = new System.Drawing.Point(146, 144);
            this.cbxNone.Name = "cbxNone";
            this.cbxNone.Size = new System.Drawing.Size(128, 17);
            this.cbxNone.TabIndex = 9;
            this.cbxNone.Text = "Empty Value as None";
            this.cbxNone.UseVisualStyleBackColor = true;
            this.cbxNone.CheckedChanged += new System.EventHandler(this.cbxNone_CheckedChanged);
            // 
            // cbxFIPS
            // 
            this.cbxFIPS.AutoSize = true;
            this.cbxFIPS.Location = new System.Drawing.Point(146, 120);
            this.cbxFIPS.Name = "cbxFIPS";
            this.cbxFIPS.Size = new System.Drawing.Size(49, 17);
            this.cbxFIPS.TabIndex = 8;
            this.cbxFIPS.Text = "FIPS";
            this.cbxFIPS.UseVisualStyleBackColor = true;
            this.cbxFIPS.CheckedChanged += new System.EventHandler(this.cbxFIPS_CheckedChanged);
            // 
            // cbxState
            // 
            this.cbxState.AutoSize = true;
            this.cbxState.Location = new System.Drawing.Point(146, 96);
            this.cbxState.Name = "cbxState";
            this.cbxState.Size = new System.Drawing.Size(51, 17);
            this.cbxState.TabIndex = 7;
            this.cbxState.Text = "State";
            this.cbxState.UseVisualStyleBackColor = true;
            this.cbxState.CheckedChanged += new System.EventHandler(this.cbxState_CheckedChanged);
            // 
            // cbxSource
            // 
            this.cbxSource.AutoSize = true;
            this.cbxSource.Location = new System.Drawing.Point(146, 72);
            this.cbxSource.Name = "cbxSource";
            this.cbxSource.Size = new System.Drawing.Size(60, 17);
            this.cbxSource.TabIndex = 6;
            this.cbxSource.Text = "Source";
            this.cbxSource.UseVisualStyleBackColor = true;
            this.cbxSource.CheckedChanged += new System.EventHandler(this.cbxSource_CheckedChanged);
            // 
            // cbxWaterPost
            // 
            this.cbxWaterPost.AutoSize = true;
            this.cbxWaterPost.Location = new System.Drawing.Point(146, 48);
            this.cbxWaterPost.Name = "cbxWaterPost";
            this.cbxWaterPost.Size = new System.Drawing.Size(101, 17);
            this.cbxWaterPost.TabIndex = 5;
            this.cbxWaterPost.Text = "Water_MilePost";
            this.cbxWaterPost.UseVisualStyleBackColor = true;
            this.cbxWaterPost.CheckedChanged += new System.EventHandler(this.cbxWaterPost_CheckedChanged);
            // 
            // cbxRailPost
            // 
            this.cbxRailPost.AutoSize = true;
            this.cbxRailPost.Location = new System.Drawing.Point(17, 144);
            this.cbxRailPost.Name = "cbxRailPost";
            this.cbxRailPost.Size = new System.Drawing.Size(90, 17);
            this.cbxRailPost.TabIndex = 4;
            this.cbxRailPost.Text = "Rail_MilePost";
            this.cbxRailPost.UseVisualStyleBackColor = true;
            this.cbxRailPost.CheckedChanged += new System.EventHandler(this.cbxRailPost_CheckedChanged);
            // 
            // cbxNoTracks
            // 
            this.cbxNoTracks.AutoSize = true;
            this.cbxNoTracks.Location = new System.Drawing.Point(17, 120);
            this.cbxNoTracks.Name = "cbxNoTracks";
            this.cbxNoTracks.Size = new System.Drawing.Size(81, 17);
            this.cbxNoTracks.TabIndex = 3;
            this.cbxNoTracks.Text = "NumTracks";
            this.cbxNoTracks.UseVisualStyleBackColor = true;
            this.cbxNoTracks.CheckedChanged += new System.EventHandler(this.cbxNoTracks_CheckedChanged);
            // 
            // cbxSubDivision
            // 
            this.cbxSubDivision.AutoSize = true;
            this.cbxSubDivision.Location = new System.Drawing.Point(17, 96);
            this.cbxSubDivision.Name = "cbxSubDivision";
            this.cbxSubDivision.Size = new System.Drawing.Size(80, 17);
            this.cbxSubDivision.TabIndex = 2;
            this.cbxSubDivision.Text = "Subdivision";
            this.cbxSubDivision.UseVisualStyleBackColor = true;
            this.cbxSubDivision.CheckedChanged += new System.EventHandler(this.cbxSubDivision_CheckedChanged);
            // 
            // cbxStreetNM
            // 
            this.cbxStreetNM.AutoSize = true;
            this.cbxStreetNM.Location = new System.Drawing.Point(17, 72);
            this.cbxStreetNM.Name = "cbxStreetNM";
            this.cbxStreetNM.Size = new System.Drawing.Size(90, 17);
            this.cbxStreetNM.TabIndex = 1;
            this.cbxStreetNM.Text = "Street Names";
            this.cbxStreetNM.UseVisualStyleBackColor = true;
            this.cbxStreetNM.CheckedChanged += new System.EventHandler(this.cbxStreetNM_CheckedChanged);
            // 
            // cbxLatLng
            // 
            this.cbxLatLng.AutoSize = true;
            this.cbxLatLng.Location = new System.Drawing.Point(17, 48);
            this.cbxLatLng.Name = "cbxLatLng";
            this.cbxLatLng.Size = new System.Drawing.Size(64, 17);
            this.cbxLatLng.TabIndex = 0;
            this.cbxLatLng.Text = "Lat/Lng";
            this.cbxLatLng.UseVisualStyleBackColor = true;
            this.cbxLatLng.CheckedChanged += new System.EventHandler(this.cbxLatLng_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Fix Highway Bridges";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(326, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Fix Waterway Bridges";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmPopulateOtherAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 335);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.suffixlistNM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.hideBtn);
            this.Controls.Add(this.populateOtherAttrBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPopulateOtherAttributes";
            this.Text = "Auto-populate Other Required Attributes";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button populateOtherAttrBtn;
        private System.Windows.Forms.Button hideBtn;
        private System.Windows.Forms.TextBox suffixlistNM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbxStreetNM;
        private System.Windows.Forms.CheckBox cbxLatLng;
        private System.Windows.Forms.CheckBox cbxNone;
        private System.Windows.Forms.CheckBox cbxFIPS;
        private System.Windows.Forms.CheckBox cbxState;
        private System.Windows.Forms.CheckBox cbxSource;
        private System.Windows.Forms.CheckBox cbxWaterPost;
        private System.Windows.Forms.CheckBox cbxRailPost;
        private System.Windows.Forms.CheckBox cbxNoTracks;
        private System.Windows.Forms.CheckBox cbxSubDivision;
        private System.Windows.Forms.CheckBox cbxNothing;
        private System.Windows.Forms.CheckBox cbxAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}