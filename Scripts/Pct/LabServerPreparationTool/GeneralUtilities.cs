using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Xml;

namespace LabServerPreparationTool
{
    class GeneralUtilities
    {

        /// <summary>
        /// Moves old logs to LogsHistory folder to cleanup the Logs folder
        /// </summary>
        public static void DeleteAllLogs()
        {
            //Copy directory to history folder
            try
            {
                DirectoryCopy("Logs", Path.Combine("LogsHistory", string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now)), true);
            }
            catch { }

            //Delete previous run
            string[] files = Directory.GetFiles("Logs", "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Logging.logToDebugLog("Deleted old log file: " + file.ToString());
                }
                catch
                {
                    Logging.logToDebugLog("Didn\'t delete log file: " + file.ToString());
                }
            }
        }


        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }


        /// <summary>
        /// Return list of files with found patterns
        /// </summary>
        /// <param name="patternsTosearch"></param>
        /// <returns></returns>
        public static List<string> SearchForPatternInLogFiles(string[] patternsTosearch)
        {
            List<string> files = new List<string>();

            string[] filesarr = Directory.GetFiles("Logs");
            foreach (string file in filesarr)
            {
                string text = File.ReadAllText(file);
                foreach (string pattern in patternsTosearch)
                {
                    if (text.ToLower().Contains(pattern.ToLower()))
                    {
                        if (text.ToLower().Contains("error.txt"))
                            break;
                        else
                        {
                            files.Add(file);
                            break;
                        }
                    }
                }
            }
            return files;
        }


        /// <summary>
        /// Creates a secured password needed to run Command Line window as a different user
        /// </summary>
        /// <param name="passwordToTransform"></param>
        /// <returns></returns>
        public static SecureString createSecuredPassword(string passwordToTransform)
        {
            SecureString securedPassword = new SecureString();
            char[] passwordChars = passwordToTransform.ToCharArray();
            foreach (char c in passwordChars)
            {
                securedPassword.AppendChar(c);
            }

            return securedPassword;
        }


        /// <summary>
        /// Taking only first part of the FQDN, until the "."
        /// </summary>
        /// <param name="FQDN"></param>
        /// <returns></returns>
        public static string getShortNameFromFQDN(string FQDN)
        {
            string[] FQDNSplitted = FQDN.Split('.');
            string shortname = FQDNSplitted[0];
            return shortname;
        }

        /// <summary>
        /// Reading a configuration property from the config.prop file
        /// </summary>
        /// <remarks>
        /// Supported properties in config.prop:
        /// EnableSilentode=true/false          -   whether the UI is shown or the tool works in silent mode
        /// SetNTFSRights=true/false            -   whether to apply permissions on folders
        /// SetRegistryRights=true              -   whether to apply permissions on registry keys
        /// EnableIndividualActions = t/f       -   whether OS settings, set Accounts in OS and Set accounts in SQL are visible or only Prepare Server
        /// AllowAllToolLocations = t/f         -   allows running the tool from any location, including network / desktop
        /// SetGPO = t/f                        -   whether GPO user rights are updated
        /// CreateSQLLogins = t/f               -   whether new logins are created in SQL
        /// ConfigureSQLSysadmin = t/f          -   whether SQL sysadmin is added to the relevant SQL logins
        /// RunDBPermissionsTool = t/f          -   whether the DB permissions tool is run from SPT
        /// UpdateSQLServicesAccount = t/f       - whether the service account of the SQL services is changed
        /// HidePasswords = t/f                 - whether passwords in the UI will be obfuscated or not
        /// 
        /// </remarks>
        public static string readConfigProp(string propertyName)
        {
            string propertyValue = "";
            Dictionary<String, String> propertyDictionary = new Dictionary<String, String>();
            try
            {
                foreach (String line in System.IO.File.ReadAllLines("config.prop"))
                {
                    if ((!String.IsNullOrEmpty(line)) &&
                        (!line.StartsWith(";")) &&
                        (!line.StartsWith("#")) &&
                        (!line.StartsWith("'")) &&
                        (line.Contains("=")))
                    {
                        int index = line.IndexOf('=');
                        String key = line.Substring(0, index).Trim();
                        String value = line.Substring(index + 1).Trim();

                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }

                        propertyDictionary.Add(key, value);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed reading config.prop. Check the logs for errors.");
                Logging.logToFile(exc);
            }

            propertyDictionary.TryGetValue(propertyName, out propertyValue);
            return propertyValue;
        }

        /// <summary>
        /// Checks that the tool doesn't run from a network location.
        /// </summary>
        public static void checkToolLocation()
        {
            if (GeneralUtilities.readConfigProp("AllowAllToolLocations") != "true")
            {
                Logging.logToDebugLog("Checking location of the tool to see it doesn\'t run from network location");
                string toolLocation = Directory.GetCurrentDirectory();
                if (toolLocation.Contains("\\\\"))
                {
                    MessageBox.Show("The tool can\'t run from a network location. Please run it locally on the server.", "Warning");
                    System.Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Reads the versions of all components from version.txt and shows them in a messagebox
        /// </summary>
        /// <param name="LogBox"></param>
        public static void readVersions(RichTextBox LogBox)
        {
            Logging.logToDebugLog("Checking Server Preparation Tool components versions");
            try
            {
                using (System.IO.StreamReader logReader = new System.IO.StreamReader("ReadMe.txt"))
                {
                    MessageBox.Show(logReader.ReadToEnd());
                    logReader.Close();
                }
            }
            catch
            {
                Logging.logToUI("Could not load versions", "error", LogBox);
                Logging.logToDebugLog("Could not load versions");
            }
        }

        /// <summary>
        /// Reads a version of specific component from ReadMe.txt
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static string readComponentVersion(string componentName)
        {
            Logging.logToDebugLog("Checking Server Preparation Tool component version: " + componentName);
            string versionValue = "";
            Dictionary<String, String> propertyDictionary = new Dictionary<String, String>();
            try
            {
                foreach (String line in System.IO.File.ReadAllLines("version.txt"))
                {
                    if ((!String.IsNullOrEmpty(line)) &&
                        (!line.StartsWith(";")) &&
                        (!line.StartsWith("#")) &&
                        (!line.StartsWith("'")) &&
                        (line.Contains("-")))
                    {
                        int index = line.IndexOf('-');
                        String key = line.Substring(0, index).Trim();
                        String value = line.Substring(index + 1).Trim();

                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }

                        propertyDictionary.Add(key, value);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed reading versions file. Check the logs for errors.");
                Logging.logToFile(exc);
            }

            if (componentName == "Server Preparation Tool")
                versionValue = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            else
                propertyDictionary.TryGetValue(componentName, out versionValue);
            Logging.logToDebugLog("Value is " + versionValue);
            return versionValue;
        }

        /// <summary>
        /// Writes the version of a component that was run to the registry
        /// </summary>
        /// <param name="componentName"></param>
        public static void writeVersionToRegistry(string componentName)
        {
            Logging.logToDebugLog("Updating version in registry for tracking, component name: " + componentName);
            string keyLocation = "HKEY_LOCAL_MACHINE\\SOFTWARE\\SPT";
            string componentVersion = GeneralUtilities.readComponentVersion(componentName);

            try
            {
                Registry.SetValue(keyLocation, componentName, componentVersion, RegistryValueKind.String);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed writing versions to registry. Check the logs for errors.");
                Logging.logToFile(exc);
            }
        }

        /// <summary>
        /// Reads the versions of components which were run already from registry and writes them to a log file
        /// </summary>
        public static void readVersionsFromRegistry()
        {
            Logging.logToDebugLog("Reading version of previous runs from registry");
            try
            {
                File.Delete(".\\Logs\\Previous_runs.txt");
            }
            catch
            {
                // in case the file isn't found / can't be deleted, don't delete.
            }

            string keyLocation = "HKEY_LOCAL_MACHINE\\SOFTWARE\\SPT";
            string componentVersion = "";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\Previous_runs.txt", true))
            {
                file.WriteLine(System.Environment.NewLine + "Server Preparation Tool" + " - " + Registry.GetValue(keyLocation, "Server Preparation Tool", "NA").ToString());
                file.Close();
            }

            try
            {
                foreach (String line in System.IO.File.ReadAllLines("version.txt"))
                {
                    if ((!String.IsNullOrEmpty(line)) &&
                        (!line.StartsWith(";")) &&
                        (!line.StartsWith("#")) &&
                        (!line.StartsWith("'")) &&
                        (line.Contains("-")))
                    {
                        int index = line.IndexOf('-');
                        String componentName = line.Substring(0, index).Trim();
                        componentVersion = Registry.GetValue(keyLocation, componentName, "NA").ToString();
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\Previous_runs.txt", true))
                        {
                            file.WriteLine(System.Environment.NewLine + componentName + " - " + componentVersion);
                            file.Close();
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\Previous_runs.txt", true))
                {
                    file.WriteLine("NA");
                    file.Close();
                }
                Logging.logToFile(exc);
            }
        }


        /// <summary>
        /// Writes the inputs to registry
        /// </summary>
        /// <param name="componentName"></param>
        public static void writeInputsToRegistry(string domainName, string installUser, string IMSA, string DMSA,
                    string SQLServicestAccount, string platform, string sqlserver, string sqlport)
        {
            Logging.logToDebugLog("Updating provided inputs in registry");
            string keyLocation = "HKEY_LOCAL_MACHINE\\SOFTWARE\\SPT";

            try
            {
                Registry.SetValue(keyLocation, "Domain", domainName, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "InstallationAccount", installUser, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "MSA", IMSA, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "DMSA", DMSA, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "SQLServicesAccount", SQLServicestAccount, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "Server Type", platform, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "SQL Server Name", sqlserver, RegistryValueKind.String);
                Registry.SetValue(keyLocation, "SQL Server Port", sqlport, RegistryValueKind.String);
            }
            catch (Exception exc)
            {
                Logging.logToDebugLog("Failed writing user inputs to registry. Check the logs for errors.");
                Logging.logToFile(exc);
            }
        }


        /// <summary>
        /// Reads the list of system folders from NTFSRights.xml
        /// </summary>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string[] readFoldersList(RichTextBox LogBox)
        {
            Logging.logToDebugLog("Reading system Folders");
            string[] foldersList = new string[] { };
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\NTFSRights.xml");
                XmlNodeList list = xml.SelectNodes("//Name/text()");
                foldersList = new string[list.Count];
                int i = 0;
                foreach (XmlNode folderNode in list)
                {
                    foldersList[i++] = folderNode.Value;
                }


            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading folders list. Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            return foldersList;
        }

        /// <summary>
        /// Reads the list of system registry keys from RegistryRights.xml
        /// </summary>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string[] readRegistryKeysList(RichTextBox LogBox)
        {
            Logging.logToDebugLog("Reading system Registry keys");
            string[] regKeysList = new string[] { };
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\RegistryRights.xml");
                XmlNodeList list = xml.SelectNodes("//Name/text()");
                regKeysList = new string[list.Count];
                int i = 0;
                foreach (XmlNode keyNode in list)
                {
                    regKeysList[i++] = keyNode.Value;
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading registry keys list. Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            return regKeysList;
        }

        /// <summary>
        /// Reading GPO items
        /// </summary>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string[] readGPObjects(RichTextBox LogBox)
        {
            Logging.logToDebugLog("Reading Group Policy Objects");
            string[] GPOList = new string[] { };
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("AccountRights\\GPORights.xml");
                XmlNodeList list = xml.SelectNodes("//Name/text()");
                GPOList = new string[list.Count];
                int i = 0;
                foreach (XmlNode GPONode in list)
                {
                    GPOList[i++] = GPONode.Value;
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading group policy objects list. Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            return GPOList;
        }


        /// <summary>
        /// Reads the path of a specific folder from the NTFSRights.xml
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string readFolderPath(string folderName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Reading path of folder: " + folderName);
            string folderLocation = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\NTFSRights.xml");
                folderLocation = xml.SelectSingleNode("//" + folderName + "/Path").InnerText;
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading folder location for " + folderName + ". Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            Logging.logToDebugLog("Value is " + folderLocation);
            return folderLocation;
        }

        /// <summary>
        /// Reads the path of a specific registry key from the RegistryRights.xml
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string readRegistryKeyPath(string keyName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Reading path of registry key: " + keyName);
            string keyPath = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\RegistryRights.xml");
                keyPath = xml.SelectSingleNode("//" + keyName + "/Path").InnerText;
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading registry key location for " + keyName + ". Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            Logging.logToDebugLog("Value is " + keyPath);
            return keyPath;
        }

        /// <summary>
        /// Returns the folder permissions for the specific account type (IMSA, DMSA, SQLServices, IUSR)
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="accountName"></param>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string readFolderPermissions(string folderName, string accountName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Checking required folder permissions for folder: " + folderName + ", account: " + accountName);
            string folderPermissions = "NA";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\NTFSRights.xml");
                if (xml.SelectSingleNode("//" + folderName + "/ReadExecuteWriteDelete").InnerText.Contains(accountName))
                {
                    folderPermissions = "ReadExecuteWriteDelete";
                }
                else if (xml.SelectSingleNode("//" + folderName + "/ReadExecuteWrite").InnerText.Contains(accountName))
                {
                    folderPermissions = "ReadExecuteWrite";
                }
                else if (xml.SelectSingleNode("//" + folderName + "/ReadExecute").InnerText.Contains(accountName))
                {
                    folderPermissions = "ReadExecute";
                }
                else if (xml.SelectSingleNode("//" + folderName + "/FullControl").InnerText.Contains(accountName))
                {
                    folderPermissions = "FullControl";
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading " + folderName + " folder permissions for " + accountName + ". Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            Logging.logToDebugLog("Value is " + folderPermissions);
            return folderPermissions;
        }

        /// <summary>
        /// Returns the registry key permissions for the specific account type (IMSA, DMSA, SQLServices, IUSR)
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="accountName"></param>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string readRegistryKeyPermissions(string keyName, string accountName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Checking required registry permissions for key: " + keyName + ", account: " + accountName);
            string keyPermissions = "NA";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("FoldersAndRegistry\\RegistryRights.xml");
                if (xml.SelectSingleNode("//" + keyName + "/FullControl").InnerText.Contains(accountName))
                {
                    keyPermissions = "FullControl";
                }
                else if (xml.SelectSingleNode("//" + keyName + "/ReadWrite").InnerText.Contains(accountName))
                {
                    keyPermissions = "ReadWrite";
                }
                else if (xml.SelectSingleNode("//" + keyName + "/Read").InnerText.Contains(accountName))
                {
                    keyPermissions = "Read";
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Failed loading " + keyName + " registry key permissions for " + accountName + ". Check the logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            Logging.logToDebugLog("Value is " + keyPermissions);
            return keyPermissions;
        }



    }
}
