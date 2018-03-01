#
#
# To run this "Database Permissions Configuration Tool" please do the following:
#  1) Right click on the script
#  2) Select the "Run with Powershell" context menu option
#
#

PARAM
    (
        [string]$SQLServerName = "",
        [int]$SQLServerPort = 1433,
        [string]$DatabaseUserName = "",
        [string]$DatabaseUserPassword = "",
        [string]$ApplicationUserName = "",
        [string]$ApplicationUserPassword = "",
        [string]$LogLocation = "",
        [switch]$SkipPermissionValidation=$false
    )

[reflection.assembly]::LoadWithPartialName("system.DirectoryServices.AccountManagement") | Out-Null

$appTitle = "Database Permissions Configuration Tool v11.2.4.0111"
$Global:consoleMode = $false

function GenerateForm {

    #region Import the Assemblies

    [reflection.assembly]::LoadWithPartialName("System.Windows.Forms") | Out-Null
    [reflection.assembly]::LoadWithPartialName("System.Drawing") | Out-Null

    #endregion

    #region Define WatermarkTextBox component

    $Refs = ("mscorlib", "System.Windows.Forms")
    $watermarkTextBox = @"

    using System.Runtime.InteropServices;

    public class WatermarkTextBox : System.Windows.Forms.TextBox
    {
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern System.IntPtr SendMessage(System.IntPtr hWnd, uint Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private string watermarkText;
        public string WatermarkText
        {
            get { return watermarkText; }
            set
            {
                watermarkText = value;
                SetWatermark(watermarkText);
            }
        }
        private void SetWatermark(string watermarkText)
        {
            SendMessage(this.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }     
    }
"@

    Add-Type -TypeDefinition $watermarkTextBox -ReferencedAssemblies $Refs

    #endregion

    [System.Windows.Forms.Application]::EnableVisualStyles()

    #region Generated Form Objects

    $form1 = New-Object System.Windows.Forms.Form
    $txtDatabaseUserName = New-Object WatermarkTextBox
    $txtDatabaseUserPassword = New-Object WatermarkTextBox
    $lblDatabaseUserName = New-Object System.Windows.Forms.Label
    $lblDatabaseUserPassword = New-Object System.Windows.Forms.Label
    $txtApplicationUserName = New-Object WatermarkTextBox
    $txtApplicationUserPassword = New-Object WatermarkTextBox
    $lblApplicationUserName = New-Object System.Windows.Forms.Label
    $lblApplicationUserPassword = New-Object System.Windows.Forms.Label
    $lblSQLServerName = New-Object System.Windows.Forms.Label
    $txtSQLServerName = New-Object System.Windows.Forms.TextBox
    $lblSQLServerPort = New-Object System.Windows.Forms.Label
    $numSQLServerPort = New-Object System.Windows.Forms.NumericUpDown
    $gbServerDetails = New-Object System.Windows.Forms.GroupBox
    $gbLoginDetails = New-Object System.Windows.Forms.GroupBox
    $txtLog = New-Object System.Windows.Forms.TextBox

    $gbServerDetails.SuspendLayout()
    $gbLoginDetails.SuspendLayout()
    $form1.SuspendLayout()

    $btnApply = New-Object System.Windows.Forms.Button
    $InitialFormWindowState = New-Object System.Windows.Forms.FormWindowState

    #endregion Generated Form Objects

    $extWidth = 120

    #region Generated Form Code

    # lblSQLServerName
    $lblSQLServerName.AutoSize = $true
    $lblSQLServerName.Location = New-Object System.Drawing.Point 17,24
    $lblSQLServerName.Name = "lblSQLServerName"
    $lblSQLServerName.Size = New-Object System.Drawing.Size 72,13
    $lblSQLServerName.TabIndex = 1
    $lblSQLServerName.Text = "SQL Server Name:"

    # lblSQLServerPort
    $lblSQLServerPort.AutoSize = $true
    $lblSQLServerPort.Location = New-Object System.Drawing.Point 17,50
    $lblSQLServerPort.Name = "lblSQLServerPort"
    $lblSQLServerPort.Size = New-Object System.Drawing.Size 29,13
    $lblSQLServerPort.TabIndex = 2
    $lblSQLServerPort.Text = "Port:"

    # txtSQLServerName
    $txtSQLServerName.Location = New-Object System.Drawing.Point 231,21
    $txtSQLServerName.Name = "txtSQLServerName"
    $txtSQLServerName.Text = hostname
    $txtSQLServerName.Size = New-Object System.Drawing.Size (157+$extWidth),20
    $txtSQLServerName.TabIndex = 3

    # numSQLServerPort
    $numSQLServerPort.Location = New-Object System.Drawing.Point 231,50
    $numSQLServerPort.Maximum = 65536
    $numSQLServerPort.Name = "numSQLServerPort";
    $numSQLServerPort.Size = New-Object System.Drawing.Size (157+$extWidth),20
    $numSQLServerPort.TabIndex = 4
    $numSQLServerPort.Value = 1433

    # txtDatabaseUserName
    $txtDatabaseUserName.Location = New-Object System.Drawing.Point 275,25
    $txtDatabaseUserName.Name = "txtDatabaseUserName"
    $txtDatabaseUserName.Size = New-Object System.Drawing.Size (113+$extWidth),20
    $txtDatabaseUserName.TabIndex = 5
    $txtDatabaseUserName.Text = ""
    $txtDatabaseUserName.WatermarkText = "<domain>\<user name>"


    # txtDatabaseUserPassword
    $txtDatabaseUserPassword.Location = New-Object System.Drawing.Point 275,52
    $txtDatabaseUserPassword.Name = "txtDatabaseUserPassword"
    $txtDatabaseUserPassword.PasswordChar = '*'
    $txtDatabaseUserPassword.Size = New-Object System.Drawing.Size (113+$extWidth),20
    $txtDatabaseUserPassword.TabIndex = 6
    $txtDatabaseUserPassword.Text = ""
    $txtDatabaseUserPassword.WatermarkText = "<password>"

    # lblDatabaseUserName
    $lblDatabaseUserName.AutoSize = $true
    $lblDatabaseUserName.Location = New-Object System.Drawing.Point 17,28
    $lblDatabaseUserName.Name = "lblDatabaseUserName"
    $lblDatabaseUserName.Size = New-Object System.Drawing.Size 237,13
    $lblDatabaseUserName.TabIndex = 7
    $lblDatabaseUserName.Text = "Database Management Account Name:"

    # lblDatabaseUserPassword
    $lblDatabaseUserPassword.AutoSize = $true
    $lblDatabaseUserPassword.Location = New-Object System.Drawing.Point 17,55
    $lblDatabaseUserPassword.Name = "lblDatabaseUserPassword"
    $lblDatabaseUserPassword.Size = New-Object System.Drawing.Size 134,13
    $lblDatabaseUserPassword.TabIndex = 8
    $lblDatabaseUserPassword.Text = "Database Management Account Password:"

    # lblApplicationUserName
    $lblApplicationUserName.AutoSize = $true
    $lblApplicationUserName.Location = New-Object System.Drawing.Point 17,85
    $lblApplicationUserName.Name = "lblApplicationUserName"
    $lblApplicationUserName.Size = New-Object System.Drawing.Size 243,13
    $lblApplicationUserName.TabIndex = 9
    $lblApplicationUserName.Text = "Management Service Account Name:"

    # lblApplicationUserPassword
    $lblApplicationUserPassword.AutoSize = $true
    $lblApplicationUserPassword.Location = New-Object System.Drawing.Point 17,112
    $lblApplicationUserPassword.Name = "lblApplicationUserPassword"
    $lblApplicationUserPassword.Size = New-Object System.Drawing.Size 134,13
    $lblApplicationUserPassword.TabIndex = 10
    $lblApplicationUserPassword.Text = "Management Service Account Password:"

    # txtApplicationUserName
    $txtApplicationUserName.Location = New-Object System.Drawing.Point  275,82
    $txtApplicationUserName.Name = "txtApplicationUserName"
    $txtApplicationUserName.Size = New-Object System.Drawing.Size (113+$extWidth),20
    $txtApplicationUserName.TabIndex = 11
    $txtApplicationUserName.Text = ""
    $txtApplicationUserName.WatermarkText = "<domain>\<user name>"

    # txtApplicationUserPassword
    $txtApplicationUserPassword.Location = New-Object System.Drawing.Point  275,109
    $txtApplicationUserPassword.Name = "txtApplicationUserPassword"
    $txtApplicationUserPassword.PasswordChar = '*'
    $txtApplicationUserPassword.Size = New-Object System.Drawing.Size (113+$extWidth),20
    $txtApplicationUserPassword.TabIndex = 12
    $txtApplicationUserPassword.Text = ""
    $txtApplicationUserPassword.WatermarkText = "<password>"

    # txtLog
    $txtLog.Location = New-Object System.Drawing.Point 18,249
    $txtLog.Multiline = $true
    $txtLog.Name = "txtLog"
    $txtLog.Size = New-Object System.Drawing.Size (403+$extWidth),199
    $txtLog.TabIndex = 15
    $txtLog.ReadOnly = $true
    $txtLog.WordWrap = $false
    $txtLog.ScrollBars = [System.Windows.Forms.ScrollBars]::Both;


    # btnApply
    $btnApply.Location = New-Object System.Drawing.Point (184+$extWidth/2), 454
    $btnApply.Name = "btnApply"
    $btnApply.Size = New-Object System.Drawing.Size 75,23
    $btnApply.TabIndex = 4
    $btnApply.Text = "Apply"
    $btnApply.UseVisualStyleBackColor = $true
    $btnApply.add_Click({
                            try
                            {
                                $btnApply.Enabled = $false
                                $txtLog.Text = ""
                            
                                fun "Verifying Database Management Account (DMSA) credentials..."
                                If ((Check-Credentials -userDomainAndName $txtDatabaseUserName.Text -password $txtDatabaseUserPassword.Text) -eq $false)
                                {
                                    [System.Windows.Forms.Messagebox]::Show('Incorrect Database Management Account Name or Password.',$appTitle,'Ok')
                                    return;
                                }
                            
                                Write-To-Log "Verifying Management Service Account (IMSA) credentials..."
                                If ((Check-Credentials -userDomainAndName $txtApplicationUserName.Text -password $txtApplicationUserPassword.Text) -eq $false)
                                {
                                    [System.Windows.Forms.Messagebox]::Show('Incorrect Management Service Account Name or Password.',$appTitle,'Ok')
                                    return;
                                }
                            }
                            finally
                            {
                                $btnApply.Enabled = $true
                            }

                            $ans = [System.Windows.Forms.Messagebox]::Show('Are you sure you want to apply the permission changes?', $appTitle, 'YesNo')
                        
                            If ($ans -eq 'Yes')
                            {
                                $btnApply.Enabled = $false
                                $overAllres = 0
                                $txtLog.Text = ""

                                ApplyChanges -SQLServerName $txtSQLServerName.Text -SQLServerPort $numSQLServerPort.Value -fullDatabaseUserName $txtDatabaseUserName.Text -DBUserPassword $txtDatabaseUserPassword.Text -fullApplicationUser $txtApplicationUserName.Text -AppUserPassword $txtApplicationUserPassword.Text
                            
                                $btnApply.Enabled = $true                                                   
                            }
                        })

    # gbServerDetails
    $gbServerDetails.Controls.Add($lblSQLServerName)
    $gbServerDetails.Controls.Add($lblSQLServerPort)
    $gbServerDetails.Controls.Add($txtSQLServerName)
    $gbServerDetails.Controls.Add($numSQLServerPort)
    $gbServerDetails.Location = New-Object System.Drawing.Point 18,12
    $gbServerDetails.Name = "gbServerDetails"
    $gbServerDetails.Size = New-Object System.Drawing.Size (403+$extWidth),84
    $gbServerDetails.TabIndex = 13
    $gbServerDetails.TabStop = $false
    $gbServerDetails.Text = "SQL Server Details"

    # gbLoginDetails
    $gbLoginDetails.Controls.Add($lblDatabaseUserName)
    $gbLoginDetails.Controls.Add($lblDatabaseUserPassword)
    $gbLoginDetails.Controls.Add($lblApplicationUserName)
    $gbLoginDetails.Controls.Add($lblApplicationUserPassword)
    $gbLoginDetails.Controls.Add($txtDatabaseUserName)
    $gbLoginDetails.Controls.Add($txtDatabaseUserPassword)
    $gbLoginDetails.Controls.Add($txtApplicationUserName)
    $gbLoginDetails.Controls.Add($txtApplicationUserPassword)
    $gbLoginDetails.Location = New-Object System.Drawing.Point 18,102
    $gbLoginDetails.Name = "gbLoginDetails"
    $gbLoginDetails.Size = New-Object System.Drawing.Size (403+$extWidth),140
    $gbLoginDetails.TabIndex = 14
    $gbLoginDetails.TabStop = $false
    $gbLoginDetails.Text = "SQL Server Login Details"

    $form1.Text = $appTitle + " (running under: " + [System.Security.Principal.WindowsIdentity]::GetCurrent().Name + ")"
    $form1.Name = "MainForm"
    $form1.StartPosition = 4
    $form1.DataBindings.DefaultDataSourceUpdateMode = 0
    $form1.ShowIcon = $false
    $form1.ClientSize = New-Object System.Drawing.Size (437+$extWidth),489
    $form1.FormBorderStyle = 1
    $form1.MaximizeBox = $false
    $form1.MinimizeBox = $false
    $form1.Controls.Add($gbServerDetails)
    $form1.Controls.Add($gbLoginDetails)
    $form1.Controls.Add($txtLog)
    $form1.Controls.Add($btnApply)

    $gbServerDetails.ResumeLayout($false)
    $gbServerDetails.PerformLayout()
    $gbLoginDetails.ResumeLayout($false)
    $gbLoginDetails.PerformLayout()
    $form1.ResumeLayout($false)

    #endregion Generated Form Code

    #Save the initial state of the form
    $InitialFormWindowState = $form1.WindowState
    #Show the Form
    $form1.ShowDialog() | Out-Null

} #End Function

function GenerateValidationDialog
{

    PARAM
    (
        [array]$grantCommands
    )

    [int]$fullSize = 0
    [int]$collapsedSize = 0

    #region Import the Assemblies

    [reflection.assembly]::LoadWithPartialName("System.Windows.Forms") | Out-Null
    [reflection.assembly]::LoadWithPartialName("System.Drawing") | Out-Null

    #endregion


    #region Generated Validation Dialog Objects
    
    $frmValidationDialog = New-Object System.Windows.Forms.Form
    $txtCommands = New-Object System.Windows.Forms.TextBox
    $btnDetails = New-Object System.Windows.Forms.Button
    $lblTextMessage = New-Object System.Windows.Forms.Label
    $gbValidationMessage = New-Object System.Windows.Forms.GroupBox

    #endregion Generated Validation Dialog Objects

    $gbValidationMessage.SuspendLayout()
    $frmValidationDialog.SuspendLayout()            

    # txtCommands
    $txtCommands.BackColor = [System.Drawing.SystemColors]::Control
    $txtCommands.Location = New-Object System.Drawing.Point 12,125
    $txtCommands.Multiline = $true
    $txtCommands.Name = "txtCommands"
    $txtCommands.ScrollBars = [System.Windows.Forms.ScrollBars]::Vertical
    $txtCommands.Size = New-Object System.Drawing.Size 427,328
    $txtCommands.TabIndex = 0
    $txtCommands.Text = ""
    $txtCommands.ReadOnly = $true
    $txtCommands.WordWrap = $false

    # btnDetails
    $btnDetails.Location = New-Object System.Drawing.Point 332,61
    $btnDetails.Name = "btnDetails"
    $btnDetails.Size = New-Object System.Drawing.Size 75,23
    $btnDetails.TabIndex = 1
    $btnDetails.Text = "Details >>"
    $btnDetails.UseVisualStyleBackColor = $true
    $btnDetails.add_Click({
        If ($frmValidationDialog.Height -Ne $Global:fullSize) {
            $frmValidationDialog.Height = $Global:fullSize
        } 
        Else {
            $frmValidationDialog.Height = $Global:collapsedSize
        }    
     })

    # lblTextMessage
    $lblTextMessage.Location = New-Object System.Drawing.Point 13,25
    $lblTextMessage.Name = "lblTextMessage"
    $lblTextMessage.Size = New-Object System.Drawing.Size 407,50
    $lblTextMessage.TabIndex = 2
    $lblTextMessage.Text = "The public server role is not granted to the EXECUTE permision on the internal extended stored procedures. " +
                            "Click Details to view the list of the required T-SQL commands to fix the issue."

    # gbValidationMessage
    $gbValidationMessage.Controls.Add($btnDetails)
    $gbValidationMessage.Controls.Add($lblTextMessage)
    $gbValidationMessage.Location = New-Object System.Drawing.Point 13,13
    $gbValidationMessage.Name = "gbValidationMessage"
    $gbValidationMessage.Size = New-Object System.Drawing.Size 426,94
    $gbValidationMessage.TabIndex = 3
    $gbValidationMessage.TabStop = $false
    $gbValidationMessage.Text = "Validation Message:"

    # frmDialogWithDetails
    $frmValidationDialog.AutoScaleMode = [System.Windows.Forms.AutoScaleMode]::Font
    $frmValidationDialog.ClientSize = New-Object System.Drawing.Size 452, 465
    $frmValidationDialog.Controls.Add($gbValidationMessage)
    $frmValidationDialog.Controls.Add($txtCommands)
    $frmValidationDialog.FormBorderStyle = [System.Windows.Forms.FormBorderStyle]::FixedDialog
    $frmValidationDialog.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen;
    $frmValidationDialog.MaximizeBox = $false
    $frmValidationDialog.MinimizeBox = $false
    $frmValidationDialog.ShowIcon = $false
    $frmValidationDialog.Name = "frmDialogWithDetails"
    $frmValidationDialog.Text = $appTitle + " - Permissions Validation"
    $gbValidationMessage.ResumeLayout($false)
    $frmValidationDialog.ResumeLayout($false)
    $frmValidationDialog.PerformLayout()

    #Save the initial state of the form
    $InitialValidationDialogState = $frmValidationDialog.WindowState
    #Init the OnLoad event to correct the initial state of the form
    $frmValidationDialog.add_Load({

        $screenRectangle = $frmValidationDialog.RectangleToScreen($frmValidationDialog.ClientRectangle)

        $Global:fullSize = $frmValidationDialog.Height
        $Global:collapsedSize = $screenRectangle.Top - $frmValidationDialog.Top + $gbValidationMessage.Top * 2 + $gbValidationMessage.Height

        $frmValidationDialog.Height = $Global:collapsedSize

        foreach($cmd in $grantCommands) {
            $txtCommands.AppendText($cmd + [System.Environment]::NewLine)
        }
       
    })

    #Show the Form
    $frmValidationDialog.ShowDialog() | Out-Null

    Remove-Variable frmValidationDialog
    Remove-Variable collapsedSize -Scope Global
    Remove-Variable fullSize -Scope Global  

}

function ApplyChanges
{
    PARAM
    (
        [string]$SQLServerName,
        [string]$SQLServerPort,
        [string]$fullDatabaseUserName,
        [string]$DBUserPassword = "",
        [string]$fullApplicationUser,
        [string]$AppUserPassword = ""
    )

    $databaseUserDomain = $fullDatabaseUserName.Split("\")[0]
    $databaseUserName = $fullDatabaseUserName.Split("\")[1]
                        
    $applicationUserDomain = $fullApplicationUser.Split("\")[0]
    $applicationUserName = $fullApplicationUser.Split("\")[1]

    $sameUsers = ( $fullDatabaseUserName -eq $fullApplicationUser)
                            
                                           
    $application = "SQLCMD"
    $scriptDir = [System.IO.Path]::GetDirectoryName((Get-Variable MyInvocation -Scope Script).Value.InvocationName)
    
    If ($SkipPermissionValidation -eq $false) {

        # Validate "public" server role EXECUTE permission

        $args = [string]::Format("-b -S{0},{1} -dmaster -i""Grant Internal SPs Execute Permissions to public.sql"" -W -h -1", $SQLServerName, $SQLServerPort)

        Write-To-Log "Validate ""public"" server role EXECUTE permission..."

        [array]$grantCommands = (Run-LocalProcessWithOutput -file $application -arguments $args -workingDirectory $scriptDir)
    
        If ($grantCommands.Count -ne 0) {

            Write-To-Log ""
            Write-To-Log "The public server role is not granted to the EXECUTE permission on the internal stored procedures."
            Write-To-Log ""

            If ($Global:consoleMode -eq $false) {

                # Show Validation Dialog
                GenerateValidationDialog -grantCommands $grantCommands
            }
            Else
            {
                foreach($_ in $grantCommands) {
                    Write-To-Log $_
                }                
            }

            Write-To-Log ""
            Write-To-Log "Apply the fix and repeat the procedure!!!"
            Write-To-Log ""

            Return -1
        }
        Else {

            Write-To-Log ""
            Write-To-Log "Status: Success"
            Write-To-Log ""
        }
    }
                            
    # Grant Execute Permissions to DMSA
    $args = [string]::Format("-b -S{0},{1} -dmaster -i""Grant Execute Permissions to DMSA.sql"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                $SQLServerName, $SQLServerPort, $databaseUserDomain, $databaseUserName)
                            
    Write-To-Log "Grant Execute Permissions (for Database Management Account)"
    Write-To-Log ""

    $overAllres = $overAllres -band ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
                            
    If (!$sameUsers)
    {
        # Grant Execute Permissions to IMSA
        $args = [string]::Format("-b -S{0},{1} -dmaster -i""Grant Execute Permissions to IMSA.sql"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                    $SQLServerName, $SQLServerPort, $applicationUserDomain, $applicationUserName)
                                
        Write-To-Log ""
        Write-To-Log "Grant Execute Permissions (for Management Service Account)"
        Write-To-Log ""                       

        $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
    }
                            
    # Grant Server Level Pemissions to DMSA
    $args = [string]::Format("-b -S{0},{1} -dmaster -i""Grant Server Level Pemissions to DMSA.sql"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                $SQLServerName, $SQLServerPort, $databaseUserDomain, $databaseUserName)
                            
    Write-To-Log ""
    Write-To-Log "Grant Server Level Pemissions (for Database Management Account)"
    Write-To-Log ""
                            
    $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
                            
    If (!$sameUsers)
    {                            
        # Grant Server Level Pemissions to IMSA
        $args = [string]::Format("-b -S{0},{1} -dmaster -i""Grant Server Level Pemissions to IMSA.sql"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                    $SQLServerName, $SQLServerPort, $applicationUserDomain, $applicationUserName)
                                
        Write-To-Log ""
        Write-To-Log "Grant Server Level Pemissions (for Management Service Account)"
        Write-To-Log ""
                                
        $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
    }
                            
    # Create Database Role & User (in MSDB database)
    $args = [string]::Format("-b -S{0},{1} -dmsdb -i""Create Database Role & User.sql"" -v I360RoleName=""I360DBRole"" -v I360UserName=""I360DBUser"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                $SQLServerName, $SQLServerPort, $databaseUserDomain, $databaseUserName)
                                                        
    Write-To-Log ""
    Write-To-Log "Create Database Role & User (in MSDB database)"
    Write-To-Log ""
                            
    $overAllres = $overAllres + ( $res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
                            

    # Create Application Role & User (in MSDB database)
    $args = [string]::Format("-b -S{0},{1} -dmsdb -i""Create Database Role & User.sql"" -v I360RoleName=""I360AppRole"" -v I360UserName=""I360AppUser"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
							    $SQLServerName, $SQLServerPort, $applicationUserDomain, $applicationUserName)
														
    Write-To-Log ""
    Write-To-Log "Create Application Role & User (in MSDB database)"
    Write-To-Log ""

    $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
                            
    # Create Job Proxy Account
    $args = [string]::Format("-b -S{0},{1} -dmsdb -i""Create Proxy Account.sql"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}"" -v I360LoginPwd=""{4}""",
                                $SQLServerName, $SQLServerPort, $databaseUserDomain, $databaseUserName, $DBUserPassword)
                            
    Write-To-Log ""
    Write-To-Log "Create Job Proxy Account"
    Write-To-Log ""
                            
    $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
                            
    # Create Database Role & User (in TEMPDB database)
    $args = [string]::Format("-b -S{0},{1} -dtempdb -i""Create Database Role & User.sql"" -v I360RoleName=""I360DBRole"" -v I360UserName=""I360DBUser"" -v I360LoginDomain=""{2}"" -v I360LoginName=""{3}""",
                                $SQLServerName, $SQLServerPort, $databaseUserDomain, $databaseUserName)
                                                        
    Write-To-Log ""
    Write-To-Log "Create Database Role & User (in TEMPDB database)"
    Write-To-Log ""
                            
    $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)

    # Create Database Role & User (in specific I360 database: CommonDB, CentralDWH, IntelliMiner etc.)

    $args = [string]::Format("-b -S{0},{1} -dmaster -i""RetrieveDatabases.sql"" -W -h -1", $SQLServerName, $SQLServerPort)

    Write-To-Log ""
    Write-To-Log "Retrieve existing database collection..."

    [array]$dbs = (Run-LocalProcessWithOutput -file $application -arguments $args -workingDirectory $scriptDir)

    foreach($d in $dbs) {
                            
        # Create Database Role & User (in $d database)
        $args = [string]::Format("-b -S{0},{1} -d{2} -i""Create Database Role & User.sql"" -v I360RoleName=""I360DBRole"" -v I360UserName=""I360DBUser"" -v I360LoginDomain=""{3}"" -v I360LoginName=""{4}""",
                                    $SQLServerName, $SQLServerPort, $d, $databaseUserDomain, $databaseUserName)
                                                        
        Write-To-Log ""
        Write-To-Log "Create Database Role & User (in $d database)"
        Write-To-Log ""
                            
        $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)

        # Create Application Role & User (in $d database)
        $args = [string]::Format("-b -S{0},{1} -d{2} -i""Create Database Role & User.sql"" -v I360RoleName=""I360AppRole"" -v I360UserName=""I360AppUser"" -v I360LoginDomain=""{3}"" -v I360LoginName=""{4}""",
                                    $SQLServerName, $SQLServerPort, $d, $applicationUserDomain, $applicationUserName)
                                                        
        Write-To-Log ""
        Write-To-Log "Create Application Role & User (in $d database)"
        Write-To-Log ""
                            
        $overAllres = $overAllres + ($res = Run-LocalProcess -file $application -arguments $args -workingDirectory $scriptDir)
    }
                            
    If ($overAllres -eq 0)
    {
        Write-To-Log ""
        Write-To-Log "All database permission scripts ran successfully!"
        Write-To-Log ""  
    }
    Else
    {
        Write-To-Log ""
        Write-To-Log "An error(s) occurred during the script execution! Please review the log for details..."
        Write-To-Log ""  
    }

    Return $overAllres
}

function Run-LocalProcess
{            
    PARAM
    (
        [string]$file,
        [string]$arguments,
        [string]$workingDirectory
    )
       
    $exitCode = 0
    $stdOut = [IO.Path]::GetTempFileName()
    $stdErr = [IO.Path]::GetTempFileName()    
    $proc = Start-Process -FilePath $file -ArgumentList $arguments -Wait -PassThru -NoNewWindow -WorkingDirectory $workingDirectory -RedirectStandardOutput $stdOut -RedirectStandardError $stdErr
    $exitCode = $proc.ExitCode
       
    If ($exitCode -eq 0) { 
        Get-Content $stdOut | ForEach-Object { Write-To-Log $_ }
            
        Write-To-Log ""        
        Write-To-Log "Status: Success"
    }
    Else {
        Get-Content $stdOut | ForEach-Object { Write-To-Log $_ }
        Get-Content $stdErr | ForEach-Object { Write-To-Log $_ }

        Write-To-Log ""
        Write-To-Log "Status: Failed"
    }
    
    Remove-Item $stdOut
    Remove-Item $stdErr
    
    Remove-Variable proc
    Remove-Variable stdOut
    Remove-Variable stdErr   
                    
    return $exitCode            
}

function Run-LocalProcessWithOutput
{            
    PARAM
    (
        [string]$file,
        [string]$arguments,
        [string]$workingDirectory
    )
       
    $exitCode = 0
    $stdOut = [IO.Path]::GetTempFileName()
    $stdErr = [IO.Path]::GetTempFileName()    
    $proc = Start-Process -FilePath $file -ArgumentList $arguments -Wait -PassThru -NoNewWindow -WorkingDirectory $workingDirectory -RedirectStandardOutput $stdOut -RedirectStandardError $stdErr
    $exitCode = $proc.ExitCode
    $databases = @()

    If ($exitCode -eq 0) { 
        Get-Content $stdOut | ForEach-Object { If ($_ -ne "") {  $databases += $_ } }
    }
    Else {
        Get-Content $stdOut | ForEach-Object { Write-To-Log $_ }
        Get-Content $stdErr | ForEach-Object { Write-To-Log $_ }

        Write-To-Log ""
        Write-To-Log "Cannot retrieve existing database collection..."
    }
    
    Remove-Item $stdOut
    Remove-Item $stdErr
    
    Remove-Variable proc
    Remove-Variable stdOut
    Remove-Variable stdErr
    Remove-Variable exitCode
                    
    return ,$databases
}

function Check-Credentials
{
    PARAM
    (
        [string]$userDomainAndName,
        [string]$password
    )
    
    If (($userDomainAndName -eq "") -or ($password -eq ""))
    {
        return $false
    }
	
	$ds = $null
	$local  = $false
	
	Try
	{
		$ds = New-Object System.DirectoryServices.AccountManagement.PrincipalContext('Domain')
	}
	Catch
	{
		$ds = New-Object System.DirectoryServices.AccountManagement.PrincipalContext('Machine')
		$local = $true
	}   
    
	If ($ds -ne $null)
	{
		If (!($result = ($ds.ValidateCredentials($userDomainAndName, $password))))
		{
			$ds.Dispose()
			
			If ($local -eq $false)
			{
				# Check for local machine
				$ds = New-Object System.DirectoryServices.AccountManagement.PrincipalContext('Machine')
				$result = $ds.ValidateCredentials($userDomainAndName, $password)
				$ds.Dispose()
			}
		}

		Remove-Variable ds
		return $result
	}
	Else
	{
		return $false
	}
}

function Write-To-Log
{
    PARAM
    (
        [string]$message
    )
    
	Write-Host $message
	
    If ($LogLocation -ne "") {
        Add-content $LogLocation -value $message
        }
}



If (($SQLServerName -eq "") -or ($DatabaseUserName -eq "") -or ($ApplicationUserName -eq "")) {

    $Global:consoleMode = $false

    #Call the Function
    GenerateForm

} Else {

    $Global:consoleMode = $true

    # Set path for Logging

    If ($LogLocation -eq "") {
        $LogLocation = [string]::Format("{0}\DatabasePermissionsConfiguration_{1}.log", "$env:IMPACT360DATADIR\Logs\CommonDB", [DateTime]::Now.ToString("yyyy-MM-dd_HH.mm.ss.fff"))
    }

    $parentLogFolder = (Split-Path $LogLocation -parent)

    If ((-not [string]::IsNullOrEmpty($parentLogFolder)) -and (-not (Test-Path -Path $parentLogFolder))) { 
        # Create Log File Function
        New-Item -ItemType Directory -Path $parentLogFolder | Out-Null
    }

    Remove-Variable parentLogFolder

    If (Test-Path -Path $LogLocation) {
        Clear-Content $LogLocation
    }

    Write-To-Log "Verifying Database Management Account (DMSA) credentials..."
    If ((Check-Credentials -userDomainAndName $DatabaseUserName -password $DatabaseUserPassword) -eq $false)
    {
        Write-To-Log "Incorrect Database Management Account Name or Password..."
        Exit -1
    }
                            
    Write-To-Log "Verifying Management Service Account (IMSA) credentials..."
    If ((Check-Credentials -userDomainAndName $ApplicationUserName -password $ApplicationUserPassword) -eq $false)
    {
        Write-To-Log "Incorrect Management Service Account Name or Password..."
        Exit -1
    }

    $returnCode = ApplyChanges -SQLServerName $SQLServerName -SQLServerPort $SQLServerPort -fullDatabaseUserName $DatabaseUserName -DBUserPassword $DatabaseUserPassword -fullApplicationUser  $ApplicationUserName -AppUserPassword $ApplicationUserPassword

    Exit $returnCode
}
# SIG # Begin signature block
# MIIeJAYJKoZIhvcNAQcCoIIeFTCCHhECAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCDUbItnSa66yj/t
# fdFWUYXWW8JFZWMqCVrN3s9aWL75lKCCGS8wggPuMIIDV6ADAgECAhB+k+v7fMZO
# WepLmnfUBvw7MA0GCSqGSIb3DQEBBQUAMIGLMQswCQYDVQQGEwJaQTEVMBMGA1UE
# CBMMV2VzdGVybiBDYXBlMRQwEgYDVQQHEwtEdXJiYW52aWxsZTEPMA0GA1UEChMG
# VGhhd3RlMR0wGwYDVQQLExRUaGF3dGUgQ2VydGlmaWNhdGlvbjEfMB0GA1UEAxMW
# VGhhd3RlIFRpbWVzdGFtcGluZyBDQTAeFw0xMjEyMjEwMDAwMDBaFw0yMDEyMzAy
# MzU5NTlaMF4xCzAJBgNVBAYTAlVTMR0wGwYDVQQKExRTeW1hbnRlYyBDb3Jwb3Jh
# dGlvbjEwMC4GA1UEAxMnU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2aWNlcyBD
# QSAtIEcyMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsayzSVRLlxwS
# CtgleZEiVypv3LgmxENza8K/LlBa+xTCdo5DASVDtKHiRfTot3vDdMwi17SUAAL3
# Te2/tLdEJGvNX0U70UTOQxJzF4KLabQry5kerHIbJk1xH7Ex3ftRYQJTpqr1SSwF
# eEWlL4nO55nn/oziVz89xpLcSvh7M+R5CvvwdYhBnP/FA1GZqtdsn5Nph2Upg4XC
# YBTEyMk7FNrAgfAfDXTekiKryvf7dHwn5vdKG3+nw54trorqpuaqJxZ9YfeYcRG8
# 4lChS+Vd+uUOpyyfqmUg09iW6Mh8pU5IRP8Z4kQHkgvXaISAXWp4ZEXNYEZ+VMET
# fMV58cnBcQIDAQABo4H6MIH3MB0GA1UdDgQWBBRfmvVuXMzMdJrU3X3vP9vsTIAu
# 3TAyBggrBgEFBQcBAQQmMCQwIgYIKwYBBQUHMAGGFmh0dHA6Ly9vY3NwLnRoYXd0
# ZS5jb20wEgYDVR0TAQH/BAgwBgEB/wIBADA/BgNVHR8EODA2MDSgMqAwhi5odHRw
# Oi8vY3JsLnRoYXd0ZS5jb20vVGhhd3RlVGltZXN0YW1waW5nQ0EuY3JsMBMGA1Ud
# JQQMMAoGCCsGAQUFBwMIMA4GA1UdDwEB/wQEAwIBBjAoBgNVHREEITAfpB0wGzEZ
# MBcGA1UEAxMQVGltZVN0YW1wLTIwNDgtMTANBgkqhkiG9w0BAQUFAAOBgQADCZuP
# ee9/WTCq72i1+uMJHbtPggZdN1+mUp8WjeockglEbvVt61h8MOj5aY0jcwsSb0ep
# rjkR+Cqxm7Aaw47rWZYArc4MTbLQMaYIXCp6/OJ6HVdMqGUY6XlAYiWWbsfHN2qD
# IQiOQerd2Vc/HXdJhyoWBl6mOGoiEqNRGYN+tjCCBKMwggOLoAMCAQICEA7P9DjI
# /r81bgTYapgbGlAwDQYJKoZIhvcNAQEFBQAwXjELMAkGA1UEBhMCVVMxHTAbBgNV
# BAoTFFN5bWFudGVjIENvcnBvcmF0aW9uMTAwLgYDVQQDEydTeW1hbnRlYyBUaW1l
# IFN0YW1waW5nIFNlcnZpY2VzIENBIC0gRzIwHhcNMTIxMDE4MDAwMDAwWhcNMjAx
# MjI5MjM1OTU5WjBiMQswCQYDVQQGEwJVUzEdMBsGA1UEChMUU3ltYW50ZWMgQ29y
# cG9yYXRpb24xNDAyBgNVBAMTK1N5bWFudGVjIFRpbWUgU3RhbXBpbmcgU2Vydmlj
# ZXMgU2lnbmVyIC0gRzQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCi
# Yws5RLi7I6dESbsO/6HwYQpTk7CY260sD0rFbv+GPFNVDxXOBD8r/amWltm+YXkL
# W8lMhnbl4ENLIpXuwitDwZ/YaLSOQE/uhTi5EcUj8mRY8BUyb05Xoa6IpALXKh7N
# S+HdY9UXiTJbsF6ZWqidKFAOF+6W22E7RVEdzxJWC5JH/Kuu9mY9R6xwcueS51/N
# ELnEg2SUGb0lgOHo0iKl0LoCeqF3k1tlw+4XdLxBhircCEyMkoyRLZ53RB9o1qh0
# d9sOWzKLVoszvdljyEmdOsXF6jML0vGjG/SLvtmzV4s73gSneiKyJK4ux3DFvk6D
# Jgj7C72pT5kI4RAocqrNAgMBAAGjggFXMIIBUzAMBgNVHRMBAf8EAjAAMBYGA1Ud
# JQEB/wQMMAoGCCsGAQUFBwMIMA4GA1UdDwEB/wQEAwIHgDBzBggrBgEFBQcBAQRn
# MGUwKgYIKwYBBQUHMAGGHmh0dHA6Ly90cy1vY3NwLndzLnN5bWFudGVjLmNvbTA3
# BggrBgEFBQcwAoYraHR0cDovL3RzLWFpYS53cy5zeW1hbnRlYy5jb20vdHNzLWNh
# LWcyLmNlcjA8BgNVHR8ENTAzMDGgL6AthitodHRwOi8vdHMtY3JsLndzLnN5bWFu
# dGVjLmNvbS90c3MtY2EtZzIuY3JsMCgGA1UdEQQhMB+kHTAbMRkwFwYDVQQDExBU
# aW1lU3RhbXAtMjA0OC0yMB0GA1UdDgQWBBRGxmmjDkoUHtVM2lJjFz9eNrwN5jAf
# BgNVHSMEGDAWgBRfmvVuXMzMdJrU3X3vP9vsTIAu3TANBgkqhkiG9w0BAQUFAAOC
# AQEAeDu0kSoATPCPYjA3eKOEJwdvGLLeJdyg1JQDqoZOJZ+aQAMc3c7jecshaAba
# tjK0bb/0LCZjM+RJZG0N5sNnDvcFpDVsfIkWxumy37Lp3SDGcQ/NlXTctlzevTcf
# Q3jmeLXNKAQgo6rxS8SIKZEOgNER/N1cdm5PXg5FRkFuDbDqOJqxOtoJcRD8HHm0
# gHusafT9nLYMFivxf1sJPZtb4hbKE4FtAC44DagpjyzhsvRaqQGvFZwsL0kb2yK7
# w/54lFHDhrGCiF3wPbRRoXkzKy57udwgCRNx62oZW8/opTBXLIlJP7nPf8m/PiJo
# Y1OavWl0rMUdPH+S4MO8HNgEdTCCBTYwggQeoAMCAQICEQCdQgnQA6vRJqFREAl2
# 9SQbMA0GCSqGSIb3DQEBCwUAMH0xCzAJBgNVBAYTAkdCMRswGQYDVQQIExJHcmVh
# dGVyIE1hbmNoZXN0ZXIxEDAOBgNVBAcTB1NhbGZvcmQxGjAYBgNVBAoTEUNPTU9E
# TyBDQSBMaW1pdGVkMSMwIQYDVQQDExpDT01PRE8gUlNBIENvZGUgU2lnbmluZyBD
# QTAeFw0xNTA2MTUwMDAwMDBaFw0xODA2MTQyMzU5NTlaMIGeMQswCQYDVQQGEwJV
# UzEOMAwGA1UEEQwFMTE3NDcxETAPBgNVBAgMCE5ldyBZb3JrMREwDwYDVQQHDAhN
# ZWx2aWxsZTEfMB0GA1UECQwWMzMwIFNvdXRoIFNlcnZpY2UgUm9hZDEbMBkGA1UE
# CgwSVmVyaW50IFN5c3RlbXMgSW5jMRswGQYDVQQDDBJWZXJpbnQgU3lzdGVtcyBJ
# bmMwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC2LgSxWe8YmDBtjWV0
# CM2AlcAElHUhWUUPAuPPFsK6jxphzxvuIIYhJh+BDzSjl9iLRr3Yb1SCwvzm/aoL
# WnZDnAanzea98h2x2cl8VdIp4Xh09l8CNE1ZqEdSdQB8mqhpwcyyChC3gdIdKMKh
# IDHSJrowkNrLv4HboNF8QjODdk/gZLB+ZO7E9QEJizwe/WiIwoCycKPWYccsZNz1
# L0+ee1QnSDVv5I9ud6Fh0uVHfmjU23UXDC1vaw65Ugr/9UQ3AtpEEcmwMeNos5b6
# nLKxF5E9aO27k9hlfiLGzAztbRZd5LA6FdAflVhq6CbDZ70VxriNZ2XwMLlyhvY+
# +SC5AgMBAAGjggGNMIIBiTAfBgNVHSMEGDAWgBQpkWD/ik366/mmarjP+eZLvUnO
# EjAdBgNVHQ4EFgQUPhvGlFrOj6TRc2aOMtR5cKcu1bEwDgYDVR0PAQH/BAQDAgeA
# MAwGA1UdEwEB/wQCMAAwEwYDVR0lBAwwCgYIKwYBBQUHAwMwEQYJYIZIAYb4QgEB
# BAQDAgQQMEYGA1UdIAQ/MD0wOwYMKwYBBAGyMQECAQMCMCswKQYIKwYBBQUHAgEW
# HWh0dHBzOi8vc2VjdXJlLmNvbW9kby5uZXQvQ1BTMEMGA1UdHwQ8MDowOKA2oDSG
# Mmh0dHA6Ly9jcmwuY29tb2RvY2EuY29tL0NPTU9ET1JTQUNvZGVTaWduaW5nQ0Eu
# Y3JsMHQGCCsGAQUFBwEBBGgwZjA+BggrBgEFBQcwAoYyaHR0cDovL2NydC5jb21v
# ZG9jYS5jb20vQ09NT0RPUlNBQ29kZVNpZ25pbmdDQS5jcnQwJAYIKwYBBQUHMAGG
# GGh0dHA6Ly9vY3NwLmNvbW9kb2NhLmNvbTANBgkqhkiG9w0BAQsFAAOCAQEAL4hU
# wODRfclM4svmYdWJkicZWI0VUxRzp+TvzoTwqKKrsOmm2gvdt9qsfhFCx8ACPDtt
# ip1azOyPAhZj6s3RQm6FOEAiXUu39Gy0SAWL+4jGGDV2MJby6DuKEJtYD1HRMgFo
# TsYSscsuY4OgNHsLFpooTwq0LmQg+6p3kpw+Lj5r5D/wk3NjIQ54YaeaYgrtFHjG
# h9hO4Vvn7BThxLUzqHXk0jFpab511bGwWEcijhLdtxCM0E0znY1D/yMXEfQJJF4h
# ZzWz1ywJjXx8YmNN8NkkUvWEFggAeji3EEgg5o+iQE2EjpmdewiWe16IS5acK4XS
# UjcFv9ITGXygbzIIVjCCBXQwggRcoAMCAQICECdm7lbrSfOOq9dwovyE3iIwDQYJ
# KoZIhvcNAQEMBQAwbzELMAkGA1UEBhMCU0UxFDASBgNVBAoTC0FkZFRydXN0IEFC
# MSYwJAYDVQQLEx1BZGRUcnVzdCBFeHRlcm5hbCBUVFAgTmV0d29yazEiMCAGA1UE
# AxMZQWRkVHJ1c3QgRXh0ZXJuYWwgQ0EgUm9vdDAeFw0wMDA1MzAxMDQ4MzhaFw0y
# MDA1MzAxMDQ4MzhaMIGFMQswCQYDVQQGEwJHQjEbMBkGA1UECBMSR3JlYXRlciBN
# YW5jaGVzdGVyMRAwDgYDVQQHEwdTYWxmb3JkMRowGAYDVQQKExFDT01PRE8gQ0Eg
# TGltaXRlZDErMCkGA1UEAxMiQ09NT0RPIFJTQSBDZXJ0aWZpY2F0aW9uIEF1dGhv
# cml0eTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAJHoVJLSClaxrA0k
# 3cXPRGd0mSs3o30jcABxvFPfxPoqEo9LfxBWvZ9wcrdhf8lLDxenPeOwBGHu/xGX
# x/SGPgr6Plz5k+Y0etkUa+ecs4Wggnp2r3GQ1+z9DfqcbPrfsIL0FH75vsSmL09/
# mX+1/GdDcr0MANaJ62ss0+2PmBwUq37l42782KjkkiTaQ2tiuFX96sG8bLaL8w6N
# muSbbGmZ+HhIMEXVreENPEVg/DKWUSe8Z8PKLrZr6kbHxyCgsR9l3kgIuqROqfKD
# RjeE6+jMgUhDZ05yKptcvUwbKIpcInu0q5jZ7uBRg8MJRk5tPpn6lRfafDNXQTyN
# Ue0LtlyvLGMa31fIP7zpXcSbr0WZ4qNaJLS6qVY9z2+q/0lYvvCo//S4rek3+7q4
# 9As6+ehDQh6J2ITLE/HZu+GJYLiMKFasFB2cCudx688O3T2plqFIvTz3r7UNIkzA
# EYHsVjv206LiW7eyBCJSlYCTaeiOTGXxkQMtcHQC6otnFSlpUgK7199QalVGv6Cj
# KGF/cNDDoqosIapHziicBkV2v4IYJ7TVrrTLUOZr9EyGcTDppt8WhuDY/0Dd+9BC
# iH+jMzouXB5BEYFjzhhxayvspoq3MVw6akfgw3lZ1iAar/JqmKpyvFdK0kuduxD8
# sExB5e0dPV4onZzMv7NR2qdH5YRTAgMBAAGjgfQwgfEwHwYDVR0jBBgwFoAUrb2Y
# ejS0Jvf6xCZU7wO94CTLVBowHQYDVR0OBBYEFLuvfgI9+qbxPISOre44mOzZMjLU
# MA4GA1UdDwEB/wQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MBEGA1UdIAQKMAgwBgYE
# VR0gADBEBgNVHR8EPTA7MDmgN6A1hjNodHRwOi8vY3JsLnVzZXJ0cnVzdC5jb20v
# QWRkVHJ1c3RFeHRlcm5hbENBUm9vdC5jcmwwNQYIKwYBBQUHAQEEKTAnMCUGCCsG
# AQUFBzABhhlodHRwOi8vb2NzcC51c2VydHJ1c3QuY29tMA0GCSqGSIb3DQEBDAUA
# A4IBAQBkv4PxX5qF0M24oSlXDeha99HpPvJ2BG7xUnC7Hjz/TQ10asyBgiXTw6Aq
# XUz1uouhbcRUCXXH4ycOXYR5N0ATd/W0rBzQO6sXEtbvNBh+K+l506tXRQyvKPrQ
# 2+VQlYi734VXaX2S2FLKc4G/HPPmuG5mEQWzHpQtf5GVklnxTM6jkXFMfEcMOwsZ
# 9qGxbIY+XKrELoLL+QeWukhNkPKUyKlzousGeyOd3qLzTVWfemFFmBhox15AayP1
# eXrvjLVri7dvRvR78T1LBNiTgFla4EEkHbKPFWBYR9vvbkb9FfXZX5qz29i45ECz
# zZc5roW7HY683Ieb0abv8TtvEDhvMIIF4DCCA8igAwIBAgIQLnyHzA6TSlL+lP0c
# t800rzANBgkqhkiG9w0BAQwFADCBhTELMAkGA1UEBhMCR0IxGzAZBgNVBAgTEkdy
# ZWF0ZXIgTWFuY2hlc3RlcjEQMA4GA1UEBxMHU2FsZm9yZDEaMBgGA1UEChMRQ09N
# T0RPIENBIExpbWl0ZWQxKzApBgNVBAMTIkNPTU9ETyBSU0EgQ2VydGlmaWNhdGlv
# biBBdXRob3JpdHkwHhcNMTMwNTA5MDAwMDAwWhcNMjgwNTA4MjM1OTU5WjB9MQsw
# CQYDVQQGEwJHQjEbMBkGA1UECBMSR3JlYXRlciBNYW5jaGVzdGVyMRAwDgYDVQQH
# EwdTYWxmb3JkMRowGAYDVQQKExFDT01PRE8gQ0EgTGltaXRlZDEjMCEGA1UEAxMa
# Q09NT0RPIFJTQSBDb2RlIFNpZ25pbmcgQ0EwggEiMA0GCSqGSIb3DQEBAQUAA4IB
# DwAwggEKAoIBAQCmmJBjd5E0f4rR3elnMRHrzB79MR2zuWJXP5O8W+OfHiQyESdr
# vFGRp8+eniWzX4GoGA8dHiAwDvthe4YJs+P9omidHCydv3Lj5HWg5TUjjsmK7hoM
# ZMfYQqF7tVIDSzqwjiNLS2PgIpQ3e9V5kAoUGFEs5v7BEvAcP2FhCoyi3PbDMKrN
# KBh1SMF5WgjNu4xVjPfUdpA6M0ZQc5hc9IVKaw+A3V7Wvf2pL8Al9fl4141fEMJE
# VTyQPDFGy3CuB6kK46/BAW+QGiPiXzjbxghdR7ODQfAuADcUuRKqeZJSzYcPe9hi
# KaR+ML0btYxytEjy4+gh+V5MYnmLAgaff9ULAgMBAAGjggFRMIIBTTAfBgNVHSME
# GDAWgBS7r34CPfqm8TyEjq3uOJjs2TIy1DAdBgNVHQ4EFgQUKZFg/4pN+uv5pmq4
# z/nmS71JzhIwDgYDVR0PAQH/BAQDAgGGMBIGA1UdEwEB/wQIMAYBAf8CAQAwEwYD
# VR0lBAwwCgYIKwYBBQUHAwMwEQYDVR0gBAowCDAGBgRVHSAAMEwGA1UdHwRFMEMw
# QaA/oD2GO2h0dHA6Ly9jcmwuY29tb2RvY2EuY29tL0NPTU9ET1JTQUNlcnRpZmlj
# YXRpb25BdXRob3JpdHkuY3JsMHEGCCsGAQUFBwEBBGUwYzA7BggrBgEFBQcwAoYv
# aHR0cDovL2NydC5jb21vZG9jYS5jb20vQ09NT0RPUlNBQWRkVHJ1c3RDQS5jcnQw
# JAYIKwYBBQUHMAGGGGh0dHA6Ly9vY3NwLmNvbW9kb2NhLmNvbTANBgkqhkiG9w0B
# AQwFAAOCAgEAAj8COcPu+Mo7id4MbU2x8U6ST6/COCwEzMVjEasJY6+rotcCP8xv
# GcM91hoIlP8l2KmIpysQGuCbsQciGlEcOtTh6Qm/5iR0rx57FjFuI+9UUS1SAuJ1
# CAVM8bdR4VEAxof2bO4QRHZXavHfWGshqknUfDdOvf+2dVRAGDZXZxHNTwLk/vPa
# /HUX2+y392UJI0kfQ1eD6n4gd2HITfK7ZU2o94VFB696aSdlkClAi997OlE5jKgf
# cHmtbUIgos8MbAOMTM1zB5TnWo46BLqioXwfy2M6FafUFRunUkcyqfS/ZEfRqh9T
# TjIwc8Jvt3iCnVz/RrtrIh2IC/gbqjSm/Iz13X9ljIwxVzHQNuxHoc/Li6jvHBhY
# xQZ3ykubUa9MCEp6j+KjUuKOjswm5LLY5TjCqO3GgZw1a6lYYUoKl7RLQrZVnb6Z
# 53BtWfhtKgx/GWBfDJqIbDCsUgmQFhv/K53b0CDKieoofjKOGd97SDMe12X4rsn4
# gxSTdn1k0I7OvjV9/3IxTZ+evR5sL6iPDAZQ+4wns3bJ9ObXwzTijIchhmH+v1V0
# 4SF3AwpobLvkyanmz1kl63zsRQ55ZmjoIs2475iFTZYRPAmK0H+8KCgT+2rKVI2S
# XM3CZZgGns5IW9S1N5NGQXwH3c/6Q++6Z2H/fUnguzB9XIDj5hY5S6cxggRLMIIE
# RwIBATCBkjB9MQswCQYDVQQGEwJHQjEbMBkGA1UECBMSR3JlYXRlciBNYW5jaGVz
# dGVyMRAwDgYDVQQHEwdTYWxmb3JkMRowGAYDVQQKExFDT01PRE8gQ0EgTGltaXRl
# ZDEjMCEGA1UEAxMaQ09NT0RPIFJTQSBDb2RlIFNpZ25pbmcgQ0ECEQCdQgnQA6vR
# JqFREAl29SQbMA0GCWCGSAFlAwQCAQUAoHwwEAYKKwYBBAGCNwIBDDECMAAwGQYJ
# KoZIhvcNAQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGCNwIBCzEOMAwGCisGAQQB
# gjcCARUwLwYJKoZIhvcNAQkEMSIEINmb2CaxdPUIw3cQRUbRyam7wnZXppgDTGZQ
# yZnMN1bWMA0GCSqGSIb3DQEBAQUABIIBAHwk9aYaZtMrNWcpKEZVM1JjTCv2as9Z
# wk3iRBtUl7kLxsUJorRQ9BTCjyyn++fVPYUDomWQ1QMj2FbMB8Uq2ueKA+VqHxS9
# 0ddCHXN/AXRrw5pUSXedw24HjrvS2qFjlZmrFaGgPFJNKQ0vmf+PZw9ub7278ONt
# HDnTv9KLYFbRt9EI1jk3Dfw2gaN909ZZg6sVTjjW1eJVjjAZkttlO7H5q/l7KH5m
# /tn2WKwrquapaHsGncAL1r5UMvVr+e7c2vK9idvKgbfO5T3dnoVT00UiJTqyAzuP
# p8dlSEciWOf8rlUoDJbzbqdhyS9IB6mefJQUvhQHO+N4Ab/nXOJhxqShggILMIIC
# BwYJKoZIhvcNAQkGMYIB+DCCAfQCAQEwcjBeMQswCQYDVQQGEwJVUzEdMBsGA1UE
# ChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xMDAuBgNVBAMTJ1N5bWFudGVjIFRpbWUg
# U3RhbXBpbmcgU2VydmljZXMgQ0EgLSBHMgIQDs/0OMj+vzVuBNhqmBsaUDAJBgUr
# DgMCGgUAoF0wGAYJKoZIhvcNAQkDMQsGCSqGSIb3DQEHATAcBgkqhkiG9w0BCQUx
# DxcNMTYwODIyMTU1NDMwWjAjBgkqhkiG9w0BCQQxFgQUk+5UlZ207JEF/LRJT9/D
# OVKzeM4wDQYJKoZIhvcNAQEBBQAEggEAkICwuCRQQAs9VBEQ6CaLHp+4qMSLaV5f
# c/c4ncsH1aYoeS2Gs42fk2WXkL+Qis0NpxG5Qm2lm617I1TkOanP1kEZCQPs9kly
# aR0CCOskAQoNPnFv/nrPm6f/71/fZ1K3Icd5m3GuaWPl1/zcbWBEeiPcx2YiZlZH
# 7UHwp7BrusqFULPb6849l0Jk1/4YfZFLXr2Dmi0pNgLAKyIGvBg//5VmJvyGXYql
# Br7bbHMpD2rb7x5EGfMKp0fgXclwfoZUjY7so4qLGZKBuvCeysh1fl21lt/FdVyl
# VtUuluGdw5PEpzVVYIGPRheSwmWSra5ipVRBgVH5OxhYFSdzBDU5eg==
# SIG # End signature block
