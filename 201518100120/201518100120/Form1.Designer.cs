namespace _201518100120
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开地图文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开shapefile文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图浏览ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.拉框放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.拉框缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.漫游ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上一个视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.后一个视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全图显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图文档保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.直接保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.书签ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加书签ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.书签管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.barCoorTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文件ToolStripMenuItem,
            this.地图浏览ToolStripMenuItem,
            this.地图文档保存ToolStripMenuItem,
            this.书签ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(762, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 打开文件ToolStripMenuItem
            // 
            this.打开文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开地图文档ToolStripMenuItem,
            this.打开shapefile文件ToolStripMenuItem});
            this.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            this.打开文件ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.打开文件ToolStripMenuItem.Text = "打开文件";
            // 
            // 打开地图文档ToolStripMenuItem
            // 
            this.打开地图文档ToolStripMenuItem.Name = "打开地图文档ToolStripMenuItem";
            this.打开地图文档ToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.打开地图文档ToolStripMenuItem.Text = "打开地图文档";
            this.打开地图文档ToolStripMenuItem.Click += new System.EventHandler(this.打开地图文档ToolStripMenuItem_Click);
            // 
            // 打开shapefile文件ToolStripMenuItem
            // 
            this.打开shapefile文件ToolStripMenuItem.Name = "打开shapefile文件ToolStripMenuItem";
            this.打开shapefile文件ToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.打开shapefile文件ToolStripMenuItem.Text = "打开shapefile文件";
            this.打开shapefile文件ToolStripMenuItem.Click += new System.EventHandler(this.打开shapefile文件ToolStripMenuItem_Click);
            // 
            // 地图浏览ToolStripMenuItem
            // 
            this.地图浏览ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.放大ToolStripMenuItem,
            this.缩小ToolStripMenuItem,
            this.拉框放大ToolStripMenuItem,
            this.拉框缩小ToolStripMenuItem,
            this.漫游ToolStripMenuItem,
            this.上一个视图ToolStripMenuItem,
            this.后一个视图ToolStripMenuItem,
            this.全图显示ToolStripMenuItem});
            this.地图浏览ToolStripMenuItem.Name = "地图浏览ToolStripMenuItem";
            this.地图浏览ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.地图浏览ToolStripMenuItem.Text = "地图浏览";
            // 
            // 放大ToolStripMenuItem
            // 
            this.放大ToolStripMenuItem.Name = "放大ToolStripMenuItem";
            this.放大ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.放大ToolStripMenuItem.Text = "放大";
            this.放大ToolStripMenuItem.Click += new System.EventHandler(this.放大ToolStripMenuItem_Click);
            // 
            // 缩小ToolStripMenuItem
            // 
            this.缩小ToolStripMenuItem.Name = "缩小ToolStripMenuItem";
            this.缩小ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.缩小ToolStripMenuItem.Text = "缩小";
            this.缩小ToolStripMenuItem.Click += new System.EventHandler(this.缩小ToolStripMenuItem_Click);
            // 
            // 拉框放大ToolStripMenuItem
            // 
            this.拉框放大ToolStripMenuItem.Name = "拉框放大ToolStripMenuItem";
            this.拉框放大ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.拉框放大ToolStripMenuItem.Text = "拉框放大";
            this.拉框放大ToolStripMenuItem.Click += new System.EventHandler(this.拉框放大ToolStripMenuItem_Click);
            // 
            // 拉框缩小ToolStripMenuItem
            // 
            this.拉框缩小ToolStripMenuItem.Name = "拉框缩小ToolStripMenuItem";
            this.拉框缩小ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.拉框缩小ToolStripMenuItem.Text = "拉框缩小";
            this.拉框缩小ToolStripMenuItem.Click += new System.EventHandler(this.拉框缩小ToolStripMenuItem_Click);
            // 
            // 漫游ToolStripMenuItem
            // 
            this.漫游ToolStripMenuItem.Name = "漫游ToolStripMenuItem";
            this.漫游ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.漫游ToolStripMenuItem.Text = "漫游";
            this.漫游ToolStripMenuItem.Click += new System.EventHandler(this.漫游ToolStripMenuItem_Click);
            // 
            // 上一个视图ToolStripMenuItem
            // 
            this.上一个视图ToolStripMenuItem.Name = "上一个视图ToolStripMenuItem";
            this.上一个视图ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.上一个视图ToolStripMenuItem.Text = "前一个视图";
            this.上一个视图ToolStripMenuItem.Click += new System.EventHandler(this.上一个视图ToolStripMenuItem_Click);
            // 
            // 后一个视图ToolStripMenuItem
            // 
            this.后一个视图ToolStripMenuItem.Name = "后一个视图ToolStripMenuItem";
            this.后一个视图ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.后一个视图ToolStripMenuItem.Text = "后一个视图";
            this.后一个视图ToolStripMenuItem.Click += new System.EventHandler(this.后一个视图ToolStripMenuItem_Click);
            // 
            // 全图显示ToolStripMenuItem
            // 
            this.全图显示ToolStripMenuItem.Name = "全图显示ToolStripMenuItem";
            this.全图显示ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.全图显示ToolStripMenuItem.Text = "全图显示";
            this.全图显示ToolStripMenuItem.Click += new System.EventHandler(this.全图显示ToolStripMenuItem_Click);
            // 
            // 地图文档保存ToolStripMenuItem
            // 
            this.地图文档保存ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.直接保存ToolStripMenuItem,
            this.另存为ToolStripMenuItem});
            this.地图文档保存ToolStripMenuItem.Name = "地图文档保存ToolStripMenuItem";
            this.地图文档保存ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.地图文档保存ToolStripMenuItem.Text = "地图文档保存";
            // 
            // 直接保存ToolStripMenuItem
            // 
            this.直接保存ToolStripMenuItem.Name = "直接保存ToolStripMenuItem";
            this.直接保存ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.直接保存ToolStripMenuItem.Text = "直接保存";
            this.直接保存ToolStripMenuItem.Click += new System.EventHandler(this.直接保存ToolStripMenuItem_Click);
            // 
            // 另存为ToolStripMenuItem
            // 
            this.另存为ToolStripMenuItem.Name = "另存为ToolStripMenuItem";
            this.另存为ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.另存为ToolStripMenuItem.Text = "另存为";
            this.另存为ToolStripMenuItem.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // 书签ToolStripMenuItem
            // 
            this.书签ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加书签ToolStripMenuItem,
            this.书签管理ToolStripMenuItem});
            this.书签ToolStripMenuItem.Name = "书签ToolStripMenuItem";
            this.书签ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.书签ToolStripMenuItem.Text = "书签";
            // 
            // 添加书签ToolStripMenuItem
            // 
            this.添加书签ToolStripMenuItem.Name = "添加书签ToolStripMenuItem";
            this.添加书签ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加书签ToolStripMenuItem.Text = "添加书签";
            this.添加书签ToolStripMenuItem.Click += new System.EventHandler(this.添加书签ToolStripMenuItem_Click);
            // 
            // 书签管理ToolStripMenuItem
            // 
            this.书签管理ToolStripMenuItem.Name = "书签管理ToolStripMenuItem";
            this.书签管理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.书签管理ToolStripMenuItem.Text = "书签管理";
            this.书签管理ToolStripMenuItem.Click += new System.EventHandler(this.书签管理ToolStripMenuItem_Click);
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(0, 37);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(265, 334);
            this.axTOCControl1.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(271, 37);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(403, 334);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barCoorTxt});
            this.statusStrip1.Location = new System.Drawing.Point(0, 461);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(762, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // barCoorTxt
            // 
            this.barCoorTxt.Name = "barCoorTxt";
            this.barCoorTxt.Size = new System.Drawing.Size(131, 17);
            this.barCoorTxt.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 483);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开地图文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开shapefile文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图浏览ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 拉框放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 拉框缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 漫游ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上一个视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 后一个视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全图显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图文档保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 直接保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 书签ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加书签ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 书签管理ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel barCoorTxt;
    }
}

