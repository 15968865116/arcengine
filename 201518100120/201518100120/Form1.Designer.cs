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
            this.components = new System.ComponentModel.Container();
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
            this.量测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.距离量测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面积量测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.要素选择操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选择要素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放至选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.barCoorTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.属性表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放到图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图层可选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图层不可选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.地图导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全域导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.区域导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文件ToolStripMenuItem,
            this.地图浏览ToolStripMenuItem,
            this.地图文档保存ToolStripMenuItem,
            this.书签ToolStripMenuItem,
            this.量测ToolStripMenuItem,
            this.要素选择操作ToolStripMenuItem,
            this.地图导出ToolStripMenuItem});
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
            // 量测ToolStripMenuItem
            // 
            this.量测ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.距离量测ToolStripMenuItem,
            this.面积量测ToolStripMenuItem});
            this.量测ToolStripMenuItem.Name = "量测ToolStripMenuItem";
            this.量测ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.量测ToolStripMenuItem.Text = "量测";
            // 
            // 距离量测ToolStripMenuItem
            // 
            this.距离量测ToolStripMenuItem.Name = "距离量测ToolStripMenuItem";
            this.距离量测ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.距离量测ToolStripMenuItem.Text = "距离量测";
            this.距离量测ToolStripMenuItem.Click += new System.EventHandler(this.距离量测ToolStripMenuItem_Click);
            // 
            // 面积量测ToolStripMenuItem
            // 
            this.面积量测ToolStripMenuItem.Name = "面积量测ToolStripMenuItem";
            this.面积量测ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.面积量测ToolStripMenuItem.Text = "面积量测";
            this.面积量测ToolStripMenuItem.Click += new System.EventHandler(this.面积量测ToolStripMenuItem_Click);
            // 
            // 要素选择操作ToolStripMenuItem
            // 
            this.要素选择操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择要素ToolStripMenuItem,
            this.缩放至选择ToolStripMenuItem,
            this.清除选择ToolStripMenuItem});
            this.要素选择操作ToolStripMenuItem.Name = "要素选择操作ToolStripMenuItem";
            this.要素选择操作ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.要素选择操作ToolStripMenuItem.Text = "要素选择操作";
            // 
            // 选择要素ToolStripMenuItem
            // 
            this.选择要素ToolStripMenuItem.Name = "选择要素ToolStripMenuItem";
            this.选择要素ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.选择要素ToolStripMenuItem.Text = "选择要素";
            this.选择要素ToolStripMenuItem.Click += new System.EventHandler(this.选择要素ToolStripMenuItem_Click);
            // 
            // 缩放至选择ToolStripMenuItem
            // 
            this.缩放至选择ToolStripMenuItem.Name = "缩放至选择ToolStripMenuItem";
            this.缩放至选择ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.缩放至选择ToolStripMenuItem.Text = "缩放至选择";
            this.缩放至选择ToolStripMenuItem.Click += new System.EventHandler(this.缩放至选择ToolStripMenuItem_Click);
            // 
            // 清除选择ToolStripMenuItem
            // 
            this.清除选择ToolStripMenuItem.Name = "清除选择ToolStripMenuItem";
            this.清除选择ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.清除选择ToolStripMenuItem.Text = "清除选择";
            this.清除选择ToolStripMenuItem.Click += new System.EventHandler(this.清除选择ToolStripMenuItem_Click);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.属性表ToolStripMenuItem,
            this.缩放到图层ToolStripMenuItem,
            this.移除图层ToolStripMenuItem,
            this.图层可选ToolStripMenuItem,
            this.图层不可选ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 114);
            // 
            // 属性表ToolStripMenuItem
            // 
            this.属性表ToolStripMenuItem.Name = "属性表ToolStripMenuItem";
            this.属性表ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.属性表ToolStripMenuItem.Text = "属性表";
            this.属性表ToolStripMenuItem.Click += new System.EventHandler(this.属性表ToolStripMenuItem_Click);
            // 
            // 缩放到图层ToolStripMenuItem
            // 
            this.缩放到图层ToolStripMenuItem.Name = "缩放到图层ToolStripMenuItem";
            this.缩放到图层ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.缩放到图层ToolStripMenuItem.Text = "缩放到图层";
            this.缩放到图层ToolStripMenuItem.Click += new System.EventHandler(this.缩放到图层ToolStripMenuItem_Click);
            // 
            // 移除图层ToolStripMenuItem
            // 
            this.移除图层ToolStripMenuItem.Name = "移除图层ToolStripMenuItem";
            this.移除图层ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.移除图层ToolStripMenuItem.Text = "移除图层";
            this.移除图层ToolStripMenuItem.Click += new System.EventHandler(this.移除图层ToolStripMenuItem_Click);
            // 
            // 图层可选ToolStripMenuItem
            // 
            this.图层可选ToolStripMenuItem.Name = "图层可选ToolStripMenuItem";
            this.图层可选ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.图层可选ToolStripMenuItem.Text = "图层可选";
            this.图层可选ToolStripMenuItem.Click += new System.EventHandler(this.图层可选ToolStripMenuItem_Click);
            // 
            // 图层不可选ToolStripMenuItem
            // 
            this.图层不可选ToolStripMenuItem.Name = "图层不可选ToolStripMenuItem";
            this.图层不可选ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.图层不可选ToolStripMenuItem.Text = "图层不可选";
            this.图层不可选ToolStripMenuItem.Click += new System.EventHandler(this.图层不可选ToolStripMenuItem_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(183, 37);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(493, 334);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnDoubleClick += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnDoubleClickEventHandler(this.axMapControl1_OnDoubleClick);
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(0, 53);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(177, 179);
            this.axTOCControl1.TabIndex = 1;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // 地图导出ToolStripMenuItem
            // 
            this.地图导出ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全域导出ToolStripMenuItem,
            this.区域导出ToolStripMenuItem});
            this.地图导出ToolStripMenuItem.Name = "地图导出ToolStripMenuItem";
            this.地图导出ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.地图导出ToolStripMenuItem.Text = "地图导出";
            // 
            // 全域导出ToolStripMenuItem
            // 
            this.全域导出ToolStripMenuItem.Name = "全域导出ToolStripMenuItem";
            this.全域导出ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.全域导出ToolStripMenuItem.Text = "全域导出";
            this.全域导出ToolStripMenuItem.Click += new System.EventHandler(this.全域导出ToolStripMenuItem_Click);
            // 
            // 区域导出ToolStripMenuItem
            // 
            this.区域导出ToolStripMenuItem.Name = "区域导出ToolStripMenuItem";
            this.区域导出ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.区域导出ToolStripMenuItem.Text = "区域导出";
            this.区域导出ToolStripMenuItem.Click += new System.EventHandler(this.区域导出ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 483);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem 量测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 距离量测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 面积量测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 要素选择操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选择要素ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩放至选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除选择ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 属性表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩放到图层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除图层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图层可选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图层不可选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全域导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 区域导出ToolStripMenuItem;
    }
}

