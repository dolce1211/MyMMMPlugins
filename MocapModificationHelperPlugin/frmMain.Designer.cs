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
            this.btnOffset = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.chkWEnterMorphs = new System.Windows.Forms.CheckBox();
            this.chkMorphOnMLS = new System.Windows.Forms.CheckBox();
            this.chkCancelForSmile = new System.Windows.Forms.CheckBox();
            this.cboBlinkCanceller = new System.Windows.Forms.ComboBox();
            this.btnBlinkCanceller = new System.Windows.Forms.Button();
            this.cboFillDisplayFrame = new System.Windows.Forms.ComboBox();
            this.btnFillDisplayFrame = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbInterpolateAll = new System.Windows.Forms.RadioButton();
            this.rbInterpolateZ = new System.Windows.Forms.RadioButton();
            this.rbInterpolateY = new System.Windows.Forms.RadioButton();
            this.rbInterpolateX = new System.Windows.Forms.RadioButton();
            this.rbInterpolateR = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.chkClickOffsetBtnByShiftEnter = new System.Windows.Forms.CheckBox();
            this.btnExecuteOffset = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlOffset.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOffset
            // 
            this.btnOffset.Location = new System.Drawing.Point(6, 3);
            this.btnOffset.Name = "btnOffset";
            this.btnOffset.Size = new System.Drawing.Size(105, 25);
            this.btnOffset.TabIndex = 0;
            this.btnOffset.Text = "オフセット付加準備";
            this.btnOffset.UseVisualStyleBackColor = true;
            this.btnOffset.Click += new System.EventHandler(this.btnOffset_Click);
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
            this.dataGridView1.Size = new System.Drawing.Size(472, 233);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Controls.Add(this.chkWEnterMorphs);
            this.pnlMain.Controls.Add(this.chkMorphOnMLS);
            this.pnlMain.Controls.Add(this.chkCancelForSmile);
            this.pnlMain.Controls.Add(this.cboBlinkCanceller);
            this.pnlMain.Controls.Add(this.btnBlinkCanceller);
            this.pnlMain.Controls.Add(this.cboFillDisplayFrame);
            this.pnlMain.Controls.Add(this.btnFillDisplayFrame);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.cboHistory);
            this.pnlMain.Controls.Add(this.chkModifiedLayerSelector);
            this.pnlMain.Controls.Add(this.cboSelectedKeyLoader);
            this.pnlMain.Controls.Add(this.cboSelectedKeySaver);
            this.pnlMain.Controls.Add(this.btnSelectedKeyLoader);
            this.pnlMain.Controls.Add(this.btnSelectedKeySaver);
            this.pnlMain.Controls.Add(this.cboModifiedLayerSelector);
            this.pnlMain.Controls.Add(this.cboLayerBoneSelector);
            this.pnlMain.Controls.Add(this.btnModifiedLayerSelector);
            this.pnlMain.Controls.Add(this.chkLayerBoneSelector);
            this.pnlMain.Controls.Add(this.btnLayerBoneSelector);
            this.pnlMain.Controls.Add(this.btnGapSelector);
            this.pnlMain.Controls.Add(this.cboGapSelector);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Location = new System.Drawing.Point(6, 30);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(472, 244);
            this.pnlMain.TabIndex = 3;
            // 
            // chkWEnterMorphs
            // 
            this.chkWEnterMorphs.AutoSize = true;
            this.chkWEnterMorphs.Location = new System.Drawing.Point(6, 218);
            this.chkWEnterMorphs.Name = "chkWEnterMorphs";
            this.chkWEnterMorphs.Size = new System.Drawing.Size(94, 16);
            this.chkWEnterMorphs.TabIndex = 27;
            this.chkWEnterMorphs.Text = "Wでモーフ確定";
            this.chkWEnterMorphs.UseVisualStyleBackColor = true;
            this.chkWEnterMorphs.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // chkMorphOnMLS
            // 
            this.chkMorphOnMLS.AutoSize = true;
            this.chkMorphOnMLS.Location = new System.Drawing.Point(392, 83);
            this.chkMorphOnMLS.Name = "chkMorphOnMLS";
            this.chkMorphOnMLS.Size = new System.Drawing.Size(84, 16);
            this.chkMorphOnMLS.TabIndex = 26;
            this.chkMorphOnMLS.Text = "モーフも適用";
            this.chkMorphOnMLS.UseVisualStyleBackColor = true;
            this.chkMorphOnMLS.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // chkCancelForSmile
            // 
            this.chkCancelForSmile.AutoSize = true;
            this.chkCancelForSmile.Location = new System.Drawing.Point(355, 165);
            this.chkCancelForSmile.Name = "chkCancelForSmile";
            this.chkCancelForSmile.Size = new System.Drawing.Size(88, 16);
            this.chkCancelForSmile.TabIndex = 23;
            this.chkCancelForSmile.Text = "笑いにも適用";
            this.chkCancelForSmile.UseVisualStyleBackColor = true;
            this.chkCancelForSmile.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // cboBlinkCanceller
            // 
            this.cboBlinkCanceller.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBlinkCanceller.FormattingEnabled = true;
            this.cboBlinkCanceller.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboBlinkCanceller.Location = new System.Drawing.Point(293, 163);
            this.cboBlinkCanceller.Name = "cboBlinkCanceller";
            this.cboBlinkCanceller.Size = new System.Drawing.Size(58, 20);
            this.cboBlinkCanceller.TabIndex = 22;
            // 
            // btnBlinkCanceller
            // 
            this.btnBlinkCanceller.Location = new System.Drawing.Point(6, 162);
            this.btnBlinkCanceller.Name = "btnBlinkCanceller";
            this.btnBlinkCanceller.Size = new System.Drawing.Size(280, 23);
            this.btnBlinkCanceller.TabIndex = 21;
            this.btnBlinkCanceller.Text = "選択中のまばたきモーフに対する目モーフをキャンセルする";
            this.btnBlinkCanceller.UseVisualStyleBackColor = true;
            this.btnBlinkCanceller.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // cboFillDisplayFrame
            // 
            this.cboFillDisplayFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFillDisplayFrame.FormattingEnabled = true;
            this.cboFillDisplayFrame.Items.AddRange(new object[] {
            "Enter",
            "Space"});
            this.cboFillDisplayFrame.Location = new System.Drawing.Point(293, 137);
            this.cboFillDisplayFrame.Name = "cboFillDisplayFrame";
            this.cboFillDisplayFrame.Size = new System.Drawing.Size(58, 20);
            this.cboFillDisplayFrame.TabIndex = 20;
            // 
            // btnFillDisplayFrame
            // 
            this.btnFillDisplayFrame.Location = new System.Drawing.Point(7, 136);
            this.btnFillDisplayFrame.Name = "btnFillDisplayFrame";
            this.btnFillDisplayFrame.Size = new System.Drawing.Size(280, 23);
            this.btnFillDisplayFrame.TabIndex = 19;
            this.btnFillDisplayFrame.Text = "選択中の表示枠のキーを全選択";
            this.btnFillDisplayFrame.UseVisualStyleBackColor = true;
            this.btnFillDisplayFrame.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbInterpolateAll);
            this.panel1.Controls.Add(this.rbInterpolateZ);
            this.panel1.Controls.Add(this.rbInterpolateY);
            this.panel1.Controls.Add(this.rbInterpolateX);
            this.panel1.Controls.Add(this.rbInterpolateR);
            this.panel1.Location = new System.Drawing.Point(121, 187);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(138, 24);
            this.panel1.TabIndex = 18;
            // 
            // rbInterpolateAll
            // 
            this.rbInterpolateAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbInterpolateAll.AutoSize = true;
            this.rbInterpolateAll.Location = new System.Drawing.Point(89, 1);
            this.rbInterpolateAll.Name = "rbInterpolateAll";
            this.rbInterpolateAll.Size = new System.Drawing.Size(35, 22);
            this.rbInterpolateAll.TabIndex = 23;
            this.rbInterpolateAll.Text = "ALL";
            this.rbInterpolateAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbInterpolateAll.UseVisualStyleBackColor = true;
            this.rbInterpolateAll.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // rbInterpolateZ
            // 
            this.rbInterpolateZ.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbInterpolateZ.AutoSize = true;
            this.rbInterpolateZ.Location = new System.Drawing.Point(65, 1);
            this.rbInterpolateZ.Name = "rbInterpolateZ";
            this.rbInterpolateZ.Size = new System.Drawing.Size(22, 22);
            this.rbInterpolateZ.TabIndex = 22;
            this.rbInterpolateZ.Text = "Z";
            this.rbInterpolateZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbInterpolateZ.UseVisualStyleBackColor = true;
            this.rbInterpolateZ.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // rbInterpolateY
            // 
            this.rbInterpolateY.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbInterpolateY.AutoSize = true;
            this.rbInterpolateY.Location = new System.Drawing.Point(44, 1);
            this.rbInterpolateY.Name = "rbInterpolateY";
            this.rbInterpolateY.Size = new System.Drawing.Size(22, 22);
            this.rbInterpolateY.TabIndex = 21;
            this.rbInterpolateY.Text = "Y";
            this.rbInterpolateY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbInterpolateY.UseVisualStyleBackColor = true;
            this.rbInterpolateY.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // rbInterpolateX
            // 
            this.rbInterpolateX.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbInterpolateX.AutoSize = true;
            this.rbInterpolateX.Location = new System.Drawing.Point(23, 1);
            this.rbInterpolateX.Name = "rbInterpolateX";
            this.rbInterpolateX.Size = new System.Drawing.Size(22, 22);
            this.rbInterpolateX.TabIndex = 20;
            this.rbInterpolateX.Text = "X";
            this.rbInterpolateX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbInterpolateX.UseVisualStyleBackColor = true;
            this.rbInterpolateX.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // rbInterpolateR
            // 
            this.rbInterpolateR.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbInterpolateR.AutoSize = true;
            this.rbInterpolateR.Checked = true;
            this.rbInterpolateR.Location = new System.Drawing.Point(1, 1);
            this.rbInterpolateR.Name = "rbInterpolateR";
            this.rbInterpolateR.Size = new System.Drawing.Size(23, 22);
            this.rbInterpolateR.TabIndex = 19;
            this.rbInterpolateR.TabStop = true;
            this.rbInterpolateR.Text = "R";
            this.rbInterpolateR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbInterpolateR.UseVisualStyleBackColor = true;
            this.rbInterpolateR.CheckedChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(3, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "補完曲線パレット";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(291, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "0~6";
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
            this.cboHistory.Location = new System.Drawing.Point(354, 110);
            this.cboHistory.Name = "cboHistory";
            this.cboHistory.Size = new System.Drawing.Size(112, 20);
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
            this.cboSelectedKeyLoader.Location = new System.Drawing.Point(292, 110);
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
            this.cboSelectedKeySaver.Location = new System.Drawing.Point(115, 110);
            this.cboSelectedKeySaver.Name = "cboSelectedKeySaver";
            this.cboSelectedKeySaver.Size = new System.Drawing.Size(58, 20);
            this.cboSelectedKeySaver.TabIndex = 12;
            this.cboSelectedKeySaver.SelectedIndexChanged += new System.EventHandler(this.cboGapSelector_SelectedIndexChanged);
            this.cboSelectedKeySaver.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cboGapSelector_Format);
            // 
            // btnSelectedKeyLoader
            // 
            this.btnSelectedKeyLoader.Location = new System.Drawing.Point(179, 108);
            this.btnSelectedKeyLoader.Name = "btnSelectedKeyLoader";
            this.btnSelectedKeyLoader.Size = new System.Drawing.Size(107, 23);
            this.btnSelectedKeyLoader.TabIndex = 11;
            this.btnSelectedKeyLoader.Text = "選択キーのペースト";
            this.btnSelectedKeyLoader.UseVisualStyleBackColor = true;
            this.btnSelectedKeyLoader.Click += new System.EventHandler(this.btnGapSelector_Click);
            // 
            // btnSelectedKeySaver
            // 
            this.btnSelectedKeySaver.Location = new System.Drawing.Point(6, 108);
            this.btnSelectedKeySaver.Name = "btnSelectedKeySaver";
            this.btnSelectedKeySaver.Size = new System.Drawing.Size(107, 23);
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
            this.pnlOffset.Controls.Add(this.chkClickOffsetBtnByShiftEnter);
            this.pnlOffset.Controls.Add(this.btnExecuteOffset);
            this.pnlOffset.Location = new System.Drawing.Point(115, 2);
            this.pnlOffset.Name = "pnlOffset";
            this.pnlOffset.Size = new System.Drawing.Size(250, 26);
            this.pnlOffset.TabIndex = 4;
            this.pnlOffset.Visible = false;
            // 
            // chkClickOffsetBtnByShiftEnter
            // 
            this.chkClickOffsetBtnByShiftEnter.AutoSize = true;
            this.chkClickOffsetBtnByShiftEnter.ForeColor = System.Drawing.SystemColors.Window;
            this.chkClickOffsetBtnByShiftEnter.Location = new System.Drawing.Point(4, 7);
            this.chkClickOffsetBtnByShiftEnter.Name = "chkClickOffsetBtnByShiftEnter";
            this.chkClickOffsetBtnByShiftEnter.Size = new System.Drawing.Size(156, 16);
            this.chkClickOffsetBtnByShiftEnter.TabIndex = 7;
            this.chkClickOffsetBtnByShiftEnter.Text = "shift+Enterでオフセット付加";
            this.chkClickOffsetBtnByShiftEnter.UseVisualStyleBackColor = true;
            this.chkClickOffsetBtnByShiftEnter.CheckedChanged += new System.EventHandler(this.chkClickOffsetBtnByShiftEnter_CheckedChanged);
            // 
            // btnExecuteOffset
            // 
            this.btnExecuteOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteOffset.BackColor = System.Drawing.Color.Orange;
            this.btnExecuteOffset.Location = new System.Drawing.Point(165, 1);
            this.btnExecuteOffset.Name = "btnExecuteOffset";
            this.btnExecuteOffset.Size = new System.Drawing.Size(85, 25);
            this.btnExecuteOffset.TabIndex = 1;
            this.btnExecuteOffset.Text = "オフセット付加";
            this.btnExecuteOffset.UseVisualStyleBackColor = false;
            this.btnExecuteOffset.Click += new System.EventHandler(this.btnExecuteOffset_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Enabled = false;
            this.btnUndo.Location = new System.Drawing.Point(402, 2);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(76, 25);
            this.btnUndo.TabIndex = 2;
            this.btnUndo.Text = "アンドゥ";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 34);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(471, 20);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessage.BackColor = System.Drawing.SystemColors.Info;
            this.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Location = new System.Drawing.Point(13, 58);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(459, 49);
            this.pnlMessage.TabIndex = 6;
            this.pnlMessage.Visible = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(457, 47);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "処理中はPCに触れないでください。\r\n\r\n必要な回数適用されなくなる可能性があります";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 278);
            this.Controls.Add(this.pnlMessage);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.pnlOffset);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnOffset);
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "色々ヘルパー For Repair System";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlOffset.ResumeLayout(false);
            this.pnlOffset.PerformLayout();
            this.pnlMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOffset;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnlMain;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbInterpolateR;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbInterpolateAll;
        private System.Windows.Forms.RadioButton rbInterpolateZ;
        private System.Windows.Forms.RadioButton rbInterpolateY;
        private System.Windows.Forms.RadioButton rbInterpolateX;
        private System.Windows.Forms.ComboBox cboFillDisplayFrame;
        private System.Windows.Forms.Button btnFillDisplayFrame;
        private System.Windows.Forms.CheckBox chkClickOffsetBtnByShiftEnter;
        private System.Windows.Forms.ComboBox cboBlinkCanceller;
        private System.Windows.Forms.Button btnBlinkCanceller;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox chkCancelForSmile;
        private System.Windows.Forms.CheckBox chkMorphOnMLS;
        private System.Windows.Forms.CheckBox chkWEnterMorphs;
    }
}