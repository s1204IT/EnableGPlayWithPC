using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace EnableGPlayWithPC
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // それぞれのTextBoxにデフォルトのパスを入れておく
            FileSelector_Vending.Init(Path.Combine(appDir, Apks.Vending));
            FileSelector_GMS.Init(Path.Combine(appDir, Apks.GMS));
            FileSelector_GSF.Init(Path.Combine(appDir, Apks.GSF));
            FileSelector_GSFLogin.Init(Path.Combine(appDir, Apks.GSFLogin));
        }

        private async void Button_Process_Click(object sender, EventArgs e)
        {
            Enabled = false;

            int process, ip;
            string TARGET_MODEL;
            var product = "UNKNOWN_MODEL";
            var progressBarDialog = new Progress();

            ip = 0;
            process = 0;

            progressBarDialog.Title = "処理中";
            progressBarDialog.Message = "初期化中";
            progressBarDialog.Value = 0;
            progressBarDialog.Show();

            foreach (var path in GetSelectedPath())
            {
                if (!File.Exists(path))
                {
                    progressBarDialog.Close();
                    Dialog.Error(string.Format(Properties.Resources.Dialog_Process_Error_Title), string.Format(Properties.Resources.Dialog_Process_Error_File404, path), Handle);
                    this.Enabled = true;
                    return;
                }
            }

            progressBarDialog.Value = progressBarDialog.Value + 5;

            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            progressBarDialog.Value = progressBarDialog.Value + 1;

            var adb = new AdbServer();

            progressBarDialog.Value = progressBarDialog.Value + 1;

            try
            {
                var result = adb.StartServer(Path.Combine(appDir, Properties.Resources.AdbPath), true);
            }
            catch (Exception)
            {
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title,
                    Properties.Resources.Dialog_Process_Error_Adb404, Handle);
                this.Enabled = true;
                return;
            }

            progressBarDialog.Value = progressBarDialog.Value + 1;

            try
            {
                process = 1;
                var device = AdbClient.Instance.GetDevices().First();

                if (AdbClient.Instance.GetDevices().Count > 1)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_TooManyDevices_Inst,
                        Properties.Resources.Dialog_TooManyDevices_Desc, Handle);
                    this.Enabled = true;
                    return;
                }
                progressBarDialog.Value = progressBarDialog.Value + 1;
                process = 2;
                var receiver = new ConsoleOutputReceiver();
                AdbClient.Instance.ExecuteRemoteCommand($"getprop ro.build.product", device, receiver);
                product = receiver.ToString();
                product = product.Substring(0, product.Length - 2); // 余計な改行は入れさせない
                TARGET_MODEL = product;
                progressBarDialog.Value = progressBarDialog.Value + 1;

                Console.WriteLine(product.Length);

                if (!BenesseTabs.Names.Contains(product))
                { // 出力が名前にあるか確認
                    progressBarDialog.Hide();
                    var result = Dialog.ShowQuestion(Properties.Resources.Dialog_Not_Benesse_Tab_Inst, string.Format(Properties.Resources.Dialog_Not_Benesse_Tab_Desc, product), Handle);
                    if (result != true)
                    {
                        progressBarDialog.Close();
                        return;
                    }
                    progressBarDialog.Show();
                }

                var packageManager = new PackageManager(device);
                process = 3;

                // それぞれアンインストール
                if (!checkBox1.Checked)
                {
                    foreach (var pkg in Packages.Package)
                    {
                        try
                        {
                            progressBarDialog.Message = pkg + "をアンインストール中";
                            packageManager.UninstallPackage(pkg);
                            progressBarDialog.Value = progressBarDialog.Value + 2;
                        }
                        catch (Exception)
                        {
                            progressBarDialog.Value = progressBarDialog.Value + 2;
                        }
                    }
                }
                else
                {
                    progressBarDialog.Message = "スキップ";
                    progressBarDialog.Value = progressBarDialog.Value + 17;
                }

                // パスを取得
                var apks = GetSelectedPath();
                progressBarDialog.Value = progressBarDialog.Value + 3;

                // それぞれインストール
                process = 4;
                ip = 1;
                await Task.Delay(1000);
                Array.ForEach(apks, apk =>
                {
                    progressBarDialog.Message = "インストール中 (" + ip + "/7)";
                    if (!BenesseTabs.TARGET_MODEL.Contains(product))
                    {
                        packageManager.InstallPackage(apk, false);
                    }
                    else
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + apk + " /data/local/tmp/base.apk && exit");
                        processStartInfo.CreateNoWindow = true;
                        processStartInfo.UseShellExecute = false;
                        Process _process = Process.Start(processStartInfo);
                        _process.WaitForExit();
                        _process.Close();
                        processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                        processStartInfo.CreateNoWindow = true;
                        processStartInfo.UseShellExecute = false;
                        _process = Process.Start(processStartInfo);
                        _process.WaitForExit();
                        _process.Close();
                    }
                    progressBarDialog.Value = progressBarDialog.Value + 10;
                    ip++;
                });

                Array.ForEach(Apks.installList, apk =>
                {
                    progressBarDialog.Message = "インストール中 (" + ip + "/7)";
                    if (!BenesseTabs.TARGET_MODEL.Contains(product))
                    {
                        packageManager.InstallPackage(apk, false);
                    }
                    else
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + apk + " /data/local/tmp/base.apk && exit");
                        processStartInfo.CreateNoWindow = true;
                        processStartInfo.UseShellExecute = false;
                        Process _process = Process.Start(processStartInfo);
                        _process.WaitForExit();
                        _process.Close();
                        processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                        processStartInfo.CreateNoWindow = true;
                        processStartInfo.UseShellExecute = false;
                        _process = Process.Start(processStartInfo);
                        _process.WaitForExit();
                        _process.Close();
                    }
                    progressBarDialog.Value = progressBarDialog.Value + 5;
                    ip++;
                });

                // Play ストアに権限付与
                process = 5;
                progressBarDialog.Message = "Google Playに権限を付与中";
                {
                    var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.Vending,
                            Permissions.Vending,
                            device,
                            Handle);
                    if (!result)
                    {
                        progressBarDialog.Close();
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        Enabled = true;
                        return;
                    }
                }
                progressBarDialog.Value = progressBarDialog.Value + 5;

                // GooglePlay開発者サービスに権限付与
                progressBarDialog.Message = "GMSに権限を付与中";
                {
                    var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GMS,
                            Permissions.GMS,
                            device,
                            Handle);
                    if (!result)
                    {
                        progressBarDialog.Close();
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        Enabled = true;
                        return;
                    }
                }
                progressBarDialog.Value = progressBarDialog.Value + 5;
                // Google Service Frameworkに権限付与
                progressBarDialog.Message = "GFSに権限を付与中";
                {
                    var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GSF,
                            Permissions.GSF,
                            device,
                            Handle);
                    if (!result)
                    {
                        progressBarDialog.Close();
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        Enabled = true;
                        return;
                    }
                }
                progressBarDialog.Value = progressBarDialog.Value + 5;

                // もういちどGMSをインストール。
                process = 6;
                progressBarDialog.Message = "最終処理中";
                if (!BenesseTabs.TARGET_MODEL.Contains(product))
                {
                    progressBarDialog.Value = 95;
                    packageManager.InstallPackage(FileSelector_GMS.GetPath(), true);
                }
                else
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + FileSelector_GMS.GetPath() + " /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    Process _process = Process.Start(processStartInfo);
                    _process.WaitForExit();
                    _process.Close();
                    processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -r -d -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    _process = Process.Start(processStartInfo);
                    _process.WaitForExit();
                    _process.Close();
                }
                progressBarDialog.Value = 100;
                AdbClient.Instance.Reboot("", device);
            }
            catch (Exception)
            {
                progressBarDialog.Close();
                switch (process)
                {
                    case 1:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, product), this.Handle);
                        break;
                    case 2:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, product), Handle);
                        break;
                    case 3:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        break;
                    case 4:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_In, this.Handle);
                        break;
                    case 5:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        break;
                    case 6:
                        Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                        break;

                }
                Enabled = true;
                return;
            }
            progressBarDialog.Close();
            var dialog = new TaskDialog();
            dialog.Caption = "Enable GPlay With PC";
            dialog.InstructionText = Properties.Resources.Dialog_Successed_Inst;
            dialog.Text = Properties.Resources.Dialog_Successed_Desc;
            dialog.Icon = TaskDialogStandardIcon.Information;
            dialog.OwnerWindowHandle = Handle;
            dialog.Show();
            Enabled = true;
        }

        private string[] GetSelectedPath()
        {
            var files = new FileSelector[] { FileSelector_GMS, FileSelector_GSF, FileSelector_GSFLogin, FileSelector_Vending };
            return files.Select(f => f.GetPath()).ToArray();
        }

        private void LinkLabel_Repo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/AioiLight/EnableGPlayWithPC");
        }

        private void 情報XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(Properties.Resources.Information, Assembly.GetExecutingAssembly().GetName().Version), Properties.Resources.Information_Title, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://ctabwiki.nerrog.net/?Describe_EnableGPlay_3-or-Later");
        }
    }
}