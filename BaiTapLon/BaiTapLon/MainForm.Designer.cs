namespace BaiTapLon
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnQLDSV = new System.Windows.Forms.Button();
            this.btnQLK = new System.Windows.Forms.Button();
            this.btnQLL = new System.Windows.Forms.Button();
            this.btnQLMH = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.btnQLSV = new System.Windows.Forms.Button();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.htmlToolTip1 = new MetroFramework.Drawing.Html.HtmlToolTip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tspHeThong = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(-7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(750, 40);
            this.label1.TabIndex = 2;
            this.label1.Text = "CHƯƠNG TRÌNH QUẢN LÍ ĐIỂM SINH VIÊN";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(285, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(743, 35);
            this.panel1.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnQLDSV);
            this.flowLayoutPanel1.Controls.Add(this.btnQLSV);
            this.flowLayoutPanel1.Controls.Add(this.btnQLK);
            this.flowLayoutPanel1.Controls.Add(this.btnQLL);
            this.flowLayoutPanel1.Controls.Add(this.btnQLMH);
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 88);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(120, 571);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnQLDSV
            // 
            this.btnQLDSV.Location = new System.Drawing.Point(3, 3);
            this.btnQLDSV.Name = "btnQLDSV";
            this.btnQLDSV.Size = new System.Drawing.Size(117, 52);
            this.btnQLDSV.TabIndex = 1;
            this.btnQLDSV.Text = "Quản lý sinh viên";
            this.btnQLDSV.UseVisualStyleBackColor = true;
            this.btnQLDSV.Click += new System.EventHandler(this.btnQuanLySinhVien_Click);
            // 
            // btnQLK
            // 
            this.btnQLK.Location = new System.Drawing.Point(3, 119);
            this.btnQLK.Name = "btnQLK";
            this.btnQLK.Size = new System.Drawing.Size(117, 52);
            this.btnQLK.TabIndex = 2;
            this.btnQLK.Text = "Quản Lý Khoa";
            this.btnQLK.UseVisualStyleBackColor = true;
            this.btnQLK.Click += new System.EventHandler(this.btnQLK_Click);
            // 
            // btnQLL
            // 
            this.btnQLL.Location = new System.Drawing.Point(3, 177);
            this.btnQLL.Name = "btnQLL";
            this.btnQLL.Size = new System.Drawing.Size(117, 52);
            this.btnQLL.TabIndex = 3;
            this.btnQLL.Text = "Quản Lý Lớp";
            this.btnQLL.UseVisualStyleBackColor = true;
            this.btnQLL.Click += new System.EventHandler(this.btnQLL_Click);
            // 
            // btnQLMH
            // 
            this.btnQLMH.Location = new System.Drawing.Point(3, 235);
            this.btnQLMH.Name = "btnQLMH";
            this.btnQLMH.Size = new System.Drawing.Size(117, 52);
            this.btnQLMH.TabIndex = 4;
            this.btnQLMH.Text = "Quản Lý Môn Học";
            this.btnQLMH.UseVisualStyleBackColor = true;
            this.btnQLMH.Click += new System.EventHandler(this.btnQLMH_Click);
            // 
            // panelBody
            // 
            this.panelBody.Location = new System.Drawing.Point(118, 88);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(1146, 565);
            this.panelBody.TabIndex = 7;
            // 
            // btnQLSV
            // 
            this.btnQLSV.Location = new System.Drawing.Point(3, 61);
            this.btnQLSV.Name = "btnQLSV";
            this.btnQLSV.Size = new System.Drawing.Size(117, 52);
            this.btnQLSV.TabIndex = 5;
            this.btnQLSV.Text = "Quản lý điểm";
            this.btnQLSV.UseVisualStyleBackColor = true;
            this.btnQLSV.Click += new System.EventHandler(this.btnQLDSV_Click);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // htmlToolTip1
            // 
            this.htmlToolTip1.OwnerDraw = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspHeThong,
            this.toolStripLabel2,
            this.toolStripLabel3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1264, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tspHeThong
            // 
            this.tspHeThong.Name = "tspHeThong";
            this.tspHeThong.Size = new System.Drawing.Size(57, 22);
            this.tspHeThong.Text = "Hệ thống";
            this.tspHeThong.Click += new System.EventHandler(this.tspHeThong_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel2.Text = "toolStripLabel2";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel3.Text = "toolStripLabel3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelBody);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "CHƯƠNG TRÌNH QUẢN LÍ ĐIỂM SINH VIÊN";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnQLDSV;
        private System.Windows.Forms.Button btnQLK;
        private System.Windows.Forms.Button btnQLL;
        private System.Windows.Forms.Button btnQLMH;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Button btnQLSV;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Drawing.Html.HtmlToolTip htmlToolTip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tspHeThong;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
    }
}