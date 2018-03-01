using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.DirectoryServices;
using System.Diagnostics;

namespace LabServerPreparationTool
{
    class OSUtilities
    {
        /// <summary>
        /// Starts a new Command Line window with provided command
        /// </summary>
        public static int launchNewCMD(string cmdLineStr)
        {
            cmdLineStr = escapeCommandLine(cmdLineStr);
            Logging.logToDebugLog("Command line to run: " + cmdLineStr, true);
            using (System.Diagnostics.Process process1 = new System.Diagnostics.Process())
            {
                process1.EnableRaisingEvents = false;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process1.StartInfo.FileName = "cmd.exe";
                process1.StartInfo.Arguments = cmdLineStr;
                process1.Start();
                process1.WaitForExit();
                return process1.ExitCode;
            }
        }

        public static int ExecuteLocalCommand(string command, string args)
        {
            int exitCode = 0;
            string output = "";
            try
            {
                ProcessStartInfo processStartInfo;
                Process process;


                StringBuilder outputBuilder = new StringBuilder();

                processStartInfo = new ProcessStartInfo();
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.Arguments = args;
                processStartInfo.FileName = command;
                processStartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process = new Process();
                process.StartInfo = processStartInfo;
                // enable raising events because Process does not raise events by default
                process.EnableRaisingEvents = true;
                // attach the event handler for OutputDataReceived before starting the process
                process.OutputDataReceived += new DataReceivedEventHandler
                (
                    delegate(object sender, DataReceivedEventArgs e)
                    {
                        // append the new data to the data already read-in
                        outputBuilder.Append(e.Data);
                    }
                );

                process.ErrorDataReceived += new DataReceivedEventHandler
                (
                    delegate(object sender, DataReceivedEventArgs e)
                    {
                        // append the new data to the data already read-in
                        outputBuilder.Append(e.Data);
                    }
                );
                // start the process
                // then begin asynchronously reading the output
                // then wait for the process to exit
                // then cancel asynchronously reading the output
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                process.CancelOutputRead();

                // use the output
                output = outputBuilder.ToString();

                Logging.logToDebugLog(string.Format("ExecuteLocalCommand() - command {0} , process output =  {1}, exit code = {2}", command, output, process.ExitCode));

                exitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                exitCode = -1;
                Logging.logToDebugLog(string.Format("ExecuteHelper->ExecuteLocalCommand() - Executed command {0} failed:{1}", command, ex.ToString()));
            }

            return exitCode;
        }


        /// <summary>
        /// Starts a new Command Line window with provided command and running it as a different user
        /// </summary>
        public static int launchNewCMDAsDifferentUser(string cmdLineStr, string accountName, SecureString securedPassword, string domain)
        {
            Logging.logToDebugLog("Launching command line as user: " + accountName);
            cmdLineStr = escapeCommandLine(cmdLineStr);
            Logging.logToDebugLog("Command line to run: " + cmdLineStr);
            using (System.Diagnostics.Process process1 = new System.Diagnostics.Process())
            {
                process1.EnableRaisingEvents = false;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process1.StartInfo.FileName = "cmd.exe";
                process1.StartInfo.Arguments = cmdLineStr;
                process1.StartInfo.Domain = domain;
                process1.StartInfo.UserName = accountName;
                process1.StartInfo.Password = securedPassword;
                process1.Start();
                process1.WaitForExit();
                return process1.ExitCode;
            }
        }

   

        /// <summary>
        /// Gets the network adapters configured on the machine
        /// </summary>
        public static ArrayList getNetworkAdapters(RichTextBox LogBox)
        {
            Logging.logToDebugLog("Getting machine\'s Network Adapters list");
            ArrayList networkAdapters = new ArrayList();
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    networkAdapters.Add(nic.Name);
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Couldn\'t get existing network adapters. Check logs for errors.", "error", LogBox);
                Logging.logToFile(exc);
            }
            return networkAdapters;
        }


        /// <summary>
        /// Checks if a service is running under a specific user
        /// </summary>
        public static bool checkServiceLogon(bool changeServiceAccounts, string serviceName, string requiredServiceAccountName, string platformName, RichTextBox LogBox)
        {
            Logging.logToDebugLog("Checking service logon for service " + serviceName + ", required service account is " + requiredServiceAccountName);
            try
            {
                using (ServiceController sController = new ServiceController(serviceName))
                {
                    RegistryKey localMachineKey = Registry.LocalMachine;
                    RegistryKey fileServiceKey = localMachineKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + sController.ServiceName);
                    string serviceAccountName = (String)fileServiceKey.GetValue("ObjectName");
                    Logging.logToDebugLog("Current service account: " + serviceAccountName);
                    if (serviceAccountName.ToLower() != requiredServiceAccountName.ToLower())
                        changeServiceAccounts = true;
                }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Couldn\'t verify logon account of " + serviceName + " service. If it is supposed to be enabled, check the logs for info.", "warning", LogBox);
                Logging.logToInternalLog(exc);
            }

            Logging.logToDebugLog("Is change needed? " + changeServiceAccounts.ToString());
            return changeServiceAccounts;
        }

        /// <summary>
        /// Determines the name of the "Administrators" group according to localization parameter in config.prop
        /// </summary>
        /// <returns></returns>
        public static string determineAdministratorsGroupName()
        {
            string administratorsGroupName;
            switch (GeneralUtilities.readConfigProp("OSLanguage"))
            {
                case "German":
                    administratorsGroupName = "Administratoren";
                    break;
                case "Spanish":
                    administratorsGroupName = "Administradores";
                    break;
                case "French":
                    administratorsGroupName = "Administrateurs";
                    break;
                case "Portuguese":
                    administratorsGroupName = "Administradores";
                    break;
                case "Dutch":
                    administratorsGroupName = "Administrators";
                    break;
                case "Polish":
                    administratorsGroupName = "Administratorzy";
                    break;
                case "Russian":
                    administratorsGroupName = "Администраторы";
                    break;
                default: // also valid for Japanese, Chinese, Korean 
                    administratorsGroupName = "Administrators";
                    break;
            }

            Logging.logToDebugLog("Administrators group name according to Language parameters is: " + administratorsGroupName);
            return administratorsGroupName;
        }


        /// <summary>
        /// Determines the name of the "Backup Operators" group according to localization parameter in config.prop
        /// </summary>
        /// <returns></returns>
        public static string determineBackupOperatorsGroupName()
        {
            string backupOperatorsGroupName;
            switch (GeneralUtilities.readConfigProp("OSLanguage"))
            {
                case "German":
                    backupOperatorsGroupName = "sicherungs-operatoren";
                    break;
                case "Spanish":
                    backupOperatorsGroupName = "Operadores de copia de seguridad";
                    break;
                case "French":
                    backupOperatorsGroupName = "Opérateurs de sauvegarde";
                    break;
                case "Portuguese":
                    backupOperatorsGroupName = "Operadores de cópia";
                    break;
                case "Dutch":
                    backupOperatorsGroupName = "Back-upoperators";
                    break;
                case "Polish":
                    backupOperatorsGroupName = "Operatorzy kopii zapasowych";
                    break;
                case "Russian":
                    backupOperatorsGroupName = "Операторы архива";
                    break;
                default: // also valid for Japanese, Chinese, Korean 
                    backupOperatorsGroupName = "Backup Operators";
                    break;
            }

            Logging.logToDebugLog("Backup Operators group name according to Language parameters is: " + backupOperatorsGroupName);
            return backupOperatorsGroupName;
        }


        /// <summary>
        /// Joins the server into the specified domain
        /// </summary>
        /// <param name="domainAddress"></param>
        /// <param name="domainName"></param>
        /// <param name="domainAdmin"></param>
        /// <param name="domainAdminPass"></param>
        /// <param name="NICName"></param>
        /// <param name="LogBox"></param>
        public static void addServerToDomain(string domainAddress, string domainName, string domainAdmin, string domainAdminPass, string NICName, RichTextBox LogBox)
        {
            Logging.logToUI("Joining the server to domain " + domainName + "...", "action", LogBox);
            Logging.logToDebugLog("Joining the server to domain " + domainName + "...");
            string cmdLineStr = "/c .\\Scripts\\AddToDomain.cmd " + domainAddress + " " + domainName + " " + domainAdmin + " " + domainAdminPass + " \"" + NICName + "\"";
            OSUtilities.launchNewCMD(cmdLineStr);
        }


        /// <summary>
        /// Removes a server from a specified domain and adds it to a workgroup
        /// </summary>
        /// <param name="domainAddress"></param>
        /// <param name="domainName"></param>
        /// <param name="domainAdmin"></param>
        /// <param name="domainAdminPass"></param>
        /// <param name="NICName"></param>
        /// <param name="LogBox"></param>
        public static void removeServerFromDomain(string domainAddress, string domainName, string domainAdmin, string domainAdminPass, string NICName, RichTextBox LogBox)
        {
            Logging.logToUI("Removing server from the domain and adding it to a workgroup...", "action", LogBox);
            Logging.logToDebugLog("Removing server from the domain and adding it to a workgroup...");
            string cmdLineStr = "/c .\\Scripts\\RemoveFromDomain.cmd " + domainAddress + " " + domainName + " " + domainAdmin + " " + domainAdminPass + " " + NICName;
            OSUtilities.launchNewCMD(cmdLineStr);
        }

        public static string escapeCommandLine(string cmdLineStr)
        {
            if (cmdLineStr.Contains("^"))
                cmdLineStr = cmdLineStr.Replace("^", "^^");
            if (cmdLineStr.Contains("&"))
                cmdLineStr = cmdLineStr.Replace("&", "^&");
            return cmdLineStr;
        }

    }
}
