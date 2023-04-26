using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace EnableGPlayWithPC {
    public partial class Main : Form {
        static Main instance;
        static UserControl1 ctr1;
        static UserControl2 ctr2;

        public static Main getInstance() {
            return instance;
        }

        public Main() {
            InitializeComponent();
            instance = this;
            ctr1 = new UserControl1 {
                Visible = true
            };
            panel1.Controls.Add(ctr1);
        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(string.Format(Properties.Resources.Information, Assembly.GetExecutingAssembly().GetName().Version), Properties.Resources.Information_Title, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public void Cancel() {
            ctr2.Visible = false;
            ctr1.Visible = true;
        }

        private void ShowErrorMessage(string str) {
            UserControl2.getInstance().StopProgress();
            UserControl2.getInstance().SetMessage("エラーが発生しました");
            UserControl2.getInstance().WriteLog(str);
        }

        private void ShowCompleteMessage() {
            UserControl2.getInstance().StopProgress();
            UserControl2.getInstance().SetMessage("完了");
            UserControl2.getInstance().WriteLog("\"OK\"ボタンをクリックすると終了します");
        }

        public void ChangeUserControl() {
            ctr2 = new UserControl2();
            ctr1.Visible = false;
            ctr2.Visible = true;
            panel1.Controls.Add(ctr2);
        }

        private void ShowDialog(string str, string str2, IntPtr intPtr) {
            TaskDialog dialog = new TaskDialog {
                Caption = "Enable GPlay With PC",
                InstructionText = str,
                Text = str2,
                Icon = TaskDialogStandardIcon.Information,
                OwnerWindowHandle = intPtr
            };
            dialog.Show();
        }

        public void TryEnableGoogleServices() {
            DeviceData deviceData;
            bool bl;
            string str, path, appDir, deviceName = "失敗しました";

            // 処理ダイアログの表示
            ShowProcessDialog(0, null, 0);

            try {
                // ファイルの存在を確認してなければエラー終了
                ShowProcessDialog(1, null, 0);
                (path, bl) = IsCheckFileExists();
                if (!bl) {
                    ShowErrorMessage(string.Format(Properties.Resources.Dialog_Process_Error_File404, path));
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_File404, path), this.Handle);
                    return;
                }

                // adb.exeの存在を確認してなければエラー終了
                ShowProcessDialog(2, null, 0);
                (appDir, bl) = StartAdbServer();
                if (!bl) {
                    ShowErrorMessage(Properties.Resources.Dialog_Process_Error_Adb404);
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Adb404, this.Handle);
                    return;
                }

                //ADB接続を確認
                ShowProcessDialog(3, null, 0);
                try {
                    ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
                    AdbClient.Instance.ExecuteRemoteCommand($"getprop ro.product.model", AdbClient.Instance.GetDevices().First(), receiver);
                    deviceName = receiver.ToString().Substring(0, receiver.ToString().Length - 2);
                } catch (Exception) {
                    ShowErrorMessage(string.Format(Properties.Resources.Dialog_Process_Error_Adb, deviceName));
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Adb, deviceName), this.Handle);
                    return;
                }

                // デバイスの接続数を確認
                (deviceData, bl) = IsCheckDeviceConnect();
                if (!bl) {
                    ShowErrorMessage(Properties.Resources.Dialog_TooManyDevices_Desc);
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), (Properties.Resources.Dialog_TooManyDevices_Inst, Properties.Resources.Dialog_TooManyDevices_Desc), this.Handle);
                    return;
                }

                // デバイス名を確認
                (deviceName, bl) = IsCheckDeviceName(deviceData);
                if (!bl) {
                    ShowErrorMessage(string.Format(Properties.Resources.Dialog_Not_Benesse_Tab_Desc, deviceName));
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Not_Benesse_Tab_Desc, deviceName), this.Handle);
                    return;
                }

                // アンインストールの試行
                (str, bl) = TryUninstallAPK(deviceData);
                if (!bl) {
                    ShowErrorMessage(string.Format(Properties.Resources.Dialog_Process_Error_Un, str));
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, string.Format(Properties.Resources.Dialog_Process_Error_Un, str), this.Handle);
                    return;
                }

                // インストールの試行
                if (!TryInstallAPK(deviceData, deviceName, appDir)) {
                    ShowErrorMessage(Properties.Resources.Dialog_Process_Error_In);
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_In, this.Handle);
                    return;
                }

                // 権限付与試行
                if (!TryGrantPermissions(deviceData)) {
                    ShowErrorMessage(Properties.Resources.Dialog_Process_Error_Unknown);
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                    return;
                }

                //再インストールの試行
                ShowProcessDialog(7, null, 0);
                if (!TryReInstallAPK(deviceData, deviceName, appDir)) {
                    ShowErrorMessage(Properties.Resources.Dialog_Process_Error_In);
                    this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_In, this.Handle);
                    return;
                }

                // 最終処理
                ShowProcessDialog(8, null, 0);
                EndProcess(deviceData);
                return;
            } catch (Exception) {
                //例外が発生したらエラー終了
                ShowErrorMessage(Properties.Resources.Dialog_Process_Error_Unknown);
                this.Invoke(new Action<string, string, IntPtr>(ShowDialog), Properties.Resources.Dialog_Process_Error_Title, Properties.Resources.Dialog_Process_Error_Unknown, this.Handle);
                return;
            }
        }

        //ファイルパスを取得
        private string[] GetSelectedPath() {
            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] files = { Path.Combine(appDir, Apks.Vending), Path.Combine(appDir, Apks.GMS), Path.Combine(appDir, Apks.GSF), Path.Combine(appDir, Apks.GSFLogin) };
            return files;
        }

        // 処理順にメッセージ変更
        private void ShowProcessDialog(int process, string msg, int count) {
            switch (process) {
                case 0:
                    UserControl2.getInstance().WriteLog("処理しています...");
                    UserControl2.getInstance().SetMessage("処理しています...");
                    break;

                case 1:
                    UserControl2.getInstance().WriteLog("ファイルを確認しています...");
                    UserControl2.getInstance().SetMessage("ファイルを確認しています...");
                    break;

                case 2:
                    UserControl2.getInstance().WriteLog("ADB を確認しています...");
                    UserControl2.getInstance().SetMessage("ADB を確認しています...");
                    break;

                case 3:
                    UserControl2.getInstance().WriteLog("デバイスを確認しています...");
                    UserControl2.getInstance().SetMessage("デバイスを確認しています...");
                    break;

                case 4:
                    UserControl2.getInstance().WriteLog(msg + "をアンインストールしています...\r\n");
                    UserControl2.getInstance().SetMessage(msg + "をアンインストールしています...");
                    break;

                case 5:
                    UserControl2.getInstance().WriteLog(msg + "をインストールしています...(" + count + "/4)\r\n");
                    UserControl2.getInstance().SetMessage(msg + "をインストールしています...(" + count + "/4)");
                    break;

                case 6:
                    UserControl2.getInstance().WriteLog(msg + "に権限を付与しています...");
                    UserControl2.getInstance().SetMessage(msg + "に権限を付与しています...");
                    break;

                case 7:
                    UserControl2.getInstance().WriteLog("GMSを再インストールしています...");
                    UserControl2.getInstance().SetMessage("GMSを再インストールしています...");
                    break;

                case 8:
                    UserControl2.getInstance().WriteLog("最終処理をしています...");
                    UserControl2.getInstance().SetMessage("最終処理をしています...");
                    break;

                default:
                    break;
            }
        }

        // ファイルの存在を確認
        private (string, bool) IsCheckFileExists() {
            foreach (string path in GetSelectedPath()) {
                if (!File.Exists(path)) {
                    return (path, false);
                }
            }
            return (null, true);
        }

        // adb.exeの確認
        private (string, bool) StartAdbServer() {
            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AdbServer adb = new AdbServer();

            // adb.exeがなければエラー終了
            try {
                StartServerResult result = adb.StartServer(Path.Combine(appDir, Properties.Resources.AdbPath), true);
            } catch (Exception) {
                return (appDir, false);
            }
            return (appDir, true);
        }

        // デバイスが接続されているか確認
        private (DeviceData, bool) IsCheckDeviceConnect() {
            try {
                DeviceData device = AdbClient.Instance.GetDevices().First();

                if (AdbClient.Instance.GetDevices().Count > 1) {
                    return (device, false);
                }
                return (device, true);
            } catch (Exception) {
                return (null, false);
            }
        }

        // デバイス名の確認
        private (string, bool) IsCheckDeviceName(DeviceData device) {
            string DeviceName;
            try {
                ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();

                AdbClient.Instance.ExecuteRemoteCommand($"getprop ro.product.model", device, receiver);

                // 余計な改行は入れさせない
                DeviceName = receiver.ToString().Substring(0, receiver.ToString().Length - 2);

                Console.WriteLine(DeviceName.Length);

                // 出力が名前にあるか確認
                if (!BenesseTabs.Names.Contains(DeviceName)) {
                    return (DeviceName, false);
                }
                return (DeviceName, true);
            } catch (Exception) {
                return (null, false);
            }
        }

        // APKのアンインストール
        private (string, bool) TryUninstallAPK(DeviceData device) {
            PackageManager packageManager = new PackageManager(device);

            // APKをアンインストール
            foreach (string pkg in Packages.Package) {
                try {
                    ShowProcessDialog(4, pkg, 0);
                    packageManager.UninstallPackage(pkg);
                } catch (Exception) {
                }
            }
            return ("", true);
        }

        // APKのインストール
        private bool TryInstallAPK(DeviceData device, string DeviceName, string appDir) {
            PackageManager packageManager = new PackageManager(device);
            string[] apks;
            if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName)) {
                apks = Apks.GAppsInstallList(appDir);
            } else {
                apks = Apks.NEO_GAppsInstallList(appDir);
            }
            int i = 1;

            // APKインストール
            Array.ForEach(apks, apk => {
                ShowProcessDialog(5, apk, i);

                // チャレンジパッド2かどうか
                if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName)) {
                    packageManager.InstallPackage(apk, false);
                } else {
                    // チャレンジパッド3・NEO
                    AndroidDebugBridgeUtils.InstallPackage(device, apk);
                }
                i++;
            });
            return true;
        }

        // 権限付与
        private bool TryGrantPermissions(DeviceData device) {
            // Google Login Serviceに権限付与
            ShowProcessDialog(6, Packages.GSFLogin, 0);
            {
                bool result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GSFLogin,
                        Permissions.GSFLogin,
                        device,
                        Handle);
                if (!result) {
                }
            }
            // Google Service Frameworkに権限付与
            ShowProcessDialog(6, Packages.GSF, 0);
            {
                bool result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GSF,
                        Permissions.GSF,
                        device,
                        Handle);
                if (!result) {
                }
            }
            // GooglePlay開発者サービスに権限付与
            ShowProcessDialog(6, Packages.GMS, 0);
            {
                bool result = AndroidDebugBridgeUtils.GrantPermissions(Packages.GMS,
                        Permissions.GMS,
                        device,
                        Handle);
                if (!result) {
                }
            }
            // Play ストアに権限付与
            ShowProcessDialog(6, Packages.Vending, 0);
            AndroidDebugBridgeUtils.GrantPermissions(Packages.Vending,
                    Permissions.Vending,
                    device,
                    Handle);

            return true;
        }

        //APKの再インストール
        private bool TryReInstallAPK(DeviceData device, string DeviceName, string appDir) {
            PackageManager packageManager = new PackageManager(device);

            // APK再インストール
            // チャレンジパッド2かどうか
            if (!BenesseTabs.TARGET_MODEL.Contains(DeviceName)) {
                packageManager.InstallPackage(Path.Combine(appDir, Apks.GMS), true);
            } else {
                // チャレンジパッド3・NEO
                AndroidDebugBridgeUtils.InstallPackage(device, Path.Combine(appDir, Apks.old_GMS));
            }
            return true;
        }

        private void EndProcess(DeviceData device) {
            AdbClient.Instance.Reboot("", device);
            AdbClient.Instance.KillAdb();
            ShowCompleteMessage();
            TaskDialog dialog = new TaskDialog {
                Caption = "Enable GPlay With PC",
                InstructionText = Properties.Resources.Dialog_Successed_Inst,
                Text = Properties.Resources.Dialog_Successed_Desc,
                Icon = TaskDialogStandardIcon.Information,
                OwnerWindowHandle = Handle
            };
            dialog.Show();
        }
    }
}