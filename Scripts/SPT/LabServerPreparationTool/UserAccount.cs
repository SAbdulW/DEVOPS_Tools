using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace LabServerPreparationTool
{
    /// <summary>
    /// System User Account
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// Create an instance of a User Account
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        public UserAccount(string accountType, string domain, string username)
        {
            Logging.logToDebugLog("Initializing user: " + username);
            _accountType = accountType;
            _domain = GeneralUtilities.getShortNameFromFQDN(domain);
            _username = username;

            if (_username.ToUpper() == "IUSR" || _username.ToUpper() == "IIS_IUSRS")
            {
                Logging.logToDebugLog("User account is IIS user, ommitting domain from full account name");
                _fullAccountName = _username;
            }
            else if (_username.ToUpper() == "NETWORK SERVICE" | _username.ToUpper() == "SERVICE RÉSEAU" | _username.ToUpper() == "netzwerkdienst"
                | _username.ToUpper() == "USŁUGA SIECIOWA" | _username.ToUpper() == "SERVIÇO DE REDE" | _username.ToUpper() == "Servicio de red")
            {
                Logging.logToDebugLog("User account is NETWORK SERVICE, ommitting domain from full account name");
                _fullAccountName = _username;
            }
            else
                _fullAccountName = _domain + "\\" + _username;

            Logging.logToDebugLog("Initialized User Account " + _fullAccountName);
        }


        /// <summary>
        /// Create an instance of a User Account
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public UserAccount(string accountType, string domain, string username, string password)
        {
            Logging.logToDebugLog("Initializing user: " + username + ", password: *******");
            _accountType = accountType;
            _domain = GeneralUtilities.getShortNameFromFQDN(domain);
            _username = username;

            if (_username.ToUpper() == "IUSR" || _username.ToUpper() == "IIS_IUSRS")
            {
                Logging.logToDebugLog("User account is IIS user, ommitting domain from full account name");
                _fullAccountName = _username;
            }
            else if (_username.ToUpper() == "NETWORK SERVICE" | _username.ToUpper() == "SERVICE RÉSEAU" | _username.ToUpper() == "netzwerkdienst"
                | _username.ToUpper() == "USŁUGA SIECIOWA" | _username.ToUpper() == "SERVIÇO DE REDE" | _username.ToUpper() == "Servicio de red")
            {
                Logging.logToDebugLog("User account is NETWORK SERVICE, ommitting domain from full account name");
                _fullAccountName = _username;
            }
            else
                _fullAccountName = _domain + "\\" + _username;

            _password = password;
            _securePassword = GeneralUtilities.createSecuredPassword(_password);
            PasswordManager.AddPassword(password);
            Logging.logToDebugLog("Initialized User Account " + _fullAccountName);
        }

        private string _accountType;
        private string _domain;
        private string _username;
        private string _fullAccountName;
        private string _password;
        private SecureString _securePassword;


        // =====================================================
        //                   PROPERTIES
        // =====================================================

        public string AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public SecureString SecuredPassword
        {
            get { return _securePassword; }
        }

        public string FullAccountName
        {
            get { return _fullAccountName; }
        }


        // =====================================================
        //                   METHODS
        // =====================================================


        /// <summary>
        /// Adds the account to SQL logins
        /// </summary>
        public void AddToSQLLogins(SqlConnection connectionToSQL, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Adding " + _fullAccountName + " to SQL logins");
            try
            {
                string addAccountToSQL =
                   "use master DECLARE @SqlStatement nvarchar(4000) DECLARE @loginName varchar (100) Select @loginName = \'" + _fullAccountName + "\' If not Exists (select loginname from master.dbo.syslogins where name = @loginName and dbname = \'master\') Begin Set @SqlStatement = \'CREATE LOGIN [\' + @loginName + \'] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]\' EXEC sp_executesql @SqlStatement End";
                connectionToSQL.Open();
                SqlCommand commandToSQL = new SqlCommand(addAccountToSQL, connectionToSQL);
                commandToSQL.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed adding " + _fullAccountName + " to SQL logins, check logs for errors!", "error", LogBox);
                Logging.logToFile(exc);
            }
            finally
            {
                connectionToSQL.Close();
            }
        }

        /// <summary>
        /// Adds 'sysadmin' role in SQL to the account
        /// </summary>
        /// <param name="connectionToSQL"></param>
        /// <param name="LogBox"></param>
        public void AddSQLsysadminRole(SqlConnection connectionToSQL, RichTextBox LogBox)
        {
            Logging.logToUI("Granting SQL \'sysadmin\' role for account " + _fullAccountName, "info", LogBox);
            Logging.logToDebugLog("Granting SQL \'sysadmin\' role for account " + _fullAccountName);
            string addSysadmin = "EXEC master..sp_addsrvrolemember @loginame = \'" + _fullAccountName + "\', @rolename = \'sysadmin\'";
            try
            {
                connectionToSQL.Open();
                SqlCommand commandToSQL = new SqlCommand(addSysadmin, connectionToSQL);
                commandToSQL.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed adding \'sysadmin\' role to " + _fullAccountName + ", check logs for errors!", "error", LogBox);
                Logging.logToFile(exc);
            }
            finally
            {
                connectionToSQL.Close();
            }
        }


        /// <summary>
        /// Adds permissions for the account on a folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="permissionType"></param>
        /// <param name="LogBox"></param>
        public void AddFolderPermissions(DirectoryInfo folderName, FileSystemRights permissionType, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Adding permissions for user " + _fullAccountName + " on folder " + folderName.ToString() + " of type " + permissionType.ToString());
            try
            {
                DirectorySecurity folderSecurity = folderName.GetAccessControl();
                folderSecurity.AddAccessRule(new FileSystemAccessRule(_fullAccountName, permissionType, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                folderSecurity.AddAccessRule(new FileSystemAccessRule(_fullAccountName, permissionType, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                folderName.SetAccessControl(folderSecurity);
            }
            catch (DirectoryNotFoundException exc)
            {
                Logging.logToUI("Didn\'t manage to configure user rights on the folder " + folderName + " because it doesn\'t exist. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            catch (Exception exc)
            {
                Logging.logToUI("Didn\'t manage to configure user rights on the folder " + folderName + ". Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
        }


        /// <summary>
        /// Adds permissions for non domain account on a folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="permissionType"></param>
        /// <param name="LogBox"></param>
        public void AddNonDomainAccountFolderPermissions(DirectoryInfo folderName, FileSystemRights permissionType, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Adding permissions for user " + _username + " on folder " + folderName.ToString() + " of type " + permissionType.ToString());
            try
            {
                DirectorySecurity folderSecurity = folderName.GetAccessControl();
                folderSecurity.AddAccessRule(new FileSystemAccessRule(_username, permissionType, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                folderSecurity.AddAccessRule(new FileSystemAccessRule(_username, permissionType, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                folderName.SetAccessControl(folderSecurity);
            }
            catch (Exception exc)
            {
                Logging.logToUI("Didn\'t manage to configure user rights on the folder " + folderName + ". Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
        }

        /// <summary>
        /// Add Registry privileges
        /// </summary>
        /// <param name="registryKeyString"></param>
        /// <param name="registryRight"></param>
        /// <param name="LogBox"></param>
        public void AddRegistryPermissions(string registryKeyString, RegistryRights registryRight, RichTextBox LogBox)
        {
            try
            {
                Logging.logToDebugLog("Adding registry privileges for account " + _fullAccountName + " to key " + registryKeyString + " of type " + registryRight.ToString());
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryKeyString, true))
                {
                    RegistrySecurity regustrySecurity = registryKey.GetAccessControl();

                    RegistryAccessRule rule = new RegistryAccessRule(_fullAccountName,
                    registryRight,
                    InheritanceFlags.ContainerInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                    regustrySecurity.AddAccessRule(rule);
                    registryKey.SetAccessControl(regustrySecurity);
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed setting registry permissions for key " + registryKeyString + ". Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }

        }

        /// <summary>
        /// Add the user account to specific GPO right
        /// </summary>
        /// <param name="objectName"></param>
        public void addToGPO(string objectName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Adding GPO rights for account " + _fullAccountName + " to object " + objectName);
            try
            {


            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed setting GPO rights for object " + objectName + ". Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }

        }



        public bool CheckAccountGroupMembership(string groupName, RichTextBox LogBox)
        {
            if (LogBox != null)
                Logging.logToUI("Checking membership of " + _username + " account, of local group: " + groupName, "info", LogBox);

            Logging.logToDebugLog("Checking membership of " + _username + " account, of local group: " + groupName);
            bool userMemberOfGroup = false;

            string userPath = string.Format("WinNT://{0}/{1},user", _domain, _username);
            string groupPath = string.Format("WinNT://{0}/{1},group", Environment.MachineName, groupName);

            using (DirectoryEntry group = new DirectoryEntry(groupPath))
            {
                foreach (object member in (IEnumerable)group.Invoke("Members"))
                {
                    using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                    {
                        if (memberEntry.Name.ToLower() == _username.ToLower())
                        {
                            userMemberOfGroup = true;
                            break;
                        }
                    }
                }
            }

            if (userMemberOfGroup)
            {
                Logging.logToUI("User exists in the group", "info", LogBox);
                Logging.logToDebugLog("User exists in the group");
            }
            else
            {
                Logging.logToUI("User doesn\'t exist in the group", "info", LogBox);
                Logging.logToDebugLog("User doesn\'t exist in the group");
            }

            return userMemberOfGroup;
        }

        /// <summary>
        /// Adds the account to a local group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="LogBox"></param>
        public bool AddToLocalGroup(string groupSID, RichTextBox LogBox)
        {
            string groupName = "";

            try
            {
                Logging.logToDebugLog("Translating group SID " + groupSID + " to group name");
                SecurityIdentifier s = new SecurityIdentifier(groupSID);
                groupName = s.Translate(typeof(NTAccount)).Value;
                Logging.logToDebugLog("Group retrieved from SID: " + groupName);
                groupName = groupName.Remove(0, 8); // removing BUILTIN\ from group name

            }
            catch (Exception exc)
            {
                if (LogBox != null)
                    Logging.logToUI("Couldn\'t retrieve group name for group SID, check the logs for additional info.", "error", LogBox);
                Logging.logToDebugLog("Couldn\'t retrieve group name for group SID");
                Logging.logToFile(exc);
            }


            if (LogBox != null)
                Logging.logToUI("Adding " + _username + " account to local group: " + groupName, "info", LogBox);

            Logging.logToDebugLog("Adding " + _username + " account to local group: " + groupName);
            bool userMemberOfGroup = false;

            string userPath = string.Format("WinNT://{0}/{1},user", _domain, _username);
            string groupPath = string.Format("WinNT://{0}/{1},group", Environment.MachineName, groupName);

            using (DirectoryEntry group = new DirectoryEntry(groupPath))
            {
                foreach (object member in (IEnumerable)group.Invoke("Members"))
                {
                    try
                    {
                        using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                        {
                            if (memberEntry.Name.ToLower() == _username.ToLower())
                            {
                                userMemberOfGroup = true;
                                break;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        if (LogBox != null)
                            Logging.logToUI("Couldn\'t check if  " + _username + " exists in group " + groupName + ", check the logs for additional info.", "error", LogBox);
                        Logging.logToFile(exc);
                    }

                }

                if (userMemberOfGroup == false)
                {
                    Logging.logToDebugLog("User is not currently a member of the group, adding user to group");
                    try
                    {
                        group.Invoke("Add", userPath);
                        group.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        if (LogBox != null)
                            Logging.logToUI("Didn\'t add " + _username + " to group " + groupName + ", check the logs for additional info.", "error", LogBox);
                        Logging.logToFile(exc);
                    }

                }
                else
                {
                    if (LogBox != null)
                        Logging.logToUI("Didn\'t add " + _username + " to group " + groupName + ", user already exists in the group", "info", LogBox);
                    Logging.logToDebugLog("User is already a member of the group, no need to add");
                }
            }
            return userMemberOfGroup;

        }

        /// <summary>
        /// Removes the account from a local group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="LogBox"></param>
        public bool RemoveFromLocalGroup(string groupName, RichTextBox LogBox)
        {
            if (LogBox != null)
                Logging.logToUI("Removing " + _username + " account from local group: " + groupName, "info", LogBox);
            bool userMemberOfGroup = false;
            Logging.logToDebugLog("Removing " + _username + " account from local group: " + groupName);

            string userPath = string.Format("WinNT://{0}/{1},user", _domain, _username);
            string groupPath = string.Format("WinNT://{0}/{1},group", Environment.MachineName, groupName);

            using (DirectoryEntry group = new DirectoryEntry(groupPath))
            {
                foreach (object member in (IEnumerable)group.Invoke("Members"))
                {
                    using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                    {
                        if (memberEntry.Name.ToLower() == _username.ToLower())
                        {
                            userMemberOfGroup = true;
                            break;
                        }
                    }
                }

                if (userMemberOfGroup == true)
                {
                    Logging.logToDebugLog("User is currently a member of the group, removing user from group");
                    try
                    {
                        group.Invoke("Remove", userPath);
                        group.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        if (LogBox != null)
                            Logging.logToUI("Didn\'t remove " + _username + " from group " + groupName + ", check the logs for additional info.", "error", LogBox);
                        Logging.logToFile(exc);
                    }

                }
                else
                {
                    if (LogBox != null)
                        Logging.logToUI("Didn\'t remove " + _username + " from group " + groupName + ", user is not a member of the group", "info", LogBox);
                    Logging.logToDebugLog("User is not a member of the group, no need to remove user from group");
                }
            }
            return userMemberOfGroup;

        }

    }
}
