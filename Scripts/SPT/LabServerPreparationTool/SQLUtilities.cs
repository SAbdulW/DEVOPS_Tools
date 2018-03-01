using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LabServerPreparationTool
{
    class SQLUtilities
    {
        
        /// <summary>
        /// Gets the SQL instance name runinng on a specific port
        /// </summary>
        /// <param name="connectionToSQL"></param>
        /// <param name="accountDomain"></param>
        /// <param name="SQLAccount"></param>
        /// <param name="SQLPassword"></param>
        /// <param name="LogBox"></param>
        /// <returns></returns>
        public static string getSQLInstanceName(SqlConnection connectionToSQL, RichTextBox LogBox)
        {
            string instanceName = "";
            try
            {
                    try
                    {
                        Logging.logToDebugLog("Querying SQL instance name");
                        connectionToSQL.Open();
                        SqlCommand commandToSQL = new SqlCommand("use master SELECT SERVERPROPERTY (\'InstanceName\') as InstanceName", connectionToSQL);
                        using (SqlDataReader SQLreader = commandToSQL.ExecuteReader())
                        {
                            while (SQLreader.Read())
                            {
                                instanceName = SQLreader["InstanceName"].ToString();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.logToUI("Couldn\'t get SQL Instance Name. Check the logs for info.", "error", LogBox);
                        Logging.logToFile(exc);
                    }
                    finally
                    {
                        connectionToSQL.Close();
                    }
            }
            catch (Exception exc)
            {
                Logging.logToUI("Couldn\'t get SQL Instance Name. Check the logs for info.", "error", LogBox);
                Logging.logToFile(exc);
            }
                

            Logging.logToDebugLog("Instance name is: " + instanceName);
            return instanceName;
        }
        
    }
}
