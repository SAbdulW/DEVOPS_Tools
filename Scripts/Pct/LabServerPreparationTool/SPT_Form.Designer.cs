namespace LabServerPreparationTool
{
    partial class SPT_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPT_Form));
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firewallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restrictSSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Accounts_UseSingleAccount = new System.Windows.Forms.CheckBox();
            this.SQLVersionBox = new System.Windows.Forms.ComboBox();
            this.PlatformBox = new System.Windows.Forms.ComboBox();
            this.FoldersButton = new System.Windows.Forms.Button();
            this.AllActionsButton = new System.Windows.Forms.Button();
            this.Accounts_IMSAPassword = new System.Windows.Forms.TextBox();
            this.Accounts_DMSAPassword = new System.Windows.Forms.TextBox();
            this.Accounts_SQLServicesPassword = new System.Windows.Forms.TextBox();
            this.Accounts_Domain = new System.Windows.Forms.TextBox();
            this.Accounts_SQLServicestAccount = new System.Windows.Forms.TextBox();
            this.Accounts_InstallUser = new System.Windows.Forms.TextBox();
            this.Accounts_DMSA = new System.Windows.Forms.TextBox();
            this.Accounts_IMSA = new System.Windows.Forms.TextBox();
            this.Accounts_LabelIMSAPassword = new System.Windows.Forms.Label();
            this.Accounts_LabelDMSAPassword = new System.Windows.Forms.Label();
            this.Accounts_LabelSQLServicesPassword = new System.Windows.Forms.Label();
            this.Accounts_LabelDomain = new System.Windows.Forms.Label();
            this.Accounts_LabelSQLServicesAccount = new System.Windows.Forms.Label();
            this.Accounts_LabelInstallationAccount = new System.Windows.Forms.Label();
            this.Accounts_LabelDMSA = new System.Windows.Forms.Label();
            this.Accounts_LabelIMSA = new System.Windows.Forms.Label();
            this.SQLConnection_Groupbox = new System.Windows.Forms.GroupBox();
            this.SQLServerVersionLabel = new System.Windows.Forms.Label();
            this.SQLConnection_SQLPort = new System.Windows.Forms.TextBox();
            this.SQLConnection_LabelSQLServerName = new System.Windows.Forms.Label();
            this.SQLConnection__LabelSQLServerPort = new System.Windows.Forms.Label();
            this.SQLConnection_SQLServer = new System.Windows.Forms.TextBox();
            this.ApplyWindowsSettingsCheckbox = new System.Windows.Forms.CheckBox();
            this.CreateFoldersAndRegistryCheckbox = new System.Windows.Forms.CheckBox();
            this.ConfigureSQLAccountsCheckbox = new System.Windows.Forms.CheckBox();
            this.SPT_Tooltips = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.SQLConnection_Groupbox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.Control;
            this.LogBox.Location = new System.Drawing.Point(9, 409);
            this.LogBox.Margin = new System.Windows.Forms.Padding(4);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(756, 100);
            this.LogBox.TabIndex = 65;
            this.LogBox.Text = "";
            this.LogBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogBox_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.advancedToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(776, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Reset,
            this.toolStripMenuItem1,
            this.Menu_Load,
            this.Menu_Save,
            this.toolStripMenuItem2,
            this.Menu_Exit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // Menu_Reset
            // 
            this.Menu_Reset.Name = "Menu_Reset";
            this.Menu_Reset.Size = new System.Drawing.Size(117, 26);
            this.Menu_Reset.Text = "New";
            this.Menu_Reset.ToolTipText = "Start a new project and reset all \r\nvalues to the default values ";
            this.Menu_Reset.Click += new System.EventHandler(this.Menu_Reset_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(114, 6);
            // 
            // Menu_Load
            // 
            this.Menu_Load.Name = "Menu_Load";
            this.Menu_Load.Size = new System.Drawing.Size(117, 26);
            this.Menu_Load.Text = "Load";
            this.Menu_Load.ToolTipText = "Load the previously saved project’s values";
            this.Menu_Load.Click += new System.EventHandler(this.Menu_Load_Click);
            // 
            // Menu_Save
            // 
            this.Menu_Save.Name = "Menu_Save";
            this.Menu_Save.Size = new System.Drawing.Size(117, 26);
            this.Menu_Save.Text = "Save";
            this.Menu_Save.ToolTipText = "Retain a project’s values for future use ";
            this.Menu_Save.Click += new System.EventHandler(this.Menu_Save_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 6);
            // 
            // Menu_Exit
            // 
            this.Menu_Exit.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.Menu_Exit.Name = "Menu_Exit";
            this.Menu_Exit.Size = new System.Drawing.Size(117, 26);
            this.Menu_Exit.Text = "Exit";
            this.Menu_Exit.Click += new System.EventHandler(this.Menu_Exit_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firewallToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(87, 24);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // firewallToolStripMenuItem
            // 
            this.firewallToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.restrictSSLToolStripMenuItem});
            this.firewallToolStripMenuItem.Name = "firewallToolStripMenuItem";
            this.firewallToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.firewallToolStripMenuItem.Text = "Firewall";
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.enableToolStripMenuItem.Text = "Enable Firewall";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.disableToolStripMenuItem.Text = "Disable Firewall";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // restrictSSLToolStripMenuItem
            // 
            this.restrictSSLToolStripMenuItem.Name = "restrictSSLToolStripMenuItem";
            this.restrictSSLToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.restrictSSLToolStripMenuItem.Text = "Restrict SSL Ports";
            this.restrictSSLToolStripMenuItem.Click += new System.EventHandler(this.restrictSSLToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(589, 517);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(177, 25);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // Accounts_UseSingleAccount
            // 
            this.Accounts_UseSingleAccount.AutoSize = true;
            this.Accounts_UseSingleAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_UseSingleAccount.ForeColor = System.Drawing.Color.Black;
            this.Accounts_UseSingleAccount.Location = new System.Drawing.Point(199, 30);
            this.Accounts_UseSingleAccount.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_UseSingleAccount.Name = "Accounts_UseSingleAccount";
            this.Accounts_UseSingleAccount.Size = new System.Drawing.Size(153, 21);
            this.Accounts_UseSingleAccount.TabIndex = 46;
            this.Accounts_UseSingleAccount.Text = "Use Single Account";
            this.SPT_Tooltips.SetToolTip(this.Accounts_UseSingleAccount, "Use a single account for all user accounts");
            this.Accounts_UseSingleAccount.UseVisualStyleBackColor = true;
            this.Accounts_UseSingleAccount.CheckedChanged += new System.EventHandler(this.Accounts_UseSingleAccount_CheckedChanged_1);
            // 
            // SQLVersionBox
            // 
            this.SQLVersionBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.SQLVersionBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.SQLVersionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SQLVersionBox.FormattingEnabled = true;
            this.SQLVersionBox.ItemHeight = 17;
            this.SQLVersionBox.Items.AddRange(new object[] {
            "",
            "SQL 2008",
            "SQL 2012",
            "SQL 2014"});
            this.SQLVersionBox.Location = new System.Drawing.Point(167, 84);
            this.SQLVersionBox.Margin = new System.Windows.Forms.Padding(4);
            this.SQLVersionBox.Name = "SQLVersionBox";
            this.SQLVersionBox.Size = new System.Drawing.Size(171, 25);
            this.SQLVersionBox.TabIndex = 57;
            this.SPT_Tooltips.SetToolTip(this.SQLVersionBox, "Select the SQL Server version");
            this.SQLVersionBox.SelectedIndexChanged += new System.EventHandler(this.SQLVersionBox_SelectedIndexChanged_1);
            // 
            // PlatformBox
            // 
            this.PlatformBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.PlatformBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PlatformBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformBox.FormattingEnabled = true;
            this.PlatformBox.ItemHeight = 17;
            this.PlatformBox.Items.AddRange(new object[] {
            "",
            "Application Server",
            "Mobile Gateway Server",
            "Application Load Balancer",
            "Centralized Archive Server",
            "Contact & QM Database Server",
            "Contact OLTP & Archive Database Server",
            "Data Warehouse Server",
            "Database Management Server",
            "Database Server",
            "EFM Application Server",
            "EFM Database Server",
            "Encryption Key Management Server",
            "Export Manager",
            "Forecasting and Scheduling Server",
            "Framework Database & Reporting Server",
            "Framework DB Server",
            "Framework Integration Service Server",
            "Import Manager Server",
            "Import Manager With Transcription Server",
            "IP Analyzer Server",
            "Lync Proxy Server",
            "Post Processing Voice Biometrics Server",
            "Recorder Integration Service Server",
            "Recorder Server",
            "Recording & QM Database Server",
            "Remote SQL Server",
            "Reporting Server",
            "Speech Analytics Application Server",
            "Speech Analytics Transcription Server",
            "Speech Database Server",
            "Survey Server with Telephony Interface",
            "Survey Server with VXML Interface",
            "Telephone Playback Service Server",
            "Text Analytics Application Server",
            "Text Analytics Consolidated Server",
            "Voiceprint Enrollment Server",
            "Consolidated Server",
            "Data Center Server",
            "Desktop",
            "TeTCPnal Server",
            "Text Analytics Datastore Server"});
            this.PlatformBox.Location = new System.Drawing.Point(7, 34);
            this.PlatformBox.Margin = new System.Windows.Forms.Padding(4);
            this.PlatformBox.Name = "PlatformBox";
            this.PlatformBox.Size = new System.Drawing.Size(329, 25);
            this.PlatformBox.TabIndex = 56;
            this.SPT_Tooltips.SetToolTip(this.PlatformBox, "Select the server type");
            this.PlatformBox.SelectedIndexChanged += new System.EventHandler(this.PlatformBox_SelectedIndexChanged_1);
            // 
            // FoldersButton
            // 
            this.FoldersButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FoldersButton.ForeColor = System.Drawing.Color.Black;
            this.FoldersButton.Location = new System.Drawing.Point(7, 197);
            this.FoldersButton.Margin = new System.Windows.Forms.Padding(4);
            this.FoldersButton.Name = "FoldersButton";
            this.FoldersButton.Size = new System.Drawing.Size(331, 32);
            this.FoldersButton.TabIndex = 60;
            this.FoldersButton.Text = "System Folders";
            this.SPT_Tooltips.SetToolTip(this.FoldersButton, "Configure system folder paths as \r\nthey will be provided to SR Installer");
            this.FoldersButton.UseVisualStyleBackColor = true;
            this.FoldersButton.Click += new System.EventHandler(this.FoldersButton_Click_1);
            // 
            // AllActionsButton
            // 
            this.AllActionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllActionsButton.ForeColor = System.Drawing.Color.Blue;
            this.AllActionsButton.Location = new System.Drawing.Point(667, 332);
            this.AllActionsButton.Margin = new System.Windows.Forms.Padding(4);
            this.AllActionsButton.Name = "AllActionsButton";
            this.AllActionsButton.Size = new System.Drawing.Size(100, 57);
            this.AllActionsButton.TabIndex = 64;
            this.AllActionsButton.Text = "Prepare Server";
            this.SPT_Tooltips.SetToolTip(this.AllActionsButton, "Run all selected operations to prepare the server");
            this.AllActionsButton.UseVisualStyleBackColor = true;
            this.AllActionsButton.Click += new System.EventHandler(this.AllActionsButton_Click);
            // 
            // Accounts_IMSAPassword
            // 
            this.Accounts_IMSAPassword.AutoCompleteCustomSource.AddRange(new string[] {
            "verint1!",
            "smart",
            "pumpkin",
            "pumpkin1"});
            this.Accounts_IMSAPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Accounts_IMSAPassword.Location = new System.Drawing.Point(173, 171);
            this.Accounts_IMSAPassword.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_IMSAPassword.Name = "Accounts_IMSAPassword";
            this.Accounts_IMSAPassword.Size = new System.Drawing.Size(215, 24);
            this.Accounts_IMSAPassword.TabIndex = 51;
            this.SPT_Tooltips.SetToolTip(this.Accounts_IMSAPassword, "Management Service Account password");
            this.Accounts_IMSAPassword.UseSystemPasswordChar = true;
            this.Accounts_IMSAPassword.TextChanged += new System.EventHandler(this.Accounts_IMSAPassword_TextChanged);
            this.Accounts_IMSAPassword.Leave += new System.EventHandler(this.Accounts_IMSAPassword_Leave_1);
            // 
            // Accounts_DMSAPassword
            // 
            this.Accounts_DMSAPassword.Location = new System.Drawing.Point(173, 230);
            this.Accounts_DMSAPassword.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_DMSAPassword.Name = "Accounts_DMSAPassword";
            this.Accounts_DMSAPassword.Size = new System.Drawing.Size(215, 24);
            this.Accounts_DMSAPassword.TabIndex = 53;
            this.SPT_Tooltips.SetToolTip(this.Accounts_DMSAPassword, "Database Management Service Account password");
            this.Accounts_DMSAPassword.UseSystemPasswordChar = true;
            this.Accounts_DMSAPassword.TextChanged += new System.EventHandler(this.Accounts_DMSAPassword_TextChanged);
            // 
            // Accounts_SQLServicesPassword
            // 
            this.Accounts_SQLServicesPassword.Location = new System.Drawing.Point(175, 311);
            this.Accounts_SQLServicesPassword.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_SQLServicesPassword.Name = "Accounts_SQLServicesPassword";
            this.Accounts_SQLServicesPassword.Size = new System.Drawing.Size(211, 24);
            this.Accounts_SQLServicesPassword.TabIndex = 55;
            this.SPT_Tooltips.SetToolTip(this.Accounts_SQLServicesPassword, "Password of the account which runs\r\nSQL Server and SQL Agent services");
            this.Accounts_SQLServicesPassword.UseSystemPasswordChar = true;
            this.Accounts_SQLServicesPassword.TextChanged += new System.EventHandler(this.Accounts_SQLServicesPassword_TextChanged);
            // 
            // Accounts_Domain
            // 
            this.Accounts_Domain.Location = new System.Drawing.Point(173, 64);
            this.Accounts_Domain.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_Domain.Name = "Accounts_Domain";
            this.Accounts_Domain.Size = new System.Drawing.Size(215, 24);
            this.Accounts_Domain.TabIndex = 47;
            this.SPT_Tooltips.SetToolTip(this.Accounts_Domain, "The domain that holds the accounts.\r\nIn case of workgroup, provide the local serv" +
        "er name.");
            this.Accounts_Domain.TextChanged += new System.EventHandler(this.Accounts_Domain_TextChanged);
            // 
            // Accounts_SQLServicestAccount
            // 
            this.Accounts_SQLServicestAccount.Location = new System.Drawing.Point(175, 281);
            this.Accounts_SQLServicestAccount.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_SQLServicestAccount.Name = "Accounts_SQLServicestAccount";
            this.Accounts_SQLServicestAccount.Size = new System.Drawing.Size(211, 24);
            this.Accounts_SQLServicestAccount.TabIndex = 54;
            this.SPT_Tooltips.SetToolTip(this.Accounts_SQLServicestAccount, "The domain or local user account defined to run \r\nthe SQL Server and the SQL Serv" +
        "er Agent services.\r\n");
            this.Accounts_SQLServicestAccount.TextChanged += new System.EventHandler(this.Accounts_SQLServicestAccount_TextChanged);
            this.Accounts_SQLServicestAccount.Leave += new System.EventHandler(this.Accounts_SQLServicestAccount_Leave);
            // 
            // Accounts_InstallUser
            // 
            this.Accounts_InstallUser.Location = new System.Drawing.Point(173, 112);
            this.Accounts_InstallUser.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_InstallUser.Name = "Accounts_InstallUser";
            this.Accounts_InstallUser.Size = new System.Drawing.Size(215, 24);
            this.Accounts_InstallUser.TabIndex = 49;
            this.SPT_Tooltips.SetToolTip(this.Accounts_InstallUser, "The account which will be used to log on \r\nto the server and run the SR installat" +
        "ion");
            this.Accounts_InstallUser.TextChanged += new System.EventHandler(this.Accounts_InstallUser_TextChanged);
            // 
            // Accounts_DMSA
            // 
            this.Accounts_DMSA.Location = new System.Drawing.Point(173, 201);
            this.Accounts_DMSA.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_DMSA.Name = "Accounts_DMSA";
            this.Accounts_DMSA.Size = new System.Drawing.Size(215, 24);
            this.Accounts_DMSA.TabIndex = 52;
            this.SPT_Tooltips.SetToolTip(this.Accounts_DMSA, "Database Management Service Account user name");
            this.Accounts_DMSA.TextChanged += new System.EventHandler(this.Accounts_DMSA_TextChanged);
            // 
            // Accounts_IMSA
            // 
            this.Accounts_IMSA.Location = new System.Drawing.Point(173, 142);
            this.Accounts_IMSA.Margin = new System.Windows.Forms.Padding(4);
            this.Accounts_IMSA.Name = "Accounts_IMSA";
            this.Accounts_IMSA.Size = new System.Drawing.Size(215, 24);
            this.Accounts_IMSA.TabIndex = 50;
            this.SPT_Tooltips.SetToolTip(this.Accounts_IMSA, "Management Service Account user name");
            this.Accounts_IMSA.TextChanged += new System.EventHandler(this.Accounts_IMSA_TextChanged);
            this.Accounts_IMSA.Leave += new System.EventHandler(this.Accounts_IMSA_Leave);
            // 
            // Accounts_LabelIMSAPassword
            // 
            this.Accounts_LabelIMSAPassword.AutoSize = true;
            this.Accounts_LabelIMSAPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelIMSAPassword.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelIMSAPassword.Location = new System.Drawing.Point(7, 175);
            this.Accounts_LabelIMSAPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelIMSAPassword.Name = "Accounts_LabelIMSAPassword";
            this.Accounts_LabelIMSAPassword.Size = new System.Drawing.Size(102, 17);
            this.Accounts_LabelIMSAPassword.TabIndex = 42;
            this.Accounts_LabelIMSAPassword.Text = "MSA Password";
            // 
            // Accounts_LabelDMSAPassword
            // 
            this.Accounts_LabelDMSAPassword.AutoSize = true;
            this.Accounts_LabelDMSAPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelDMSAPassword.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelDMSAPassword.Location = new System.Drawing.Point(7, 235);
            this.Accounts_LabelDMSAPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelDMSAPassword.Name = "Accounts_LabelDMSAPassword";
            this.Accounts_LabelDMSAPassword.Size = new System.Drawing.Size(112, 17);
            this.Accounts_LabelDMSAPassword.TabIndex = 48;
            this.Accounts_LabelDMSAPassword.Text = "DMSA Password";
            // 
            // Accounts_LabelSQLServicesPassword
            // 
            this.Accounts_LabelSQLServicesPassword.AutoSize = true;
            this.Accounts_LabelSQLServicesPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelSQLServicesPassword.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelSQLServicesPassword.Location = new System.Drawing.Point(7, 315);
            this.Accounts_LabelSQLServicesPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelSQLServicesPassword.Name = "Accounts_LabelSQLServicesPassword";
            this.Accounts_LabelSQLServicesPassword.Size = new System.Drawing.Size(159, 17);
            this.Accounts_LabelSQLServicesPassword.TabIndex = 40;
            this.Accounts_LabelSQLServicesPassword.Text = "SQL Services Password";
            // 
            // Accounts_LabelDomain
            // 
            this.Accounts_LabelDomain.AutoSize = true;
            this.Accounts_LabelDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelDomain.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelDomain.Location = new System.Drawing.Point(7, 68);
            this.Accounts_LabelDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelDomain.Name = "Accounts_LabelDomain";
            this.Accounts_LabelDomain.Size = new System.Drawing.Size(56, 17);
            this.Accounts_LabelDomain.TabIndex = 35;
            this.Accounts_LabelDomain.Text = "Domain";
            // 
            // Accounts_LabelSQLServicesAccount
            // 
            this.Accounts_LabelSQLServicesAccount.AutoSize = true;
            this.Accounts_LabelSQLServicesAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelSQLServicesAccount.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelSQLServicesAccount.Location = new System.Drawing.Point(7, 284);
            this.Accounts_LabelSQLServicesAccount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelSQLServicesAccount.Name = "Accounts_LabelSQLServicesAccount";
            this.Accounts_LabelSQLServicesAccount.Size = new System.Drawing.Size(149, 17);
            this.Accounts_LabelSQLServicesAccount.TabIndex = 39;
            this.Accounts_LabelSQLServicesAccount.Text = "SQL Services Account";
            // 
            // Accounts_LabelInstallationAccount
            // 
            this.Accounts_LabelInstallationAccount.AutoSize = true;
            this.Accounts_LabelInstallationAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelInstallationAccount.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelInstallationAccount.Location = new System.Drawing.Point(7, 116);
            this.Accounts_LabelInstallationAccount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelInstallationAccount.Name = "Accounts_LabelInstallationAccount";
            this.Accounts_LabelInstallationAccount.Size = new System.Drawing.Size(130, 17);
            this.Accounts_LabelInstallationAccount.TabIndex = 37;
            this.Accounts_LabelInstallationAccount.Text = "Installation Account";
            // 
            // Accounts_LabelDMSA
            // 
            this.Accounts_LabelDMSA.AutoSize = true;
            this.Accounts_LabelDMSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelDMSA.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelDMSA.Location = new System.Drawing.Point(7, 206);
            this.Accounts_LabelDMSA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelDMSA.Name = "Accounts_LabelDMSA";
            this.Accounts_LabelDMSA.Size = new System.Drawing.Size(47, 17);
            this.Accounts_LabelDMSA.TabIndex = 36;
            this.Accounts_LabelDMSA.Text = "DMSA";
            // 
            // Accounts_LabelIMSA
            // 
            this.Accounts_LabelIMSA.AutoSize = true;
            this.Accounts_LabelIMSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts_LabelIMSA.ForeColor = System.Drawing.Color.Black;
            this.Accounts_LabelIMSA.Location = new System.Drawing.Point(7, 145);
            this.Accounts_LabelIMSA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Accounts_LabelIMSA.Name = "Accounts_LabelIMSA";
            this.Accounts_LabelIMSA.Size = new System.Drawing.Size(37, 17);
            this.Accounts_LabelIMSA.TabIndex = 41;
            this.Accounts_LabelIMSA.Text = "MSA";
            // 
            // SQLConnection_Groupbox
            // 
            this.SQLConnection_Groupbox.Controls.Add(this.SQLServerVersionLabel);
            this.SQLConnection_Groupbox.Controls.Add(this.PlatformBox);
            this.SQLConnection_Groupbox.Controls.Add(this.FoldersButton);
            this.SQLConnection_Groupbox.Controls.Add(this.SQLConnection_SQLPort);
            this.SQLConnection_Groupbox.Controls.Add(this.SQLConnection_LabelSQLServerName);
            this.SQLConnection_Groupbox.Controls.Add(this.SQLConnection__LabelSQLServerPort);
            this.SQLConnection_Groupbox.Controls.Add(this.SQLConnection_SQLServer);
            this.SQLConnection_Groupbox.Controls.Add(this.SQLVersionBox);
            this.SQLConnection_Groupbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLConnection_Groupbox.ForeColor = System.Drawing.Color.Blue;
            this.SQLConnection_Groupbox.Location = new System.Drawing.Point(417, 38);
            this.SQLConnection_Groupbox.Margin = new System.Windows.Forms.Padding(4);
            this.SQLConnection_Groupbox.Name = "SQLConnection_Groupbox";
            this.SQLConnection_Groupbox.Padding = new System.Windows.Forms.Padding(4);
            this.SQLConnection_Groupbox.Size = new System.Drawing.Size(349, 241);
            this.SQLConnection_Groupbox.TabIndex = 0;
            this.SQLConnection_Groupbox.TabStop = false;
            this.SQLConnection_Groupbox.Text = "Server Details";
            // 
            // SQLServerVersionLabel
            // 
            this.SQLServerVersionLabel.AutoSize = true;
            this.SQLServerVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLServerVersionLabel.ForeColor = System.Drawing.Color.Black;
            this.SQLServerVersionLabel.Location = new System.Drawing.Point(8, 89);
            this.SQLServerVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SQLServerVersionLabel.Name = "SQLServerVersionLabel";
            this.SQLServerVersionLabel.Size = new System.Drawing.Size(134, 17);
            this.SQLServerVersionLabel.TabIndex = 56;
            this.SQLServerVersionLabel.Text = "SQL Server Version";
            // 
            // SQLConnection_SQLPort
            // 
            this.SQLConnection_SQLPort.Location = new System.Drawing.Point(167, 146);
            this.SQLConnection_SQLPort.Margin = new System.Windows.Forms.Padding(4);
            this.SQLConnection_SQLPort.Name = "SQLConnection_SQLPort";
            this.SQLConnection_SQLPort.Size = new System.Drawing.Size(169, 24);
            this.SQLConnection_SQLPort.TabIndex = 59;
            this.SPT_Tooltips.SetToolTip(this.SQLConnection_SQLPort, "The port on which the SQL Server listens");
            this.SQLConnection_SQLPort.TextChanged += new System.EventHandler(this.SQLConnection_SQLPort_TextChanged);
            // 
            // SQLConnection_LabelSQLServerName
            // 
            this.SQLConnection_LabelSQLServerName.AutoSize = true;
            this.SQLConnection_LabelSQLServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLConnection_LabelSQLServerName.ForeColor = System.Drawing.Color.Black;
            this.SQLConnection_LabelSQLServerName.Location = new System.Drawing.Point(8, 119);
            this.SQLConnection_LabelSQLServerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SQLConnection_LabelSQLServerName.Name = "SQLConnection_LabelSQLServerName";
            this.SQLConnection_LabelSQLServerName.Size = new System.Drawing.Size(123, 17);
            this.SQLConnection_LabelSQLServerName.TabIndex = 0;
            this.SQLConnection_LabelSQLServerName.Text = "SQL Server Name";
            // 
            // SQLConnection__LabelSQLServerPort
            // 
            this.SQLConnection__LabelSQLServerPort.AutoSize = true;
            this.SQLConnection__LabelSQLServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLConnection__LabelSQLServerPort.ForeColor = System.Drawing.Color.Black;
            this.SQLConnection__LabelSQLServerPort.Location = new System.Drawing.Point(8, 150);
            this.SQLConnection__LabelSQLServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SQLConnection__LabelSQLServerPort.Name = "SQLConnection__LabelSQLServerPort";
            this.SQLConnection__LabelSQLServerPort.Size = new System.Drawing.Size(112, 17);
            this.SQLConnection__LabelSQLServerPort.TabIndex = 0;
            this.SQLConnection__LabelSQLServerPort.Text = "SQL Server Port";
            // 
            // SQLConnection_SQLServer
            // 
            this.SQLConnection_SQLServer.Location = new System.Drawing.Point(167, 116);
            this.SQLConnection_SQLServer.Margin = new System.Windows.Forms.Padding(4);
            this.SQLConnection_SQLServer.Name = "SQLConnection_SQLServer";
            this.SQLConnection_SQLServer.Size = new System.Drawing.Size(169, 24);
            this.SQLConnection_SQLServer.TabIndex = 58;
            this.SPT_Tooltips.SetToolTip(this.SQLConnection_SQLServer, "Name of the SQL Server to connect to \r\n(must be local on current server)");
            this.SQLConnection_SQLServer.TextChanged += new System.EventHandler(this.SQLConnection_SQLServer_TextChanged);
            this.SQLConnection_SQLServer.Leave += new System.EventHandler(this.SQLConnection_SQLServer_Leave);
            // 
            // ApplyWindowsSettingsCheckbox
            // 
            this.ApplyWindowsSettingsCheckbox.AutoSize = true;
            this.ApplyWindowsSettingsCheckbox.Checked = true;
            this.ApplyWindowsSettingsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ApplyWindowsSettingsCheckbox.Location = new System.Drawing.Point(432, 322);
            this.ApplyWindowsSettingsCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ApplyWindowsSettingsCheckbox.Name = "ApplyWindowsSettingsCheckbox";
            this.ApplyWindowsSettingsCheckbox.Size = new System.Drawing.Size(180, 21);
            this.ApplyWindowsSettingsCheckbox.TabIndex = 61;
            this.ApplyWindowsSettingsCheckbox.Text = "Apply Windows Settings";
            this.SPT_Tooltips.SetToolTip(this.ApplyWindowsSettingsCheckbox, "Windows settings & services\r\nUser accounts groups and GPO");
            this.ApplyWindowsSettingsCheckbox.UseVisualStyleBackColor = true;
            // 
            // CreateFoldersAndRegistryCheckbox
            // 
            this.CreateFoldersAndRegistryCheckbox.AutoSize = true;
            this.CreateFoldersAndRegistryCheckbox.Checked = true;
            this.CreateFoldersAndRegistryCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateFoldersAndRegistryCheckbox.Location = new System.Drawing.Point(432, 346);
            this.CreateFoldersAndRegistryCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.CreateFoldersAndRegistryCheckbox.Name = "CreateFoldersAndRegistryCheckbox";
            this.CreateFoldersAndRegistryCheckbox.Size = new System.Drawing.Size(207, 21);
            this.CreateFoldersAndRegistryCheckbox.TabIndex = 62;
            this.CreateFoldersAndRegistryCheckbox.Text = "Create Folders and Registry";
            this.SPT_Tooltips.SetToolTip(this.CreateFoldersAndRegistryCheckbox, "Creates required folders and registry keys \r\nand assigns them required permission" +
        "s");
            this.CreateFoldersAndRegistryCheckbox.UseVisualStyleBackColor = true;
            // 
            // ConfigureSQLAccountsCheckbox
            // 
            this.ConfigureSQLAccountsCheckbox.AutoSize = true;
            this.ConfigureSQLAccountsCheckbox.Checked = true;
            this.ConfigureSQLAccountsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ConfigureSQLAccountsCheckbox.Location = new System.Drawing.Point(432, 369);
            this.ConfigureSQLAccountsCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ConfigureSQLAccountsCheckbox.Name = "ConfigureSQLAccountsCheckbox";
            this.ConfigureSQLAccountsCheckbox.Size = new System.Drawing.Size(200, 21);
            this.ConfigureSQLAccountsCheckbox.TabIndex = 63;
            this.ConfigureSQLAccountsCheckbox.Text = "Configure Accounts in SQL";
            this.SPT_Tooltips.SetToolTip(this.ConfigureSQLAccountsCheckbox, "Creates SQL logins for MSA and DMSA\r\nGrants DMSA \'sysadmin\' right\r\nRuns DB Permis" +
        "sions Configuration Tool");
            this.ConfigureSQLAccountsCheckbox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Accounts_UseSingleAccount);
            this.groupBox1.Controls.Add(this.Accounts_LabelIMSA);
            this.groupBox1.Controls.Add(this.Accounts_LabelDMSA);
            this.groupBox1.Controls.Add(this.Accounts_LabelInstallationAccount);
            this.groupBox1.Controls.Add(this.Accounts_LabelDomain);
            this.groupBox1.Controls.Add(this.Accounts_IMSAPassword);
            this.groupBox1.Controls.Add(this.Accounts_LabelDMSAPassword);
            this.groupBox1.Controls.Add(this.Accounts_LabelSQLServicesAccount);
            this.groupBox1.Controls.Add(this.Accounts_DMSAPassword);
            this.groupBox1.Controls.Add(this.Accounts_LabelSQLServicesPassword);
            this.groupBox1.Controls.Add(this.Accounts_LabelIMSAPassword);
            this.groupBox1.Controls.Add(this.Accounts_SQLServicestAccount);
            this.groupBox1.Controls.Add(this.Accounts_IMSA);
            this.groupBox1.Controls.Add(this.Accounts_SQLServicesPassword);
            this.groupBox1.Controls.Add(this.Accounts_Domain);
            this.groupBox1.Controls.Add(this.Accounts_DMSA);
            this.groupBox1.Controls.Add(this.Accounts_InstallUser);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(9, 38);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(400, 351);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accounts Details";
            // 
            // SPT_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(776, 544);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ConfigureSQLAccountsCheckbox);
            this.Controls.Add(this.CreateFoldersAndRegistryCheckbox);
            this.Controls.Add(this.ApplyWindowsSettingsCheckbox);
            this.Controls.Add(this.AllActionsButton);
            this.Controls.Add(this.SQLConnection_Groupbox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.LogBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "SPT_Form";
            this.Text = "WFO Server Preparation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SPT_Form_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.SQLConnection_Groupbox.ResumeLayout(false);
            this.SQLConnection_Groupbox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_Load;
        private System.Windows.Forms.ToolStripMenuItem Menu_Reset;
        private System.Windows.Forms.ToolStripMenuItem Menu_Exit;
        private System.Windows.Forms.CheckBox Accounts_UseSingleAccount;
        private System.Windows.Forms.ComboBox SQLVersionBox;
        private System.Windows.Forms.ComboBox PlatformBox;
        private System.Windows.Forms.Button FoldersButton;
        private System.Windows.Forms.Button AllActionsButton;
        private System.Windows.Forms.TextBox Accounts_IMSAPassword;
        private System.Windows.Forms.TextBox Accounts_DMSAPassword;
        private System.Windows.Forms.TextBox Accounts_SQLServicesPassword;
        private System.Windows.Forms.TextBox Accounts_Domain;
        private System.Windows.Forms.TextBox Accounts_SQLServicestAccount;
        private System.Windows.Forms.TextBox Accounts_InstallUser;
        private System.Windows.Forms.TextBox Accounts_DMSA;
        private System.Windows.Forms.TextBox Accounts_IMSA;
        private System.Windows.Forms.Label Accounts_LabelIMSAPassword;
        private System.Windows.Forms.Label Accounts_LabelDMSAPassword;
        private System.Windows.Forms.Label Accounts_LabelSQLServicesPassword;
        private System.Windows.Forms.Label Accounts_LabelDomain;
        private System.Windows.Forms.Label Accounts_LabelSQLServicesAccount;
        private System.Windows.Forms.Label Accounts_LabelInstallationAccount;
        private System.Windows.Forms.Label Accounts_LabelDMSA;
        private System.Windows.Forms.Label Accounts_LabelIMSA;
        private System.Windows.Forms.GroupBox SQLConnection_Groupbox;
        private System.Windows.Forms.TextBox SQLConnection_SQLPort;
        private System.Windows.Forms.Label SQLConnection_LabelSQLServerName;
        private System.Windows.Forms.Label SQLConnection__LabelSQLServerPort;
        private System.Windows.Forms.TextBox SQLConnection_SQLServer;
        private System.Windows.Forms.CheckBox ApplyWindowsSettingsCheckbox;
        private System.Windows.Forms.CheckBox CreateFoldersAndRegistryCheckbox;
        private System.Windows.Forms.CheckBox ConfigureSQLAccountsCheckbox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolTip SPT_Tooltips;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label SQLServerVersionLabel;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firewallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restrictSSLToolStripMenuItem;

    }
}

