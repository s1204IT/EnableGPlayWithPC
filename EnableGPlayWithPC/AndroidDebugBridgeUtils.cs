using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpAdbClient;

namespace EnableGPlayWithPC {
    internal static class AndroidDebugBridgeUtils {
        /// <summary>
        /// ADBからの文字列を調べて、権限付与に成功しているかどうかチェックする。
        /// </summary>
        /// <param name="str">出力。</param>
        /// <returns>権限付与に成功しているかどうか。</returns>
        internal static bool IsPermissionGranted(string str) {
            string[] lines = str.Split('\n');
            if (lines.Where(s => s.StartsWith("Operation not allowed:")).Any()) {
                // Operation not allowed:で始まる行が少なくともひとつはある
                return false;
            } else {
                // パーミション付与に成功している
                return true;
            }
        }

        /// <summary>
        /// 権限を付与する。
        /// </summary>
        /// <param name="packageName">パッケージ名。</param>
        /// <param name="perms">権限の一覧。</param>
        /// <param name="device">ADB デバイス。</param>
        /// <param name="handle">ウィンドウハンドル。</param>
        /// <returns>成功したかどうか。</returns>
        internal static bool GrantPermissions(string packageName, IEnumerable<string> perms, DeviceData device, IntPtr handle) {
            foreach (string perm in perms) {
                ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
                string cmd = $"pm grant {packageName} {Permissions.Prefix}{perm}";
                AdbClient.Instance.ExecuteRemoteCommand(cmd, device, receiver);

                string result = receiver.ToString();

                if (!IsPermissionGranted(result)) {
                    // 権限付与に失敗してなおかつキャンセルされた
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// アプリのインストール
        /// </summary>
        /// <param name="device">デバイスデータ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>成功したかどうか。</returns>
        internal static bool InstallPackage(DeviceData device, string fileName) {
            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string installCmd = $"pm install -i \"com.android.vending\" -r /data/local/tmp/base.apk";
            ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + Path.Combine(appDir, Properties.Resources.AdbPath) + " push " + fileName + " /data/local/tmp/base.apk");
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            process.Close();
            AdbClient.Instance.ExecuteRemoteCommand(installCmd, device, receiver);
            return true;
        }
    }
}