using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnableGPlayWithPC {
    public partial class UserControl1 : UserControl {
        public UserControl1() {
            InitializeComponent();
            textBox1.ReadOnly = true;
            linkLabel1.Text = "ソースコードはここから";
            linkLabel1.Links.Add(7, 4, "");
            string[] str = { "Googleサービスのインストール" };
            checkedListBox1.Items.AddRange(str);
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.Enabled = false;
            checkedListBox1.SetItemCheckState(0, CheckState.Checked);
            writeConsole($"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright}\r\n");
            writeConsole("\"実行\"を押すと端末にGoogle系のアプリがインストールされます。\r\n処理が完了後、端末は再起動されます。\r\n実行している間は端末に触らないでください！");
        }

        void writeConsole(string text) {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = text + "\r\n";
        }

        public async void Button_Click(object sender, EventArgs e) {
            Main.getInstance().ChangeUserControl();
            Task task = Task.Run(Main.getInstance().TryEnableGoogleServices);
            await task;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(@"https://github.com/s1204IT/EnableGPlayWithPC");
        }
    }
}