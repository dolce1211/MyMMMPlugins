namespace FaceExpressionHelper
{
    partial class frmMainBase
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.適用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUp = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lstMissingMorphs = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblActiveModel = new System.Windows.Forms.Label();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.lblMissingMorphs = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.閉じるXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.対象制御ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.対象外の目まゆリップモーフToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.対象のその他モーフToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblWait = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblReplaced = new System.Windows.Forms.Label();
            this.mmdSelector = new MMDUtil.MMDSelectorControl();
            this.現在の状態に更新するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.スクショを取り直すToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 54);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(3, 128);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(281, 260);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listBox1_Format);
            this.listBox1.DoubleClick += new System.EventHandler(this.btnOK_Click);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            this.listBox1.MouseLeave += new System.EventHandler(this.listBox1_MouseLeave);
            this.listBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.適用ToolStripMenuItem,
            this.toolStripSeparator2,
            this.編集ToolStripMenuItem,
            this.スクショを取り直すToolStripMenuItem,
            this.現在の状態に更新するToolStripMenuItem,
            this.toolStripSeparator1,
            this.削除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 158);
            // 
            // 適用ToolStripMenuItem
            // 
            this.適用ToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F);
            this.適用ToolStripMenuItem.Name = "適用ToolStripMenuItem";
            this.適用ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.適用ToolStripMenuItem.Text = "適用";
            this.適用ToolStripMenuItem.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.編集ToolStripMenuItem.Text = "名前変更";
            this.編集ToolStripMenuItem.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // 削除ToolStripMenuItem
            // 
            this.削除ToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
            this.削除ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.削除ToolStripMenuItem.Text = "削除";
            this.削除ToolStripMenuItem.Click += new System.EventHandler(this.削除ToolStripMenuItem_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(210, 54);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(35, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(248, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "↓";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // lstMissingMorphs
            // 
            this.lstMissingMorphs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMissingMorphs.BackColor = System.Drawing.SystemColors.Control;
            this.lstMissingMorphs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMissingMorphs.ContextMenuStrip = this.contextMenuStrip1;
            this.lstMissingMorphs.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstMissingMorphs.ForeColor = System.Drawing.Color.Red;
            this.lstMissingMorphs.FormattingEnabled = true;
            this.lstMissingMorphs.IntegralHeight = false;
            this.lstMissingMorphs.ItemHeight = 12;
            this.lstMissingMorphs.Items.AddRange(new object[] {
            "あああ",
            "いいい"});
            this.lstMissingMorphs.Location = new System.Drawing.Point(4, 409);
            this.lstMissingMorphs.Name = "lstMissingMorphs";
            this.lstMissingMorphs.Size = new System.Drawing.Size(128, 66);
            this.lstMissingMorphs.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(192, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "適用";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(74, 8);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 19);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "フレーム後に";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblActiveModel
            // 
            this.lblActiveModel.AutoSize = true;
            this.lblActiveModel.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblActiveModel.Location = new System.Drawing.Point(4, 4);
            this.lblActiveModel.Name = "lblActiveModel";
            this.lblActiveModel.Size = new System.Drawing.Size(226, 15);
            this.lblActiveModel.TabIndex = 9;
            this.lblActiveModel.Text = "選択中のモデルあいうえおかきくけこ";
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(3, 27);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(69, 16);
            this.chkTopMost.TabIndex = 10;
            this.chkTopMost.Text = "常に手前";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Location = new System.Drawing.Point(166, 21);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(116, 26);
            this.btnReplace.TabIndex = 14;
            this.btnReplace.Text = "モーフ置換設定";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // lblMissingMorphs
            // 
            this.lblMissingMorphs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMissingMorphs.AutoSize = true;
            this.lblMissingMorphs.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMissingMorphs.ForeColor = System.Drawing.Color.Red;
            this.lblMissingMorphs.Location = new System.Drawing.Point(1, 391);
            this.lblMissingMorphs.Name = "lblMissingMorphs";
            this.lblMissingMorphs.Size = new System.Drawing.Size(98, 15);
            this.lblMissingMorphs.TabIndex = 15;
            this.lblMissingMorphs.Text = "不足モーフあり";
            this.lblMissingMorphs.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.対象制御ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(287, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.閉じるXToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 閉じるXToolStripMenuItem
            // 
            this.閉じるXToolStripMenuItem.Name = "閉じるXToolStripMenuItem";
            this.閉じるXToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.閉じるXToolStripMenuItem.Text = "閉じる(&X)";
            this.閉じるXToolStripMenuItem.Click += new System.EventHandler(this.閉じるXToolStripMenuItem_Click);
            // 
            // 対象制御ToolStripMenuItem
            // 
            this.対象制御ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.対象外の目まゆリップモーフToolStripMenuItem,
            this.toolStripSeparator3,
            this.対象のその他モーフToolStripMenuItem});
            this.対象制御ToolStripMenuItem.Name = "対象制御ToolStripMenuItem";
            this.対象制御ToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
            this.対象制御ToolStripMenuItem.Text = "対象・対象外制御";
            // 
            // 対象外の目まゆリップモーフToolStripMenuItem
            // 
            this.対象外の目まゆリップモーフToolStripMenuItem.Name = "対象外の目まゆリップモーフToolStripMenuItem";
            this.対象外の目まゆリップモーフToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.対象外の目まゆリップモーフToolStripMenuItem.Text = "処理対象外の「目・まゆ・リップ」モーフ";
            this.対象外の目まゆリップモーフToolStripMenuItem.Click += new System.EventHandler(this.対象外の目眉リップモーフToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(246, 6);
            // 
            // 対象のその他モーフToolStripMenuItem
            // 
            this.対象のその他モーフToolStripMenuItem.Name = "対象のその他モーフToolStripMenuItem";
            this.対象のその他モーフToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.対象のその他モーフToolStripMenuItem.Text = "処理対象の「その他」モーフ";
            this.対象のその他モーフToolStripMenuItem.Click += new System.EventHandler(this.対象外の目眉リップモーフToolStripMenuItem_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.numericUpDown1);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 481);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(287, 36);
            this.pnlBottom.TabIndex = 17;
            // 
            // lblWait
            // 
            this.lblWait.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWait.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWait.Location = new System.Drawing.Point(3, 178);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(280, 132);
            this.lblWait.TabIndex = 19;
            this.lblWait.Text = "「なんとかかんとか」\r\nのモーフ情報を取得中です。\r\nしばらくお待ち下さい。\r\n";
            this.lblWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWait.Visible = false;
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.Controls.Add(this.btnReplace);
            this.pnlTop.Controls.Add(this.lblReplaced);
            this.pnlTop.Controls.Add(this.btnAdd);
            this.pnlTop.Controls.Add(this.btnUp);
            this.pnlTop.Controls.Add(this.button2);
            this.pnlTop.Controls.Add(this.lblActiveModel);
            this.pnlTop.Location = new System.Drawing.Point(0, 49);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(286, 78);
            this.pnlTop.TabIndex = 20;
            // 
            // lblReplaced
            // 
            this.lblReplaced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblReplaced.AutoSize = true;
            this.lblReplaced.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblReplaced.ForeColor = System.Drawing.Color.Black;
            this.lblReplaced.Location = new System.Drawing.Point(12, 28);
            this.lblReplaced.Name = "lblReplaced";
            this.lblReplaced.Size = new System.Drawing.Size(89, 12);
            this.lblReplaced.TabIndex = 16;
            this.lblReplaced.Text = "置換設定*件あり";
            // 
            // mmdSelector
            // 
            this.mmdSelector.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mmdSelector.Location = new System.Drawing.Point(0, 517);
            this.mmdSelector.Name = "mmdSelector";
            this.mmdSelector.Size = new System.Drawing.Size(287, 46);
            this.mmdSelector.TabIndex = 18;
            this.mmdSelector.Visible = false;
            // 
            // 現在の状態に更新するToolStripMenuItem
            // 
            this.現在の状態に更新するToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F);
            this.現在の状態に更新するToolStripMenuItem.Name = "現在の状態に更新するToolStripMenuItem";
            this.現在の状態に更新するToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.現在の状態に更新するToolStripMenuItem.Text = "現在の状態に更新する";
            this.現在の状態に更新するToolStripMenuItem.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // スクショを取り直すToolStripMenuItem
            // 
            this.スクショを取り直すToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F);
            this.スクショを取り直すToolStripMenuItem.Name = "スクショを取り直すToolStripMenuItem";
            this.スクショを取り直すToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.スクショを取り直すToolStripMenuItem.Text = "スクショを取り直す";
            this.スクショを取り直すToolStripMenuItem.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmMainBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 563);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.mmdSelector);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblMissingMorphs);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.lstMissingMorphs);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMainBase";
            this.Text = "表情選択";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMainBase_FormClosed);
            this.Load += new System.EventHandler(this.frmMainBase_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 編集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 削除ToolStripMenuItem;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem 適用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.ListBox lstMissingMorphs;
        private System.Windows.Forms.CheckBox chkTopMost;
        protected System.Windows.Forms.Label lblActiveModel;
        protected System.Windows.Forms.Label lblMissingMorphs;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 閉じるXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 対象外の目まゆリップモーフToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 対象のその他モーフToolStripMenuItem;
        public MMDUtil.MMDSelectorControl mmdSelector;
        public System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.Label lblWait;
        public System.Windows.Forms.Panel pnlTop;
        public System.Windows.Forms.Panel pnlBottom;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.ToolStripMenuItem 対象制御ToolStripMenuItem;
        protected System.Windows.Forms.Label lblReplaced;
        public System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.ToolStripMenuItem 現在の状態に更新するToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem スクショを取り直すToolStripMenuItem;
    }
}