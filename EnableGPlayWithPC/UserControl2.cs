using System;
using System.Windows.Forms;

namespace EnableGPlayWithPC {
    public partial class UserControl2 : UserControl {
        static UserControl2 instance;

        public static UserControl2 getInstance() {
            return instance;
        }

        public UserControl2() {
            InitializeComponent();
            instance = this;
            progressBar1.Style = ProgressBarStyle.Marquee;
            textBox1.ReadOnly = true;
            button1.Text = "OK";
            button1.Enabled = false;
        }

        public void SetMessage(string str) {
            this.Invoke(new Action<string>(this.c), str);
        }

        public void StopProgress() {
            this.Invoke(new Action(this.b));
        }
        public void WriteLog(string logText) {
            this.Invoke(new Action<string>(this.a), logText);
        }

        public void Button_Click(object sender, EventArgs e) {
            Main.getInstance().Cancel();
        }

        private void a(string logText) {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "[" + System.DateTime.Now.ToString() + "]" + logText + "\r\n";
        }

        private void b() {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;
            button1.Enabled = true;
        }

        private void c(string str) {
            label1.Text = str;
        }
    }
}