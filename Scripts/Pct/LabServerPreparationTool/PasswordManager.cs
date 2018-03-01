using System;
using System.Collections.Generic;
using System.Text;

namespace LabServerPreparationTool
{
    class PasswordManager
    {
        private static List<string> passwordsList = new List<string>();

        public static void AddPassword(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return;
            }

            if (!passwordsList.Contains(pwd))
            {
                passwordsList.Add(pwd);
            }
        }


        public static string RemovePasswords(string msg)
        {
            string res = msg;
            foreach (string pwd in passwordsList)
            {
                if (res.Contains(pwd))
                    res = res.Replace(pwd, "******");
            }

            return res;
        }       
    }
}
