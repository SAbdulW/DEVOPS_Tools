using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.Data.SqlClient;
using System.Linq;

namespace LabServerPreparationTool
{
    /// <summary>
    /// I360 Platform
    /// </summary>
    public class I360Platform
    {
        /// <summary>
        /// Create an instance of system Server
        /// </summary>
        /// <param name="platformName"></param>
        /// <param name="SQLversion"></param>
        public I360Platform(string platformName, string SQLversion)
        {
            Logging.logToDebugLog("Initializing Server " + platformName);
            _platformName = platformName;
            _SQLversion = SQLversion;
            updatePlatformType();
        }


        private string _platformName;
        private string _platformType;
        private string _SQLversion;
        private bool _platformHasSQL;



        // =====================================================
        //                   PROPERTIES
        // =====================================================


        public string platformName
        {
            get { return _platformName; }
            set { _platformName = value; }
        }

        public string SQLversion
        {
            get { return _SQLversion; }
            set { _SQLversion = value; }
        }

        public string platformType
        {
            get { return _platformType; }
            set { _platformType = value; }
        }

        public bool platformHasSQL
        {
            get { return _platformHasSQL; }
            set { _platformHasSQL = value; }
        }




        // =====================================================
        //                   METHODS
        // =====================================================


        /// <summary>
        /// Sets the Server type according to the selected Server
        /// Available types: DCServer, RemoteSQL, SiteServer
        /// Also sets value true / false for platformHasSQL property.
        /// </summary>
        public void updatePlatformType()
        {
            if (_platformName.Contains("Database") | _platformName.Contains("Consolidated") | _platformName.Contains("Survey Server")
                | _platformName.Contains("Data Analytics") | _platformName.Contains("Data Center")
                | _platformName.Contains("Data Warehouse") | _platformName.Contains("Framework DB")
                | _platformName.Contains("Reporting") | _platformName == "Remote SQL Server"
                | _platformName.Contains("Single Box Solution"))
                _platformHasSQL = true;
            else
                _platformHasSQL = false;

            if (_platformName.Contains("Database Management"))
                _platformHasSQL = false;

            if (_platformName.Contains("Recorder") | _platformName.Contains("Centralized Archive")
                | _platformName.Contains("Acquisition ") | _platformName.Contains("Import Manager")
                | _platformName.Contains("Export Manager") | _platformName.Contains("IP Analyzer")
                | _platformName.Contains("Speech Analytics Transcription") | _platformName.Contains("Telephone Playback Service")
                | _platformName.Contains("Voice Biometrics") | _platformName.Contains("Voiceprint Enrollment"))
                _platformType = "SiteServer";
            else if (_platformName == "Remote SQL Server")
                _platformType = "RemoteSQL";
            else
                _platformType = "DCServer";

        }

        /// <summary>
        /// Change GPO according to OS hardening guidelines
        /// </summary>
        public void applyWindowsSettings(RichTextBox LogBox)
        {
            // Running GPO update script
            Logging.logToUI("Updating Windows settings and services...", "info", LogBox);
            Logging.logToDebugLog("Updating Windows settings and services");
            try
            {
                Logging.logToDebugLog("Creating Logs\\WindowsSettings logs folder");
                Directory.CreateDirectory(".\\Logs\\WindowsSettings");
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed creating logs folder. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            Logging.logToDebugLog("Applying OS settings script");
            string cmdLineStr = "/c .\\Scripts\\RunWindowsSettings.cmd";
            int processExitCode = OSUtilities.launchNewCMD(cmdLineStr);
            if (processExitCode == 0)
            {
                GeneralUtilities.writeVersionToRegistry("User Rights, Windows Services and Settings");
                Logging.logToDebugLog("Completed setting Windows settings and services");
            }
            else
            {
                Logging.logToUI("Failed updating Windows settings and services. Check logs for errros.", "error", LogBox);
                Logging.logToDebugLog("Failed applying Windows settings, script exited with code " + processExitCode);
            }

        }



        /// <summary>
        /// Adding the Installation Account to the Local Administrators group
        /// </summary>
        /// <param name="installUser"></param>
        /// <param name="LogBox"></param>
        public void addInstallUserToLocalAdmins(UserAccount installUser, RichTextBox LogBox)
        {
            if (_platformType != "RemoteSQL")
            {
                string administratorsSID = "S-1-5-32-544";
                //string administratorsGroupName = OSUtilities.determineAdministratorsGroupName();
                bool userAlreadyAdmin = installUser.AddToLocalGroup(administratorsSID, LogBox);
            }
            else
            {
                Logging.logToDebugLog("Server is Remote SQL, not adding Install User to local administrators");
            }
        }

        /// <summary>
        /// Adding the IMSA to the Backup Operators group
        /// </summary>
        /// <param name="installUser"></param>
        /// <param name="LogBox"></param>
        public void addIMSAToBackupOperators(UserAccount imsaUser, RichTextBox LogBox)
        {
            if (_platformType != "RemoteSQL")
            {
                string backupOperatorsSID = "S-1-5-32-551";
                //string backupOperatorsGroupName = OSUtilities.determineBackupOperatorsGroupName();
                bool userAlreadyInGroup = imsaUser.AddToLocalGroup(backupOperatorsSID, LogBox);
            }
            else
            {
                Logging.logToDebugLog("Server is Remote SQL, not adding IMSA to Backup Operators");
            }
        }



        /// <summary>
        /// Sets the NTFS rights on the Server
        /// </summary>
        public void setNTFSRights(List<UserAccount> permissionsUserAccounts, RichTextBox LogBox)
        {

            Logging.logToUI("Creating system folders and adding required permissions", "info", LogBox);
            Logging.logToDebugLog("Adding NTFS permissions...");

            try
            {
                string[] foldersList = new string[] { };
                foldersList = GeneralUtilities.readFoldersList(LogBox);
                foreach (string folderName in foldersList)
                {
                    string folderPath = GeneralUtilities.readFolderPath(folderName, LogBox);
                    if (folderPath != "NA")
                    {
                        try
                        {
                            Logging.logToDebugLog("Creating directory " + folderPath);
                            Directory.CreateDirectory(folderPath);
                        }
                        catch (Exception exc)
                        {
                            Logging.logToUI("Couldn\'t create system folders. Check logs for errors.", "error", LogBox);
                            Logging.logToFile(exc);
                        }

                        DirectoryInfo folderDirectoryInfo = new DirectoryInfo(folderPath);

                        foreach (UserAccount userAccount in permissionsUserAccounts)
                        {
                            if (userAccount.Username == "")
                                break;
                            string requiredRights = GeneralUtilities.readFolderPermissions(folderName, userAccount.AccountType, LogBox);
                            if (requiredRights == "ReadExecute")
                            {
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.ReadAndExecute, LogBox);
                            }
                            else if (requiredRights == "ReadExecuteWrite")
                            {
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.ReadAndExecute, LogBox);
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.Write, LogBox);
                            }
                            else if (requiredRights == "ReadExecuteWriteDelete")
                            {
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.ReadAndExecute, LogBox);
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.Write, LogBox);
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.Delete, LogBox);
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.DeleteSubdirectoriesAndFiles, LogBox);
                            }
                            else if (requiredRights == "FullControl")
                            {
                                userAccount.AddFolderPermissions(folderDirectoryInfo, FileSystemRights.FullControl, LogBox);
                            }
                            else
                            {
                                Logging.logToDebugLog("No need to set rights for this account on this folder");
                                
                            }
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Couldn\'t set NTFS permissions. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }

        }


        /// <summary>
        /// Sets the Registry rights on the Server
        /// </summary>
        /// <param name="permissionsUserAccounts"></param>
        /// <param name="LogBox"></param>
        public void setRegistryRights(List<UserAccount> permissionsUserAccounts, RichTextBox LogBox)
        {

            // Adding required registry permissions, aside from on Remote SQL Server
            if (_platformType != "RemoteSQL")
            {
                Logging.logToDebugLog("Setting the Registry permissions on the Server");
                try
                {
                    Logging.logToUI("Adding Registry permissions", "info", LogBox);
                    string[] registryKeys = GeneralUtilities.readRegistryKeysList(LogBox);
                    foreach (string keyName in registryKeys)
                    {
                        string keyPath = GeneralUtilities.readRegistryKeyPath(keyName, LogBox);
                        if (keyPath != "NA")
                        {
                            try
                            {
                                Logging.logToDebugLog("Creating registry key " + keyPath);
                                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath, Microsoft.Win32.RegistryKeyPermissionCheck.Default);
                            }
                            catch (Exception exc)
                            {
                                Logging.logToUI("Couldn\'t create registry key " + keyPath + ". Check logs for errors.", "error", LogBox);
                                Logging.logToFile(exc);
                            }


                            foreach (UserAccount userAccount in permissionsUserAccounts)
                            {
                                if (userAccount.Username == "")
                                    break;
                                string requiredRights = GeneralUtilities.readRegistryKeyPermissions(keyName, userAccount.AccountType, LogBox);
                                if (requiredRights == "Read")
                                {
                                    userAccount.AddRegistryPermissions(keyPath, RegistryRights.ReadKey, LogBox);
                                }
                                else if (requiredRights == "ReadWrite")
                                {
                                    userAccount.AddRegistryPermissions(keyPath, RegistryRights.ReadKey, LogBox);
                                    userAccount.AddRegistryPermissions(keyPath, RegistryRights.WriteKey, LogBox);
                                }
                                else if (requiredRights == "FullControl")
                                {
                                    userAccount.AddRegistryPermissions(keyPath, RegistryRights.FullControl, LogBox);
                                }
                                else
                                {
                                    // no need to set rights for this account on this key
                                }
                            }

                        }
                    }

                }

                catch (Exception exc)
                {
                    Logging.logToUI("Couldn\'t set Registry permissions. Check logs for errors.", "error", LogBox);
                    Logging.logToFile(exc);
                }
            }
            else
            {
                Logging.logToDebugLog("No need to set Registry permissions on this Server");
            }
        }


     
        /// <summary>
        /// Sets the reuiqred User Rights in the GPO
        /// </summary>
        public void setUserRightsInGpo(List<UserAccount> permissionsUserAccounts, RichTextBox LogBox)
        {
            try
            {
                if (GeneralUtilities.readConfigProp("SetGPO") == "false")
                {
                    Logging.logToDebugLog("SetGPO wasn't selected , Exiting.");
                    return;
                }

                Logging.logToUI("Setting the required user rights in GPO", "info", LogBox);
                Logging.logToDebugLog("Setting the required user rights in GPO");

                Dictionary<string, List<string>> usersStructure = BuildUsersPermissionsStructure();
                foreach (string accounttype in usersStructure.Keys)
                {
                    Logging.logToDebugLog(string.Format("Set User Righs Accounts for account [{0}]", accounttype));

                    var acc = permissionsUserAccounts.Where(p => p.AccountType == accounttype).FirstOrDefault();

                    if (acc == null)
                    {
                        Logging.logToDebugLog(string.Format("Skipping setUserRightsInGpo for account not relevant to platform - [{0}] ", accounttype));
                        continue;
                    }

                    string fullAccountName = acc.FullAccountName;
                    if (fullAccountName.StartsWith("\\"))
                        fullAccountName = fullAccountName.Replace("\\", "");

            

                    foreach (string privelege in usersStructure[accounttype])
                    {
                        long result = LsaUtility.SetRight(fullAccountName, privelege);
                        if (result == 0)
                        {
                            Logging.logToDebugLog(string.Format("User [{0}] got privelege [{1}]", fullAccountName, privelege));
                        }
                        else
                        {
                            string error = string.Format("Failed to set privelege privelege [{0}] for user [{1}]. Error = [{2}]", privelege, fullAccountName, result);
                            Logging.logToUI(error, "error", LogBox);
                            Logging.logToDebugLog(error);
                        }

                    }
                }

            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed setting the required user rights. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
        }


        /// <summary>
        /// Sets the Accounts in SQL Server on the Server
        /// </summary>
        /// <param name="IMSA"></param>
        /// <param name="DMSA"></param>
        /// <param name="SQLServicesUser"></param>
        /// <param name="SQLConnectionUser"></param>
        /// <param name="SQLServer"></param>
        /// <param name="SQLPort"></param>
        /// <param name="LogBox"></param>
        public void setAccountsInSQL(UserAccount IMSA, UserAccount DMSA, UserAccount SQLServicesUser, string SQLServer, string SQLPort, RichTextBox LogBox)
        {
            bool connectUsingSA = false; // can be used in order to connect to SQL using 'sa' account. Currently not supported by DB permissions tool.

            // Creating SQL Connection

            string sqlConnectionString = (connectUsingSA) ?

                // Connecting using DB authentication with 'sa' user

                    (
                       "user id=sa" +
                       ";password=" + "SAAccountPassword" +
                       ";server=" + SQLServer + "," + SQLPort +
                       ";Trusted_Connection=false" +
                       ";database=master" +
                       ";connection timeout=30") :

                       // Connecting using Windows Authentication

                    (
                        "server=" + SQLServer + "," + SQLPort +
                        ";Trusted_Connection=true" +
                        ";Integrated Security=SSPI" +
                        ";database=master " +
                        ";connection timeout=30");

            Logging.logToDebugLog("SQL connection string: " + sqlConnectionString);

            try
            {
                using (SqlConnection connectionToSQL = new SqlConnection(sqlConnectionString))
                {

                    try
                    {
                        // Dummy test connection.
                        // Done to prevent transport level error when running the tool twice with SQL restart in the middle.
                        Logging.logToDebugLog("Performing SQL connection check");
                        connectionToSQL.Open();
                        SqlCommand commandToSQL = new SqlCommand("use master select @@version", connectionToSQL);
                        commandToSQL.ExecuteNonQuery();
                    }
                    catch
                    {
                        // do nothing
                    }
                    finally
                    {
                        connectionToSQL.Close();
                    }


                    // Add SQL Services Account, IMSA and DMSA to SQL logons

                    if (GeneralUtilities.readConfigProp("CreateSQLLogins") == "true")
                    {
                        Logging.logToUI("Adding SQL Logins", "info", LogBox);

                        try
                        {

                            IMSA.AddToSQLLogins(connectionToSQL, LogBox);
                            DMSA.AddToSQLLogins(connectionToSQL, LogBox);
                            if (GeneralUtilities.readConfigProp("UpdateSQLServicesAccount") == "true")
                                SQLServicesUser.AddToSQLLogins(connectionToSQL, LogBox);
                        }
                        catch (Exception exc)
                        {
                            Logging.logToUI("Failed adding SQL logons. Check logs for errors.", "error", LogBox);
                            Logging.logToFile(exc);
                        }

                    }
                    else
                    {
                        Logging.logToUI("CreateSQLLogins property is not set to true, skipping action to create SQL Server logins", "info", LogBox);
                        Logging.logToDebugLog("CreateSQLLogins property is not set to true, skipping step");
                    }


                    // Add to SQL Services Account and DMSA DB sysadmin


                    if (GeneralUtilities.readConfigProp("ConfigureSQLSysadmin") == "true")
                    {
                        Logging.logToDebugLog("ConfigureSQLSysadmin is set to true, adding \'sysadmin\' role to required users");

                        try
                        {
                            DMSA.AddSQLsysadminRole(connectionToSQL, LogBox);
                            if (GeneralUtilities.readConfigProp("UpdateSQLServicesAccount") == "true")
                                SQLServicesUser.AddSQLsysadminRole(connectionToSQL, LogBox);
                        }
                        catch (Exception exc)
                        {
                            Logging.logToUI("Failed adding \'sysadmin\' role. Check logs for errors.", "error", LogBox);
                            Logging.logToFile(exc);
                        }

                    }
                    else
                    {
                        Logging.logToUI("ConfigureSQLSysadmin property is not set to true, skipping action to add SQL \'sysadmin\' role to required users", "info", LogBox);
                        Logging.logToDebugLog("ConfigureSQLSysadmin property is set to false, skipping step");
                    }



                    // Run DB permissions tool

                    if ((GeneralUtilities.readConfigProp("RunDBPermissionsTool") == "true") | (_platformType == "RemoteSQL"))
                    {

                        Logging.logToUI("Running the DB Permissions Configuration Tool", "info", LogBox);
                        Logging.logToDebugLog("Running the DB Permissions Configuration Tool");

                        try
                        {
                            string cmdLineStr = "/c .\\Scripts\\RunDBPermissionsTool.cmd " + DMSA.FullAccountName + " " + DMSA.Password + " " + IMSA.FullAccountName + " " + IMSA.Password + " " + SQLServer + " " + SQLPort;
                            if (GeneralUtilities.readConfigProp("EnableSilentMode") == "true")
                            {
                                cmdLineStr = cmdLineStr + " silent";
                            }
                            int processExitCode = OSUtilities.launchNewCMD(cmdLineStr);
                            if (processExitCode == 0)
                                GeneralUtilities.writeVersionToRegistry("DB Permissions");
                            else
                            {
                                Logging.logToUI("DB Permissions Configuration Tool completed with errors. Check the logs for info.", "error", LogBox);
                                Logging.logToDebugLog("DB Permissions Configuration Tool completed with errors, process exit code is " + processExitCode.ToString());
                            }
                        }
                        catch (Exception exc)
                        {
                            Logging.logToUI("Couldn\'t run DB Permissions Configuration Tool. Check the logs for info.", "error", LogBox);
                            Logging.logToFile(exc);
                        }

                    }
                    else
                    {
                        Logging.logToUI("RunDBPermissionsTool property is not set to true, skipping action to run the DB Permissoins Tool", "info", LogBox);
                        Logging.logToDebugLog("RunDBPermissionsTool property is set to false, skipping step");
                    }


                    // Change SQL services to run under the SQL Services Account
                    // Relevant for SQL Server, SQL Agent

                    if (GeneralUtilities.readConfigProp("UpdateSQLServicesAccount") == "true")
                    {

                        Logging.logToUI("Checking logon information for SQL services", "info", LogBox);
                        Logging.logToDebugLog("Checking SQL Services logon information...");

                        bool changeServiceAccounts = false;

                        string I360SQLInstancename = SQLUtilities.getSQLInstanceName(connectionToSQL, LogBox);
                        string SQLServerServiceName = (I360SQLInstancename == "") ? "MSSQLSERVER" + I360SQLInstancename : "MSSQL$" + I360SQLInstancename;
                        string SQLAgentSServiceName = (I360SQLInstancename == "") ? "SQLSERVERAGENT" + I360SQLInstancename : "SQLAgent$" + I360SQLInstancename;

                        changeServiceAccounts = OSUtilities.checkServiceLogon(changeServiceAccounts, SQLServerServiceName, SQLServicesUser.FullAccountName, _platformName, LogBox); // Checking logon account for SQL server
                        changeServiceAccounts = OSUtilities.checkServiceLogon(changeServiceAccounts, SQLAgentSServiceName, SQLServicesUser.FullAccountName, _platformName, LogBox); // Checking logon account for SQL agent

                        string cmdLineStr2 = "/c .\\Scripts\\ChangeSQLServicesAccount.cmd " + SQLServicesUser.FullAccountName + " " + SQLServicesUser.Password + " "
                            + SQLServerServiceName + " " + SQLAgentSServiceName;

                        string command = ".\\Scripts\\ChangeSQLServicesAccount.cmd";
                        string args = SQLServicesUser.FullAccountName + " " + SQLServicesUser.Password + " " + SQLServerServiceName + " " + SQLAgentSServiceName;

                        if (changeServiceAccounts == true)
                        {
                            I360SQLInstancename = (I360SQLInstancename == "") ? "Default" : I360SQLInstancename;
                            Logging.logToUI("Changing SQL services logon to " + SQLServicesUser.FullAccountName + ", for the SQL instance: " + I360SQLInstancename, "info", LogBox);
                            Logging.logToDebugLog("Service logon modification is required, changing SQL Services logon to " + SQLServicesUser.FullAccountName + ", for the SQL instance: " + I360SQLInstancename);
                            if (GeneralUtilities.readConfigProp("EnableSilentMode") == "true")
                            {
                                cmdLineStr2 = cmdLineStr2 + " silent";
                            }
                            int processExitCode = OSUtilities.ExecuteLocalCommand(command, args);
                            if (processExitCode == 0)
                            {
                                Logging.logToDebugLog("Completed Change SQL Services Account");
                            }
                            else
                            {
                                Logging.logToUI("Failed to change SQL Services Account. Check logs for errros.", "error", LogBox);
                                Logging.logToDebugLog("Failed to Change SQL Services Account, ChangeSQLServicesAccount.cmd script exited with code " + processExitCode);
                            }
                        }
                        else
                        {
                            Logging.logToUI("No service logon changes required, didn\'t find SQL services of SQL instance: " + I360SQLInstancename + " that are not running under " + SQLServicesUser.FullAccountName + ".", "info", LogBox);
                            Logging.logToDebugLog("No service logon changes required, didn\'t find SQL services of SQL instance: " + I360SQLInstancename + " that are not running under " + SQLServicesUser.FullAccountName);
                        }
                    }
                    else
                    {
                        Logging.logToDebugLog("UpdateSQLServicesAccount property is set to false, skipping step");
                    }


                }  // Done, closing 'Using' clause for connectionToSQL
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed setting user accounts in SQL. Check logs for errors and make sure no redundant account exists in the Local Administrators group.", "error", LogBox);
                Logging.logToFile(exc);
            }
        }


        private Dictionary<string, List<string>> BuildUsersPermissionsStructure()
        {
            Dictionary<string, List<string>> accounts = new Dictionary<string, List<string>>();

            // reading windows names for privileges display names
            Logging.logToDebugLog("Reading WindowsSettings\\AccountRights\\UserRightsConstantNames.csv...");
            string UserRightsConstantNamesCSV = @"WindowsSettings\AccountRights\UserRightsConstantNames.csv";

            Logging.logToDebugLog("Reading WindowsSettings\\AccountRights\\UserRights.csv...");
            string UserRightsCSV = @"WindowsSettings\AccountRights\UserRights.csv";

            // mapping of users to SPT users
            Logging.logToDebugLog("Reading WindowsSettings\\AccountRights\\AccountsMapping.csv...");
            string AccountsMappingCSV = @"WindowsSettings\AccountRights\AccountsMapping.csv";

            var privDict = File.ReadLines(UserRightsConstantNamesCSV).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
            var accountsMapDict = File.ReadLines(AccountsMappingCSV).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
            var lines = File.ReadAllLines(UserRightsCSV).Select(a => a.Split(','));
            foreach (string[] line in lines)
            {
                string name = line[0];
                string privelegeDescName = line[1];

                if (!accountsMapDict.ContainsKey(name))
                {
                    throw new Exception(string.Format("Account name {0} does not exist in the {1} file ", name, AccountsMappingCSV));
                }

                string mapped_name = accountsMapDict[name];

                if (!accounts.ContainsKey(mapped_name))
                {
                    accounts[mapped_name] = new List<string>();
                }

                if (!privDict.ContainsKey(privelegeDescName))
                {
                    throw new Exception(string.Format("Privelege {0} does not exist in the {1} file ", privelegeDescName, UserRightsConstantNamesCSV));
                }
                string privName = privDict[privelegeDescName];
                if (!accounts[mapped_name].Contains(privName))
                {
                    accounts[mapped_name].Add(privName);
                }
            }


            return accounts;
        }


        /// <summary>
        /// Hardens SQL Server according to the hardening guidelines
        /// </summary>
        /// <param name="SQLServicesUser"></param>
        /// <param name="SQLServer"></param>
        /// <param name="SQLPort"></param>
        /// <param name="LogBox"></param>
        public void hardenSQLServer(string SQLServer, string SQLPort, RichTextBox LogBox)
        {
            Logging.logToUI("Performing SQL Hardening on SQL server " + SQLServer + ":" + SQLPort + ", using SQL Services account...", "action", LogBox);
            Logging.logToDebugLog("Hardening SQL Server settings...");

            try
            {
                Logging.logToDebugLog("Creating logs folder under Logs\\SQLHardening");
                Directory.CreateDirectory(".\\Logs\\SQLHardening");
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed creating hardening logs directory. Check logs for info.", "warning", LogBox);
                Logging.logToFile(exc);
            }


            try
            {
                Logging.logToDebugLog("Running SQL Hardening script");
                string cmdLineStr = "/c .\\Scripts\\RunSQLHardening.cmd " + SQLServer + " " + SQLPort;
                int processExitCode = OSUtilities.launchNewCMD(cmdLineStr);
                if (processExitCode == 0)
                    GeneralUtilities.writeVersionToRegistry("SQL Hardening");
                else
                {
                    Logging.logToUI("SQL Hardening scripts completed with errors. Check the logs for info.", "error", LogBox);
                    Logging.logToDebugLog("SQL Hardening scripts completed with error, script exit code is " + processExitCode.ToString());
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Didn\'t manage to run SQL hardening. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }


        }





    }
}
