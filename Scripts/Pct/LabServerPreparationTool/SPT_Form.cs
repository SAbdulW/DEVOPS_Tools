using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Data.SqlClient;
using System.Threading;
using FirewallSettings;


namespace LabServerPreparationTool
{
    public partial class SPT_Form : Form
    {
        private I360Platform _i360platform = null;
        public event EventHandler OnCompletedSPT;
        public static bool formValuesChanged = false;


        public SPT_Form()
        {

            GeneralUtilities.DeleteAllLogs();
            Logging.logToDebugLog("Initalizing Server Preparation Tool");
            InitializeComponent();
            OnCompletedSPT += SPT_Form_OnCompletedTask;
            GeneralUtilities.checkToolLocation();
            GeneralUtilities.writeVersionToRegistry("Server Preparation Tool");
            initializeDefaults();
            if (GeneralUtilities.readConfigProp("LabMode") == "true")
                Logging.logToUI("Server Preparation Tool is wokring in Lab Mode", "project", LogBox);
            if (GeneralUtilities.readConfigProp("AutoLoadSavedProject") == "true")
                Menu_Load_Click(null, null);
            enableDisableFormItems();
            runSilentMode();
            this.Text += " (" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
        }


        public I360Platform I360Platform
        {
            get
            {
                if (_i360platform == null)
                {
                    _i360platform = new I360Platform(PlatformBox.Text, SQLVersionBox.Text);
                }
                return _i360platform;
            }
        }


        # region Server Preparation

        // =====================================================
        //                   SERVER PREPARATION
        // =====================================================


        private void FoldersButton_Click_1(object sender, EventArgs e)
        {
            if (PlatformBox.Text == "")
                MessageBox.Show("Please select server type");
            else if (I360Platform.platformHasSQL & SQLVersionBox.Text == "")
                MessageBox.Show("Please select SQL version, selected server contains SQL");
            else
            {
                Logging.logToDebugLog("Opening System Folders form");
                using (FoldersForm folderForm = new FoldersForm(I360Platform))
                {
                    folderForm.ShowDialog();
                }
            }
        }



        private void ApplyWindowsSettings()
        {
            if (ApplyWindowsSettingsCheckbox.Checked)
            {
                UserAccount installUser = new UserAccount("InstallUser", Accounts_Domain.Text, Accounts_InstallUser.Text);
                UserAccount IMSA = new UserAccount("IMSA", Accounts_Domain.Text, Accounts_IMSA.Text);
                UserAccount DMSA = new UserAccount("DMSA", Accounts_Domain.Text, Accounts_DMSA.Text);
                UserAccount SQLServicesUser = new UserAccount("SQLServices", Accounts_Domain.Text, Accounts_SQLServicestAccount.Text);
                UserAccount iisUser = new UserAccount("IIS_IUSRS", "", "IIS_IUSRS");

                I360Platform i360platform = I360Platform;

                Logging.logToUI("Applying Windows Settings...", "action", LogBox);

                /// Change GPO according to OS hardening guidelines
                if (GeneralUtilities.readConfigProp("SetWindowsGPO") == "true")
                    I360Platform.applyWindowsSettings(LogBox);
                else
                {
                    Logging.logToUI("SetWindowsGPO property is not set to true, skipping action to configure Windows GPO", "info", LogBox);
                    Logging.logToDebugLog("SetWindowsGPO property is not set to true, skipping action to configure Windows GPO");
                }


                if (GeneralUtilities.readConfigProp("SetAccountsInWindows") == "true")
                {

                    List<UserAccount> permissionsUserAccounts = new List<UserAccount>();
                    if (i360platform.platformType != "RemoteSQL")
                    {
                        permissionsUserAccounts.Add(IMSA);
                        permissionsUserAccounts.Add(installUser);
                    }
                    if (i360platform.platformHasSQL | i360platform.platformName == "Database Management Server")
                        permissionsUserAccounts.Add(DMSA);

                    if (i360platform.platformHasSQL == true)
                        permissionsUserAccounts.Add(SQLServicesUser);

                    if (i360platform.platformName.Contains("Application") | i360platform.platformName == "Consolidated Server" |
                        i360platform.platformName == "Data Center Server" | i360platform.platformName.Contains("Single Box"))
                        permissionsUserAccounts.Add(iisUser);

                    // Adding install user to local admin group
                    i360platform.addInstallUserToLocalAdmins(installUser, LogBox);

                    // Adding IMSA to Backup Operators group
                    i360platform.addIMSAToBackupOperators(IMSA, LogBox);

                    // Setting user rights (GPO)
                    i360platform.setUserRightsInGpo(permissionsUserAccounts, LogBox);
                }
                else
                {
                    Logging.logToUI("SetAccountsInWindows property is not set to true, skipping action to configure accounts in Windows", "info", LogBox);
                    Logging.logToDebugLog("SetAccountsInWindows property is not set to true, skipping action to configure accounts in Windows");
                }

            }
            else
            {
                Logging.logToUI("Skipping Windows Setting Configuration, checkbox is disabled", "action", LogBox);
                Logging.logToDebugLog("Skipping Windows Setting Configuration, checkbox is disabled");
            }
        }




        private void ApplyFoldersRegistry()
        {
            if (CreateFoldersAndRegistryCheckbox.Checked)
            {
                Logging.logToUI("Creating Folders and Registry keys...", "action", LogBox);

                UserAccount installUser = new UserAccount("InstallUser", Accounts_Domain.Text, Accounts_InstallUser.Text);
                UserAccount IMSA = new UserAccount("IMSA", Accounts_Domain.Text, Accounts_IMSA.Text);
                UserAccount DMSA = new UserAccount("DMSA", Accounts_Domain.Text, Accounts_DMSA.Text);
                UserAccount SQLServicesUser = new UserAccount("SQLServices", Accounts_Domain.Text, Accounts_SQLServicestAccount.Text);
                UserAccount iisUser = new UserAccount("IUSR", "", "IUSR");

                I360Platform i360platform = I360Platform;


                // Building User Accounts list for NTFS and registry rights

                List<UserAccount> permissionsUserAccounts = new List<UserAccount>();
                if (i360platform.platformType != "RemoteSQL")
                {
                    permissionsUserAccounts.Add(IMSA);
                    permissionsUserAccounts.Add(installUser);
                }
                if (i360platform.platformHasSQL | i360platform.platformName == "Database Management Server")
                    permissionsUserAccounts.Add(DMSA);

                if (i360platform.platformHasSQL == true)
                    permissionsUserAccounts.Add(SQLServicesUser);

                if (i360platform.platformName.Contains("Application") | i360platform.platformName == "Consolidated Server" |
                    i360platform.platformName == "Data Center Server" |
                    i360platform.platformName.Contains("Single Box"))
                    permissionsUserAccounts.Add(iisUser);

                // Adding required NTFS permissions 
                if (GeneralUtilities.readConfigProp("SetNTFSRights") == "true")
                    i360platform.setNTFSRights(permissionsUserAccounts, LogBox);
                else
                {
                    Logging.logToUI("SetNTFSRights property is not set to true, skipping action to create system folders", "info", LogBox);
                    Logging.logToDebugLog("SetNTFSRights property is not set to true, skipping action to create system folders");
                }


                // Adding required Registry permissions
                if (GeneralUtilities.readConfigProp("SetRegistryRights") == "true")
                    i360platform.setRegistryRights(permissionsUserAccounts, LogBox);
                else
                {
                    Logging.logToUI("SetRegistryRights property is not set to true, skipping action to create system registry keys", "info", LogBox);
                    Logging.logToDebugLog("SetRegistryRights property is not set to true, skipping action to create system registry keys");
                }

                // Done
                Logging.logToDebugLog("Completed folders and registry creation action");
            }
            else
            {
                Logging.logToUI("Skipping folders and registry creation, checkbox is disabled", "action", LogBox);
                Logging.logToDebugLog("Skipping folders and registry creation, checkbox is disabled");
            }
        }



        private void ApplySQLSettings()
        {
            if (ConfigureSQLAccountsCheckbox.Checked)
            {
                Logging.logToUI("Configuring accounts in SQL...", "action", LogBox);

                UserAccount IMSA = new UserAccount("IMSA", Accounts_Domain.Text, Accounts_IMSA.Text, Accounts_IMSAPassword.Text);
                UserAccount DMSA = new UserAccount("DMSA", Accounts_Domain.Text, Accounts_DMSA.Text, Accounts_DMSAPassword.Text);
                UserAccount SQLServicesUser = new UserAccount("SQLServices", Accounts_Domain.Text, Accounts_SQLServicestAccount.Text, Accounts_SQLServicesPassword.Text);

                I360Platform.setAccountsInSQL(IMSA, DMSA, SQLServicesUser, SQLConnection_SQLServer.Text, SQLConnection_SQLPort.Text, LogBox);


                Logging.logToDebugLog("Completed setting user accounts in SQL");
            }
            else
            {
                Logging.logToUI("Skipping SQL accounts configuration, checkbox is disabled", "action", LogBox);
                Logging.logToDebugLog("Skipping SQL accounts configuration, checkbox is disabled");
            }
        }


        private void AllActionsButton_Click(object sender, EventArgs e)
        {
            _i360platform = null;


            bool UIValidatedSuccessfully = validateUIInputs(I360Platform);

            if (UIValidatedSuccessfully)
            {
                GeneralUtilities.writeInputsToRegistry(Accounts_Domain.Text, Accounts_InstallUser.Text, Accounts_IMSA.Text, Accounts_DMSA.Text,
                    Accounts_SQLServicestAccount.Text, PlatformBox.Text, SQLConnection_SQLServer.Text, SQLConnection_SQLPort.Text);
                GeneralUtilities.DeleteAllLogs();

                Logging.logToUI("Starting Server Preparation...", "project", LogBox);
                Logging.logToDebugLog("Starting server preparation...");

                DialogResult dialogResult = DialogResult.Yes;
                if ((GeneralUtilities.readConfigProp("EnableSilentMode")) != "true" & CreateFoldersAndRegistryCheckbox.Checked)
                {
                    dialogResult = MessageBox.Show("Verify that all system folders locations are configured properly before performing this action. Continue?", "Attention", MessageBoxButtons.YesNo);
                }
                if (dialogResult == DialogResult.No)
                {
                    // Not doing anything, need to verify folders first
                }
                else
                {
                    AllActionsButton.Enabled = false;
                    progressBar1.Visible = true;
                    object args = new object[3] { sender, e, I360Platform };

                    Thread t = new Thread(new ParameterizedThreadStart(DoSPTWork));
                    t.Start(args);
                    if (GeneralUtilities.readConfigProp("EnableSilentMode") == "true")
                    {
                        t.Join();
                    }

                }
            }
        }


        private void DoSPTWork(object args)
        {
            Array argArray = new object[3];
            argArray = (Array)args;
            object sender = argArray.GetValue(0);
            EventArgs e = argArray.GetValue(1) as EventArgs;
            I360Platform i360platform = argArray.GetValue(2) as I360Platform;

            if (i360platform.platformHasSQL && ConfigureSQLAccountsCheckbox.Checked)
            {
                Logging.logToUI("Validating the logged on user is a member of SQL sysadmin role", "action", LogBox);
                bool isUserSysadmin;
                isUserSysadmin = validateSQLRights(SQLConnection_SQLServer.Text, SQLConnection_SQLPort.Text);
                if (!isUserSysadmin)
                {
                    Logging.logToUI("In order to configure a platform with SQL Server, run the tool as a user that is both a local administrator and has SQL sysadmin role.", "error", LogBox);
                }
                else
                {
                    ApplyWindowsSettings();
                    ApplyFoldersRegistry();
                    ApplySQLSettings();
                }
            }
            else
            {
                ApplyWindowsSettings();
                ApplyFoldersRegistry();
            }

            if (OnCompletedSPT != null)
            {
                OnCompletedSPT(null, null);
            }
        }

        private void SPTReady()
        {
            if (InvokeRequired)
            {
                Invoke(new ThreadStart(SPTReady));
            }
            else
            {
                progressBar1.Visible = false;
                AllActionsButton.Enabled = true;

                Logging.logToUI("Completed server preparation!", "project", LogBox);

                //search for errors in the files
                List<string> problematic_files = GeneralUtilities.SearchForPatternInLogFiles(new string[] { "error", "fail", "exception" });
                foreach (string file in problematic_files)
                {
                    Logging.logToUI(string.Format("Log File '{0}' contains error(s). Please check it.", Path.GetFullPath(file)), "error", LogBox);
                }
            }
        }

        void SPT_Form_OnCompletedTask(object sender, EventArgs e)
        {
            SPTReady();

            if (GeneralUtilities.readConfigProp("EnableSilentMode") != "true")
            {
                MessageBox.Show("Completed Server Preparation");
            }

            Logging.logToDebugLog("Completed server preparation");

        }

        // =====================================================
        //                END SERVER PREPARATION TAB
        // =====================================================

        #endregion


        #region Menu Items

        // =====================================================
        //                MENU ITEMS
        // =====================================================

        private void Menu_Save_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Saving project settings");
            try
            {
                Logging.logToDebugLog("Encrypting passwords");
                AESEncryption encryptionInstance = new AESEncryption();
                string IMSAPassEncryption = encryptionInstance.Encrypt(Accounts_IMSAPassword.Text);
                string DMSAPassEncryption = encryptionInstance.Encrypt(Accounts_DMSAPassword.Text);
                string SQLServicePassEncryption = encryptionInstance.Encrypt(Accounts_SQLServicesPassword.Text);

                Logging.logToDebugLog("Writing to XML");
                using (XmlWriter writer = XmlWriter.Create("SavedProject.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("LabServerPreparationTool");
                    writer.WriteAttributeString("Domain", Accounts_Domain.Text);
                    writer.WriteAttributeString("InstallUser", Accounts_InstallUser.Text);
                    writer.WriteAttributeString("IMSA", Accounts_IMSA.Text);
                    writer.WriteAttributeString("IMSAPassword", IMSAPassEncryption);
                    writer.WriteAttributeString("DMSA", Accounts_DMSA.Text);
                    writer.WriteAttributeString("DMSAPassword", DMSAPassEncryption);
                    writer.WriteAttributeString("SQLServicesAccount", Accounts_SQLServicestAccount.Text);
                    writer.WriteAttributeString("SQLServicesPassword", SQLServicePassEncryption);
                    writer.WriteAttributeString("SQLServer", SQLConnection_SQLServer.Text);
                    writer.WriteAttributeString("SQLPort", SQLConnection_SQLPort.Text);
                    writer.WriteAttributeString("PlatformName", PlatformBox.Text);
                    writer.WriteAttributeString("SQLVersion", SQLVersionBox.Text);
                    writer.WriteAttributeString("UseSingleAccount", (Accounts_UseSingleAccount.Checked == true) ? "true" : "false");
                    writer.WriteAttributeString("DefaultSavedProject", "false");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    Logging.logToUI("Project was saved", "action", LogBox);
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Can\'t save project. Check the logs for info.", "error", LogBox);
                Logging.logToFile(exc);
            }
            formValuesChanged = false;
        }



        private void Menu_Load_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Loading project settings from XML");
            try
            {
                AESEncryption decryptionInstance = new AESEncryption();
                using (XmlReader reader = XmlReader.Create("SavedProject.xml"))
                {

                    reader.Read();
                    reader.ReadToFollowing("LabServerPreparationTool");
                    Accounts_Domain.Text = reader[0]; // Domain
                    Accounts_InstallUser.Text = reader[1]; // InstallUser
                    Accounts_IMSA.Text = reader[2]; // IMSA
                    Accounts_IMSAPassword.Text = decryptionInstance.Decrypt(reader[3]); // IMSA password
                    Accounts_DMSA.Text = reader[4]; // DMSA
                    Accounts_DMSAPassword.Text = decryptionInstance.Decrypt(reader[5]); // DMSA password
                    Accounts_SQLServicestAccount.Text = reader[6]; // SQL services account
                    Accounts_SQLServicesPassword.Text = decryptionInstance.Decrypt(reader[7]); // SQL services password
                    SQLConnection_SQLServer.Text = reader[8]; // SQL server name
                    SQLConnection_SQLPort.Text = reader[9]; // SQL server port
                    PlatformBox.Text = reader[10]; // Platform
                    SQLVersionBox.Text = reader[11]; // SQL Version
                    if (reader[12] == "true") // Use signle account definition
                        Accounts_UseSingleAccount.Checked = true;
                    else
                        Accounts_UseSingleAccount.Checked = false;
                    if (reader[13] == "true")
                    {
                        Logging.logToUI("No saved project, starting a new project", "action", LogBox);
                        initializeDefaults();
                    }
                    else
                        Logging.logToUI("Saved project was loaded", "action", LogBox);
                    reader.MoveToElement();
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Can\'t load project. Check the logs for info.", "error", LogBox);
                Logging.logToFile(exc);
            }
            formValuesChanged = false;
        }



        private void Menu_Reset_Click(object sender, EventArgs e)
        {
            LogBox.Text = "";
            Logging.logToDebugLog("New project, settings to defaults");
            Accounts_UseSingleAccount.Checked = false;
            Accounts_Domain.Text = "";
            Accounts_InstallUser.Text = "";
            Accounts_IMSA.Text = "";
            Accounts_IMSAPassword.Text = "";
            Accounts_DMSA.Text = "";
            Accounts_DMSAPassword.Text = "";
            Accounts_SQLServicestAccount.Text = "";
            Accounts_SQLServicesPassword.Text = "";
            SQLConnection_SQLServer.Text = "";
            SQLConnection_SQLPort.Text = "";
            PlatformBox.Text = "";
            SQLVersionBox.Text = "";
            initializeDefaults();
            Logging.logToUI("New project, loading default settings", "action", LogBox);
            formValuesChanged = false;
        }

        private void Menu_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Opening About SPT...");
            using (AboutForm aboutForm = new AboutForm(this.Text))
            {
                aboutForm.ShowDialog();
            }
        }


        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Enabling Firewall for server: " + PlatformBox.Text);
            Logging.logToUI("Enabling Firewall for server: " + PlatformBox.Text, "action", LogBox);
            FirewallSettings.FirewallSettings.enableFirewall(PlatformBox.Text, false);
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Disabling Firewall for server: " + PlatformBox.Text);
            Logging.logToUI("Disabling Firewall for server: " + PlatformBox.Text, "action", LogBox);
            FirewallSettings.FirewallSettings.disableFirewall(PlatformBox.Text, false);
        }


        private void restrictSSLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Blocking non SSL ports");
            Logging.logToUI("Blocking non SSL ports", "action", LogBox);
            FirewallSettings.FirewallSettings.disableNonSSLPorts();
        }



        // =====================================================
        //               END MENU ITEMS
        // =====================================================


        #endregion

        #region UI Events

        // =====================================================
        //                  UI EVENTS
        // =====================================================


        private void LogBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
                LogBox.Text = "";
        }


        private void Accounts_UseSingleAccount_CheckedChanged_1(object sender, EventArgs e)
        {
            formValuesChanged = true;

            if (Accounts_UseSingleAccount.Checked == true)
            {
                Accounts_InstallUser.ReadOnly = true;
                Accounts_DMSA.ReadOnly = true;
                Accounts_DMSAPassword.ReadOnly = true;
                Accounts_SQLServicestAccount.ReadOnly = true;
                Accounts_SQLServicesPassword.ReadOnly = true;
                Accounts_IMSA_Leave(sender, e);
                Accounts_IMSAPassword_Leave_1(sender, e);
            }
            else
            {
                Accounts_InstallUser.ReadOnly = false;
                Accounts_DMSA.ReadOnly = false;
                Accounts_DMSAPassword.ReadOnly = false;
                Accounts_SQLServicestAccount.ReadOnly = false;
                Accounts_SQLServicesPassword.ReadOnly = false;
                Accounts_InstallUser.Text = "";
                Accounts_DMSA.Text = "";
                Accounts_DMSAPassword.Text = "";
                Accounts_SQLServicestAccount.Text = "";
                Accounts_SQLServicesPassword.Text = "";

            }
        }



        private void Accounts_InstallUsertextBox_Leave(object sender, EventArgs e)
        {
            if (Accounts_InstallUser.Text.Contains("\\"))
            {
                MessageBox.Show("User name should be in short format. Domain is already specified in the Domain field.");
                this.ActiveControl = Accounts_InstallUser;
            }
        }


        private void Accounts_IMSA_Leave(object sender, EventArgs e)
        {
            if (Accounts_IMSA.Text.Contains("\\"))
            {
                MessageBox.Show("User name should be in short format. Domain is already specified in the Domain field.");
                this.ActiveControl = Accounts_IMSA;
            }
            if (Accounts_UseSingleAccount.Checked == true)
            {
                Accounts_InstallUser.Text = Accounts_IMSA.Text;
                Accounts_DMSA.Text = Accounts_IMSA.Text;
                Accounts_SQLServicestAccount.Text = Accounts_IMSA.Text;
            }
        }


        private void Accounts_IMSAPassword_Leave_1(object sender, EventArgs e)
        {
            if (Accounts_UseSingleAccount.Checked == true)
            {
                Accounts_DMSAPassword.Text = Accounts_IMSAPassword.Text;
                Accounts_SQLServicesPassword.Text = Accounts_IMSAPassword.Text;
            }
        }


        private void Accounts_DMSAtextBox_Leave(object sender, EventArgs e)
        {
            if (Accounts_DMSA.Text.Contains("\\"))
            {
                MessageBox.Show("User name should be in short format. Domain is already specified in the Domain field.");
                this.ActiveControl = Accounts_DMSA;
            }
        }

        private void Accounts_SQLServicestAccount_Leave(object sender, EventArgs e)
        {
            if (Accounts_SQLServicestAccount.Text.Contains("\\"))
            {
                MessageBox.Show("User name should be in short format. Domain is already specified in the Domain field.");
                this.ActiveControl = Accounts_SQLServicestAccount;
            }
        }


        private void SQLConnection_SQLServer_Leave(object sender, EventArgs e)
        {
            if (SQLConnection_SQLServer.Text.Contains("\\"))
            {
                MessageBox.Show("SQL Server Name should only include the host name of the SQL Server, without the instance name.");
                this.ActiveControl = SQLConnection_SQLServer;
            }
        }






        private void PlatformBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            formValuesChanged = true;

            _i360platform = new I360Platform(PlatformBox.Text, SQLVersionBox.Text);
            if (_i360platform.platformHasSQL)
            {
                SQLVersionBox.Enabled = true;
                SQLConnection_SQLServer.Enabled = true;
                SQLConnection_SQLPort.Enabled = true;
                if (!Accounts_UseSingleAccount.Checked)
                {
                    Accounts_SQLServicestAccount.Enabled = true;
                    Accounts_SQLServicesPassword.Enabled = true;
                }
            }
            else
            {
                SQLVersionBox.Text = "";
                SQLVersionBox.Enabled = false;
                SQLConnection_SQLServer.Enabled = false;
                SQLConnection_SQLPort.Enabled = false;
                Accounts_SQLServicestAccount.Enabled = false;
                Accounts_SQLServicesPassword.Enabled = false;
            }
        }

        private void SQLVersionBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            formValuesChanged = true;
            _i360platform = null;
        }



        private bool validateUIInputs(I360Platform i360platform)
        {
            Logging.logToDebugLog("Validating UI inputs");
            bool UIValidatedSuccessfully = false;
            if (PlatformBox.Text == "")
                MessageBox.Show("Please select server type");
            else if (i360platform.platformHasSQL & SQLVersionBox.Text == "")
                MessageBox.Show("Please select SQL version, selected server contains SQL");
            else if (i360platform.platformType == "RemoteSQL" & (Accounts_Domain.Text == ""
                | Accounts_IMSA.Text == "" | Accounts_IMSAPassword.Text == ""
                | Accounts_DMSA.Text == "" | Accounts_DMSAPassword.Text == ""
                | Accounts_SQLServicestAccount.Text == "" | Accounts_SQLServicesPassword.Text == ""
                | SQLConnection_SQLServer.Text == "" | SQLConnection_SQLPort.Text == ""))
                MessageBox.Show("Missing account or SQL details for Remote SQL Server");
            else if (i360platform.platformType == "DCServer" & !i360platform.platformHasSQL & (Accounts_Domain.Text == ""
                | Accounts_InstallUser.Text == "" | Accounts_IMSA.Text == ""
                | Accounts_IMSAPassword.Text == "" | Accounts_DMSA.Text == "" | Accounts_DMSAPassword.Text == ""))
                MessageBox.Show("Missing account details for a server that belongs to the data center");
            else if (i360platform.platformType == "SiteServer" & (Accounts_Domain.Text == ""
            | Accounts_InstallUser.Text == "" | Accounts_IMSA.Text == ""
            | Accounts_IMSAPassword.Text == ""))
                MessageBox.Show("Missing account details for a site server");
            else if (i360platform.platformType == "DCServer" & i360platform.platformHasSQL & (Accounts_Domain.Text == ""
                | Accounts_InstallUser.Text == "" | Accounts_IMSA.Text == ""
                | Accounts_IMSAPassword.Text == "" | Accounts_DMSA.Text == "" | Accounts_DMSAPassword.Text == ""
                | Accounts_SQLServicestAccount.Text == "" | Accounts_SQLServicesPassword.Text == ""
                | SQLConnection_SQLServer.Text == "" | SQLConnection_SQLPort.Text == ""))
                MessageBox.Show("Missing account or SQL details for a server hosting SQL Server");
            else
                UIValidatedSuccessfully = true;

            Logging.logToDebugLog("UI validated successfully results: " + UIValidatedSuccessfully.ToString());
            return UIValidatedSuccessfully;
        }

        private void Accounts_Domain_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_InstallUser_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_IMSA_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_IMSAPassword_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_DMSA_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_DMSAPassword_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_SQLServicestAccount_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void Accounts_SQLServicesPassword_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void SQLConnection_SQLServer_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }

        private void SQLConnection_SQLPort_TextChanged(object sender, EventArgs e)
        {
            formValuesChanged = true;
        }


        // =====================================================
        //                   END UI EVENTS
        // =====================================================

        #endregion

        #region Methods

        // =====================================================
        //                      METHODS
        // =====================================================


        /// <summary>
        /// Initializes the defaults in the form
        /// </summary>
        private void initializeDefaults()
        {
            Logging.logToDebugLog("Initializing form defaults");
            try
            {
                if (Environment.UserDomainName != Environment.MachineName)
                {
                    Accounts_Domain.Text = Environment.UserDomainName;
                }
            }
            catch
            {
                // coudlnt get domain name, leave empty
            }
            SQLConnection_SQLServer.Text = "localhost";
            SQLConnection_SQLPort.Text = "1433";

        }


        /// <summary>
        /// Enables / Disables specific tabs and buttons in the form
        /// </summary>
        public void enableDisableFormItems()
        {
            Logging.logToDebugLog("Setting form items visibility");
            if (GeneralUtilities.readConfigProp("EnableIndividualActions") == "false")
            {
                ApplyWindowsSettingsCheckbox.Checked = true;
                CreateFoldersAndRegistryCheckbox.Checked = true;
                ConfigureSQLAccountsCheckbox.Checked = true;

                ApplyWindowsSettingsCheckbox.Visible = false;
                CreateFoldersAndRegistryCheckbox.Visible = false;
                ConfigureSQLAccountsCheckbox.Visible = false;
            }


            if (GeneralUtilities.readConfigProp("HidePasswords") == "false")
            {
                Accounts_IMSAPassword.UseSystemPasswordChar = false;
                Accounts_DMSAPassword.UseSystemPasswordChar = false;
                Accounts_SQLServicesPassword.UseSystemPasswordChar = false;
            }

            if (GeneralUtilities.readConfigProp("LabMode") != "true")
            {
                advancedToolStripMenuItem.Visible = false;
            }


        }


        /// <summary>
        /// Runs the tool in silent mode if the relevant property is set to true.
        /// Else, does nothing and the program continues in standard mode.
        /// </summary>
        private void runSilentMode()
        {
            if (GeneralUtilities.readConfigProp("EnableSilentMode") == "true")
            {
                Logging.logToDebugLog("Server Preparation Tool is running in silent mode");
                object sender = new object();
                EventArgs e = new EventArgs();
                Menu_Load_Click(sender, e);
                AllActionsButton_Click(sender, e);

                Logging.logToDebugLog("Updating actions in Silent_log.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\Silent_log.txt", true))
                {
                    file.WriteLine(System.Environment.NewLine + DateTime.Now.ToString("HH:mm:ss tt") + System.Environment.NewLine);
                    foreach (string logline in LogBox.Lines)
                    {
                        file.WriteLine(logline + System.Environment.NewLine);
                    }
                    file.Close();
                }

                System.Environment.Exit(0);
            }
            else
            {
                Logging.logToDebugLog("Silent mode is turned Off");
            }
        }

        private void SPT_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = DialogResult.Yes;
            if (formValuesChanged)
            {
                dialogResult = MessageBox.Show("Project values were changed but not saved. Exit without saving?", "Attention", MessageBoxButtons.YesNo);
            }
            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // Exiting without saving
            }
        }


        private bool validateSQLRights(string SQLServer, string SQLPort)
        {
            Logging.logToDebugLog("Validating if logged in user is SQL sysadmin");

            bool isUserSysadmin = false;
            string sqlConnectionString =
                  "server=" + SQLServer + "," + SQLPort +
                  ";Trusted_Connection=true" +
                  ";Integrated Security=SSPI" +
                  ";database=master " +
                  ";connection timeout=30";

            Logging.logToDebugLog("SQL connection string: " + sqlConnectionString);

            using (SqlConnection connectionToSQL = new SqlConnection(sqlConnectionString))
            {
                SqlCommand commandToSQL = new SqlCommand("use master select @@version", connectionToSQL);

                try
                {
                    // Dummy test connection.
                    // Done to prevent transport level error when running the tool twice with SQL restart in the middle.
                    Logging.logToDebugLog("Performing SQL connection check");
                    connectionToSQL.Open();
                    commandToSQL.ExecuteNonQuery();
                }
                catch
                {
                    // do nothing
                }

                try
                {
                    commandToSQL = new SqlCommand("SELECT IS_SRVROLEMEMBER(\'sysadmin\') AS IsSysadmin;", connectionToSQL);
                    using (SqlDataReader SQLreader = commandToSQL.ExecuteReader())
                    {
                        Logging.logToDebugLog("Getting IS_SRVROLEMEMBER(\'sysadmin\') results");
                        string resultsFromSQL = "0";
                        while (SQLreader.Read())
                        {
                            resultsFromSQL = SQLreader["IsSysadmin"].ToString();
                            Logging.logToDebugLog("Results returned from SQL Server: " + resultsFromSQL);
                        }
                        if (resultsFromSQL == "1")
                            isUserSysadmin = true;
                    }
                    connectionToSQL.Close();

                }
                catch (Exception exc)
                {
                    Logging.logToDebugLog("Failed reading \'sysadmin\' membership from SQL Server");
                    Logging.logToFile(exc);
                }
            }

            if (isUserSysadmin)
                Logging.logToDebugLog("Logged in user is a member of SQL sysadmin role");
            else
                Logging.logToDebugLog("Logged in user is not a member of SQL sysadmin role");

            return isUserSysadmin;

        }




        // =====================================================
        //                      END METHODS
        // =====================================================

        #endregion

    }

}
