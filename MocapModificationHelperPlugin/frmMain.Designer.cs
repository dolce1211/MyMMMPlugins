namespace MoCapModificationHelperPlugin
{
    partial class frmMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel = new System.Windows.Forms.Panel();
            this.cboHistory = new System.Windows.Forms.ComboBox();
            this.chkModifiedLayerSelector = new System.Windows.Forms.CheckBox();
            this.cboSelectedKeyLoader = new System.Windows.Forms.ComboBox();
            this.cboSelectedKeySaver = new System.Windows.Forms.ComboBox();
            this.btnSelectedKeyLoader = new System.Windows.Forms.Button();
            this.btnSelectedKeySaver = new System.Windows.Forms.Button();
            this.cboModifiedLayerSelector = new System.Windows.Forms.ComboBox();
            this.cboLayerBoneSelector = new System.Windows.Forms.ComboBox();
            this.btnModifiedLayerSelector = new System.Windows.Forms.Button();
            this.chkLayerBoneSelector = new System.Windows.Forms.CheckBox();
            this.btnLayerBoneSelector = new System.Windows.Forms.Button();
            this.btnGapSelector = new System.Windows.Forms.Button();
            this.cboGapSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlOffset = new System.Windows.Forms.Panel();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnExecuteOffset = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel.SuspendLayout();
            this.pnlOffset.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "オフセット付与準備";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(462, 166);
            this.dataGridView1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.cboHistory);
            this.panel.Controls.Add(this.chkModifiedLayerSelector);
            this.panel.Controls.Add(this.cboSelectedKeyLoader);
            this.panel.Controls.Add(this.cboSelectedKeySaver);
            this.panel.Controls.Add(this.btnSelectedKeyLoader);
            this.panel.Controls.Add(this.btnSelectedKeySaver);
            this.panel.Controls.Add(this.cboModifiedLayerSelector);
            this.panel.Controls.Add(this.cboLayerBoneSelector);
            this.panel.Controls.Add(this.btnModifiedLayerSelector);
            this.panel.Controls.Add(this.chkLayerBoneSelector);
            this.panel.Controls.Add(this.btnLayerBoneSelector);
            this.panel.Controls.Add(this.btnGapSelector);
            this.panel.Controls.Add(this.cboGapSelector);
            this.panel.Controls.Add(this.label1);
            this.panel.Location = new System.Drawing.Point(12, 60);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(462, 166);
            this.panel.TabIndex = 3;
            // 
            // cboHistory
            // 
            this.cboHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHistory.FormattingEnabled = true;
            this.cboHistory.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboHistory.Location = new System.Drawing.Point(356, 139);
            this.cboHistory.Name = "cboHistory";
            this.cboHistory.Size = new System.Drawing.Size(102, 20);
            this.cboHistory.TabIndex = 15;
            this.cboHistory.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboHistory_Format);
            // 
            // chkModifiedLayerSelector
            // 
            this.chkModifiedLayerSelector.AutoSize = true;
            this.chkModifiedLayerSelector.Location = new System.Drawing.Point(356, 83);
            this.chkModifiedLayerSelector.Name = "chkModifiedLayerSelector";
            this.chkModifiedLayerSelector.Size = new System.Drawing.Size(36, 16);
            this.chkModifiedLayerSelector.TabIndex = 14;
            this.chkModifiedLayerSelector.Text = "逆";
            this.chkModifiedLayerSelector.UseVisualStyleBackColor = true;
            this.chkModifiedLayerSelector.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // cboSelectedKeyLoader
            // 
            this.cboSelectedKeyLoader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedKeyLoader.FormattingEnabled = true;
            this.cboSelectedKeyLoader.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboSelectedKeyLoader.Location = new System.Drawing.Point(292, 139);
            this.cboSelectedKeyLoader.Name = "cboSelectedKeyLoader";
            this.cboSelectedKeyLoader.Size = new System.Drawing.Size(58, 20);
            this.cboSelectedKeyLoader.TabIndex = 13;
            this.cboSelectedKeyLoader.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboSelectedKeyLoader.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // cboSelectedKeySaver
            // 
            this.cboSelectedKeySaver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedKeySaver.FormattingEnabled = true;
            this.cboSelectedKeySaver.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboSelectedKeySaver.Location = new System.Drawing.Point(292, 110);
            this.cboSelectedKeySaver.Name = "cboSelectedKeySaver";
            this.cboSelectedKeySaver.Size = new System.Drawing.Size(58, 20);
            this.cboSelectedKeySaver.TabIndex = 12;
            this.cboSelectedKeySaver.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboSelectedKeySaver.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // btnSelectedKeyLoader
            // 
            this.btnSelectedKeyLoader.Location = new System.Drawing.Point(6, 139);
            this.btnSelectedKeyLoader.Name = "btnSelectedKeyLoader";
            this.btnSelectedKeyLoader.Size = new System.Drawing.Size(280, 23);
            this.btnSelectedKeyLoader.TabIndex = 11;
            this.btnSelectedKeyLoader.Text = "選択キーのペースト";
            this.btnSelectedKeyLoader.UseVisualStyleBackColor = true;
            this.btnSelectedKeyLoader.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // btnSelectedKeySaver
            // 
            this.btnSelectedKeySaver.Location = new System.Drawing.Point(6, 110);
            this.btnSelectedKeySaver.Name = "btnSelectedKeySaver";
            this.btnSelectedKeySaver.Size = new System.Drawing.Size(280, 23);
            this.btnSelectedKeySaver.TabIndex = 10;
            this.btnSelectedKeySaver.Text = "選択キーのコピー";
            this.btnSelectedKeySaver.UseVisualStyleBackColor = true;
            this.btnSelectedKeySaver.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // cboModifiedLayerSelector
            // 
            this.cboModifiedLayerSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModifiedLayerSelector.FormattingEnabled = true;
            this.cboModifiedLayerSelector.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboModifiedLayerSelector.Location = new System.Drawing.Point(292, 81);
            this.cboModifiedLayerSelector.Name = "cboModifiedLayerSelector";
            this.cboModifiedLayerSelector.Size = new System.Drawing.Size(58, 20);
            this.cboModifiedLayerSelector.TabIndex = 9;
            this.cboModifiedLayerSelector.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboModifiedLayerSelector.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // cboLayerBoneSelector
            // 
            this.cboLayerBoneSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLayerBoneSelector.FormattingEnabled = true;
            this.cboLayerBoneSelector.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboLayerBoneSelector.Location = new System.Drawing.Point(292, 52);
            this.cboLayerBoneSelector.Name = "cboLayerBoneSelector";
            this.cboLayerBoneSelector.Size = new System.Drawing.Size(58, 20);
            this.cboLayerBoneSelector.TabIndex = 8;
            this.cboLayerBoneSelector.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboLayerBoneSelector.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // btnModifiedLayerSelector
            // 
            this.btnModifiedLayerSelector.Location = new System.Drawing.Point(6, 81);
            this.btnModifiedLayerSelector.Name = "btnModifiedLayerSelector";
            this.btnModifiedLayerSelector.Size = new System.Drawing.Size(280, 23);
            this.btnModifiedLayerSelector.TabIndex = 7;
            this.btnModifiedLayerSelector.Text = "現Frで移動・回転が加えられているボーンを全て選択";
            this.btnModifiedLayerSelector.UseVisualStyleBackColor = true;
            this.btnModifiedLayerSelector.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // chkLayerBoneSelector
            // 
            this.chkLayerBoneSelector.AutoSize = true;
            this.chkLayerBoneSelector.Location = new System.Drawing.Point(356, 54);
            this.chkLayerBoneSelector.Name = "chkLayerBoneSelector";
            this.chkLayerBoneSelector.Size = new System.Drawing.Size(36, 16);
            this.chkLayerBoneSelector.TabIndex = 6;
            this.chkLayerBoneSelector.Text = "逆";
            this.chkLayerBoneSelector.UseVisualStyleBackColor = true;
            this.chkLayerBoneSelector.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // btnLayerBoneSelector
            // 
            this.btnLayerBoneSelector.Location = new System.Drawing.Point(6, 52);
            this.btnLayerBoneSelector.Name = "btnLayerBoneSelector";
            this.btnLayerBoneSelector.Size = new System.Drawing.Size(280, 23);
            this.btnLayerBoneSelector.TabIndex = 5;
            this.btnLayerBoneSelector.Text = "選択中のキーの内レイヤーボーンのみ選択";
            this.btnLayerBoneSelector.UseVisualStyleBackColor = true;
            this.btnLayerBoneSelector.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // btnGapSelector
            // 
            this.btnGapSelector.Location = new System.Drawing.Point(6, 23);
            this.btnGapSelector.Name = "btnGapSelector";
            this.btnGapSelector.Size = new System.Drawing.Size(280, 23);
            this.btnGapSelector.TabIndex = 4;
            this.btnGapSelector.Text = "選択したボーンの連続したキーの中身を全て選択";
            this.btnGapSelector.UseVisualStyleBackColor = true;
            this.btnGapSelector.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // cboGapSelector
            // 
            this.cboGapSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGapSelector.FormattingEnabled = true;
            this.cboGapSelector.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboGapSelector.Location = new System.Drawing.Point(292, 23);
            this.cboGapSelector.Name = "cboGapSelector";
            this.cboGapSelector.Size = new System.Drawing.Size(58, 20);
            this.cboGapSelector.TabIndex = 3;
            this.cboGapSelector.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboGapSelector.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(288, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "二度押しキー";
            // 
            // pnlOffset
            // 
            this.pnlOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOffset.Controls.Add(this.btnUndo);
            this.pnlOffset.Controls.Add(this.btnExecuteOffset);
            this.pnlOffset.Location = new System.Drawing.Point(117, 2);
            this.pnlOffset.Name = "pnlOffset";
            this.pnlOffset.Size = new System.Drawing.Size(350, 26);
            this.pnlOffset.TabIndex = 4;
            this.pnlOffset.Visible = false;
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Enabled = false;
            this.btnUndo.Location = new System.Drawing.Point(274, 1);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(76, 25);
            this.btnUndo.TabIndex = 2;
            this.btnUndo.Text = "アンドゥ";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnExecuteOffset
            // 
            this.btnExecuteOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteOffset.BackColor = System.Drawing.Color.Orange;
            this.btnExecuteOffset.Location = new System.Drawing.Point(190, 1);
            this.btnExecuteOffset.Name = "btnExecuteOffset";
            this.btnExecuteOffset.Size = new System.Drawing.Size(85, 25);
            this.btnExecuteOffset.TabIndex = 1;
            this.btnExecuteOffset.Text = "オフセット付与";
            this.btnExecuteOffset.UseVisualStyleBackColor = false;
            this.btnExecuteOffset.Click += new System.EventHandler(this.btnExecuteOffset_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 34);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(461, 20);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 211);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pnlOffset);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "色々ヘルパー";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.pnlOffset.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ComboBox cboGapSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLayerBoneSelector;
        private System.Windows.Forms.Button btnLayerBoneSelector;
        private System.Windows.Forms.Button btnGapSelector;
        private System.Windows.Forms.ComboBox cboSelectedKeyLoader;
        private System.Windows.Forms.ComboBox cboSelectedKeySaver;
        private System.Windows.Forms.Button btnSelectedKeyLoader;
        private System.Windows.Forms.Button btnSelectedKeySaver;
        private System.Windows.Forms.ComboBox cboModifiedLayerSelector;
        private System.Windows.Forms.ComboBox cboLayerBoneSelector;
        private System.Windows.Forms.Button btnModifiedLayerSelector;
        private System.Windows.Forms.Panel pnlOffset;
        private System.Windows.Forms.Button btnExecuteOffset;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkModifiedLayerSelector;
        private System.Windows.Forms.ComboBox cboHistory;
    }
}