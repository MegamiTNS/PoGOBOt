using System.Windows.Forms;

namespace PokemonGo.RocketBot.Window.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startStopBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.todoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.olvPokemonList = new BrightIdeasSoftware.ObjectListView();
            this.pkmnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnCP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnAtkIV = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnDefIV = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnStaIV = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnIV = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnCandy = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnCandyToEvolve = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnEvolveTimes = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnNickname = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnLevel = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnMove1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnMove2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnTransferButton = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnPowerUpButton = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pkmnEvolveButton = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cmsPokemonList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.largePokemonImageList = new System.Windows.Forms.ImageList(this.components);
            this.smallPokemonImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.togglePrecalRoute = new System.Windows.Forms.CheckBox();
            this.followTrainerCheckBox = new System.Windows.Forms.CheckBox();
            this.showMoreCheckBox = new System.Windows.Forms.CheckBox();
            this.speedLable = new System.Windows.Forms.Label();
            this.lblInventory = new System.Windows.Forms.Label();
            this.flpItems = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPokemonList = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvPokemonList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.Black;
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.logTextBox.ForeColor = System.Drawing.Color.White;
            this.logTextBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.logTextBox.Size = new System.Drawing.Size(611, 244);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 498);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1254, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(43, 17);
            this.statusLabel.Text = "Status";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStopBotToolStripMenuItem,
            this.todoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1254, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // startStopBotToolStripMenuItem
            // 
            this.startStopBotToolStripMenuItem.Name = "startStopBotToolStripMenuItem";
            this.startStopBotToolStripMenuItem.Size = new System.Drawing.Size(85, 21);
            this.startStopBotToolStripMenuItem.Text = "▶ Start Bot";
            this.startStopBotToolStripMenuItem.Click += new System.EventHandler(this.startStopBotToolStripMenuItem_Click);
            // 
            // todoToolStripMenuItem
            // 
            this.todoToolStripMenuItem.Enabled = false;
            this.todoToolStripMenuItem.Name = "todoToolStripMenuItem";
            this.todoToolStripMenuItem.Size = new System.Drawing.Size(540, 21);
            this.todoToolStripMenuItem.Text = "Settings (Not finish yet, please go to the config folder and set your information" +
    " manually)";
            this.todoToolStripMenuItem.Click += new System.EventHandler(this.todoToolStripMenuItem_Click);
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // showAllToolStripMenuItem1
            // 
            this.showAllToolStripMenuItem1.Name = "showAllToolStripMenuItem1";
            this.showAllToolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // gMapControl1
            // 
            this.gMapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(3, 3);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(605, 219);
            this.gMapControl1.TabIndex = 23;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.Load += new System.EventHandler(this.gMapControl1_Load);
            // 
            // olvPokemonList
            // 
            this.olvPokemonList.AllColumns.Add(this.pkmnName);
            this.olvPokemonList.AllColumns.Add(this.pkmnCP);
            this.olvPokemonList.AllColumns.Add(this.pkmnAtkIV);
            this.olvPokemonList.AllColumns.Add(this.pkmnDefIV);
            this.olvPokemonList.AllColumns.Add(this.pkmnStaIV);
            this.olvPokemonList.AllColumns.Add(this.pkmnIV);
            this.olvPokemonList.AllColumns.Add(this.pkmnCandy);
            this.olvPokemonList.AllColumns.Add(this.pkmnCandyToEvolve);
            this.olvPokemonList.AllColumns.Add(this.pkmnEvolveTimes);
            this.olvPokemonList.AllColumns.Add(this.pkmnNickname);
            this.olvPokemonList.AllColumns.Add(this.pkmnLevel);
            this.olvPokemonList.AllColumns.Add(this.pkmnMove1);
            this.olvPokemonList.AllColumns.Add(this.pkmnMove2);
            this.olvPokemonList.AllColumns.Add(this.pkmnTransferButton);
            this.olvPokemonList.AllColumns.Add(this.pkmnPowerUpButton);
            this.olvPokemonList.AllColumns.Add(this.pkmnEvolveButton);
            this.olvPokemonList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvPokemonList.CellEditUseWholeCell = false;
            this.olvPokemonList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pkmnName,
            this.pkmnCP,
            this.pkmnAtkIV,
            this.pkmnDefIV,
            this.pkmnStaIV,
            this.pkmnIV,
            this.pkmnCandy,
            this.pkmnCandyToEvolve,
            this.pkmnEvolveTimes,
            this.pkmnNickname,
            this.pkmnLevel,
            this.pkmnMove1,
            this.pkmnMove2,
            this.pkmnTransferButton,
            this.pkmnPowerUpButton,
            this.pkmnEvolveButton});
            this.olvPokemonList.ContextMenuStrip = this.cmsPokemonList;
            this.olvPokemonList.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvPokemonList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvPokemonList.FullRowSelect = true;
            this.olvPokemonList.GridLines = true;
            this.olvPokemonList.LargeImageList = this.largePokemonImageList;
            this.olvPokemonList.Location = new System.Drawing.Point(3, 3);
            this.olvPokemonList.Name = "olvPokemonList";
            this.olvPokemonList.RowHeight = 32;
            this.olvPokemonList.ShowGroups = false;
            this.olvPokemonList.Size = new System.Drawing.Size(633, 299);
            this.olvPokemonList.SmallImageList = this.smallPokemonImageList;
            this.olvPokemonList.TabIndex = 25;
            this.olvPokemonList.UseCompatibleStateImageBehavior = false;
            this.olvPokemonList.View = System.Windows.Forms.View.Details;
            this.olvPokemonList.ButtonClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.olvPokemonList_ButtonClick);
            this.olvPokemonList.SelectedIndexChanged += new System.EventHandler(this.olvPokemonList_SelectedIndexChanged);
            // 
            // pkmnName
            // 
            this.pkmnName.AspectName = "PokemonId";
            this.pkmnName.AspectToStringFormat = "";
            this.pkmnName.Text = "Name";
            this.pkmnName.Width = 120;
            // 
            // pkmnCP
            // 
            this.pkmnCP.AspectName = "Cp";
            this.pkmnCP.Text = "CP";
            this.pkmnCP.Width = 40;
            // 
            // pkmnAtkIV
            // 
            this.pkmnAtkIV.AspectName = "IndividualAttack";
            this.pkmnAtkIV.Text = "Atk IV";
            this.pkmnAtkIV.Width = 45;
            // 
            // pkmnDefIV
            // 
            this.pkmnDefIV.AspectName = "IndividualDefense";
            this.pkmnDefIV.Text = "Def IV";
            this.pkmnDefIV.Width = 45;
            // 
            // pkmnStaIV
            // 
            this.pkmnStaIV.AspectName = "IndividualStamina";
            this.pkmnStaIV.Text = "Sta IV";
            this.pkmnStaIV.Width = 45;
            // 
            // pkmnIV
            // 
            this.pkmnIV.AspectName = "GetIV";
            this.pkmnIV.AspectToStringFormat = "{0:P2}";
            this.pkmnIV.Text = "IV %";
            this.pkmnIV.Width = 52;
            // 
            // pkmnCandy
            // 
            this.pkmnCandy.AspectName = "Candy";
            this.pkmnCandy.Text = "Candy";
            this.pkmnCandy.Width = 46;
            // 
            // pkmnCandyToEvolve
            // 
            this.pkmnCandyToEvolve.AspectName = "CandyToEvolve";
            this.pkmnCandyToEvolve.Text = "CtE";
            this.pkmnCandyToEvolve.Width = 31;
            // 
            // pkmnEvolveTimes
            // 
            this.pkmnEvolveTimes.AspectName = "EvolveTimes";
            this.pkmnEvolveTimes.Text = "# Evolves";
            this.pkmnEvolveTimes.Width = 66;
            // 
            // pkmnNickname
            // 
            this.pkmnNickname.AspectName = "Nickname";
            this.pkmnNickname.Text = "Nickname";
            this.pkmnNickname.Width = 80;
            // 
            // pkmnLevel
            // 
            this.pkmnLevel.AspectName = "GetLv";
            this.pkmnLevel.Text = "Lv";
            this.pkmnLevel.Width = 50;
            // 
            // pkmnMove1
            // 
            this.pkmnMove1.AspectName = "Move1";
            this.pkmnMove1.Text = "Move1";
            // 
            // pkmnMove2
            // 
            this.pkmnMove2.AspectName = "Move2";
            this.pkmnMove2.Text = "Move2";
            // 
            // pkmnTransferButton
            // 
            this.pkmnTransferButton.AspectName = "Id";
            this.pkmnTransferButton.AspectToStringFormat = "Transfer";
            this.pkmnTransferButton.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.pkmnTransferButton.IsButton = true;
            this.pkmnTransferButton.Text = "";
            // 
            // pkmnPowerUpButton
            // 
            this.pkmnPowerUpButton.AspectName = "Id";
            this.pkmnPowerUpButton.AspectToStringFormat = "Power Up";
            this.pkmnPowerUpButton.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.pkmnPowerUpButton.IsButton = true;
            this.pkmnPowerUpButton.Text = "";
            // 
            // pkmnEvolveButton
            // 
            this.pkmnEvolveButton.AspectName = "Id";
            this.pkmnEvolveButton.AspectToStringFormat = "Evolve";
            this.pkmnEvolveButton.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.pkmnEvolveButton.IsButton = true;
            this.pkmnEvolveButton.Text = "";
            // 
            // cmsPokemonList
            // 
            this.cmsPokemonList.Name = "cmsPokemonList";
            this.cmsPokemonList.ShowImageMargin = false;
            this.cmsPokemonList.Size = new System.Drawing.Size(36, 4);
            // 
            // largePokemonImageList
            // 
            this.largePokemonImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.largePokemonImageList.ImageSize = new System.Drawing.Size(96, 96);
            this.largePokemonImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallPokemonImageList
            // 
            this.smallPokemonImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.smallPokemonImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.smallPokemonImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 446);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(633, 26);
            this.btnRefresh.TabIndex = 26;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblInventory);
            this.splitContainer1.Panel2.Controls.Add(this.flpItems);
            this.splitContainer1.Panel2.Controls.Add(this.lblPokemonList);
            this.splitContainer1.Panel2.Controls.Add(this.olvPokemonList);
            this.splitContainer1.Panel2.Controls.Add(this.btnRefresh);
            this.splitContainer1.Size = new System.Drawing.Size(1254, 473);
            this.splitContainer1.SplitterDistance = 611;
            this.splitContainer1.TabIndex = 27;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.logTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.togglePrecalRoute);
            this.splitContainer2.Panel2.Controls.Add(this.followTrainerCheckBox);
            this.splitContainer2.Panel2.Controls.Add(this.showMoreCheckBox);
            this.splitContainer2.Panel2.Controls.Add(this.speedLable);
            this.splitContainer2.Panel2.Controls.Add(this.gMapControl1);
            this.splitContainer2.Size = new System.Drawing.Size(611, 473);
            this.splitContainer2.SplitterDistance = 244;
            this.splitContainer2.TabIndex = 0;
            // 
            // togglePrecalRoute
            // 
            this.togglePrecalRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.togglePrecalRoute.AutoSize = true;
            this.togglePrecalRoute.BackColor = System.Drawing.Color.Transparent;
            this.togglePrecalRoute.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.togglePrecalRoute.Checked = true;
            this.togglePrecalRoute.CheckState = System.Windows.Forms.CheckState.Checked;
            this.togglePrecalRoute.Enabled = false;
            this.togglePrecalRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.togglePrecalRoute.Location = new System.Drawing.Point(420, 53);
            this.togglePrecalRoute.Name = "togglePrecalRoute";
            this.togglePrecalRoute.Size = new System.Drawing.Size(184, 19);
            this.togglePrecalRoute.TabIndex = 27;
            this.togglePrecalRoute.Text = "Toggle Pre-Calculated Route";
            this.togglePrecalRoute.UseVisualStyleBackColor = false;
            this.togglePrecalRoute.Visible = false;
            this.togglePrecalRoute.CheckedChanged += new System.EventHandler(this.togglePrecalRoute_CheckedChanged);
            // 
            // followTrainerCheckBox
            // 
            this.followTrainerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.followTrainerCheckBox.AutoSize = true;
            this.followTrainerCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.followTrainerCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.followTrainerCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.followTrainerCheckBox.Location = new System.Drawing.Point(472, 28);
            this.followTrainerCheckBox.Name = "followTrainerCheckBox";
            this.followTrainerCheckBox.Size = new System.Drawing.Size(132, 19);
            this.followTrainerCheckBox.TabIndex = 26;
            this.followTrainerCheckBox.Text = "Map Follow Trainer";
            this.followTrainerCheckBox.UseVisualStyleBackColor = false;
            this.followTrainerCheckBox.Visible = false;
            this.followTrainerCheckBox.CheckedChanged += new System.EventHandler(this.followTrainerCheckBox_CheckedChanged);
            // 
            // showMoreCheckBox
            // 
            this.showMoreCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showMoreCheckBox.AutoSize = true;
            this.showMoreCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.showMoreCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showMoreCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showMoreCheckBox.Location = new System.Drawing.Point(453, 3);
            this.showMoreCheckBox.Name = "showMoreCheckBox";
            this.showMoreCheckBox.Size = new System.Drawing.Size(151, 19);
            this.showMoreCheckBox.TabIndex = 25;
            this.showMoreCheckBox.Text = "Show Advance Options";
            this.showMoreCheckBox.UseVisualStyleBackColor = false;
            this.showMoreCheckBox.CheckedChanged += new System.EventHandler(this.showMoreCheckBox_CheckedChanged);
            // 
            // speedLable
            // 
            this.speedLable.AutoSize = true;
            this.speedLable.BackColor = System.Drawing.Color.Transparent;
            this.speedLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speedLable.Location = new System.Drawing.Point(1, 3);
            this.speedLable.Name = "speedLable";
            this.speedLable.Size = new System.Drawing.Size(0, 15);
            this.speedLable.TabIndex = 24;
            // 
            // lblInventory
            // 
            this.lblInventory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInventory.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInventory.Location = new System.Drawing.Point(3, 432);
            this.lblInventory.Name = "lblInventory";
            this.lblInventory.Size = new System.Drawing.Size(633, 12);
            this.lblInventory.TabIndex = 33;
            this.lblInventory.Text = "0 / 0 ";
            this.lblInventory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpItems
            // 
            this.flpItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpItems.AutoScroll = true;
            this.flpItems.BackColor = System.Drawing.SystemColors.Window;
            this.flpItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpItems.Location = new System.Drawing.Point(3, 317);
            this.flpItems.Name = "flpItems";
            this.flpItems.Size = new System.Drawing.Size(633, 111);
            this.flpItems.TabIndex = 32;
            // 
            // lblPokemonList
            // 
            this.lblPokemonList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPokemonList.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPokemonList.Location = new System.Drawing.Point(3, 303);
            this.lblPokemonList.Name = "lblPokemonList";
            this.lblPokemonList.Size = new System.Drawing.Size(633, 11);
            this.lblPokemonList.TabIndex = 27;
            this.lblPokemonList.Text = "0 / 0";
            this.lblPokemonList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 520);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RocketBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvPokemonList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem todoToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem startStopBotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem1;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private BrightIdeasSoftware.ObjectListView olvPokemonList;
        private BrightIdeasSoftware.OLVColumn pkmnName;
        private BrightIdeasSoftware.OLVColumn pkmnCP;
        private BrightIdeasSoftware.OLVColumn pkmnAtkIV;
        private BrightIdeasSoftware.OLVColumn pkmnDefIV;
        private BrightIdeasSoftware.OLVColumn pkmnStaIV;
        private BrightIdeasSoftware.OLVColumn pkmnIV;
        private BrightIdeasSoftware.OLVColumn pkmnTransferButton;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ImageList smallPokemonImageList;
        private System.Windows.Forms.ImageList largePokemonImageList;
        private BrightIdeasSoftware.OLVColumn pkmnPowerUpButton;
        private BrightIdeasSoftware.OLVColumn pkmnEvolveButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblPokemonList;
        private BrightIdeasSoftware.OLVColumn pkmnCandy;
        private BrightIdeasSoftware.OLVColumn pkmnCandyToEvolve;
        private BrightIdeasSoftware.OLVColumn pkmnEvolveTimes;
        private System.Windows.Forms.ContextMenuStrip cmsPokemonList;
        private System.Windows.Forms.FlowLayoutPanel flpItems;
        private System.Windows.Forms.Label lblInventory;
        private BrightIdeasSoftware.OLVColumn pkmnNickname;
        private BrightIdeasSoftware.OLVColumn pkmnLevel;
        private BrightIdeasSoftware.OLVColumn pkmnMove1;
        private BrightIdeasSoftware.OLVColumn pkmnMove2;
        private Label speedLable;
        private CheckBox togglePrecalRoute;
        private CheckBox followTrainerCheckBox;
        private CheckBox showMoreCheckBox;
    }
}
