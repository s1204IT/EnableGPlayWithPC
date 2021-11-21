﻿using System;
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


        //実行がクリックされた
        private void Button_Process_Click(object sender, EventArgs e)
        {
            bool bl;
            string path, appDir, DeviceName = "UNKNOWN_MODEL";
            DeviceData device;
            Progress progressBarDialog = new Progress();

            // 処理ダイアログの表示
            ShowProcessDialog(progressBarDialog, 0, null, 0);

            // ファイルの存在を確認してなければエラー終了
            ShowProcessDialog(progressBarDialog, 1, null, 0);
            try
            {
                (path, bl) = IsCheckFileExists();

                if (!bl)
                {
                    progressBarDialog.Close();
                    Dialog.Error(string.Format(Properties.Resources.Dialog_Process_Error_Title), string.Format(Properties.Resources.Dialog_Process_Error_File404, path), Handle);
                    Enabled = true;
                    return;
                }
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, DeviceName), this.Handle);
                Enabled = true;
                return;
            }

            // adb.exeの存在を確認してなければエラー終了
            ShowProcessDialog(progressBarDialog, 2, null, 0);
            try
            {
                (appDir, bl) = StartAdbServer(progressBarDialog);

                if (!bl)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Adb404, Handle);
                    Enabled = true;
                    return;
                }
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, DeviceName), this.Handle);
                Enabled = true;
                return;
            }

            // デバイスが接続されているかを確認してされていないならエラー終了
            ShowProcessDialog(progressBarDialog, 3, null, 0);
            try
            {
                (device, bl) = IsCheckDeviceConnect(progressBarDialog);

                if (!bl)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_TooManyDevices_Inst, Properties.Resources.Dialog_TooManyDevices_Desc, Handle);
                    Enabled = true;
                    return;
                }
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, DeviceName), this.Handle);
                Enabled = true;
                return;
            }

            // デバイス名を確認
            try
            {
                (DeviceName, bl) = IsCheckDeviceName(progressBarDialog, device);

                if (!bl)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                    Enabled = true;
                    return;
                }
            }
            catch (Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, DeviceName), this.Handle);
                Enabled = true;
                return;
            }

            // アンインストールの試行
            try
            {
                if (!TryUninstallAPK(progressBarDialog, device))
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                    Enabled = true;
                    return;
                }
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                Enabled = true;
                return;
            }

            // インストールの試行
            try
            {
                if (!TryInstallAPKAsync(progressBarDialog, device, DeviceName, appDir).Result)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_In, this.Handle);
                    Enabled = true;
                    return;
                }
            }
            catch (Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                Enabled = true;
                return;
            }

            // 権限付与試行
            try
            {
                if (!TryGrantPermissions(progressBarDialog, device))
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                    Enabled = true;
                    return;
                }
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                Enabled = true;
                return;
            }

            //再インストールの試行
            ShowProcessDialog(progressBarDialog, 10, null, 0);
            try
            {
                if (!TryReInstallAPKAsync(progressBarDialog, device, DeviceName, appDir).Result)
                {
                    progressBarDialog.Close();
                    Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_In, this.Handle);
                    Enabled = true;
                    return;
                }
            }
            catch (Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                Enabled = true;
                return;
            }

            // 最終処理
            try
            {
                EndProcess(progressBarDialog, device);
            }
            catch(Exception)
            {
                //例外が発生したらエラー終了
                progressBarDialog.Close();
                Dialog.Error(Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                Enabled = true;
                return;
            }
            return;
        }

        // 処理ダイアログのパーセンテージの値を変更
        private void ProgressDialogCountUp(Progress progress, int value)
        {
            progress.Value += value;
        }

        // 処理ダイアログを表示する
        private void ShowProcessDialog(Progress progress, int process, string msg, int count)
        {
            // 画面操作無効
            Enabled = false;

            // 処理順にメッセージ変更
            switch (process)
            {
                case 0:
                    progress.Value = 0;
                    progress.Title = "処理中";
                    progress.Message = "初期化中";
                    progress.Show();
                    break;

                case 1:
                    progress.Message = "ファイルを確認中";
                    break;

                case 2:
                    progress.Message = "adbを確認中";
                    break;

                case 3:
                    progress.Message = "デバイスを確認中";
                    break;

                case 4:
                    progress.Message = msg + "をアンインストール中";
                    break;

                case 5:
                    progress.Message = "スキップします";
                    break;

                case 6:
                    progress.Message = "アプリをインストール中（" + count + "/7）";
                    break;

                case 7:
                    progress.Message = "Google Playに権限を付与中";
                    break;

                case 8:
                    progress.Message = "GMSに権限を付与中";
                    break;

                case 9:
                    progress.Message = "GFSに権限を付与中";
                    break;

                case 10:
                    progress.Message = "最終処理中";
                    break;

                default:
                    break;
            }
        }

        // ファイルの存在を確認
        private (string, bool) IsCheckFileExists()
        {
            foreach (string path in GetSelectedPath())
            {
                if (!File.Exists(path))
                {
                    return (path, false);
                }
            }
            return (null, true);
        }

        // adb.exeの確認
        private (string, bool) StartAdbServer(Progress progressBarDialog)
        {
            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AdbServer adb = new AdbServer();

            ProgressDialogCountUp(progressBarDialog, 7);

            // adb.exeがなければエラー終了
            try
            {
                var result = adb.StartServer(Path.Combine(appDir, Properties.Resources.AdbPath), true);
            }
            catch (Exception)
            {
                return (appDir, false);
            }
            return (appDir, true);
        }

        // デバイスが接続されているか確認
        private (DeviceData, bool) IsCheckDeviceConnect(Progress progressBarDialog)
        {
            DeviceData device = AdbClient.Instance.GetDevices().First();

            ProgressDialogCountUp(progressBarDialog, 1);

            if (AdbClient.Instance.GetDevices().Count > 1)
            {
                return (device, false);
            }
            return (device, true);
        }

        // デバイス名の確認
        private (string, bool) IsCheckDeviceName(Progress progressBarDialog, DeviceData device)
        {
            string DeviceName;
            ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();

            ProgressDialogCountUp(progressBarDialog, 2);

            AdbClient.Instance.ExecuteRemoteCommand($"getprop ro.build.product", device, receiver);

            // 余計な改行は入れさせない
            DeviceName = receiver.ToString().Substring(0, receiver.ToString().Length - 2);

            Console.WriteLine(DeviceName.Length);

            // 出力が名前にあるか確認
            if (!BenesseTabs.Names.Contains(DeviceName))
            { 
                progressBarDialog.Hide();
                var result = Dialog.ShowQuestion(Properties.Resources.Dialog_Not_Benesse_Tab_Inst, string.Format(Properties.Resources.Dialog_Not_Benesse_Tab_Desc, DeviceName), Handle);
                if (result != true)
                {
                    progressBarDialog.Close();
                    return (DeviceName, false);
                }
                progressBarDialog.Show();
            }
            return (DeviceName, true);
        }

        // APKのアンインストール
        private bool TryUninstallAPK(Progress progressBarDialog, DeviceData device)
        {
            PackageManager packageManager = new PackageManager(device);

            // チェックボックスがクリックされていない
            if (!checkBox1.Checked)
            {
                // APKをアンインストール
                foreach (string pkg in Packages.Package)
                {
                    try
                    {
                        ShowProcessDialog(progressBarDialog, 4, pkg, 0);
                        packageManager.UninstallPackage(pkg);
                        ProgressDialogCountUp(progressBarDialog, 2);
                    }
                    catch (Exception)
                    {
                        ProgressDialogCountUp(progressBarDialog, 2);
                    }
                }
            }
            else
            {
                ShowProcessDialog(progressBarDialog, 5, null, 0);
                ProgressDialogCountUp(progressBarDialog, 17);
            }
            return true;
        }

        // APKのインストール
        private async Task<bool> TryInstallAPKAsync(Progress progressBarDialog, DeviceData device, string DeviceName, string appDir)
        {
            PackageManager packageManager = new PackageManager(device);
            string[] apks = GetSelectedPath();
            int i = 1;

            ProgressDialogCountUp(progressBarDialog, 3);

            // APKインストール
            Array.ForEach(apks, apk =>
            {
                ShowProcessDialog(progressBarDialog, 6, null, i);

                    // チャレンジパッド2かどうか
                    if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName))
                {
                    packageManager.InstallPackage(apk, false);
                }
                else
                {
                        // チャレンジパッド3・NEO
                        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + apk + " /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    Process process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    process.Close();
                    processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    process.Close();
                }
                ProgressDialogCountUp(progressBarDialog, 10);
                i++;
            });

            // あとから追加したAPKもインストール
            Array.ForEach(Apks.installList, apk =>
            {
                ShowProcessDialog(progressBarDialog, 6, null, i);

                    // チャレンジパッド2かどうか
                    if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName))
                {
                    packageManager.InstallPackage(apk, false);
                }
                else
                {
                        // チャレンジパッド3・NEO
                        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + apk + " /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    Process process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    process.Close();
                    processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    process.Close();
                }
                ProgressDialogCountUp(progressBarDialog, 5);
                i++;
            });
            return true;
        }

        // 権限付与
        private bool TryGrantPermissions(Progress progressBarDialog, DeviceData device)
        {
            // Play ストアに権限付与
            ShowProcessDialog(progressBarDialog, 7, null, 0);
            {
                var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.Vending,
                        Permissions.Vending,
                        device,
                        Handle);
                if (!result)
                {
                    return false ;
                }
            }

            ProgressDialogCountUp(progressBarDialog, 5);

            // GooglePlay開発者サービスに権限付与
            ShowProcessDialog(progressBarDialog, 8, null, 0);
            {
                var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GMS,
                        Permissions.GMS,
                        device,
                        Handle);
                if (!result)
                {
                    return false;
                }
            }

            ProgressDialogCountUp(progressBarDialog, 5);

            // Google Service Frameworkに権限付与
            ShowProcessDialog(progressBarDialog, 9, null, 0);
            {
                var result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GSF,
                        Permissions.GSF,
                        device,
                        Handle);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        //APKの再インストール
        private async Task<bool> TryReInstallAPKAsync(Progress progressBarDialog, DeviceData device, string DeviceName, string appDir)
        {
            PackageManager packageManager = new PackageManager(device);

            ProgressDialogCountUp(progressBarDialog, 5);

            // APK再インストール
            // チャレンジパッド2かどうか
            if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName))
            {
                progressBarDialog.Value = 95;
                packageManager.InstallPackage(FileSelector_GMS.GetPath(), true);
            }
            else
            {
                // チャレンジパッド3・NEO
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + FileSelector_GMS.GetPath() + " /data/local/tmp/base.apk && exit");
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                process.Close();
                processStartInfo = new ProcessStartInfo("cmd.exe", "/k " + Path.Combine(appDir, Properties.Resources.AdbPath) + " shell pm install -r -d -i \"com.android.vending\" /data/local/tmp/base.apk && exit");
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                process = Process.Start(processStartInfo);
                process.WaitForExit();
                process.Close();
            }
            return true;
        }

        private void EndProcess(Progress progressBarDialog, DeviceData device)
        {
            progressBarDialog.Value = 100;

            AdbClient.Instance.Reboot("", device);

            progressBarDialog.Close();

            TaskDialog dialog = new TaskDialog();

            dialog.Caption = "Enable GPlay With PC";
            dialog.InstructionText = Properties.Resources.Dialog_Successed_Inst;
            dialog.Text = Properties.Resources.Dialog_Successed_Desc;
            dialog.Icon = TaskDialogStandardIcon.Information;
            dialog.OwnerWindowHandle = Handle;
            dialog.Show();

            // 画面操作有効
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