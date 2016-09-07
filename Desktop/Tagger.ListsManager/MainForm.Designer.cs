﻿namespace ProcessingTools.ListsManager
{
    public partial class MainForm
    {
        private ListManagerControl blackListManager;

        private System.Windows.Forms.TabPage blackListTabPage;

        private System.Windows.Forms.ToolStripMenuItem closeConfigFileToolStripMenuItem;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;

        private System.Windows.Forms.MenuStrip menuStrip;

        private System.Windows.Forms.OpenFileDialog openConfigFileDialog;

        private System.Windows.Forms.ToolStripMenuItem openConfigFileToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem openDefaultConfigFileToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem qiutToolStripMenuItem;

        private ListManagerControl rankListManager;

        private System.Windows.Forms.TabPage rankListTabPage;

        private System.Windows.Forms.StatusStrip statusStrip;

        private System.Windows.Forms.TabControl tabControl;

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConfigOutput;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConfigOutput = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConfigFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDefaultConfigFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeConfigFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.qiutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.blackListTabPage = new System.Windows.Forms.TabPage();
            this.blackListManager = new ProcessingTools.ListsManager.ListManagerControl();
            this.rankListTabPage = new System.Windows.Forms.TabPage();
            this.rankListManager = new ProcessingTools.ListsManager.ListManagerControl();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.blackListTabPage.SuspendLayout();
            this.rankListTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelConfigOutput});
            this.statusStrip.Location = new System.Drawing.Point(0, 594);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(639, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(106, 17);
            this.toolStripStatusLabel1.Text = "Current configuration file:";
            // 
            // toolStripStatusLabelConfigOutput
            // 
            this.toolStripStatusLabelConfigOutput.Name = "toolStripStatusLabelConfigOutput";
            this.toolStripStatusLabelConfigOutput.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(639, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openConfigFileToolStripMenuItem,
            this.openDefaultConfigFileToolStripMenuItem,
            this.closeConfigFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.qiutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openConfigFileToolStripMenuItem
            // 
            this.openConfigFileToolStripMenuItem.Name = "openConfigFileToolStripMenuItem";
            this.openConfigFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openConfigFileToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.openConfigFileToolStripMenuItem.Text = "&Open configuration file";
            this.openConfigFileToolStripMenuItem.Click += new System.EventHandler(this.OpenConfigFileToolStripMenuItem_Click);
            // 
            // openDefaultConfigFileToolStripMenuItem
            // 
            this.openDefaultConfigFileToolStripMenuItem.Name = "openDefaultConfigFileToolStripMenuItem";
            this.openDefaultConfigFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openDefaultConfigFileToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.openDefaultConfigFileToolStripMenuItem.Text = "Open default configuration file";
            this.openDefaultConfigFileToolStripMenuItem.Click += new System.EventHandler(this.OpenDefaultConfigFileToolStripMenuItem_Click);
            // 
            // closeConfigFileToolStripMenuItem
            // 
            this.closeConfigFileToolStripMenuItem.Name = "closeConfigFileToolStripMenuItem";
            this.closeConfigFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.closeConfigFileToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.closeConfigFileToolStripMenuItem.Text = "&Close configuration file";
            this.closeConfigFileToolStripMenuItem.Click += new System.EventHandler(this.CloseConfigFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(271, 6);
            // 
            // qiutToolStripMenuItem
            // 
            this.qiutToolStripMenuItem.Name = "qiutToolStripMenuItem";
            this.qiutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.qiutToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.qiutToolStripMenuItem.Text = "&Quit";
            this.qiutToolStripMenuItem.Click += new System.EventHandler(this.QiutToolStripMenuItem_Click);
            // 
            // openConfigFileDialog
            // 
            this.openConfigFileDialog.DefaultExt = "xml";
            this.openConfigFileDialog.InitialDirectory = "C:\\";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.blackListTabPage);
            this.tabControl.Controls.Add(this.rankListTabPage);
            this.tabControl.Location = new System.Drawing.Point(13, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(614, 563);
            this.tabControl.TabIndex = 0;
            // 
            // blackListTabPage
            // 
            this.blackListTabPage.Controls.Add(this.blackListManager);
            this.blackListTabPage.Location = new System.Drawing.Point(4, 22);
            this.blackListTabPage.Name = "blackListTabPage";
            this.blackListTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.blackListTabPage.Size = new System.Drawing.Size(606, 537);
            this.blackListTabPage.TabIndex = 2;
            this.blackListTabPage.Text = "BlackList";
            this.blackListTabPage.ToolTipText = "Edit rank list content";
            this.blackListTabPage.UseVisualStyleBackColor = true;
            // 
            // blackListManager
            // 
            this.blackListManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blackListManager.IsRankList = false;
            this.blackListManager.ListGroupBoxLabel = "listManagerGroupBox";
            this.blackListManager.Location = new System.Drawing.Point(6, 6);
            this.blackListManager.Name = "blackListManager";
            this.blackListManager.Size = new System.Drawing.Size(594, 525);
            this.blackListManager.TabIndex = 0;
            // 
            // rankListTabPage
            // 
            this.rankListTabPage.Controls.Add(this.rankListManager);
            this.rankListTabPage.Location = new System.Drawing.Point(4, 22);
            this.rankListTabPage.Name = "rankListTabPage";
            this.rankListTabPage.Size = new System.Drawing.Size(606, 537);
            this.rankListTabPage.TabIndex = 3;
            this.rankListTabPage.Text = "RankList";
            this.rankListTabPage.UseVisualStyleBackColor = true;
            // 
            // rankListManager
            // 
            this.rankListManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rankListManager.IsRankList = false;
            this.rankListManager.ListGroupBoxLabel = "listManagerGroupBox";
            this.rankListManager.Location = new System.Drawing.Point(3, 3);
            this.rankListManager.Name = "rankListManager";
            this.rankListManager.Size = new System.Drawing.Size(600, 531);
            this.rankListManager.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 616);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Tagger Lists Manager";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.blackListTabPage.ResumeLayout(false);
            this.rankListTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}