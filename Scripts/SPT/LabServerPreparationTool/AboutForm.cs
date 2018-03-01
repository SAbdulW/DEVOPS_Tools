using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LabServerPreparationTool
{
    public partial class AboutForm : Form
    {
        //public AboutForm()
        //{
        //    InitializeComponent();
        //    label1.Text = label1.Text += " (" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
        //}

        public AboutForm(string SPTText)
        {
            InitializeComponent();
            label1.Text = SPTText;
        }
    }
}
