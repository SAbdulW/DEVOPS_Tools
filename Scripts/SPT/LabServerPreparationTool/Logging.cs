using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace LabServerPreparationTool
{

    class Logging
    {

        /// <summary>
        /// Logs a message to a log box
        /// </summary>
        public static void logToUI(string messageText, string messageLevel, RichTextBox logBox)
        {

            if (GeneralUtilities.readConfigProp("EnableSilentMode") == "true")
            {
                //no need to print to GUI in silent mode
                return;
            }


            if (logBox.InvokeRequired)
            {
                logBox.Invoke((MethodInvoker)delegate
                {
                    logToUI(messageText, messageLevel, logBox);
                });
                return;
            }

            int currentPosition = logBox.TextLength;

            logBox.AppendText(messageText + System.Environment.NewLine);
            logBox.SelectionStart = logBox.TextLength;
            logBox.ScrollToCaret();
            if (messageLevel == "error")
            {
                logBox.Select(currentPosition, messageText.Length);
                logBox.SelectionColor = Color.Red;
            }
            if (messageLevel == "warning")
            {
                logBox.Select(currentPosition, messageText.Length);
                logBox.SelectionColor = Color.DarkOrange;
            }
            if (messageLevel == "action")
            {
                // Standard message, no style change
            }
            if (messageLevel == "project")
            {
                logBox.Select(currentPosition, messageText.Length);
                logBox.SelectionFont = new Font(logBox.Font, FontStyle.Bold);
            }
            if (messageLevel == "info")
            {
                logBox.Select(currentPosition, messageText.Length);
                logBox.SelectionBullet = true;
                logBox.BulletIndent = 3;
            }
        }



        /// <summary>
        /// Logs an exception to the error.txt file in the Logs folder
        /// </summary>
        public static void logToFile(Exception exc)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\error.txt", true))
            {
                file.WriteLine(System.Environment.NewLine + DateTime.Now.ToString("HH:mm:ss tt") + System.Environment.NewLine + exc);
                file.Close();
            }
            logToDebugLog(exc.ToString());
        }

        /// <summary>
        /// Logs an exception to internal_log.txt file in the Logs folder
        /// </summary>
        /// <param name="exc"></param>
        public static void logToInternalLog(Exception exc)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\internal_log.txt", true))
            {
                file.WriteLine(System.Environment.NewLine + DateTime.Now.ToString("HH:mm:ss tt") + System.Environment.NewLine + exc);
                file.Close();
            }
        }

        /// <summary>
        /// Logs a message into debug verbose log (log.txt) in the Logs folder
        /// </summary>
        /// <param name="logMessage"></param>

        public static void logToDebugLog(string logMessage, bool replacePwd = false)
        {
            if (replacePwd)
            {
                logMessage = PasswordManager.RemovePasswords(logMessage);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\log.txt", true))
            {
                file.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss tt") + "\t" + logMessage);
                file.Close();
            }
        }

        
        public static void logToDebugLog(string logMessage)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(".\\Logs\\log.txt", true))
            {
                file.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss tt") + "\t" + logMessage);
                file.Close();
            }
        }


        public static string getLogFileName(string logName)
        {
            string logFileToLoad;
            switch (logName)
            {

                case ("OS Settings"):
                    logFileToLoad = ".\\Logs\\WindowsSettings\\WindowsSettingsLog.txt";
                    break;

                case ("User Rights Assignment"):
                    logFileToLoad = ".\\Logs\\WindowsSettings\\SetUserRightsLog.txt";
                    break;

                case ("DB Permissions Configuration"):
                    logFileToLoad = ".\\Logs\\DBPermissions.txt";
                    break;

                case ("SQL Hardening"):
                    try
                    {
                        try
                        {
                            File.Delete(".\\Logs\\SQLHardening\\allLogs.txt");
                        }
                        catch
                        {
                            // in case the file isn't found / can't be deleted, don't delete.
                        }
                        string[] hardeningLogList = Directory.GetFiles(".\\Logs\\SQLHardening");
                        foreach (string i in hardeningLogList)
                        {
                            using (TextReader obj1 = new StreamReader(i))
                            {
                                String sz = obj1.ReadToEnd();
                                using (TextWriter obj2 = new StreamWriter(".\\Logs\\SQLHardening\\allLogs.txt", true))
                                {
                                    obj2.WriteLine(i);
                                    obj2.WriteLine(sz);
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.logToInternalLog(exc);
                    }
                    logFileToLoad = ".\\Logs\\SQLHardening\\allLogs.txt";
                    break;

                case ("Previous Runs"):
                    GeneralUtilities.readVersionsFromRegistry();
                    logFileToLoad = ".\\Logs\\Previous_runs.txt";
                    break;

                case ("Error Log"):
                    logFileToLoad = ".\\Logs\\error.txt";
                    break;

                default:
                    logFileToLoad = ".\\Logs\\log.txt";
                    break;

            }

            return logFileToLoad;
        }




        /// <summary>
        /// Loads a log file into the UI log text box
        /// </summary>
        public static void loadLogFile(string logFileName, RichTextBox logTextBoxName)
        {
            try
            {
                using (System.IO.StreamReader logReader = new System.IO.StreamReader(logFileName))
                {
                    logTextBoxName.Text = logReader.ReadToEnd();
                    logReader.Close();
                }
                logTextBoxName.SelectionStart = logTextBoxName.TextLength;
                logTextBoxName.ScrollToCaret();
            }
            catch (System.IO.FileNotFoundException)
            {
                logTextBoxName.Text = "";
                logTextBoxName.AppendText("Log file doesn\'t exist under the Logs folder." + System.Environment.NewLine);
                logTextBoxName.SelectionStart = logTextBoxName.TextLength;
                logTextBoxName.ScrollToCaret();
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                logTextBoxName.Text = "";
                logTextBoxName.AppendText("Log folder doesn\'t exist under the Logs folder. Make sure the relevant procedure was run." + System.Environment.NewLine);
                logTextBoxName.SelectionStart = logTextBoxName.TextLength;
                logTextBoxName.ScrollToCaret();
            }
            catch (Exception exc)
            {
                logTextBoxName.Text = "";
                logTextBoxName.AppendText("Couldn\'t open log file." + System.Environment.NewLine);
                logTextBoxName.AppendText(exc + System.Environment.NewLine);
                logTextBoxName.SelectionStart = logTextBoxName.TextLength;
                logTextBoxName.ScrollToCaret();
            }

        }

    }
}
