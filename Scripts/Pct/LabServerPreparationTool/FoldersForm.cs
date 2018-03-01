using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LabServerPreparationTool
{
    public partial class FoldersForm : Form
    {

        private I360Platform _i360platform = new I360Platform("", "");
        private string NTFSRightsXML = "FoldersAndRegistry\\NTFSRights.xml";
        private string defaultFolderLocationsXML = "FoldersAndRegistry\\DefaultFolderLocations.xml";

        /// <summary>
        /// Recieves a platform and manages the required directories in an XML.
        /// Sets defaults / save custom folder paths.
        /// </summary>
        /// <param name="i360platform"></param>
        public FoldersForm(I360Platform i360platform)
        {
            InitializeComponent();
            _i360platform = i360platform;   
            loadFolders();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            saveFolders();
            this.Close();
        }

        private void CancelFolderButton_Click(object sender, EventArgs e)
        {
            Logging.logToDebugLog("Folder location update was cancelled");
            this.Close();
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            setDefaults();
        }




        // =====================================================
        //                      METHODS
        // =====================================================


        private void loadFolders()
        {
            Logging.logToDebugLog("Loading system folder locations");


            try
            {
                using (XmlReader reader = XmlReader.Create(defaultFolderLocationsXML))
                {
                    reader.Read();
                    reader.ReadToFollowing("FolderLocations");
                    if (reader[0] == "Yes")
                    {
                        Logging.logToDebugLog("UseDefaults attribute is set to YES, loading platform defaults");
                        object sender = new object();
                        EventArgs e = new EventArgs();
                        DefaultButton_Click(sender, e);
                    }
                    else
                    {
                        try
                        {
                            Logging.logToDebugLog("UseDefaults attribute is set to NO, loading previously saved folder locations");
                            XmlDocument xml = new XmlDocument();
                            xml.Load(NTFSRightsXML);
                            SoftwareDirTextbox.Text = xml.SelectSingleNode("//Softwaredir/Path").InnerText;
                            DataDirTextbox.Text = xml.SelectSingleNode("//Datadir/Path").InnerText;
                            DatabaseFolderTextbox.Text = xml.SelectSingleNode("//DatabaseDir/Path").InnerText;
                            TransactionLogFolderTextbox.Text = xml.SelectSingleNode("//TransactionLogDir/Path").InnerText;
                            OLTPFolderTextbox.Text = xml.SelectSingleNode("//ContactOLTPDir/Path").InnerText;
                            TempBDFolderTextBox.Text = xml.SelectSingleNode("//TempDBDir/Path").InnerText;
                            SpeechDatadirTextbox.Text = xml.SelectSingleNode("//SpeechDatadir/Path").InnerText;
                            SRFolderTextbox.Text = xml.SelectSingleNode("//SRFolder/Path").InnerText;
                            WindowsTempFolderTextbox.Text = xml.SelectSingleNode("//WindowsTempFolder/Path").InnerText;
                            MediaFolderTextbox.Text = xml.SelectSingleNode("//MediaFolder/Path").InnerText;
                            Media2FolderTextbox.Text = xml.SelectSingleNode("//Media2Folder/Path").InnerText;
                            SQLRootFolderTextbox.Text = xml.SelectSingleNode("//SQLRootFolder/Path").InnerText;
                            SQL2008FolderTextbox.Text = xml.SelectSingleNode("//SQL2008Folder/Path").InnerText;
                            SQL2012FolderTextbox.Text = xml.SelectSingleNode("//SQL2012Folder/Path").InnerText;
                            SQL2014FolderTextbox.Text = xml.SelectSingleNode("//SQL2014Folder/Path").InnerText;
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Failed loading folder locations. Check the logs for errors.");
                            Logging.logToFile(exc);
                        }
                    }
                    reader.MoveToElement();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed reading folder locations. Check the logs for errors.");
                Logging.logToFile(exc);
            }

        }


        private void saveFolders()
        {
            Logging.logToDebugLog("Saving system folder locations");
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(NTFSRightsXML);
                xml.SelectSingleNode("//Softwaredir/Path").InnerText = SoftwareDirTextbox.Text;
                xml.SelectSingleNode("//Datadir/Path").InnerText = DataDirTextbox.Text;
                xml.SelectSingleNode("//DatabaseDir/Path").InnerText = DatabaseFolderTextbox.Text;
                xml.SelectSingleNode("//TransactionLogDir/Path").InnerText = TransactionLogFolderTextbox.Text;
                xml.SelectSingleNode("//ContactOLTPDir/Path").InnerText = OLTPFolderTextbox.Text;
                xml.SelectSingleNode("//TempDBDir/Path").InnerText = TempBDFolderTextBox.Text;
                xml.SelectSingleNode("//SpeechDatadir/Path").InnerText = SpeechDatadirTextbox.Text;
                xml.SelectSingleNode("//SRFolder/Path").InnerText = SRFolderTextbox.Text;
                xml.SelectSingleNode("//WindowsTempFolder/Path").InnerText = WindowsTempFolderTextbox.Text;
                xml.SelectSingleNode("//MediaFolder/Path").InnerText = MediaFolderTextbox.Text;
                xml.SelectSingleNode("//Media2Folder/Path").InnerText = Media2FolderTextbox.Text;
                xml.SelectSingleNode("//SQLRootFolder/Path").InnerText = SQLRootFolderTextbox.Text;
                xml.SelectSingleNode("//SQL2008Folder/Path").InnerText = SQL2008FolderTextbox.Text;
                xml.SelectSingleNode("//SQL2012Folder/Path").InnerText = SQL2012FolderTextbox.Text;
                xml.SelectSingleNode("//SQL2014Folder/Path").InnerText = SQL2014FolderTextbox.Text;
                xml.Save(NTFSRightsXML);

                Logging.logToDebugLog("User saved folder locations, changing UseDefaults attribute to NO");
                xml.Load(defaultFolderLocationsXML);
                XmlNode folderLocations = xml.SelectSingleNode("//FolderLocations");
                folderLocations.Attributes[0].Value = "No";
                xml.Save(defaultFolderLocationsXML);
                
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed setting folder locations. Check the logs for errors.");
                Logging.logToFile(exc);
            }
        }

  

        private void setDefaults()
        {
            Logging.logToDebugLog("Loading default system folder locations");
            try
            {
                using (XmlReader reader = XmlReader.Create(defaultFolderLocationsXML))
                {
                    reader.Read();
                    reader.ReadToFollowing("FolderLocations");
                    SoftwareDirTextbox.Text = reader[1];
                    DataDirTextbox.Text = reader[2];
                    DatabaseFolderTextbox.Text = reader[3];
                    TransactionLogFolderTextbox.Text = reader[4];
                    OLTPFolderTextbox.Text = reader[5];
                    TempBDFolderTextBox.Text = reader[6];
                    SpeechDatadirTextbox.Text = reader[7];
                    SRFolderTextbox.Text = reader[8];
                    WindowsTempFolderTextbox.Text = reader[9];
                    MediaFolderTextbox.Text = reader[10];
                    Media2FolderTextbox.Text = reader[11];
                    SQLRootFolderTextbox.Text = reader[12];
                    SQL2008FolderTextbox.Text = reader[13];
                    SQL2012FolderTextbox.Text = reader[14];
                    SQL2014FolderTextbox.Text = reader[15];
                    reader.MoveToElement();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed loading default folder locations. Check the logs for errors.");
                Logging.logToFile(exc);
            }


            _i360platform.updatePlatformType();

            if (!_i360platform.platformHasSQL)
            {
                DatabaseFolderTextbox.Text = "NA";
                TransactionLogFolderTextbox.Text = "NA";
                OLTPFolderTextbox.Text = "NA";
                TempBDFolderTextBox.Text = "NA";
                SQLRootFolderTextbox.Text = "NA";
                SQL2008FolderTextbox.Text = "NA";
                SQL2012FolderTextbox.Text = "NA";
                SQL2014FolderTextbox.Text = "NA";
            }

            if (_i360platform.platformType == "RemoteSQL")
            {
                SoftwareDirTextbox.Text = "NA";
                DataDirTextbox.Text = "NA";
                SpeechDatadirTextbox.Text = "NA";
                SRFolderTextbox.Text = "NA";
                WindowsTempFolderTextbox.Text = "NA";
                MediaFolderTextbox.Text = "NA";
                Media2FolderTextbox.Text = "NA";
            }

            if (!_i360platform.platformName.Contains("Speech Analytics Application") & !_i360platform.platformName.Contains("Consolidated"))
            {
                SpeechDatadirTextbox.Text = "NA";
            }

            if (!(_i360platform.platformName.Contains("Import Manager") | _i360platform.platformName.Contains("Export Manager")))
            {
                MediaFolderTextbox.Text = "NA";
            }

            if (!(_i360platform.platformName.Contains("Warehouse") | _i360platform.platformName.Contains("Consolidated")))
            {
                Media2FolderTextbox.Text = "NA";
            }

            if (_i360platform.SQLversion == "SQL 2008")
            {
                SQL2012FolderTextbox.Text = "NA";
                SQL2014FolderTextbox.Text = "NA";
            }
            else if (_i360platform.SQLversion == "SQL 2012")
            {
                SQL2008FolderTextbox.Text = "NA";
                SQL2014FolderTextbox.Text = "NA";
            }
            else if (_i360platform.SQLversion == "SQL 2014")
            {
                SQL2008FolderTextbox.Text = "NA";
                SQL2012FolderTextbox.Text = "NA";
            }
        }

    }
}
