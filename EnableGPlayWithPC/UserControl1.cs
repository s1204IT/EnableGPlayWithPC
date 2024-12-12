using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnableGPlayWithPC {
    public partial class UserControl1 : UserControl {

        public static UserControl1 instance;

        /* 実行リスト */
        readonly string[] str = { "既存システムへのGoogleサービスの修正" };

        public static UserControl1 GetInstance() {
            return instance;
        }

        public UserControl1() {
            InitializeComponent();
            instance = this;
            checkedListBox1.Items.AddRange(str);
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.Enabled = false;
            checkedListBox1.SetItemCheckState(0, CheckState.Checked);
        }

        public void WriteConsole(string text) {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = text + "\r\n";
        }

        public async void Button_Click(object sender, EventArgs e) {
            Main.GetInstance().ChangeUserControl();
            await (Task.Run(Main.GetInstance().TryEnableGoogleServices));
        }
    }
}