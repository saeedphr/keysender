
using KeySender.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace KeySender;

public class Main : Form
{
    public Decimal DelaySwitchApplication;
    public List<string> excludes = new List<string>()
  {
    "conhost",
    "svchost",
    "smss",
    "RuntimeBroker",
    "taskhostw",
    "winlogon",
    "wininit",
    "WinStore.App",
    "sihost",
    "csrss",
    "ctfmon",
    "dashost",
    "dllhost",
    "dwm",
    "fontdrvhost",
    "idle",
    "System",
    "ApplicationFrameHost",
    "explorer",
    "GoogleCrashHandler",
    "GoogleCrashHandler64",
    "SearchApp",
    "MsMpEng",
    "NisSrv",
    "SearchFilterHost",
    "Registry",
    "SearchProtocolHost",
    "SearchIndexer",
    "services",
    "ShellExprienceHost",
    "SystemSettings",
    "spoolsv",
    "SettingSyncHost",
    "lsass",
    "SgrmBroker"
  };
    private IContainer components = (IContainer)null;
    private TextBox txtSend;
    private Label label3;
    private NumericUpDown nupDelay;
    private Button btnSend;
    private CheckBox cbPressEnterAtEnd;
    private GroupBox groupBox1;
    private Label label4;
    private NumericUpDown nudApplicationSwitch;
    private GroupBox groupBox2;
    private GroupBox groupBox4;
    private TabControl tabControl1;
    private TabPage tpManual;
    private Label label1;
    private TabPage tpAuto;
    private ComboBox cbTargetApplication;
    private Label lblApplications;
    private CheckBox cbTitleLess;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
    private ToolStripStatusLabel lblversion;
    private Button btnRefreshProcesses;

    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(int hwnd);

    public Main()
    {
        this.InitializeComponent();
        this.TopMost = true;
        this.Load += new EventHandler(this.Main_Load);
        this.Leave += new EventHandler(this.Main_Activated);
        this.DelaySwitchApplication = this.nudApplicationSwitch.Value;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, Main.ShowWindowEnum flags);

    public void BringMainWindowToFront(string processName)
    {
        Process process = ((IEnumerable<Process>)Process.GetProcessesByName(processName)).FirstOrDefault<Process>();
        if (process != null)
        {
            if (process.MainWindowHandle == IntPtr.Zero)
                Main.ShowWindow(process.Handle, Main.ShowWindowEnum.Restore);
            Main.SetForegroundWindow((int)process.MainWindowHandle);
        }
        else
            Process.Start(processName);
    }

    private void Main_Activated(object sender, EventArgs e) => this.Refresh();

    private void Main_Load(object sender, EventArgs e) => this.RefreshProcessList();

    private void RefreshProcessList()
    {
        this.cbTargetApplication.Items.Clear();
        foreach (Process process in Process.GetProcesses())
        {
            if ((this.cbTitleLess.Checked || !(process.MainWindowTitle == "")) && process.Id != 0 && !this.excludes.Contains(process.ProcessName))
            {
                ComboboxItem comboboxItem = new ComboboxItem();
                comboboxItem.Text = $"{process.ProcessName} {process.MainWindowTitle} - {process.Id}";
                comboboxItem.Value = process.Id;
                if (process.MainWindowTitle == "" && this.cbTargetApplication.Items.Count > 1)
                    this.cbTargetApplication.Items.Insert(this.cbTargetApplication.Items.Count - 1, (object)comboboxItem);
                else
                    this.cbTargetApplication.Items.Insert(0, (object)comboboxItem);
            }
        }
        if (this.cbTargetApplication.Items.Count < 1)
            return;
        this.cbTargetApplication.SelectedIndex = 0;
    }

    protected override void OnLoad(EventArgs e)
    {
        Screen screen = Screen.FromPoint(this.Location);
        Rectangle workingArea = screen.WorkingArea;
        int x = workingArea.Right - this.Width + 7;
        workingArea = screen.WorkingArea;
        int y = workingArea.Bottom - this.Height + 7;
        this.Location = new Point(x, y);
        base.OnLoad(e);
    }

    private void TrayIcon_DoubleClick(object sender, EventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
    }

    private void btnRefreshProcesses_Click(object sender, EventArgs e) => this.RefreshProcessList();

    private void btnPaste_Click(object sender, EventArgs e)
    {
        if (!Clipboard.ContainsText(TextDataFormat.Text))
            return;
        this.txtSend.Text = Clipboard.GetText(TextDataFormat.Text);
    }

    private void btnSend_Click(object sender, EventArgs e) => this.Send();

    private void Send()
    {
        this.btnSend.Text = "Sending ...";
        this.btnSend.Enabled = false;
        this.btnSend.UseWaitCursor = true;
        try
        {
            if (this.tabControl1.SelectedIndex == 1 && this.cbTargetApplication.SelectedItem is ComboboxItem selectedItem)
                this.BringMainWindowToFront(Process.GetProcessById(((ComboboxItem)cbTargetApplication.SelectedItem).Value).ProcessName);
            Thread.Sleep((int)this.nudApplicationSwitch.Value);
            foreach (char ch in this.txtSend.Text.ToCharArray())
            {
                SendKeys.SendWait(EscapeSendKeysChar(ch));
                Thread.Sleep((int)this.nupDelay.Value);
            }
            if (this.cbPressEnterAtEnd.Checked)
                SendKeys.SendWait("{Enter}");
        }
        catch (Exception ex)
        {
            int num = (int)MessageBox.Show(ex.Message);
        }
        this.btnSend.Text = "Send";
        this.btnSend.Enabled = true;
        this.btnSend.UseWaitCursor = false;
    }

    private void cbTitleLess_CheckedChanged(object sender, EventArgs e) => this.RefreshProcessList();



    protected override void Dispose(bool disposing)
    {
        if (disposing && this.components != null)
            this.components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtSend = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nupDelay = new System.Windows.Forms.NumericUpDown();
            this.cbPressEnterAtEnd = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudApplicationSwitch = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpManual = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tpAuto = new System.Windows.Forms.TabPage();
            this.cbTargetApplication = new System.Windows.Forms.ComboBox();
            this.lblApplications = new System.Windows.Forms.Label();
            this.cbTitleLess = new System.Windows.Forms.CheckBox();
            this.btnRefreshProcesses = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblversion = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.nupDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudApplicationSwitch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpManual.SuspendLayout();
            this.tpAuto.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSend
            // 
            this.txtSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSend.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSend.Location = new System.Drawing.Point(3, 21);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(589, 25);
            this.txtSend.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(213, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sendkey delay (ms)";
            // 
            // nupDelay
            // 
            this.nupDelay.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nupDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupDelay.Location = new System.Drawing.Point(322, 16);
            this.nupDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nupDelay.Name = "nupDelay";
            this.nupDelay.Size = new System.Drawing.Size(50, 22);
            this.nupDelay.TabIndex = 7;
            this.nupDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cbPressEnterAtEnd
            // 
            this.cbPressEnterAtEnd.AutoSize = true;
            this.cbPressEnterAtEnd.Checked = true;
            this.cbPressEnterAtEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPressEnterAtEnd.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPressEnterAtEnd.Location = new System.Drawing.Point(378, 19);
            this.cbPressEnterAtEnd.Name = "cbPressEnterAtEnd";
            this.cbPressEnterAtEnd.Size = new System.Drawing.Size(124, 17);
            this.cbPressEnterAtEnd.TabIndex = 9;
            this.cbPressEnterAtEnd.Text = "Press {Enter} at end";
            this.cbPressEnterAtEnd.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(524, 201);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 50);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send (Enter)";
            this.btnSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPressEnterAtEnd);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudApplicationSwitch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nupDelay);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 47);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Window switch delay (ms)";
            // 
            // nudApplicationSwitch
            // 
            this.nudApplicationSwitch.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudApplicationSwitch.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudApplicationSwitch.Location = new System.Drawing.Point(152, 16);
            this.nudApplicationSwitch.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudApplicationSwitch.Name = "nudApplicationSwitch";
            this.nudApplicationSwitch.Size = new System.Drawing.Size(55, 22);
            this.nudApplicationSwitch.TabIndex = 8;
            this.nudApplicationSwitch.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSend);
            this.groupBox2.Location = new System.Drawing.Point(12, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 62);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Text to send";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tabControl1);
            this.groupBox4.Location = new System.Drawing.Point(12, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(595, 123);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Window focus mode";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpManual);
            this.tabControl1.Controls.Add(this.tpAuto);
            this.tabControl1.Location = new System.Drawing.Point(6, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(582, 94);
            this.tabControl1.TabIndex = 16;
            // 
            // tpManual
            // 
            this.tpManual.Controls.Add(this.label1);
            this.tpManual.Location = new System.Drawing.Point(4, 26);
            this.tpManual.Name = "tpManual";
            this.tpManual.Padding = new System.Windows.Forms.Padding(3);
            this.tpManual.Size = new System.Drawing.Size(574, 64);
            this.tpManual.TabIndex = 0;
            this.tpManual.Text = "Manual";
            this.tpManual.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(565, 58);
            this.label1.TabIndex = 0;
            this.label1.Text = "In this mode, you have some time to select (focus) the desired software window af" +
    "ter pressing the send button. The amount of this time period can be adjusted by " +
    "window switch delay option.";
            // 
            // tpAuto
            // 
            this.tpAuto.Controls.Add(this.cbTargetApplication);
            this.tpAuto.Controls.Add(this.lblApplications);
            this.tpAuto.Controls.Add(this.cbTitleLess);
            this.tpAuto.Controls.Add(this.btnRefreshProcesses);
            this.tpAuto.Location = new System.Drawing.Point(4, 26);
            this.tpAuto.Name = "tpAuto";
            this.tpAuto.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuto.Size = new System.Drawing.Size(574, 64);
            this.tpAuto.TabIndex = 1;
            this.tpAuto.Text = "Automatic";
            this.tpAuto.UseVisualStyleBackColor = true;
            // 
            // cbTargetApplication
            // 
            this.cbTargetApplication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetApplication.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTargetApplication.FormattingEnabled = true;
            this.cbTargetApplication.Location = new System.Drawing.Point(114, 12);
            this.cbTargetApplication.Name = "cbTargetApplication";
            this.cbTargetApplication.Size = new System.Drawing.Size(417, 25);
            this.cbTargetApplication.TabIndex = 0;
            // 
            // lblApplications
            // 
            this.lblApplications.AutoSize = true;
            this.lblApplications.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplications.Location = new System.Drawing.Point(15, 15);
            this.lblApplications.Name = "lblApplications";
            this.lblApplications.Size = new System.Drawing.Size(86, 17);
            this.lblApplications.TabIndex = 1;
            this.lblApplications.Text = "Applications :";
            // 
            // cbTitleLess
            // 
            this.cbTitleLess.AutoSize = true;
            this.cbTitleLess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTitleLess.Location = new System.Drawing.Point(18, 43);
            this.cbTitleLess.Name = "cbTitleLess";
            this.cbTitleLess.Size = new System.Drawing.Size(155, 17);
            this.cbTitleLess.TabIndex = 10;
            this.cbTitleLess.Text = "List title-less applications";
            this.cbTitleLess.UseVisualStyleBackColor = true;
            this.cbTitleLess.CheckedChanged += new System.EventHandler(this.cbTitleLess_CheckedChanged);
            // 
            // btnRefreshProcesses
            // 
            this.btnRefreshProcesses.Location = new System.Drawing.Point(537, 11);
            this.btnRefreshProcesses.Name = "btnRefreshProcesses";
            this.btnRefreshProcesses.Size = new System.Drawing.Size(23, 25);
            this.btnRefreshProcesses.TabIndex = 2;
            this.btnRefreshProcesses.UseVisualStyleBackColor = true;
            this.btnRefreshProcesses.Click += new System.EventHandler(this.btnRefreshProcesses_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblversion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 257);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(614, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripStatusLabel1.IsLink = true;
            this.toolStripStatusLabel1.LinkColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(129, 17);
            this.toolStripStatusLabel1.Text = "KeySender by saeedphr";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // lblversion
            // 
            this.lblversion.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(48, 17);
            this.lblversion.Text = "version:";
            // 
            // Main
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 279);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Key Sender";
            this.Load += new System.EventHandler(this.Main_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.nupDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudApplicationSwitch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpManual.ResumeLayout(false);
            this.tpAuto.ResumeLayout(false);
            this.tpAuto.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private enum ShowWindowEnum
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        Maximize = 3,
        ShowMaximized = 3,
        ShowNormalNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActivate = 7,
        ShowNoActivate = 8,
        Restore = 9,
        ShowDefault = 10, // 0x0000000A
        ForceMinimized = 11, // 0x0000000B
    }


    private void Main_Load_1(object sender, EventArgs e)
    {
        lblversion.Text = $"Version {Application.ProductVersion}";
        if (Clipboard.GetText().Length < 85)
            txtSend.Text = Clipboard.GetText();
        txtSend.Select();
        txtSend.Focus();
    }

    private static string EscapeSendKeysChar(char ch)
    {
        return ch switch
        {
            '{' => "{{}",
            '}' => "{}}",
            '+' => "{+}",
            '^' => "{^}",
            '%' => "{%}",
            '~' => "{~}",
            '(' => "{(}",
            ')' => "{)}",
            _ => ch.ToString()
        };
    }

    private void toolStripStatusLabel1_Click(object sender, EventArgs e)
    {
       
        System.Diagnostics.Process.Start("https://github.com/saeedphr/keysender");

    }
}
