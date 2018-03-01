using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using GenericParsing;
using System.Data;
using FirewallSettings;
using System.Windows.Forms;


namespace FirewallSettings
{
    public class FirewallSettings
    {



        /// <summary>
        /// Enables the Firewall on the Server and sets the needed exception rules
        /// </summary>
        /// <param name="blockPort80"></param>
        /// <param name="LogBox"></param>
        public static void enableFirewall(string serverType, bool blockPort80)
        {

            if (serverType == "")
            {
                MessageBox.Show("Please select a Server");
            }
            else
            {
                // Enable firewall and set proper firewall rules

        
                setFirewallRegistry("on");
                if (serverType == "Application Server")
                    disableStealthMode();

                serverType = "\"" + serverType + "\"";

                string cmdLineStr = "/c .\\WindowsSettings\\Firewall\\RunFirewallOpenPorts.cmd " + serverType;
                int processExitCode = launchNewCMD(cmdLineStr);
                if (processExitCode == 0)
                    MessageBox.Show("Firewall was enabled successfully");
                else
                {
                    MessageBox.Show("Firewall script completed with errros, exit code is " + processExitCode.ToString());
                }
            }
        }


        /// <summary>
        /// Disables the Firewall on the Server and removes any system exception rules if existed
        /// </summary>
        /// <param name="blockPort80"></param>
        /// <param name="LogBox"></param>
        public static void disableFirewall(string serverType, bool blockPort80)
        {

            if (serverType == "")
            {
                MessageBox.Show("Please Select a Server");
            }
            else
            {
                // Disable firewall and delete the firewall rules of I360


                serverType = "\"" + serverType + "\"";

                setFirewallRegistry("off");
                string cmdLineStr = "/c .\\WindowsSettings\\Firewall\\RunFirewallClosePorts.cmd " + serverType;
                int processExitCode = launchNewCMD(cmdLineStr);
                if (processExitCode == 0)
                    MessageBox.Show("Firewall was disabled successfully");
                else
                    MessageBox.Show("Firewall script completed with errros, exit code is " + processExitCode.ToString());
            }
        }


        /// <summary>
        /// Enables / Disables Windows Firewall
        /// </summary>
        public static void setFirewallRegistry(string onOff)
        {
            string domainProfileKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\WindowsFirewall\\DomainProfile";
            string privateProfileKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\WindowsFirewall\\PrivateProfile";
            string publicProfileKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\WindowsFirewall\\PublicProfile";

            if (onOff == "on")
            {
                // Enabling Windows Firewall
                Registry.SetValue(domainProfileKey, "EnableFirewall", 1, RegistryValueKind.DWord);
                Registry.SetValue(privateProfileKey, "EnableFirewall", 1, RegistryValueKind.DWord);
                Registry.SetValue(publicProfileKey, "EnableFirewall", 1, RegistryValueKind.DWord);
            }
            else if (onOff == "off")
            {
                // Disabling Windows Firewall
                Console.WriteLine("Disabling Windows Firewall");
                Registry.SetValue(domainProfileKey, "EnableFirewall", 0, RegistryValueKind.DWord);
                Registry.SetValue(privateProfileKey, "EnableFirewall", 0, RegistryValueKind.DWord);
                Registry.SetValue(publicProfileKey, "EnableFirewall", 0, RegistryValueKind.DWord);
            }
            else
            {
                MessageBox.Show("Wrong Firewall Parameter");
            }
        }


        /// <summary>
        /// Disables Windows Firewall Stealth mode
        /// </summary>
        public static void disableStealthMode()
        {
            string stealthModeKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\WindowsFirewall\\DomainProfile";
            try
            {
                Registry.SetValue(stealthModeKey, "DisableStealthMode", 1, RegistryValueKind.DWord);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed disabling Windows Firewall Stealth mode.\n" + exc);
            }

        }


        public static void disableNonSSLPorts()
        {
            string cmdLineStr = "/c powershell.exe -ExecutionPolicy AllSigned /f .\\WindowsSettings\\Firewall\\RestrictSSLPort.ps1";
            int processExitCode = launchNewCMD(cmdLineStr);
            if (processExitCode == 0)
                MessageBox.Show("Disabling non-SSL ports completed successfully");
            else
                MessageBox.Show("Disabling non-SSL ports completed with errros, exit code is " + processExitCode.ToString());
        }


        public static void resetFirewallDefaults()
        {
            string cmdLineStr = "/c powershell.exe -ExecutionPolicy AllSigned /f .\\WindowsSettings\\Firewall\\ResetFirewallDefaults.ps1";
            int processExitCode = launchNewCMD(cmdLineStr);
            if (processExitCode == 0)
                MessageBox.Show("Reset Windows Firewall defaults completed successfully");
            else
                MessageBox.Show("Reset Windows Firewall defaults completed with errros, exit code is " + processExitCode.ToString());
        }


        /// <summary>
        /// Starts a new Command Line window with provided command
        /// </summary>
        public static int launchNewCMD(string cmdLineStr)
        {
            cmdLineStr = escapeCommandLine(cmdLineStr);
            Console.WriteLine("Command line to run: " + cmdLineStr);
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
