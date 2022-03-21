using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnableGPlayWithPC
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
            linkLabel1.Text = "開発者のページはここをクリックしてください。";
            linkLabel1.Links.Add(8, 2, "");
            string[] str = { "基本的な変更（Googleサービスの有効化）" };
            checkedListBox1.Items.AddRange(str);
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SetItemCheckState(0, CheckState.Checked);
            writeConsole($"{Assembly.GetExecutingAssembly().GetName().Name}   " + $"Ver.{Assembly.GetExecutingAssembly().GetName().Version}");
            writeConsole($"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright}");
            writeConsole("\"実行\"を押すとタブレット内のGSF系のアプリが再インストールされます。タブレットは自動的に再起動します。実行している間は絶対にタブレットに触らないでください！");
        }

        private void CheckedListBox1_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (!checkedListBox1.GetItemChecked(e.Index))
            {
                writeLog($"{checkedListBox1.Items[e.Index]}がチェックされました");
            }
        }

        void writeLog(String logText)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "[" + System.DateTime.Now.ToString() + "]" + logText + "\r\n";
        }

        void writeConsole(String text)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = text + "\r\n";
        }

        public async void Button_Click(Object sender, EventArgs e)
        {
            if (!checkedListBox1.GetItemChecked(0))
            {
                checkedListBox1.SetItemChecked(0, true);
                writeLog("基本的な変更（Googleサービスの有効化）を無効にすることはできません");
                return;
            }
            Main.getInstance().ChangeUserControl();
            Task task = Task.Run(Main.getInstance().TryEnableGoogleServices);
            await task;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/AioiLight/EnableGPlayWithPC");
        }
    }
}