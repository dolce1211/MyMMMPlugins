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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainBase));
            this.btnAdd = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.適用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.スクショを取り直すToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.現在の状態に更新するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUp = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblFrame = new System.Windows.Forms.Label();
            this.lblActiveModel = new System.Windows.Forms.Label();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.閉じるXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.対象制御ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.対象外の目まゆリップモーフToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.対象のその他モーフToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWait = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblReplaced = new System.Windows.Forms.Label();
            this.cboSet = new System.Windows.Forms.ComboBox();
            this.btnEditSet = new System.Windows.Forms.Button();
            this.btnAddSet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lstMorphs = new FaceExpressionHelper.UI.UserControls.MorphListBox();
            this.mmdSelector = new MMDUtil.MMDSelectorControl();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 54);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "表情追加";
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
            this.listBox1.Location = new System.Drawing.Point(3, 131);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(291, 281);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 136);
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
            // スクショを取り直すToolStripMenuItem
            // 
            this.スクショを取り直すToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F);
            this.スクショを取り直すToolStripMenuItem.Name = "スクショを取り直すToolStripMenuItem";
            this.スクショを取り直すToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.スクショを取り直すToolStripMenuItem.Text = "スクショを取り直す";
            this.スクショを取り直すToolStripMenuItem.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // 現在の状態に更新するToolStripMenuItem
            // 
            this.現在の状態に更新するToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F);
            this.現在の状態に更新するToolStripMenuItem.Name = "現在の状態に更新するToolStripMenuItem";
            this.現在の状態に更新するToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.現在の状態に更新するToolStripMenuItem.Text = "現在の状態に更新する";
            this.現在の状態に更新するToolStripMenuItem.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.btnUp.Location = new System.Drawing.Point(220, 54);
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
            this.button2.Location = new System.Drawing.Point(258, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "↓";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(218, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "適用";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblFrame
            // 
            this.lblFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFrame.BackColor = System.Drawing.SystemColors.Control;
            this.lblFrame.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFrame.ForeColor = System.Drawing.Color.Violet;
            this.lblFrame.Location = new System.Drawing.Point(147, 23);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(64, 16);
            this.lblFrame.TabIndex = 8;
            this.lblFrame.Text = "10fr前から";
            this.lblFrame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.chkTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(7, 43);
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
            this.btnReplace.Location = new System.Drawing.Point(176, 21);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(116, 26);
            this.btnReplace.TabIndex = 14;
            this.btnReplace.Text = "モーフ置換設定";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.対象制御ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(297, 24);
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
            this.pnlBottom.Controls.Add(this.lblFrame);
            this.pnlBottom.Controls.Add(this.chkTopMost);
            this.pnlBottom.Controls.Add(this.trackBar1);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 500);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(297, 65);
            this.pnlBottom.TabIndex = 17;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBar1.Location = new System.Drawing.Point(-4, 17);
            this.trackBar1.Minimum = -10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(157, 45);
            this.trackBar1.TabIndex = 11;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "|";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWait
            // 
            this.lblWait.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWait.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWait.Location = new System.Drawing.Point(3, 193);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(290, 132);
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
            this.pnlTop.Location = new System.Drawing.Point(-1, 51);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(296, 78);
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
            // cboSet
            // 
            this.cboSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSet.FormattingEnabled = true;
            this.cboSet.Location = new System.Drawing.Point(45, 28);
            this.cboSet.Name = "cboSet";
            this.cboSet.Size = new System.Drawing.Size(149, 20);
            this.cboSet.TabIndex = 22;
            this.cboSet.SelectedIndexChanged += new System.EventHandler(this.cboSet_SelectedIndexChanged);
            this.cboSet.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboSet_Format);
            // 
            // btnEditSet
            // 
            this.btnEditSet.Location = new System.Drawing.Point(196, 27);
            this.btnEditSet.Name = "btnEditSet";
            this.btnEditSet.Size = new System.Drawing.Size(45, 23);
            this.btnEditSet.TabIndex = 25;
            this.btnEditSet.Text = "編集";
            this.btnEditSet.UseVisualStyleBackColor = true;
            this.btnEditSet.Click += new System.EventHandler(this.btnAddSet_Click);
            // 
            // btnAddSet
            // 
            this.btnAddSet.Location = new System.Drawing.Point(242, 27);
            this.btnAddSet.Name = "btnAddSet";
            this.btnAddSet.Size = new System.Drawing.Size(45, 23);
            this.btnAddSet.TabIndex = 26;
            this.btnAddSet.Text = "追加";
            this.btnAddSet.UseVisualStyleBackColor = true;
            this.btnAddSet.Click += new System.EventHandler(this.btnAddSet_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(-1, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 27;
            this.label2.Text = "セット";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(218, 415);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(78, 23);
            this.btnReset.TabIndex = 28;
            this.btnReset.Text = "表情リセット";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstMorphs
            // 
            this.lstMorphs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMorphs.BackColor = System.Drawing.SystemColors.Control;
            this.lstMorphs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMorphs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMorphs.FormattingEnabled = true;
            this.lstMorphs.IntegralHeight = false;
            this.lstMorphs.Location = new System.Drawing.Point(6, 417);
            this.lstMorphs.Name = "lstMorphs";
            this.lstMorphs.Size = new System.Drawing.Size(205, 81);
            this.lstMorphs.TabIndex = 21;
            this.lstMorphs.SelectedValueChanged += new System.EventHandler(this.lstMorphs_SelectedValueChanged);
            // 
            // mmdSelector
            // 
            this.mmdSelector.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mmdSelector.Location = new System.Drawing.Point(0, 565);
            this.mmdSelector.Name = "mmdSelector";
            this.mmdSelector.Size = new System.Drawing.Size(297, 46);
            this.mmdSelector.TabIndex = 18;
            this.mmdSelector.Visible = false;
            // 
            // frmMainBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 611);
            this.Controls.Add(this.cboSet);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddSet);
            this.Controls.Add(this.btnEditSet);
            this.Controls.Add(this.lstMorphs);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.mmdSelector);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMainBase";
            this.Text = "表情選択";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMainBase_FormClosed);
            this.Load += new System.EventHandler(this.frmMainBase_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
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
        private System.Windows.Forms.Label lblFrame;
        private System.Windows.Forms.CheckBox chkTopMost;
        protected System.Windows.Forms.Label lblActiveModel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 閉じるXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 対象外の目まゆリップモーフToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 対象のその他モーフToolStripMenuItem;
        public MMDUtil.MMDSelectorControl mmdSelector;
        public System.Windows.Forms.Label lblWait;
        public System.Windows.Forms.Panel pnlTop;
        public System.Windows.Forms.Panel pnlBottom;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.ToolStripMenuItem 対象制御ToolStripMenuItem;
        protected System.Windows.Forms.Label lblReplaced;
        public System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.ToolStripMenuItem 現在の状態に更新するToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem スクショを取り直すToolStripMenuItem;
        private UI.UserControls.MorphListBox lstMorphs;
        private System.Windows.Forms.ComboBox cboSet;
        private System.Windows.Forms.Button btnEditSet;
        private System.Windows.Forms.Button btnAddSet;
        protected System.Windows.Forms.Label label2;
        public System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnReset;
    }
}